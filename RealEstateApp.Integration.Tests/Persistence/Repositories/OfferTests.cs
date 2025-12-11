using Domain.Common.Enums;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Integration.Tests.Persistence.Repositories
{
    public class OfferTests
    {
        private readonly DbContextOptions<RealEstateAppContext> _dbOptions;

        public OfferTests()
        {
            // Cada test usa una base en memoria única para evitar contaminación entre pruebas
            _dbOptions = new DbContextOptionsBuilder<RealEstateAppContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_Offers_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task CreateOfferAsync_Should_Add_Offer_With_Pending_Status()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var repository = new OfferRepository(context);
            var offer = new Offer { PropertyId = 1, ClientId = "client1", Amount = 100000m };

            // Act
            var result = await repository.CreateOfferAsync(offer);

            // Assert
            result.Should().NotBeNull();
            result!.Status.Should().Be(OfferStatus.Pending);
            result.Date.Should().NotBe(default);
        }

        [Fact]
        public async Task GetOfferByIdAsync_Should_Return_Offer_When_Exists()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);

            // Aqui creo mi propiedad relacionada
            var property = new Property
            {
                Id = 1,
                Code = "PROP-001",
                PropertyTypeId = 1,
                AgentId = "agent1",
                SaleTypeId = 1,
                Price = 100000m,
                Description = "Propiedad de prueba",
                SizeInMeters = 120,
                Bedrooms = 3,
                Bathrooms = 2,
                Status = PropertyStatus.Available
            };
            context.Properties.Add(property);

            // Luego pongo la oferta vinculada a ese propeidad
            var offer = new Offer
            {
                PropertyId = 1,
                ClientId = "client2",
                Amount = 200000m,
                Status = OfferStatus.Pending,
                Date = DateTime.UtcNow
            };
            context.Offers.Add(offer);

            await context.SaveChangesAsync();

            var repository = new OfferRepository(context);

            // Act
            var result = await repository.GetOfferByIdAsync(offer.Id);

            // Assert
            result.Should().NotBeNull();
            result!.ClientId.Should().Be("client2");
            result.Property.Should().NotBeNull();// valido que la propuedad se incluyo por si aca
            result.Property!.Code.Should().Be("PROP-001");
        }

        [Fact]
        public async Task GetOfferByIdAsync_Should_Return_Null_When_NotExists()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var repository = new OfferRepository(context);

            // Act
            var result = await repository.GetOfferByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetOffersByPropertyAsync_Should_Return_Offers_For_Property()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var offers = new List<Offer>
        {
            new Offer { PropertyId = 1, ClientId = "c1", Amount = 100m, Date = DateTime.UtcNow, Status = OfferStatus.Pending },
            new Offer { PropertyId = 1, ClientId = "c2", Amount = 200m, Date = DateTime.UtcNow.AddMinutes(1), Status = OfferStatus.Pending },
            new Offer { PropertyId = 2, ClientId = "c3", Amount = 300m, Date = DateTime.UtcNow, Status = OfferStatus.Pending }
        };
            context.Offers.AddRange(offers);
            await context.SaveChangesAsync();
            var repository = new OfferRepository(context);

            // Act
            var result = await repository.GetOffersByPropertyAsync(1);

            // Assert
            result.Should().HaveCount(2);
            result.First().ClientId.Should().Be("c2"); // ordenado por fecha descendente
        }

        [Fact]
        public async Task GetOffersByClientAsync_Should_Return_Offers_For_Client()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var offers = new List<Offer>
        {
            new Offer { PropertyId = 1, ClientId = "clientX", Amount = 100m, Date = DateTime.UtcNow, Status = OfferStatus.Pending },
            new Offer { PropertyId = 2, ClientId = "clientX", Amount = 200m, Date = DateTime.UtcNow.AddMinutes(1), Status = OfferStatus.Pending },
            new Offer { PropertyId = 3, ClientId = "clientY", Amount = 300m, Date = DateTime.UtcNow, Status = OfferStatus.Pending }
        };
            context.Offers.AddRange(offers);
            await context.SaveChangesAsync();
            var repository = new OfferRepository(context);

            // Act
            var result = await repository.GetOffersByClientAsync("clientX");

            // Assert
            result.Should().HaveCount(2);
            result.First().Amount.Should().Be(200m); // ordenado por fecha descendente
        }

        [Fact]
        public async Task AcceptOfferAsync_Should_Accept_One_And_Reject_Others()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var property = new Property
            {
                Id = 1,
                Code = "PROP-001",
                PropertyTypeId = 1,
                AgentId = "agent1",
                SaleTypeId = 1,
                Price = 100000m,
                Description = "Propiedad",
                SizeInMeters = 120,
                Bedrooms = 3,
                Bathrooms = 2,
                Status = PropertyStatus.Available
            };
            context.Properties.Add(property);
            var offers = new List<Offer>
        {
            new Offer { PropertyId = 1, ClientId = "c1", Amount = 100m, Date = DateTime.UtcNow, Status = OfferStatus.Pending },
            new Offer { PropertyId = 1, ClientId = "c2", Amount = 200m, Date = DateTime.UtcNow, Status = OfferStatus.Pending }
        };
            context.Offers.AddRange(offers);
            await context.SaveChangesAsync();
            var repository = new OfferRepository(context);

            // Act
            var result = await repository.AcceptOfferAsync(offers[0].Id);

            // Assert
            result.Should().BeTrue();
            var accepted = await context.Offers.FindAsync(offers[0].Id);
            accepted!.Status.Should().Be(OfferStatus.Accepted);
            var rejected = await context.Offers.FindAsync(offers[1].Id);
            rejected!.Status.Should().Be(OfferStatus.Rejected);
            var updatedProperty = await context.Properties.FindAsync(1);
            updatedProperty!.Status.Should().Be(PropertyStatus.Sold);
        }

        [Fact]
        public async Task RejectOfferAsync_Should_Change_Status_To_Rejected()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var offer = new Offer { PropertyId = 1, ClientId = "c1", Amount = 100m, Date = DateTime.UtcNow, Status = OfferStatus.Pending };
            context.Offers.Add(offer);
            await context.SaveChangesAsync();
            var repository = new OfferRepository(context);

            // Act
            var result = await repository.RejectOfferAsync(offer.Id);

            // Assert
            result.Should().BeTrue();
            var updated = await context.Offers.FindAsync(offer.Id);
            updated!.Status.Should().Be(OfferStatus.Rejected);
        }

        [Fact]
        public async Task RejectOfferAsync_Should_Return_False_When_NotExists()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var repository = new OfferRepository(context);

            // Act
            var result = await repository.RejectOfferAsync(999);

            // Assert
            result.Should().BeFalse();
        }
    }
}

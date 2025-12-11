using FluentAssertions;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Unit.Tests.Features.PropertyType
{
    public class GetAllPropertyTypesQueryHandlerTests
    {
        private readonly DbContextOptions<RealEstateAppContext> _dbOptions;

        public GetAllPropertyTypesQueryHandlerTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealEstateAppContext>()
                .UseInMemoryDatabase($"TestDb_GetAllPropertyTypes_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task Handle_Should_Return_List_When_PropertyTypes_Exist()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var propertyTypes = new List<Domain.Entities.PropertyType>
        {
            new Domain.Entities.PropertyType { Id = 1, Name = "Residencial", Description = "Casa o apartamento" },
            new Domain.Entities.PropertyType { Id = 2, Name = "Comercial", Description = "Locales y oficinas" }
        };
            context.PropertyTypes.AddRange(propertyTypes);
            await context.SaveChangesAsync();

            var repository = new PropertyTypeRepository(context);
            var handler = new Application.Features.PropertyType.Queries.GetAll.GetAllPropertyTypesQueryHandler(repository);

            var query = new Application.Features.PropertyType.Queries.GetAll.GetAllPropertyTypesQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.First().Name.Should().Be("Residencial");
            result.Last().Name.Should().Be("Comercial");
        }

        [Fact]
        public async Task Handle_Should_Return_Empty_List_When_No_PropertyTypes()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var repository = new PropertyTypeRepository(context);
            var handler = new Application.Features.PropertyType.Queries.GetAll.GetAllPropertyTypesQueryHandler(repository);

            var query = new Application.Features.PropertyType.Queries.GetAll.GetAllPropertyTypesQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}

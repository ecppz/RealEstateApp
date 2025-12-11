using Application.Features.SaleType.Queries.GetById;
using FluentAssertions;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Unit.Tests.Features.SaleType
{
    public class GetSaleTypeByIdQueryHandlerTests
    {
        private readonly DbContextOptions<RealEstateAppContext> _dbOptions;

        public GetSaleTypeByIdQueryHandlerTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealEstateAppContext>()
                .UseInMemoryDatabase($"TestDb_GetSaleTypeById_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task Handle_Should_Return_SaleType_When_It_Exists()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var saleType = new Domain.Entities.SaleType
            {
                Id = 1,
                Name = "Permuta",
                Description = "Intercambio de propiedades"
            };
            context.SaleTypes.Add(saleType);
            await context.SaveChangesAsync();

            var repository = new SaleTypeRepository(context);
            var handler = new GetSaleTypeByIdQueryHandler(repository);

            var query = new GetSaleTypeByIdQuery { Id = 1 };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(1);
            result.Name.Should().Be("Permuta");
        }

        [Fact]
        public async Task Handle_Should_Return_Null_When_SaleType_NotExists()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var repository = new SaleTypeRepository(context);
            var handler = new GetSaleTypeByIdQueryHandler(repository);

            var query = new GetSaleTypeByIdQuery { Id = 999 };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }
    }
}

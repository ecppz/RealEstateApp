using Application.Features.SaleType.Queries.GetAll;
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
    public class GetAllSaleTypesQueryHandlerTests
    {
        private readonly DbContextOptions<RealEstateAppContext> _dbOptions;

        public GetAllSaleTypesQueryHandlerTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealEstateAppContext>()
                .UseInMemoryDatabase($"TestDb_GetAllSaleTypes_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task Handle_Should_Return_List_When_SaleTypes_Exist()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var saleTypes = new List<Domain.Entities.SaleType>
        {
            new Domain.Entities.SaleType { Id = 1, Name = "Venta Directa", Description = "Sin intermediarios" },
            new Domain.Entities.SaleType { Id = 2, Name = "Subasta", Description = "Venta mediante puja" }
        };
            context.SaleTypes.AddRange(saleTypes);
            await context.SaveChangesAsync();

            var repository = new SaleTypeRepository(context);
            var handler = new GetAllSaleTypesQueryHandler(repository);

            var query = new GetAllSaleTypesQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.First().Name.Should().Be("Venta Directa");
            result.Last().Name.Should().Be("Subasta");
        }

        [Fact]
        public async Task Handle_Should_Return_Empty_List_When_No_SaleTypes()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var repository = new SaleTypeRepository(context);
            var handler = new GetAllSaleTypesQueryHandler(repository);

            var query = new GetAllSaleTypesQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}

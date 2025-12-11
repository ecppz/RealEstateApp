using Application.Features.SaleType.Commands.Delete;
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
    public class DeleteSaleTypeCommandHandlerTests
    {
        private readonly DbContextOptions<RealEstateAppContext> _dbOptions;

        public DeleteSaleTypeCommandHandlerTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealEstateAppContext>()
                .UseInMemoryDatabase($"TestDb_DeleteSaleType_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task Handle_Should_Delete_SaleType_When_It_Exists()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var saleType = new Domain.Entities.SaleType
            {
                Id = 1,
                Name = "Subasta",
                Description = "Venta mediante puja"
            };
            context.SaleTypes.Add(saleType);
            await context.SaveChangesAsync();

            var repository = new SaleTypeRepository(context);
            var handler = new DeleteSaleTypeCommandHandler(repository);

            var command = new DeleteSaleTypeCommand { Id = 1 };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(1);
            var deleted = await context.SaleTypes.FindAsync(1);
            deleted.Should().BeNull();
        }

        [Fact]
        public async Task Handle_Should_Return_Null_When_SaleType_NotExists()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var repository = new SaleTypeRepository(context);
            var handler = new DeleteSaleTypeCommandHandler(repository);

            var command = new DeleteSaleTypeCommand { Id = 999 };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }
    }
}

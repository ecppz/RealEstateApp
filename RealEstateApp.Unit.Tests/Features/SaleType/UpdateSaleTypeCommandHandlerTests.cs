using Application.Features.SaleType.Commands.Update;
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
    public class UpdateSaleTypeCommandHandlerTests
    {
        private readonly DbContextOptions<RealEstateAppContext> _dbOptions;

        public UpdateSaleTypeCommandHandlerTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealEstateAppContext>()
                .UseInMemoryDatabase($"TestDb_UpdateSaleType_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task Handle_Should_Update_SaleType_When_It_Exists()
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
            var handler = new UpdateSaleTypeCommandHandler(repository);

            var command = new UpdateSaleTypeCommand
            {
                Id = 1,
                Name = "Permuta Actualizada",
                Description = "Intercambio actualizado"
            };

            // Act
            var resultId = await handler.Handle(command, CancellationToken.None);

            // Assert
            resultId.Should().Be(1);
            var updated = await context.SaleTypes.FindAsync(1);
            updated!.Name.Should().Be("Permuta Actualizada");
            updated.Description.Should().Be("Intercambio actualizado");
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_SaleType_NotExists()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var repository = new SaleTypeRepository(context);
            var handler = new UpdateSaleTypeCommandHandler(repository);

            var command = new UpdateSaleTypeCommand
            {
                Id = 999,
                Name = "No Existe",
                Description = "No existe"
            };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}

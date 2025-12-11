using Application.Features.SaleType.Commands.Create;
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
    public class CreateSaleTypeCommandHandlerTests
    {
        private readonly DbContextOptions<RealEstateAppContext> _dbOptions;

        public CreateSaleTypeCommandHandlerTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealEstateAppContext>()
                .UseInMemoryDatabase($"TestDb_CreateSaleType_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task Handle_Should_Create_SaleType_When_Command_Is_Valid()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var repository = new SaleTypeRepository(context);
            var handler = new CreateSaleTypeCommandHandler(repository);

            var command = new CreateSaleTypeCommand
            {
                Name = "Venta Directa",
                Description = "Se vende sin intermediarios"
            };

            // Act
            var resultId = await handler.Handle(command, CancellationToken.None);

            // Assert
            resultId.Should().BeGreaterThan(0);
            var created = await context.SaleTypes.FindAsync(resultId);
            created.Should().NotBeNull();
            created!.Name.Should().Be("Venta Directa");
            created.Description.Should().Be("Se vende sin intermediarios");
        }
    }
}

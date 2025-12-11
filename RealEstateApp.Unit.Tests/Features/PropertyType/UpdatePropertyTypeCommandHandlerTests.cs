using Application.Features.PropertyType.Commands.Update;
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
    public class UpdatePropertyTypeCommandHandlerTests
    {
        private readonly DbContextOptions<RealEstateAppContext> _dbOptions;

        public UpdatePropertyTypeCommandHandlerTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealEstateAppContext>()
                .UseInMemoryDatabase($"TestDb_UpdatePropertyType_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task Handle_Should_Update_PropertyType_When_It_Exists()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);

            var propertyType = new Domain.Entities.PropertyType { Id = 1, Name = "Residencial", Description = "Casa o apartamento" };
            context.PropertyTypes.Add(propertyType);
            await context.SaveChangesAsync();

            var repository = new PropertyTypeRepository(context);
            var handler = new UpdatePropertyTypeCommandHandler(repository);

            var command = new UpdatePropertyTypeCommandWrapper
            {
                Id = 1,
                Name = "Residencial Actualizado",
                Description = "Propiedad habitacional actualizada"
            };

            // Act
            var resultId = await handler.Handle(command, CancellationToken.None);

            // Assert
            resultId.Should().Be(1);
            var updated = await context.PropertyTypes.FindAsync(1);
            updated!.Name.Should().Be("Residencial Actualizado");
            updated.Description.Should().Be("Propiedad habitacional actualizada");
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_PropertyType_NotExists()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var repository = new PropertyTypeRepository(context);
            var handler = new UpdatePropertyTypeCommandHandler(repository);

            var command = new UpdatePropertyTypeCommandWrapper
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

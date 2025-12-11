using Application.Features.Improvement.Commands.Update;
using FluentAssertions;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Unit.Tests.Features.Improvement
{
    public class UpdateImprovementCommandHandlerTests
    {
        private readonly DbContextOptions<RealEstateAppContext> _dbOptions;

        public UpdateImprovementCommandHandlerTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealEstateAppContext>()
                .UseInMemoryDatabase($"TestDb_UpdateImprovement_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task Handle_Should_Update_Improvement_When_It_Exists()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var improvement = new Domain.Entities.Improvement
            {
                Id = 1,
                Name = "Jardín",
                Description = "Área verde"
            };
            context.Improvements.Add(improvement);
            await context.SaveChangesAsync();

            var repository = new ImprovementRepository(context);
            var handler = new UpdateImprovementCommandHandler(repository);

            var command = new UpdateImprovementCommand
            {
                Id = 1,
                Name = "Jardín Actualizado",
                Description = "Área verde con diseño moderno"
            };

            // Act
            var resultId = await handler.Handle(command, CancellationToken.None);

            // Assert
            resultId.Should().Be(1);
            var updated = await context.Improvements.FindAsync(1);
            updated!.Name.Should().Be("Jardín Actualizado");
            updated.Description.Should().Be("Área verde con diseño moderno");
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_Improvement_NotExists()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var repository = new ImprovementRepository(context);
            var handler = new UpdateImprovementCommandHandler(repository);

            var command = new UpdateImprovementCommand
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

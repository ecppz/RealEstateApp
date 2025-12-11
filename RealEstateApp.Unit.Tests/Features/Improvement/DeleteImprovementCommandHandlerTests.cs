using Application.Features.Improvement.Commands.Delete;
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
    public class DeleteImprovementCommandHandlerTests
    {
        private readonly DbContextOptions<RealEstateAppContext> _dbOptions;

        public DeleteImprovementCommandHandlerTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealEstateAppContext>()
                .UseInMemoryDatabase($"TestDb_DeleteImprovement_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task Handle_Should_Delete_Improvement_When_It_Exists()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var improvement = new Domain.Entities.Improvement
            {
                Id = 1,
                Name = "Piscina",
                Description = "Construcción de piscina"
            };
            context.Improvements.Add(improvement);
            await context.SaveChangesAsync();

            var repository = new ImprovementRepository(context);
            var handler = new DeleteImprovementCommandHandler(repository);

            var command = new DeleteImprovementCommand { Id = 1 };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(1);
            var deleted = await context.Improvements.FindAsync(1);
            deleted.Should().BeNull();
        }

        [Fact]
        public async Task Handle_Should_Return_Null_When_Improvement_NotExists()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var repository = new ImprovementRepository(context);
            var handler = new DeleteImprovementCommandHandler(repository);

            var command = new DeleteImprovementCommand { Id = 999 };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }
    }
}

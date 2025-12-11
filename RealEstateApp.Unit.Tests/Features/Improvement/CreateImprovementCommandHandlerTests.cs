using Application.Features.Improvement.Commands.Create;
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
    public class CreateImprovementCommandHandlerTests
    {
        private readonly DbContextOptions<RealEstateAppContext> _dbOptions;

        public CreateImprovementCommandHandlerTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealEstateAppContext>()
                .UseInMemoryDatabase($"TestDb_CreateImprovement_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task Handle_Should_Create_Improvement_When_Command_Is_Valid()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var repository = new ImprovementRepository(context);
            var handler = new CreateImprovementCommandHandler(repository);

            var command = new CreateImprovementCommand
            {
                Name = "Piscina",
                Description = "Construcción de piscina en la propiedad"
            };

            // Act
            var resultId = await handler.Handle(command, CancellationToken.None);

            // Assert
            resultId.Should().BeGreaterThan(0);
            var created = await context.Improvements.FindAsync(resultId);
            created.Should().NotBeNull();
            created!.Name.Should().Be("Piscina");
            created.Description.Should().Be("Construcción de piscina en la propiedad");
        }
    }
}

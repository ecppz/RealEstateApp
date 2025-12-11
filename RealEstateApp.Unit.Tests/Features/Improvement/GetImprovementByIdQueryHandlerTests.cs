using Application.Features.Improvement.Queries.GetById;
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
    public class GetImprovementByIdQueryHandlerTests
    {
        private readonly DbContextOptions<RealEstateAppContext> _dbOptions;

        public GetImprovementByIdQueryHandlerTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealEstateAppContext>()
                .UseInMemoryDatabase($"TestDb_GetImprovementById_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task Handle_Should_Return_Improvement_When_It_Exists()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var improvement = new Domain.Entities.Improvement
            {
                Id = 1,
                Name = "Terraza",
                Description = "Espacio abierto"
            };
            context.Improvements.Add(improvement);
            await context.SaveChangesAsync();

            var repository = new ImprovementRepository(context);
            var handler = new GetImprovementByIdQueryHandler(repository);

            var query = new GetImprovementByIdQuery { Id = 1 };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(1);
            result.Name.Should().Be("Terraza");
        }

        [Fact]
        public async Task Handle_Should_Return_Null_When_Improvement_NotExists()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var repository = new ImprovementRepository(context);
            var handler = new GetImprovementByIdQueryHandler(repository);

            var query = new GetImprovementByIdQuery { Id = 999 };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }
    }
}

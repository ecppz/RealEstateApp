using Application.Features.Improvement.Queries.GetAll;
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
    public class GetAllImprovementsQueryHandlerTests
    {
        private readonly DbContextOptions<RealEstateAppContext> _dbOptions;

        public GetAllImprovementsQueryHandlerTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealEstateAppContext>()
                .UseInMemoryDatabase($"TestDb_GetAllImprovements_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task Handle_Should_Return_List_When_Improvements_Exist()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var improvements = new List<Domain.Entities.Improvement>
        {
            new Domain.Entities.Improvement { Id = 1, Name = "Piscina", Description = "Construcción de piscina" },
            new Domain.Entities.Improvement { Id = 2, Name = "Jardín", Description = "Área verde" }
        };
            context.Improvements.AddRange(improvements);
            await context.SaveChangesAsync();

            var repository = new ImprovementRepository(context);
            var handler = new GetAllImprovementsQueryHandler(repository);

            var query = new GetAllImprovementsQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.First().Name.Should().Be("Piscina");
            result.Last().Name.Should().Be("Jardín");
        }

        [Fact]
        public async Task Handle_Should_Return_Empty_List_When_No_Improvements()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var repository = new ImprovementRepository(context);
            var handler = new GetAllImprovementsQueryHandler(repository);

            var query = new GetAllImprovementsQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}

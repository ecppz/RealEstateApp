using Application.Features.PropertyType.Queries.GetById;
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
    public class GetPropertyTypeByIdQueryHandlerTests
    {
        private readonly DbContextOptions<RealEstateAppContext> _dbOptions;

        public GetPropertyTypeByIdQueryHandlerTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealEstateAppContext>()
                .UseInMemoryDatabase($"TestDb_GetPropertyTypeById_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task Handle_Should_Return_PropertyType_When_It_Exists()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var propertyType = new Domain.Entities.PropertyType
            {
                Id = 1,
                Name = "Residencial",
                Description = "Casa o apartamento"
            };
            context.PropertyTypes.Add(propertyType);
            await context.SaveChangesAsync();

            var repository = new PropertyTypeRepository(context);
            var handler = new GetPropertyTypeByIdQueryHandler(repository);

            var query = new GetPropertyTypeByIdQuery { Id = 1 };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(1);
            result.Name.Should().Be("Residencial");
        }

        [Fact]
        public async Task Handle_Should_Return_Null_When_PropertyType_NotExists()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var repository = new PropertyTypeRepository(context);
            var handler = new GetPropertyTypeByIdQueryHandler(repository);

            var query = new GetPropertyTypeByIdQuery { Id = 999 };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }
    }
}

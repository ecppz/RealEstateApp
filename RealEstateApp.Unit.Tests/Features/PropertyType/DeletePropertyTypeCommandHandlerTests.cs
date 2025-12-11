using Application.Features.PropertyType.Commands.Delete;
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
    public class DeletePropertyTypeCommandHandlerTests
    {
        private readonly DbContextOptions<RealEstateAppContext> _dbOptions;

        public DeletePropertyTypeCommandHandlerTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealEstateAppContext>()
                .UseInMemoryDatabase($"TestDb_DeletePropertyType_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task Handle_Should_Delete_PropertyType_When_It_Exists()
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
            var handler = new DeletePropertyTypeCommandHandler(repository);

            var command = new DeletePropertyTypeCommand { Id = 1 };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(1);
            var deleted = await context.PropertyTypes.FindAsync(1);
            deleted.Should().BeNull();
        }
    }
}

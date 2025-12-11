
using FluentAssertions;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;


namespace RealEstateApp.Unit.Tests.Features.PropertyType
{
    public class CreatePropertyTypeHandlerTests
    {
        private readonly DbContextOptions<RealEstateAppContext> _dbOptions;

        public CreatePropertyTypeHandlerTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealEstateAppContext>()
                .UseInMemoryDatabase($"TestDb_CreatePropertyType_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task Handle_Should_Create_PropertyType_When_Command_Is_Valid()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var repository = new PropertyTypeRepository(context);
            var handler = new CreatePropertyTypeCommandHandler(repository);

            var command = new CreatePropertyTypeCommand
            {
                Name = "Residencial",
                Description = "Propiedad de uso habitacional"
            };

            // Act
            var resultId = await handler.Handle(command, CancellationToken.None);

            // Assert
            resultId.Should().BeGreaterThan(0);
            var created = await context.PropertyTypes.FindAsync(resultId);
            created.Should().NotBeNull();
            created!.Name.Should().Be("Residencial");
            created.Description.Should().Be("Propiedad de uso habitacional");
        }
    }
}

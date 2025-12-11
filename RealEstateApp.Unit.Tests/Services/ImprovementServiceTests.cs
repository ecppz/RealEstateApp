using Application.Dtos.Improvement;
using Application.Mappings.EntitiesAndDtos;
using Application.Services;
using AutoMapper;
using FluentAssertions;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace RealEstateApp.Unit.Tests.Services
{
    public class ImprovementServiceTests
    {
        private readonly DbContextOptions<RealEstateAppContext> _dbOptions;
        private readonly IMapper _mapper;

        public ImprovementServiceTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealEstateAppContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_ImprovementService_{Guid.NewGuid()}")
                .Options;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ImprovementMappingProfile>();
            });
            _mapper = config.CreateMapper();
        }

        private ImprovementService CreateService()
        {
            var context = new RealEstateAppContext(_dbOptions);
            var repo = new ImprovementRepository(context);
            return new ImprovementService(repo, _mapper);
        }

        [Fact]
        public async Task AddAsync_Should_Return_Added_Dto_When_Valid()
        {
            // Arrange
            var service = CreateService();
            var dto = new ImprovementCreateDto { Name = "Piscina", Description = "Construcción de piscina" };

            // Act
            var result = await service.AddAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().BeGreaterThan(0);
            result.Name.Should().Be("Piscina");
        }

        [Fact]
        public async Task GetById_Should_Return_Dto_When_Exists()
        {
            // Arrange
            var service = CreateService();
            var added = await service.AddAsync(new ImprovementCreateDto { Name = "Jardín", Description = "Área verde" });

            // Act
            var result = await service.GetById(added!.Id);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("Jardín");
        }

        [Fact]
        public async Task GetById_Should_Return_Null_When_NotExists()
        {
            // Arrange
            var service = CreateService();

            // Act
            var result = await service.GetById(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllList_Should_Return_List_When_Exists()
        {
            // Arrange
            var service = CreateService();
            await service.AddAsync(new ImprovementCreateDto { Name = "Piscina", Description = "Construcción de piscina" });
            await service.AddAsync(new ImprovementCreateDto { Name = "Terraza", Description = "Espacio abierto" });

            // Act
            var result = await service.GetAllList();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetAllList_Should_Return_Empty_When_None()
        {
            // Arrange
            var service = CreateService();

            // Act
            var result = await service.GetAllList();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_When_Exists()
        {
            // Arrange
            var service = CreateService();
            var added = await service.AddAsync(new ImprovementCreateDto { Name = "Garaje", Description = "Espacio para autos" });

            var dto = new ImprovementUpdateDto { Name = "Garaje Actualizado", Description = "Espacio techado para autos" };

            // Act
            var updated = await service.UpdateAsync(added!.Id, dto);

            // Assert
            updated.Should().NotBeNull();
            updated!.Name.Should().Be("Garaje Actualizado");
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_Null_When_NotExists()
        {
            // Arrange
            var service = CreateService();
            var dto = new ImprovementUpdateDto { Name = "Fantasma", Description = "No existe" };

            // Act
            var result = await service.UpdateAsync(999, dto);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Entity_When_Exists()
        {
            // Arrange
            var service = CreateService();
            var added = await service.AddAsync(new ImprovementCreateDto { Name = "Eliminar", Description = "Para borrar" });

            // Act
            await service.DeleteAsync(added!.Id);

            // Assert
            var result = await service.GetById(added.Id);
            result.Should().BeNull();
        }
    }
}

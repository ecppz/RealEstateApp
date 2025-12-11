using Application.Dtos.PropertyType;
using Application.Mappings.EntitiesAndDtos;
using Application.Services;
using AutoMapper;
using FluentAssertions;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Unit.Tests.Services
{
    public class PropertyTypeServiceTests
    {
        private readonly DbContextOptions<RealEstateAppContext> _dbOptions;
        private readonly IMapper _mapper;

        public PropertyTypeServiceTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealEstateAppContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_PropertyTypeService_{Guid.NewGuid()}")
                .Options;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PropertyTypeMappingProfile>();
            });
            _mapper = config.CreateMapper();
        }

        private PropertyTypeService CreateService()
        {
            var context = new RealEstateAppContext(_dbOptions);
            var repo = new PropertyTypeRepository(context);
            return new PropertyTypeService(repo, _mapper);
        }

        [Fact]
        public async Task AddPropertyAsync_Should_Return_Added_Dto_When_Valid()
        {
            // Arrange
            var service = CreateService();
            var dto = new PropertyTypeCreateDto { Name = "Residencial", Description = "Casa o apartamento" };

            // Act
            var result = await service.AddPropertyAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().BeGreaterThan(0);
            result.Name.Should().Be("Residencial");
        }

        [Fact]
        public async Task AddPropertyAsync_Should_Return_Null_When_Invalid()
        {
            // Arrange
            var service = CreateService();
            var dto = new PropertyTypeCreateDto { Name = "", Description = "" };

            // Act
            var result = await service.AddPropertyAsync(dto);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddPropertyAsync_Should_Return_Null_When_Duplicate()
        {
            // Arrange
            var service = CreateService();
            await service.AddPropertyAsync(new PropertyTypeCreateDto { Name = "Residencial", Description = "Casa" });

            var dto = new PropertyTypeCreateDto { Name = "Residencial", Description = "Duplicado" };

            // Act
            var result = await service.AddPropertyAsync(dto);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdatePropertyAsync_Should_Update_When_Exists()
        {
            // Arrange
            var service = CreateService();
            var added = await service.AddPropertyAsync(new PropertyTypeCreateDto { Name = "Comercial", Description = "Locales" });

            var dto = new PropertyTypeUpdateDto { Id = added!.Id, Name = "Comercial Actualizado", Description = "Locales y oficinas" };

            // Act
            var updated = await service.UpdatePropertyAsync(dto, added.Id);

            // Assert
            updated.Should().NotBeNull();
            updated!.Name.Should().Be("Comercial Actualizado");
        }

        [Fact]
        public async Task UpdatePropertyAsync_Should_Return_Null_When_NotExists()
        {
            // Arrange
            var service = CreateService();
            var dto = new PropertyTypeUpdateDto { Id = 999, Name = "Fantasma", Description = "No existe" };

            // Act
            var result = await service.UpdatePropertyAsync(dto, 999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeletePropertyAsync_Should_Return_True_When_Deleted()
        {
            // Arrange
            var service = CreateService();
            var added = await service.AddPropertyAsync(new PropertyTypeCreateDto { Name = "Temporal", Description = "Para borrar" });

            // Act
            var result = await service.DeletePropertyAsync(added!.Id);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeletePropertyAsync_Should_Return_False_When_NotExists()
        {
            // Arrange
            var service = CreateService();

            // Act
            var result = await service.DeletePropertyAsync(999);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task GetPropertyById_Should_Return_Dto_When_Exists()
        {
            // Arrange
            var service = CreateService();
            var added = await service.AddPropertyAsync(new PropertyTypeCreateDto { Name = "Industrial", Description = "Fábricas" });

            // Act
            var result = await service.GetPropertyById(added!.Id);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("Industrial");
        }

        [Fact]
        public async Task GetPropertyById_Should_Return_Null_When_NotExists()
        {
            // Arrange
            var service = CreateService();

            // Act
            var result = await service.GetPropertyById(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllPropertyList_Should_Return_List_When_Exists()
        {
            // Arrange
            var service = CreateService();
            await service.AddPropertyAsync(new PropertyTypeCreateDto { Name = "Residencial", Description = "Casa" });
            await service.AddPropertyAsync(new PropertyTypeCreateDto { Name = "Comercial", Description = "Locales" });

            // Act
            var result = await service.GetAllPropertyList();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetAllPropertyList_Should_Return_Empty_When_None()
        {
            // Arrange
            var service = CreateService();

            // Act
            var result = await service.GetAllPropertyList();

            // Assert
            result.Should().BeEmpty();
        }
    }
}

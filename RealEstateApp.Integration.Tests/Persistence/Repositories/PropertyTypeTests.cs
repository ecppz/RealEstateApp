using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Integration.Tests.Persistence.Repositories
{
    public class PropertyTypeTests
    {
        private readonly DbContextOptions<RealEstateAppContext> _dbOptions;

        public PropertyTypeTests()
        {
            // Cada test usa una base en memoria única para evitar contaminación entre pruebas
            _dbOptions = new DbContextOptionsBuilder<RealEstateAppContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_PropertyTypes_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task AddPropertyAsync_Should_Add_PropertyType_To_Database()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var repository = new PropertyTypeRepository(context);
            var propertyType = new PropertyType { Name = "Casa", Description = "Propiedad residencial" };

            // Act
            var result = await repository.AddPropertyAsync(propertyType);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().BeGreaterThan(0);
            var propertyTypes = await context.PropertyTypes.ToListAsync();
            propertyTypes.Should().ContainSingle();
        }

        [Fact]
        public async Task GetPropertyById_Should_Return_PropertyType_When_Exists()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var propertyType = new PropertyType { Name = "Apartamento", Description = "Propiedad vertical" };
            context.PropertyTypes.Add(propertyType);
            await context.SaveChangesAsync();
            var repository = new PropertyTypeRepository(context);

            // Act
            var result = await repository.GetPropertyById(propertyType.Id);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("Apartamento");
        }

        [Fact]
        public async Task GetPropertyById_Should_Return_Null_When_NotExists()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var repository = new PropertyTypeRepository(context);

            // Act
            var result = await repository.GetPropertyById(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdatePropertyAsync_Should_Update_Name_And_Description()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var propertyType = new PropertyType { Name = "Local", Description = "Comercial" };
            context.PropertyTypes.Add(propertyType);
            await context.SaveChangesAsync();
            var repository = new PropertyTypeRepository(context);

            var updated = new PropertyType { Name = "Local Comercial", Description = "Espacio para negocios" };

            // Act
            var result = await repository.UpdatePropertyAsync(propertyType.Id, updated);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("Local Comercial");
            result.Description.Should().Be("Espacio para negocios");
        }

        [Fact]
        public async Task DeletePropertyAsync_Should_Remove_PropertyType_When_Exists()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var propertyType = new PropertyType { Name = "Terreno", Description = "Solar vacío" };
            context.PropertyTypes.Add(propertyType);
            await context.SaveChangesAsync();
            var repository = new PropertyTypeRepository(context);

            // Act
            await repository.DeletePropertyAsync(propertyType.Id);

            // Assert
            var propertyTypes = await context.PropertyTypes.ToListAsync();
            propertyTypes.Should().BeEmpty();
        }

        [Fact]
        public async Task DeletePropertyAsync_Should_DoNothing_When_NotExists()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var repository = new PropertyTypeRepository(context);

            // Act
            await repository.DeletePropertyAsync(999);

            // Assert
            var propertyTypes = await context.PropertyTypes.ToListAsync();
            propertyTypes.Should().BeEmpty();
        }
    }
}

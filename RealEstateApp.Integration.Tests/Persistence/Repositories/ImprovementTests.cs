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
    public class ImprovementTests
    {
        private readonly DbContextOptions<RealEstateAppContext> _dbOptions;

        public ImprovementTests()
        {
            // Cada test usa una base en memoria única para evitar contaminación entre pruebas
            _dbOptions = new DbContextOptionsBuilder<RealEstateAppContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_Improvements_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task AddAsync_Should_Add_Improvement_To_Database()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var repository = new ImprovementRepository(context);
            var improvement = new Improvement { Name = "Piscina", Description = "Área recreativa con piscina" };

            // Act
            var result = await repository.AddAsync(improvement);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().BeGreaterThan(0);
            var improvements = await context.Improvements.ToListAsync();
            improvements.Should().ContainSingle();
        }

        [Fact]
        public async Task GetById_Should_Return_Improvement_When_Exists()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var improvement = new Improvement { Name = "Jardín", Description = "Espacio verde" };
            context.Improvements.Add(improvement);
            await context.SaveChangesAsync();
            var repository = new ImprovementRepository(context);

            // Act
            var result = await repository.GetById(improvement.Id);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("Jardín");
        }

        [Fact]
        public async Task GetById_Should_Return_Null_When_NotExists()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var repository = new ImprovementRepository(context);

            // Act
            var result = await repository.GetById(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Name_And_Description()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var improvement = new Improvement { Name = "Garaje", Description = "Espacio para vehículos" };
            context.Improvements.Add(improvement);
            await context.SaveChangesAsync();
            var repository = new ImprovementRepository(context);

            var updated = new Improvement { Name = "Garaje Cubierto", Description = "Espacio techado para vehículos" };

            // Act
            var result = await repository.UpdateAsync(improvement.Id, updated);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("Garaje Cubierto");
            result.Description.Should().Be("Espacio techado para vehículos");
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Improvement_When_Exists()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var improvement = new Improvement { Name = "Terraza", Description = "Área exterior" };
            context.Improvements.Add(improvement);
            await context.SaveChangesAsync();
            var repository = new ImprovementRepository(context);

            // Act
            await repository.DeleteAsync(improvement.Id);

            // Assert
            var improvements = await context.Improvements.ToListAsync();
            improvements.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteAsync_Should_DoNothing_When_NotExists()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var repository = new ImprovementRepository(context);

            // Act
            await repository.DeleteAsync(999);

            // Assert
            var improvements = await context.Improvements.ToListAsync();
            improvements.Should().BeEmpty();
        }
    }
}

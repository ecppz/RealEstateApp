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
    public class SaleTypeTests
    {
        private readonly DbContextOptions<RealEstateAppContext> _dbOptions;

        public SaleTypeTests()
        {
            // Cada test usa una base en memoria única para evitar contaminación entre pruebas
            _dbOptions = new DbContextOptionsBuilder<RealEstateAppContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_SaleTypes_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task AddAsync_Should_Add_SaleType_To_Database()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var repository = new SaleTypeRepository(context);
            var saleType = new SaleType { Name = "Venta Directa", Description = "Se vende sin intermediarios" };

            // Act
            var result = await repository.AddAsync(saleType);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().BeGreaterThan(0);
            var saleTypes = await context.SaleTypes.ToListAsync();
            saleTypes.Should().ContainSingle();
        }

        [Fact]
        public async Task GetById_Should_Return_SaleType_When_Exists()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var saleType = new SaleType { Name = "Subasta", Description = "Venta mediante puja" };
            context.SaleTypes.Add(saleType);
            await context.SaveChangesAsync();
            var repository = new SaleTypeRepository(context);

            // Act
            var result = await repository.GetById(saleType.Id);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("Subasta");
        }

        [Fact]
        public async Task GetById_Should_Return_Null_When_NotExists()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var repository = new SaleTypeRepository(context);

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
            var saleType = new SaleType { Name = "Alquiler", Description = "Renta mensual" };
            context.SaleTypes.Add(saleType);
            await context.SaveChangesAsync();
            var repository = new SaleTypeRepository(context);

            var updated = new SaleType { Name = "Alquiler Temporal", Description = "Renta por corto plazo" };

            // Act
            var result = await repository.UpdateAsync(saleType.Id, updated);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("Alquiler Temporal");
            result.Description.Should().Be("Renta por corto plazo");
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_SaleType_When_Exists()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var saleType = new SaleType { Name = "Permuta", Description = "Intercambio de propiedades" };
            context.SaleTypes.Add(saleType);
            await context.SaveChangesAsync();
            var repository = new SaleTypeRepository(context);

            // Act
            await repository.DeleteAsync(saleType.Id);

            // Assert
            var saleTypes = await context.SaleTypes.ToListAsync();
            saleTypes.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteAsync_Should_DoNothing_When_NotExists()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var repository = new SaleTypeRepository(context);

            // Act
            await repository.DeleteAsync(999);

            // Assert
            var saleTypes = await context.SaleTypes.ToListAsync();
            saleTypes.Should().BeEmpty();
        }
    }
}

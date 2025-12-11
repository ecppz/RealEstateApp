using Application.Dtos.SaleType;
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
    public class SaleTypeServiceTests
    {
        private readonly DbContextOptions<RealEstateAppContext> _dbOptions;
        private readonly IMapper _mapper;

        public SaleTypeServiceTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealEstateAppContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_SaleTypeService_{Guid.NewGuid()}")
                .Options;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<SaleTypeMappingProfile>();
            });
            _mapper = config.CreateMapper();
        }

        private SaleTypeService CreateService()
        {
            var context = new RealEstateAppContext(_dbOptions);
            var repo = new SaleTypeRepository(context);
            // IBaseAccountService no se usa en los métodos, así que podemos pasar null
            return new SaleTypeService(repo, _mapper, null!);
        }

        [Fact]
        public async Task AddAsync_Should_Return_Added_Dto_When_Valid()
        {
            // Arrange
            var service = CreateService();
            var dto = new SaleTypeCreateDto { Name = "Venta Directa", Description = "Sin intermediarios" };

            // Act
            var result = await service.AddAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().BeGreaterThan(0);
            result.Name.Should().Be("Venta Directa");
        }

        [Fact]
        public async Task GetById_Should_Return_Dto_When_Exists()
        {
            // Arrange
            var service = CreateService();
            var added = await service.AddAsync(new SaleTypeCreateDto { Name = "Subasta", Description = "Venta mediante puja" });

            // Act
            var result = await service.GetById(added!.Id);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("Subasta");
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
            await service.AddAsync(new SaleTypeCreateDto { Name = "Venta Directa", Description = "Sin intermediarios" });
            await service.AddAsync(new SaleTypeCreateDto { Name = "Permuta", Description = "Intercambio de propiedades" });

            // Act
            var result = await service.GetAllList();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Any(r => r.Name == "Venta Directa").Should().BeTrue();
            result.Any(r => r.Name == "Permuta").Should().BeTrue();
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
            var added = await service.AddAsync(new SaleTypeCreateDto { Name = "Temporal", Description = "Para actualizar" });

            var dto = new SaleTypeUpdateDto { Name = "Temporal Actualizado", Description = "Actualizado correctamente" };

            // Act
            var updated = await service.UpdateAsync(dto, added!.Id);

            // Assert
            updated.Should().NotBeNull();
            updated!.Name.Should().Be("Temporal Actualizado");
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_Null_When_NotExists()
        {
            // Arrange
            var service = CreateService();
            var dto = new SaleTypeUpdateDto { Name = "Fantasma", Description = "No existe" };

            // Act
            var result = await service.UpdateAsync(dto, 999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_Should_Return_True_When_Deleted()
        {
            // Arrange
            var service = CreateService();
            var added = await service.AddAsync(new SaleTypeCreateDto { Name = "Eliminar", Description = "Para borrar" });

            // Act
            var result = await service.DeleteSaleTypeAsync(added!.Id);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_Should_Return_False_When_NotExists()
        {
            // Arrange
            var service = CreateService();

            // Act
            var result = await service.DeleteSaleTypeAsync(999);

            // Assert
            result.Should().BeFalse();
        }
    }
}

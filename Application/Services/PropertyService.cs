using Application.Dtos.Property;
using Application.Interfaces;
using AutoMapper;
using Domain.Common.Enums;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class PropertyService : GenericService<Property, PropertyDto>, IPropertyService
    {
        private readonly IMapper mapper;
        private readonly IPropertyRepository propertyRepository;
        private readonly IBaseAccountService accountService;
        public PropertyService(IPropertyRepository propertyRepository, IBaseAccountService accountService, IMapper mapper) : base(propertyRepository, mapper)
        {
            this.propertyRepository = propertyRepository;
            this.accountService = accountService;
            this.mapper = mapper;
        }

        public async Task<List<PropertyDto>> GetProperties(string agentId, bool onlyAvailable)
        {
            var entities = await propertyRepository.GetPropertiesByAgentAsync(agentId, onlyAvailable);
            return mapper.Map<List<PropertyDto>>(entities);
        }

        public async Task<PropertyDto?> GetPropertyById(int id)
        {
            var entity = await propertyRepository.GetPropertyByIdAsync(id);
            if (entity == null)
                return null;

            return mapper.Map<PropertyDto>(entity);
        }

        public async Task<PropertyDto?> AddPropertyAsync(CreatePropertyDto dto)
        {
            dto.Code = await GenerateUniqueCodeAsync();

            var entity = mapper.Map<Property>(dto);

            if (dto.Improvements.Any())
            {
                entity.Improvements = dto.Improvements
                    .Select(i => new PropertyImprovement
                    {
                        Id = 0,
                        PropertyId = entity.Id,
                        ImprovementId = i.ImprovementId
                    })
                    .ToList();
            }

            if (dto.Images.Any())
            {
                entity.Images = dto.Images
                    .Select(img => new PropertyImage
                    {
                        Id = 0,
                        PropertyId = entity.Id,
                        ImageUrl = img.ImageUrl
                    })
                    .ToList();
            }

            await propertyRepository.AddAsync(entity);
            return mapper.Map<PropertyDto>(entity);
        }

        public async Task<bool> DeletePropertyAsync(int id)
        {
            return await propertyRepository.DeletePropertyAsync(id);
        }


        //private methods
        private async Task<string> GenerateUniqueCodeAsync()
        {
            string code;
            bool exists;

            do
            {
                code = new Random().Next(0, 999999).ToString("D6");
                exists = await propertyRepository.ExistsByCodeAsync(code);

            } while (exists);

            return code;
        }

        // 🔹Nuevo método para clientes
        public async Task<List<PropertyDto>> GetAvailablePropertiesAsync()
        {
           
            var properties = await propertyRepository.GetPropertiesByAgentAsync(agentId: null, onlyAvailable: true);

            return mapper.Map<List<PropertyDto>>(properties);
        }



    }
}

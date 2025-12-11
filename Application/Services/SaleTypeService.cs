using Application.Dtos.SaleType;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class SaleTypeService : GenericService<SaleType, SaleTypeDto>, ISaleTypeService
    {
        private readonly ISaleTypeRepository saleTypeRepository;
        private readonly IMapper mapper;

        public SaleTypeService(ISaleTypeRepository saleTypeRepository, IMapper mapper, IBaseAccountService accountService)
            : base(saleTypeRepository, mapper)
        {
            this.saleTypeRepository = saleTypeRepository;
            this.mapper = mapper;
        }

        public async Task<SaleTypeDto?> AddAsync(SaleTypeCreateDto dto)
        {
            var entity = mapper.Map<SaleType>(dto);
            var result = await saleTypeRepository.AddAsync(entity);
            return mapper.Map<SaleTypeDto>(result);
        }

        public async Task<SaleTypeDto?> GetById(int id)
        {
            var entity = await saleTypeRepository.GetById(id);
            return mapper.Map<SaleTypeDto>(entity);
        }

        public async Task<List<SaleTypeListDto>> GetAllList()
        {
            var entities = await saleTypeRepository.GetAllList();
            var list = mapper.Map<List<SaleTypeListDto>>(entities);

            // aseguramos conteo de propiedades
            foreach (var item in list)
            {
                var saleTypeEntity = entities.FirstOrDefault(st => st.Id == item.Id);
                item.PropertyCount = saleTypeEntity?.Properties?.Count ?? 0;
            }

            return list;
        }

        public async Task<SaleTypeDto?> UpdateAsync(SaleTypeUpdateDto dto, int id)
        {
            var entity = mapper.Map<SaleType>(dto);
            var updated = await saleTypeRepository.UpdateAsync(id, entity);
            return mapper.Map<SaleTypeDto>(updated);
        }

        public async Task<bool> DeleteSaleTypeAsync(int id)
        {
            var existing = await saleTypeRepository.GetById(id);
            if (existing == null)
                return false;

            await saleTypeRepository.DeleteAsync(id);
            return true;
        }



        //  public async Task DeleteAsync(int id)
        //  {
        //      //Eliminar propiedades asociadas antes de borrar el tipo de venta
        ////      var properties = await propertyRepository.GetAllList();
        //  //    var toDelete = properties.Where(p => p.SaleTypeId == id).ToList();

        //      if (toDelete.Any())
        //      {
        //          foreach (var property in toDelete)
        //          {
        //     //         await propertyRepository.DeleteAsync(property.Id);
        //          }
        //      }

        //      await saleTypeRepository.DeleteAsync(id);
        //  }


    }
}

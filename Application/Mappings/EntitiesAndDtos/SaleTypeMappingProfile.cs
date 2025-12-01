using Application.Dtos.SaleType;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings.EntitiesAndDtos
{
    public class SaleTypeMappingProfile : Profile
    {
        public SaleTypeMappingProfile()
        {
            // Entidad ↔ DTO general
            CreateMap<SaleType, SaleTypeDto>()
                .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => src.Properties))
                .ReverseMap();

            // CreateDto → Entidad
            CreateMap<SaleTypeCreateDto, SaleType>();

            // UpdateDto → Entidad
            CreateMap<SaleTypeUpdateDto, SaleType>();

            // Entidad ↔ ListDto
            CreateMap<SaleType, SaleTypeListDto>()
                .ForMember(dest => dest.PropertyCount, opt => opt.MapFrom(src => src.Properties != null ? src.Properties.Count : 0))
                .ReverseMap();
        }
    }
}

using Application.Dtos.Offer;
using Application.ViewModels.Offer;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings.EntitiesAndDtos
{
    public class OfferMappingProfile : Profile
    {
        public OfferMappingProfile()
        {
            // Entity ↔ DTO
            CreateMap<Offer, OfferDto>()
                .ForMember(dest => dest.Property, opt => opt.MapFrom(src => src.Property))
                .ReverseMap();

            // Crear oferta (cliente) → Entity
            CreateMap<CreateOfferDto, Offer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // EF Core lo genera
                .ForMember(dest => dest.Date, opt => opt.Ignore()) // se setea en el servicio
                .ForMember(dest => dest.Status, opt => opt.Ignore()) // se setea en el servicio
                .ReverseMap();

            // Actualizar estado de oferta (agente)
            CreateMap<UpdateOfferStatusDto, Offer>()
                .ForMember(dest => dest.PropertyId, opt => opt.Ignore())
                .ForMember(dest => dest.ClientId, opt => opt.Ignore())
                .ForMember(dest => dest.Amount, opt => opt.Ignore())
                .ForMember(dest => dest.Date, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<OfferDto, OfferViewModel>()
            .ForMember(dest => dest.Property, opt => opt.MapFrom(src => src.Property));

            CreateMap<Offer, OfferViewModel>()
    .ForMember(dest => dest.Property, opt => opt.MapFrom(src => src.Property));




        }
    }

}

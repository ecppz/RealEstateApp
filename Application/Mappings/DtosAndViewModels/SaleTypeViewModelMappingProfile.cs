using Application.Dtos.SaleType;
using Application.ViewModels.SaleType;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings.DtosAndViewModels
{
    public class SaleTypeViewModelMappingProfile : Profile
    {
        public SaleTypeViewModelMappingProfile()
        {
            // Create
            CreateMap<SaleTypeCreateViewModel, SaleTypeCreateDto>().ReverseMap();

            // Edit
            CreateMap<SaleTypeEditViewModel, SaleTypeUpdateDto>().ReverseMap();

            // List
            CreateMap<SaleTypeListViewModel, SaleTypeListDto>().ReverseMap();

            // General
            CreateMap<SaleTypeViewModel, SaleTypeDto>().ReverseMap();

            CreateMap<SaleTypeDto, SaleTypeEditViewModel>().ReverseMap();

            CreateMap<SaleTypeDto, SaleTypeListViewModel>().ReverseMap();


        }
    }
}

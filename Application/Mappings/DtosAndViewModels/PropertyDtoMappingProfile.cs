using Application.Dtos.Property;
using Application.ViewModels.Improvement;
using Application.ViewModels.Property;
using Application.ViewModels.PropertyImage;
using Application.ViewModels.PropertyImprovement;
using AutoMapper;

namespace Application.Mappings.DtosAndViewModels
{
    public class PropertyDtoMappingProfile : Profile
    {
        public PropertyDtoMappingProfile()
        {

            CreateMap<PropertyDto, EditPropertyDto>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ForMember(dest => dest.Improvements, opt => opt.MapFrom(src => src.Improvements));



            CreateMap<EditPropertyDto, PropertyDto>();

            CreateMap<EditPropertyDto, EditPropertyViewModel>().ReverseMap();


            CreateMap<PropertyDto, DeletePropertyViewModel>().ReverseMap();


            CreateMap<PropertyDto, PropertyViewModel>()
                .ForMember(dest => dest.Improvements, opt => opt.MapFrom(src =>
                    src.Improvements.Select(i => new PropertyImprovementViewModel
                    {
                        ImprovementId = i.ImprovementId,
                        PropertyId = i.PropertyId,
                        Improvement = i.Improvement == null ? null : new ImprovementViewModel
                        {
                            Id = i.Improvement.Id,
                            Name = i.Improvement.Name,
                            Description = i.Improvement.Description
                        }
                    }).ToList()))
                .ReverseMap();


            CreateMap<PropertyDto, EditPropertyViewModel>()
                .ForMember(dest => dest.PropertyTypeId, opt => opt.MapFrom(src => src.PropertyTypeId))
                .ForMember(dest => dest.SaleTypeId, opt => opt.MapFrom(src => src.SaleTypeId))
                .ForMember(dest => dest.Improvements, opt => opt.MapFrom(src =>
                    src.Improvements.Select(i => i.ImprovementId).ToList()))

                .ForMember(dest => dest.ExistingImages, opt => opt.MapFrom(src =>
                    src.Images.Select(img => new PropertyImageViewModel
                    {
                        Id = img.Id,
                        PropertyId = img.PropertyId,
                        ImageUrl = img.ImageUrl
                    }).ToList()));

            CreateMap<PropertyDto, PropertyDisplayViewModel>()
                .ForMember(dest => dest.PropertyType, opt => opt.MapFrom(src => src.PropertyType))
                .ForMember(dest => dest.SaleType, opt => opt.MapFrom(src => src.SaleType))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ForMember(dest => dest.Improvements, opt => opt.MapFrom(src =>
                    src.Improvements.Select(i => new PropertyImprovementViewModel
                    {
                        ImprovementId = i.ImprovementId,
                        Improvement = i.Improvement == null ? null : new ImprovementViewModel
                        {
                            Id = i.Improvement.Id,
                            Name = i.Improvement.Name,
                            Description = i.Improvement.Description
                        }
                    }).ToList()))
                .ForMember(dest => dest.IsFavorite, opt => opt.Ignore());


            CreateMap<CreatePropertyDto, CreatePropertyViewModel>().ReverseMap();

        }
    }
}

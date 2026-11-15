using AutoMapper;
using MainHW.Models;
using MainHW.Models.DTO;
namespace MainHW.Models.Mapper;

public class CategoryDtoToCategoryMapperProfile : Profile
{
    public CategoryDtoToCategoryMapperProfile()
    {
        CreateMap<CategoryDto, Category>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

    }
}

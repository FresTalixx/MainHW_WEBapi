using AutoMapper;
using MainHW.Models.DTO;

namespace MainHW.Models.Mapper;

public class OrderDtoToOrderMapperProfile : Profile
{
    public OrderDtoToOrderMapperProfile()
    {
        CreateMap<OrderDto, Order>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => src.DateTime))
            .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => src.IsCompleted))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.TotalCost, opt => opt.MapFrom(src => src.TotalCost));
    }
}

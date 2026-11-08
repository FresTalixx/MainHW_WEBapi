using AutoMapper;
using MainHW.Models.DTO;
namespace MainHW.Models.Mapper;

public class MessageAddRequestToMessageMapperProfile : Profile
{
    public MessageAddRequestToMessageMapperProfile()
    {
        CreateMap<MessageAddRequest, Message>()
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
            .ForMember(dest => dest.From, opt => opt.MapFrom(src => src.From));
    }
}

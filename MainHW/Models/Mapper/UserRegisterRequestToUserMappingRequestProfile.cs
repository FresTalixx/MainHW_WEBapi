using AutoMapper;
using MainHW.Models.DTO;

namespace MainHW.Models.Mapper;

public class UserRegisterRequestToUserMappingRequestProfile : Profile
{
    public UserRegisterRequestToUserMappingRequestProfile()
    {
        CreateMap<UserRegisterRequest, User>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));
    }
}

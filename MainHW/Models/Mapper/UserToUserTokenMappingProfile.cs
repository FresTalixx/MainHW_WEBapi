using AutoMapper;
using MainHW.Models.DTO;

namespace MainHW.Models.Mapper;

public class UserToUserTokenMappingProfile : Profile
{
    public UserToUserTokenMappingProfile()
    {
        CreateMap<User, UserToken>()
            .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.Token));
    }
}

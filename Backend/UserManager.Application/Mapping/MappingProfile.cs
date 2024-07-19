using AutoMapper;
using UserManager.Domain.Entities;
using UserManager.Application.DTO;

namespace UserManager.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();         
        }
    }
}

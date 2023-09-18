using AutoMapper;
using Internal.Contracts.DTOs.UsersDto;
using Internal.Domain.Entities;

namespace Internal.Application.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserWOPass>();
        }
    }
}

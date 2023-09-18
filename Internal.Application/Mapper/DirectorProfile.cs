using AutoMapper;
using Internal.Contracts.DTOs.Directors;
using Internal.Domain.Entities;

namespace Internal.Application.Mapper
{
    public class DirectorProfile : Profile
    {
        public DirectorProfile()
        {
            CreateMap<DirectorCreateRequest, MovieDirector>();
        }
    }
}

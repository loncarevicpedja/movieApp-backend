using AutoMapper;
using Internal.Application.Common.Helpers;
using Internal.Contracts.DTOs.Movie;
using Internal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internal.Application.Mapper
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<MovieCreateRequest, Movie>();

            CreateMap<Movie, MovieResponse>()
                .ForMember(dest => dest.AvgRating, opt => opt.MapFrom(src => src.Ratings.Any() ? src.Ratings.Average(r => r.Value) : 0));

            CreateMap(typeof(PagedList<Movie>), typeof(PagedList<MovieResponse>))
                .ConvertUsing(typeof(PagedListConverter<Movie, MovieResponse>));
        }
    }
}

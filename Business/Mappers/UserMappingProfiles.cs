using AutoMapper;
using GoodJobGames.Models.Requests;
using GoodJobGames.Models.Responses;
using GoodJobGames.Data.EntityModels;

namespace GoodJobGames.Business.Mappers
{
    public class UserMappingProfiles : Profile
    {
        public UserMappingProfiles()
        {
            base.CreateMap<UserRequest, User>().ReverseMap();
            base.CreateMap<UserResponse, User>().ReverseMap()
                .ForMember(destination => destination.Score,
               opts => opts.MapFrom(source => source.Score.Score))
                 .ForMember(destination => destination.CountryIsoCode,
               opts => opts.MapFrom(source => source.Country.CountryIsoCode));

            base.CreateMap<UserResponse, ScoreRequest>().ReverseMap();
        }
    }
}
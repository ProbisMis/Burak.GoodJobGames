using AutoMapper;
using GoodJobGames.Models.Requests;
using GoodJobGames.Models.Responses;
using GoodJobGames.Data.EntityModels;

namespace GoodJobGames.Business.Mappers
{
    public class ScoreMappingProfiles : Profile
    {
        public ScoreMappingProfiles()
        {
            base.CreateMap<ScoreRequest, Score>().ReverseMap();
            base.CreateMap<ScoreResponse, Score>().ReverseMap();
        }
    }
}
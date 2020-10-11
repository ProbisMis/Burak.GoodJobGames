using AutoMapper;
using Burak.GoodJobGames.Models.Requests;
using Burak.GoodJobGames.Models.Responses;
using Burak.GoodJobGames.Data.EntityModels;

namespace Burak.GoodJobGames.Business.Mappers
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
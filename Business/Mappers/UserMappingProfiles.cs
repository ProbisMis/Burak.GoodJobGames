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
            /* Appointment Mappers */
            //CreateMap<AppointmentRequest, Data.EntityModels.Appointment>()
            //    .ForMember( x => x.Type, opt => opt.Ignore())
            //    .ForMember(x => x.Status, opt => opt.Ignore())
            //    .ForMember(x => x.Slot, opt => opt.Ignore());
            base.CreateMap<UserRequest, User>().ReverseMap();
            base.CreateMap<UserResponse, User>().ReverseMap().ForMember(destination => destination.Score,
               opts => opts.MapFrom(source => source.Score.UserScore));

            base.CreateMap<UserResponse, ScoreRequest>().ReverseMap();


            //CreateMap<Data.EntityModels.Appointment, AppointmentResponse>();
        }
    }
}
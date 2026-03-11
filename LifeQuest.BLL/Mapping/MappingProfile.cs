using AutoMapper;
using LifeQuest.BLL.DTOs;
using LifeQuest.DAL.Models;

namespace LifeQuest.BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            
             CreateMap<DailyLog, DailyLogDTO>()
            .ForMember(dest => dest.ChallengeName, opt => opt.MapFrom(src => src.Challenge.Title));

            
             CreateMap<DailyLogDTO, DailyLog>()
            .ForMember(dest => dest.Challenge, opt => opt.Ignore()); 


        }
    }
}

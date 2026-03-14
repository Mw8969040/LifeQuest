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

            CreateMap<UserChallenge, UserChallengeDTO>()
                .ForMember(dest => dest.ChallengeName, opt => opt.MapFrom(src => src.Challenge.Title));

            CreateMap<UserChallengeDTO, UserChallenge>()
                .ForMember(dest => dest.Challenge , opt => opt.Ignore());

            // Category Mappings
            CreateMap<Category, CategoryDto>().ReverseMap();

            // Challenge Mappings
            CreateMap<Challenge, ChallengeDto>().ReverseMap();

            // UserBadge Mappings
            CreateMap<UserBadge, UserBadgeDTO>()
                .ForMember(dest => dest.BadgeName, opt => opt.MapFrom(src => src.Badge.Name));
            CreateMap<UserBadgeDTO, UserBadge>()
                .ForMember(dest => dest.Badge, opt => opt.Ignore());

            // Badge Mappings
            CreateMap<Badges, BadgeDTO>().ReverseMap();

            // Level Mappings
            CreateMap<Level, LevelDto>().ReverseMap();

            // Decision Mappings
            CreateMap<Decision, DecisionDto>().ReverseMap();

            // Metrics Mappings
            CreateMap<MetricsCalc, MetricsCalcDto>().ReverseMap();
        }
    }
}

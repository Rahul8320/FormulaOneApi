using AutoMapper;
using FormulaOne.Api.Models.Requests;
using FormulaOne.Entities.DbSet;

namespace FormulaOne.Api.MappingProfiles;

public class RequestToDomain : Profile
{
    public RequestToDomain()
    {
        CreateMap<CreateDriverAchievementRequest, Achievement>()
            .ForMember(dest => dest.RaceWins, opt => opt.MapFrom(src => src.Wins))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => 1))
            .ForMember(dest => dest.AddedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<UpdateDriverAchievementRequest, Achievement>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AchievementId))
            .ForMember(dest => dest.RaceWins, opt => opt.MapFrom(src => src.Wins))
            .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<CreateDriverRequest, Driver>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => 1))
            .ForMember(dest => dest.AddedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<UpdateDriverRequest, Driver>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.DriverId))
            .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
    }
}

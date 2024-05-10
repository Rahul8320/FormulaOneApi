using AutoMapper;
using FormulaOne.Api.Models.Responses;
using FormulaOne.Entities.DbSet;

namespace FormulaOne.Api.MappingProfiles;

public class DomainToResponse : Profile
{
    public DomainToResponse()
    {
        CreateMap<Achievement, DriverAchievementResponse>()
            .ForMember(dest => dest.AchievementId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Wins, opt => opt.MapFrom(src => src.RaceWins));

        CreateMap<Driver, GetDriverResponse>()
            .ForMember(dest => dest.DriverId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToString("dd-MM-yyyy")));
    }
}

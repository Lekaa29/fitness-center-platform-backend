using AutoMapper;
using FitnessCenterApi.Dtos;
using FitnessCenterApi.Dtos.Coach;
using FitnessCenterApi.Models;

namespace FitnessCenterApi.Mappings;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Attendance, AttendanceDto>().ReverseMap();
        CreateMap<FitnessCentar, FitnessCenterDto>().ReverseMap();
        CreateMap<Membership, MembershipDto>().ReverseMap();
        CreateMap<Coach, CoachDto>().ReverseMap();
        CreateMap<CoachProgram, CoachProgramDto>().ReverseMap();

    }
}
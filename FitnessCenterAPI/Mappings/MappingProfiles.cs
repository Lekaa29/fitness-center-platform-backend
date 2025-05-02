using AutoMapper;
using FitnessCenterApi.Dtos;
using FitnessCenterApi.Models;

namespace FitnessCenterApi.Mappings;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Attendance, AttendanceDto>().ReverseMap();
        CreateMap<FitnessCentar, FitnessCenterDto>().ReverseMap();
        CreateMap<Membership, MembershipDto>().ReverseMap();
    }
}
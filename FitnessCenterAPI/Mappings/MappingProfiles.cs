using System.Text.RegularExpressions;
using AutoMapper;
using FitnessCenterApi.Dtos;
using FitnessCenterApi.Dtos.Chat;
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
        CreateMap<UserConversation, UserConversationDto>().ReverseMap();
        CreateMap<Conversation, ConversationDto>().ReverseMap();
        CreateMap<Message, MessageDto>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<Group, GroupDto>().ReverseMap();
        CreateMap<ShopItem, ShopItemDto>().ReverseMap();

    }
}
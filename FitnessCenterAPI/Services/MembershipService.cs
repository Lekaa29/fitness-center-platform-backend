using AutoMapper;
using FitnessCenterApi.Dtos;
using FitnessCenterApi.Models;
using FitnessCenterApi.Repositories;
using FitnessCenterApi.Repositories.UserRepositories;

namespace FitnessCenterApi.Services;

public class MembershipService
{
    private readonly IMapper _mapper;
    private readonly MembershipRepository _memberShipRepository;
    private readonly UserRepository _userRepository;
    private readonly FitnessCenterRepository _fitnessCenterRepository;

    public MembershipService(IMapper mapper, MembershipRepository memberShipRepository, UserRepository userRepository, FitnessCenterRepository fitnessCenterRepository)
    {
        _mapper = mapper;
        _memberShipRepository = memberShipRepository;
        _userRepository = userRepository;
        _fitnessCenterRepository = fitnessCenterRepository;
    }

    public async Task<ICollection<MembershipDto>?> GetUserMembershipsAsync(string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return null;
        }
        var memberships = await _memberShipRepository.GetUserMembershipsAsync(user.Id);

        var membershipsDtos = _mapper.Map<ICollection<MembershipDto>>(memberships);
        
        return membershipsDtos;
    }
    
    public async Task<MembershipDto?> GetUserMembershipByFitnessCenterAsync(int fitnessCenterId, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return null;
        }
        var membership = await _memberShipRepository.GetUserMembershipByFitnessCenterAsync(user.Id, fitnessCenterId);

        var membershipDtos = _mapper.Map<MembershipDto>(membership);
        
        return membershipDtos;
    }
    
    public async Task<ICollection<MembershipDto>?> GetFitnessCenterMembershipsAsync(int fitnessCenterId, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return null;
        }
        var memberships = await _memberShipRepository.GetFitnessCenterMembershipsAsync(fitnessCenterId);

        var membershipsDtos = _mapper.Map<ICollection<MembershipDto>>(memberships);
        
        return membershipsDtos;
    }
    
    public async Task<bool> AddMembershipAsync(MembershipDto membershipDto, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return false;
        }
        var membership = _mapper.Map<Membership>(membershipDto);

        membership.IdFitnessCentarNavigation = await _fitnessCenterRepository.GetFitnessCenterAsync(membershipDto.IdFitnessCentar);
        membership.IdUserNavigation = user;

        return await _memberShipRepository.AddMembershipAsync(membership);
    }
    
    
}
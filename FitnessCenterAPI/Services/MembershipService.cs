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
    private readonly IConfiguration _configuration;


    public MembershipService(IMapper mapper, IConfiguration configuration, MembershipRepository memberShipRepository, UserRepository userRepository, FitnessCenterRepository fitnessCenterRepository)
    {
        _mapper = mapper;
        _memberShipRepository = memberShipRepository;
        _userRepository = userRepository;
        _fitnessCenterRepository = fitnessCenterRepository;
        _configuration = configuration;
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


        foreach (var membership in membershipsDtos)
        {
            var fitnessCentar = await _fitnessCenterRepository.GetFitnessCenterAsync(membership.IdFitnessCentar);
            membership.FitnessCentarLogoUrl = fitnessCentar.LogoUrl;
            membership.FitnessCentarBannerUrl = fitnessCentar.BannerUrl;
            membership.FitnessCentarName = fitnessCentar.Name;
        }
        
        
        
        
        
        
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
    
    public async Task<ICollection<MembershipDto>?> GetFitnessCenterLeaderboardAsync(int fitnessCenterId, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return null;
        }
        var memberships = await _memberShipRepository.GetFitnessCenterLeaderboardAsync(fitnessCenterId);

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
        
        membership.MembershipDeadline = DateTime.Now.AddDays(31);

        return await _memberShipRepository.AddMembershipAsync(membership);
    }
    
    public async Task<bool> AddMembershipPackageAsync(MembershipPackageDto membershipPackageDto, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return false;
        }
        var membershipPackage = _mapper.Map<MembershipPackage>(membershipPackageDto);
        
        membershipPackage.FitnessCentar = await _fitnessCenterRepository.GetFitnessCenterAsync(membershipPackageDto.IdFitnessCentar);
        membershipPackage.IdFitnessCentar = membershipPackage.FitnessCentar.IdFitnessCentar;
        

        return await _memberShipRepository.AddMembershipPackageAsync(membershipPackage);
    }
    
    public async Task<bool> UpdateMembershipAsync(MembershipDto membershipDto, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return false;
        }

        if (membershipDto.IdMembershipPackage == null)
        {
            return false;
        }
        var membershipPackage = await _memberShipRepository.GetFitnessCenterMembershipPackageAsync(membershipDto.IdMembershipPackage);
        
        
        var existingMembership =
           await _memberShipRepository.GetUserMembershipByFitnessCenterAsync(user.Id, membershipDto.IdFitnessCentar);
        if (existingMembership == null)
        {
            var membership = _mapper.Map<Membership>(membershipDto);

            membership.IdFitnessCentarNavigation = await _fitnessCenterRepository.GetFitnessCenterAsync(membershipDto.IdFitnessCentar);
            membership.IdUserNavigation = user;
        
            membership.MembershipDeadline = DateTime.Now.AddDays(31);

            return await _memberShipRepository.AddMembershipAsync(membership);
        }
        existingMembership.MembershipDeadline = DateTime.Now.AddDays(membershipPackage.Days);
        
        
        return await _memberShipRepository.UpdateMembershipAsync(existingMembership);
    }

    public async Task<MembershipPackageDto> GetMembershipPackageAsync(int idMembershipPackage, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return null;
        }
        var membershipPackage = await _memberShipRepository.GetFitnessCenterMembershipPackageAsync(idMembershipPackage);

        var membershipPackageDto = _mapper.Map<MembershipPackageDto>(membershipPackage);
        
        return membershipPackageDto;
    }
    
    public async Task<ICollection<MembershipPackageDto>> GetMembershipPackagesAsync(int fitnessCenterId, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return new List<MembershipPackageDto>();
        }

        var membershipPackages = await _memberShipRepository.GetFitnessCenterMembershipPackagesAsync(fitnessCenterId);

        var membershipPackageDtos = _mapper.Map<ICollection<MembershipPackageDto>>(membershipPackages);

        for (int i = 0; i < membershipPackages.Count; i++)
        {
            var fitnessCenter = membershipPackages.ElementAt(i).FitnessCentar;
            membershipPackageDtos.ElementAt(i).FitnessCentarName = fitnessCenter != null ? fitnessCenter.Name : "Nepoznato";
        }


        return membershipPackageDtos;
    }

    public async Task<bool> UpdateMembershipAsync(int membershipId, MembershipDto membershipDto, string email)
    {
        var membership = await _memberShipRepository.GetMembershipAsync(membershipId);
        if (membership == null)
        {
            return false; // Not found
        }
        if (email != _configuration["AdminSettings:AdminEmail"])
        {
            return false; // Only admin can update
        }

        _mapper.Map(membershipDto, membership);

        return await _memberShipRepository.UpdateMembershipAsync(membership);
    }

    public async Task<bool> DeleteMembershipAsync(int membershipId, string email)
    {
        var membership = await _memberShipRepository.GetMembershipAsync(membershipId);
        if (membership == null)
        {
            return false; // Not found
        }
        if (email != _configuration["AdminSettings:AdminEmail"])
        {
            return false; // Only admin can delete
        }

        return await _memberShipRepository.DeleteMembershipAsync(membership);
    }

    
    
}
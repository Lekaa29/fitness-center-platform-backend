using AutoMapper;
using FitnessCenterApi.Dtos;
using FitnessCenterApi.Dtos.Chat;
using FitnessCenterApi.Dtos.Coach;
using FitnessCenterApi.Models;
using FitnessCenterApi.Repositories;
using FitnessCenterApi.Repositories.UserRepositories;

namespace FitnessCenterApi.Services;

public class FitnessCenterService
{
    private readonly IMapper _mapper;
    private readonly FitnessCenterRepository _fitnessCenterRepository;
    private readonly MembershipRepository _membershipRepository;
    private readonly CoachRepository _coachRepository;
    private readonly IConfiguration _configuration;

    
    private readonly UserRepository _userRepository;

    public FitnessCenterService(IMapper mapper, IConfiguration configuration, CoachRepository coachRepository, MembershipRepository membershipRepository, FitnessCenterRepository fitnessCenterRepository, UserRepository userRepository)
    {
        _mapper = mapper;
        _fitnessCenterRepository = fitnessCenterRepository;
        _userRepository = userRepository;
        _membershipRepository = membershipRepository;
        _coachRepository = coachRepository;
        _configuration = configuration;
    }
    
    public async Task<ICollection<FitnessCenterDto>> GetFitnessCentersAsync(string email)
    {
        
        var fitnessCenters = await _fitnessCenterRepository.GetFitnessCentersAsync();

        var fitnessCentersDtos = _mapper.Map<ICollection<FitnessCenterDto>>(fitnessCenters);

        
        return fitnessCentersDtos;
    }
    
    public async Task<ICollection<CoachDto>> GetFitnessCentarsCoaches(int fitnessCentarId, string email)
    {
        
        var coaches = await _coachRepository.GetFitnessCentarsCoachesAsync(fitnessCentarId);
        var coachesDtos = _mapper.Map<ICollection<CoachDto>>(coaches);

        for (int i = 0; i < coachesDtos.Count; i++)
        {
            var coachDto = coachesDtos.ElementAt(i);
            var coachEntity = coaches.ElementAt(i);

            var programs = await _coachRepository.GetCoachProgramsAsync(coachDto.IdCoach);
            coachDto.Programs = _mapper.Map<ICollection<CoachProgramDto>>(programs);
            coachDto.User = _mapper.Map<UserDto>(coachEntity.User);
        }

        
        return coachesDtos;
    }
    
    
    
    public async Task<ICollection<FitnessCenterDto>> GetPromoFitnessCenter(string email)
    {
        var fitnessCenterPackages = await _membershipRepository.GetAllMembershipPackagesAsync();

        // Filter only packages with a discount
        var discountedPackages = fitnessCenterPackages
            .Where(p => p.Discount != null && p.Discount > 0)
            .ToList();

        // Get distinct fitness centers that have at least one discounted package
        var discountedFitnessCenters = discountedPackages
            .Select(p => p.FitnessCentar)
            .Distinct()
            .ToList();

        // Map FitnessCentar to DTO
        var fitnessCenterDtos = _mapper.Map<ICollection<FitnessCenterDto>>(discountedFitnessCenters);

        // Assign the discount from one of the discounted packages to the DTOs
        foreach (var fitnessCenterDto in fitnessCenterDtos)
        {
            var matchingPackage = discountedPackages
                .FirstOrDefault(p => p.FitnessCentar.IdFitnessCentar == fitnessCenterDto.IdFitnessCentar);

            if (matchingPackage != null)
            {
                fitnessCenterDto.Promotion = matchingPackage.Discount ?? 0;
            }
        }

        return fitnessCenterDtos;
    }


    
        
        
        
    public async Task<ICollection<FitnessCenterDto>> GetClosestFitnessCentersAsync(double userLat, double userLng, string email)
    {
        
        var fitnessCenters = await _fitnessCenterRepository.GetFitnessCentersAsync();

        var fitnessCentersDtos = _mapper.Map<ICollection<FitnessCenterDto>>(fitnessCenters);
        
        foreach (var fitnessCenter in fitnessCentersDtos)
        {
            // Assuming you have these coordinates from the user
            double lat1 = userLat;
            double lon1 = userLng;
            double lat2 = fitnessCenter.Latitude;
            double lon2 = fitnessCenter.Longitude;

            const double R = 6371000; // Earth's radius in meters

            double DegreesToRadians(double degrees) => degrees * (Math.PI / 180);

            var latRad1 = DegreesToRadians(lat1);
            var latRad2 = DegreesToRadians(lat2);
            var deltaLat = DegreesToRadians(lat2 - lat1);
            var deltaLon = DegreesToRadians(lon2 - lon1);

            var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                    Math.Cos(latRad1) * Math.Cos(latRad2) *
                    Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var distance = R * c;

            fitnessCenter.DistanceInMeters = distance;
        }
        
        return fitnessCentersDtos.OrderByDescending(d => d.DistanceInMeters).Take(3).ToList();
    }
    
    
    
    public async Task<FitnessCenterDto> GetFitnessCenterAsync(int fitnessCenterId, string email)
    {
        
        var fitnessCenter = await _fitnessCenterRepository.GetFitnessCenterAsync(fitnessCenterId);

        var fitnessCentersDto = _mapper.Map<FitnessCenterDto>(fitnessCenter);
        
        return fitnessCentersDto;
    }
    
    public async Task<bool> AddFitnessCenterAsync(FitnessCenterDto fitnessCenterDto, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return false;
        }
        var fitnessCenter = _mapper.Map<FitnessCentar>(fitnessCenterDto);

        fitnessCenter.Memberships = new List<Membership>();

        return await _fitnessCenterRepository.AddFitnessCenterAsync(fitnessCenter);
    }
    public async Task<bool> UpdateFitnessCentarAsync(int fitnessCentarId, FitnessCenterDto fitnessCentarDto, string email)
    {
        var fitnessCentar = await _fitnessCenterRepository.GetFitnessCenterAsync(fitnessCentarId);
        if (fitnessCentar == null)
        {
            return false; // Not found
        }
        if (email != _configuration["AdminSettings:AdminEmail"])
        {
            return false; // Only admin can update
        }

        _mapper.Map(fitnessCentarDto, fitnessCentar);

        return await _fitnessCenterRepository.UpdateFitnessCenterAsync(fitnessCentar);
    }

    public async Task<bool> DeleteFitnessCentarAsync(int fitnessCentarId, string email)
    {
        var fitnessCentar = await _fitnessCenterRepository.GetFitnessCenterAsync(fitnessCentarId);
        if (fitnessCentar == null)
        {
            return false; // Not found
        }
        if (email != _configuration["AdminSettings:AdminEmail"])
        {
            return false; // Only admin can delete
        }

        return await _fitnessCenterRepository.DeleteFitnessCenterAsync(fitnessCentar);
    }

    
}
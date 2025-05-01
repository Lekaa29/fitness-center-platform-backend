using AutoMapper;
using FitnessCenterApi.Dtos;
using FitnessCenterApi.Models;
using FitnessCenterApi.Repositories;
using FitnessCenterApi.Repositories.UserRepositories;

namespace FitnessCenterApi.Services;

public class FitnessCenterService
{
    private readonly IMapper _mapper;
    private readonly FitnessCenterRepository _fitnessCenterRepository;
    private readonly UserRepository _userRepository;

    public FitnessCenterService(IMapper mapper, FitnessCenterRepository fitnessCenterRepository, UserRepository userRepository)
    {
        _mapper = mapper;
        _fitnessCenterRepository = fitnessCenterRepository;
        _userRepository = userRepository;
    }
    
    public async Task<ICollection<FitnessCenterDto>> GetFitnessCentersAsync(string email)
    {
        
        var fitnessCenters = await _fitnessCenterRepository.GetFitnessCentersAsync();

        var fitnessCentersDtos = _mapper.Map<ICollection<FitnessCenterDto>>(fitnessCenters);
        
        return fitnessCentersDtos;
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
    
    
}
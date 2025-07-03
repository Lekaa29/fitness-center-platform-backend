using AutoMapper;
using FitnessCenterApi.Dtos.Coach;
using FitnessCenterApi.Models;
using FitnessCenterApi.Repositories;
using FitnessCenterApi.Repositories.UserRepositories;

namespace FitnessCenterApi.Services;

public class CoachService
{
    private readonly IMapper _mapper;
    private readonly CoachRepository _coachRepository;
    private readonly UserRepository _userRepository;
    private readonly FitnessCenterRepository _fitnessCenterRepository;
    private readonly IConfiguration _configuration;


    public CoachService(IMapper mapper, IConfiguration configuration, CoachRepository coachRepository, UserRepository userRepository, FitnessCenterRepository fitnessCenterRepository)
    {
        _mapper = mapper;
        _coachRepository = coachRepository;
        _userRepository = userRepository;
        _fitnessCenterRepository = fitnessCenterRepository;
        _configuration = configuration;
    }
    
    public async Task<CoachDto> GetCoachAsync(int coachId, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return null;
        }
        var coach = await _coachRepository.GetCoachAsync(coachId);

        var coachDto = _mapper.Map<CoachDto>(coach);
        
        return coachDto;
    }
    
    public async Task<CoachProgramDto?> GetCoachProgramAsync(int coachProgramId, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return null;
        }
        var coachProgram = await _coachRepository.GetCoachProgramAsync(coachProgramId);
        if (coachProgram == null)
        {
            return null;
        }

        var coachProgramDto = _mapper.Map<CoachProgramDto>(coachProgram);
        coachProgramDto.IdUser = coachProgram.Coach.IdUser;
        
        
        return coachProgramDto;
    }
    
    public async Task<ICollection<CoachProgramDto?>?> GetCoachProgramsAsync(int coachId, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return null;
        }
        var coachPrograms = await _coachRepository.GetCoachProgramsAsync(coachId);
        if (coachPrograms == null)
        {
            return null;
        }

        
        var coachProgramDtos = _mapper.Map<ICollection<CoachProgramDto>>(coachPrograms);

        int index = 0;
        foreach (var coachProgramDto in coachProgramDtos)
        {
            coachProgramDto.IdUser = coachPrograms.ElementAt(index).Coach.IdUser;
            index++;
        }
        
        

        return coachProgramDtos;
    }
    
    public async Task<bool> AddCoachAsync(CoachDto coachDto, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return false;
        }

        Coach coach = new Coach();
        coach.User = user;
        coach.Description = coachDto.Description;
        coach.BannerPictureLink = coachDto.BannerPictureLink;
        coach.IdUser = user.Id;
        
        coach.User = user;


        return await _coachRepository.AddCoachAsync(coach);
    }
    
    public async Task<bool> AddCoachProgramAsync(CoachProgramDto coachProgramDto, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return false;
        }
        var coachProgram = _mapper.Map<CoachProgram>(coachProgramDto);
        
        coachProgram.FitnessCentar = await _fitnessCenterRepository.GetFitnessCenterAsync(coachProgramDto.IdFitnessCentar);
        coachProgram.Coach = await _coachRepository.GetCoachAsync(coachProgramDto.IdCoach);
        coachProgram.IdFitnessCentar = coachProgram.FitnessCentar.IdFitnessCentar;
        if (coachProgram.Coach == null) return false;
        coachProgram.IdCoach = coachProgram.Coach.IdCoach;

        return await _coachRepository.AddCoachProgramAsync(coachProgram);
    }
    public async Task<bool> UpdateCoachAsync(int coachId, CoachDto coachDto, string email)
    {
        var coach = await _coachRepository.GetCoachAsync(coachId);
        if (coach == null)
        {
            return false; // Not found
        }
        if (email != _configuration["AdminSettings:AdminEmail"])
        {
            return false; // Only admin can update
        }

        _mapper.Map(coachDto, coach);

        return await _coachRepository.UpdateCoachAsync(coach);
    }

    public async Task<bool> DeleteCoachAsync(int coachId, string email)
    {
        var coach = await _coachRepository.GetCoachAsync(coachId);
        if (coach == null)
        {
            return false; // Not found
        }
        if (email != _configuration["AdminSettings:AdminEmail"])
        {
            return false; // Only admin can delete
        }

        return await _coachRepository.DeleteCoachAsync(coach);
    }

}
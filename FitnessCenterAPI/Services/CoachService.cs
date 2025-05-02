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

    public CoachService(IMapper mapper, CoachRepository coachRepository, UserRepository userRepository, FitnessCenterRepository fitnessCenterRepository)
    {
        _mapper = mapper;
        _coachRepository = coachRepository;
        _userRepository = userRepository;
        _fitnessCenterRepository = fitnessCenterRepository;
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
    
    public async Task<CoachProgramDto> GetCoachProgramAsync(int coachProgramId, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return null;
        }
        var coachProgram = await _coachRepository.GetCoachProgramAsync(coachProgramId);

        var coachProgramDto = _mapper.Map<CoachProgramDto>(coachProgram);
        
        return coachProgramDto;
    }
    
    public async Task<bool> AddCoachAsync(CoachDto coachDto, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return false;
        }
        var coach = _mapper.Map<Coach>(coachDto);
        
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

        return await _coachRepository.AddCoachProgramAsync(coachProgram);
    }
}
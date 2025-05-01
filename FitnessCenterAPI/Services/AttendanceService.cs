using AutoMapper;
using FitnessCenterApi.Dtos;
using FitnessCenterApi.Models;
using FitnessCenterApi.Repositories;
using FitnessCenterApi.Repositories.UserRepositories;

namespace FitnessCenterApi.Services;

public class AttendanceService
{
    private readonly IMapper _mapper;
    private readonly UserRepository _userRepository;
    private readonly AttendanceRepository _attendanceRepository;
    private readonly FitnessCenterRepository _fitnessCenterRepository;

    public AttendanceService(IMapper mapper, UserRepository userRepository, AttendanceRepository attendanceRepository, FitnessCenterRepository fitnessCenterRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _attendanceRepository = attendanceRepository;
        _fitnessCenterRepository = fitnessCenterRepository;
    }

    public async Task<ICollection<AttendanceDto>> GetAttendancesByUserAsync(string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        
        var attendances = await _attendanceRepository.GetAttendancesByUser(user.Id);

        var attendancesDtos = _mapper.Map<ICollection<AttendanceDto>>(attendances);
        
        return attendancesDtos;
    }
    
    public async Task<ICollection<AttendanceDto>> GetAttendancesByUserAtFitnessCenterAsync(string email, int fitnessCenterId)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        
        var attendances = await _attendanceRepository.GetAttendancesByUserAtFitnessCenterAsync(user.Id, fitnessCenterId);

        var attendancesDtos = _mapper.Map<ICollection<AttendanceDto>>(attendances);
        
        return attendancesDtos;
    }
    
    public async Task<ICollection<AttendanceDto>> GetAttendancesByFitnessCenter(string email, int fitnessCenterId)
    {
        var attendances = await _attendanceRepository.GetAttendancesByFitnessCenter(fitnessCenterId);

        var attendancesDtos = _mapper.Map<ICollection<AttendanceDto>>(attendances);
        
        return attendancesDtos;
    }
    
    public async Task<bool> AddAttendanceAsync(AttendanceDto attendanceDto, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return false;
        }
        var attendance = _mapper.Map<Attendance>(attendanceDto);
        
        attendance.FitnessCentar = await _fitnessCenterRepository.GetFitnessCenterAsync(attendanceDto.FitnessCentarId);
        attendance.User = user;

        return await _attendanceRepository.AddAttendanceAsync(attendance);
    }
}
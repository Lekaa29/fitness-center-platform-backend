using System.Security.Claims;
using FitnessCenterApi.Dtos;
using FitnessCenterApi.Models;
using FitnessCenterApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessCenterApi.Controllers;


[Route("Api/Attendance/")]
[ApiController]
public class AttendanceController : ControllerBase
{
    private readonly AttendanceService _attendanceService;

    public AttendanceController(AttendanceService attendanceService)
    {
        _attendanceService = attendanceService;
    }
    
    [HttpGet("/users/")]
    [ProducesResponseType(200, Type = typeof(List<AttendanceDto>))]
    public async Task<IActionResult> GetAttendancesByUser()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }

        var attendances = await _attendanceService.GetAttendancesByUserAsync(email);
        return Ok(attendances);
    }
    [HttpGet("/users/{fitnessCenterId}")]
    [ProducesResponseType(200, Type = typeof(List<AttendanceDto>))]
    public async Task<IActionResult> GetAttendancesByUsersAtFitnessCenter(int fitnessCenterId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }

        var attendances = await _attendanceService.GetAttendancesByUserAtFitnessCenterAsync(email, fitnessCenterId);
        return Ok(attendances);
    }
    
    [HttpGet("/fitnesscenters/{fitnessCenterId}")]
    [ProducesResponseType(200, Type = typeof(List<AttendanceDto>))]
    public async Task<IActionResult> GetAttendancesByFitnessCenter(int fitnessCenterId, [FromQuery] int start = 0, [FromQuery] int limit = 20)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }

        var attendances = await _attendanceService.GetAttendancesByFitnessCenter(email, fitnessCenterId);
        return Ok(attendances);
    }


    [HttpPost("AddAttendance")]
    public async Task<IActionResult> AddAttendance([FromBody] AttendanceDto attendanceDto)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }
        if (attendanceDto == null)
        {
            return BadRequest("Client object is null");
        }
        var result = await _attendanceService.AddAttendanceAsync(attendanceDto, email);
        if (result)
        {
            return Ok("Attendance added successfully");
        }
        return BadRequest("Attendance not added");
    }
}
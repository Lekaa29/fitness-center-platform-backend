using System.Security.Claims;
using FitnessCenterApi.Dtos;
using FitnessCenterApi.Models;
using FitnessCenterApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessCenterApi.Controllers;


[Route("api/Attendance/")]
[ApiController]
public class AttendanceController : ControllerBase
{
    private readonly AttendanceService _attendanceService;

    public AttendanceController(AttendanceService attendanceService)
    {
        _attendanceService = attendanceService;
    }
    
    [HttpGet("users/")]
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
    [HttpGet("users/{fitnessCentarIdfitnessCentarId}")]
    [ProducesResponseType(200, Type = typeof(List<AttendanceDto>))]
    public async Task<IActionResult> GetAttendancesByUsersAtFitnessCenter(int fitnessCentarId)
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

        var attendances = await _attendanceService.GetAttendancesByUserAtFitnessCenterAsync(email, fitnessCentarId);
        return Ok(attendances);
    }
    
    [HttpGet("fitnesscenters/{fitnessCentarId}")]
    [ProducesResponseType(200, Type = typeof(List<AttendanceDto>))]
    public async Task<IActionResult> GetAttendancesByFitnessCenter(int fitnessCentarId, [FromQuery] int start = 0, [FromQuery] int limit = 20)
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

        var attendances = await _attendanceService.GetAttendancesByFitnessCenter(email, fitnessCentarId);
        return Ok(attendances);
    }
    
    [HttpGet("fitnesscenters/recent/{fitnessCentarId}")]
    [ProducesResponseType(200, Type = typeof(int))]
    public async Task<IActionResult> GetRecentFitnessCenterAttendees(int fitnessCentarId, [FromQuery] int start = 0, [FromQuery] int limit = 20)
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

        var attendances = await _attendanceService.GetRecentFitnessCenterAttendeesAsync(email, fitnessCentarId);
        return Ok(attendances.Count);
    }
    
    [HttpGet("fitnesscenters/leaving/{fitnessCentarId}")]
    [ProducesResponseType(200, Type = typeof(int))]
    public async Task<IActionResult> GetLeavingFitnessCenterAttendees(int fitnessCentarId, [FromQuery] int start = 0, [FromQuery] int limit = 20)
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

        var attendances = await _attendanceService.GetLeavingFitnessCenterAttendeesAsync(email, fitnessCentarId);
        return Ok(attendances.Count);
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
    
    [HttpPut("UpdateAttendance/{attendanceId}")]
    public async Task<IActionResult> UpdateAttendance(int attendanceId, [FromBody] AttendanceDto attendanceDto)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }

        if (attendanceDto == null)
        {
            return BadRequest("Attendance object is null");
        }

        var result = await _attendanceService.UpdateAttendanceAsync(attendanceId, attendanceDto, email);
        if (result)
        {
            return Ok("Attendance updated successfully");
        }

        return BadRequest("Attendance not updated");
    }

    [HttpDelete("DeleteAttendance/{attendanceId}")]
    public async Task<IActionResult> DeleteAttendance(int attendanceId)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }

        var result = await _attendanceService.DeleteAttendanceAsync(attendanceId, email);
        if (result)
        {
            return Ok("Attendance deleted successfully");
        }

        return BadRequest("Attendance not deleted");
    }

}
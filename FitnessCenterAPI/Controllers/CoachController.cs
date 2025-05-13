using System.Security.Claims;
using FitnessCenterApi.Dtos.Coach;
using FitnessCenterApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessCenterApi.Controllers;

[Route("api/Coach/")]
[ApiController]
public class CoachController : ControllerBase
{
    private readonly CoachService _coachService;

    public CoachController(CoachService coachService)
    {
        _coachService = coachService;
    }
    
    [HttpGet("/{coachId}/")]
    [ProducesResponseType(200, Type = typeof(CoachDto))]
    public async Task<IActionResult> GetCoach(int coachId)
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

        var coach = await _coachService.GetCoachAsync(coachId, email);
        return Ok(coach);
    }
    
    [HttpGet("/coachProgram/{coachProgramId}/")]
    [ProducesResponseType(200, Type = typeof(CoachProgramDto))]
    public async Task<IActionResult> GetCoachProgram(int coachProgramId)
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

        var coach = await _coachService.GetCoachProgramAsync(coachProgramId, email);
        return Ok(coach);
    }
    [HttpPost("AddCoach")]
    public async Task<IActionResult> AddCoach([FromBody] CoachDto coachDto)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
     
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }
        if (coachDto == null)
        {
            return BadRequest("Coach object is null");
        }
        var result = await _coachService.AddCoachAsync(coachDto, email);
        if (result)
        {
            return Ok("Coach added successfully");
        }
        return BadRequest("Coach not added");
    }   
    
    [HttpPost("AddCoachProgram")]
    public async Task<IActionResult> AddCoachProgram([FromBody] CoachProgramDto coachProgramDto)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
     
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }
        if (coachProgramDto == null)
        {
            return BadRequest("Coach program object is null");
        }
        var result = await _coachService.AddCoachProgramAsync(coachProgramDto, email);
        if (result)
        {
            return Ok("Coach program added successfully");
        }
        return BadRequest("Coach program not added");
    }   
    
    
}
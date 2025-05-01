using System.Security.Claims;
using FitnessCenterApi.Dtos;
using FitnessCenterApi.Models;
using FitnessCenterApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessCenterApi.Controllers;


[Route("Api/FitnessCenter/")]
[ApiController]
public class FitnessCenterController : ControllerBase
{
    private readonly FitnessCenterService _fitnessCenterService;

    public FitnessCenterController(FitnessCenterService fitnessCenterService)
    {
        _fitnessCenterService = fitnessCenterService;
    }
    
    
    [HttpGet("")]
    [ProducesResponseType(200, Type = typeof(List<FitnessCenterDto>))]
    public async Task<IActionResult> GetFitnessCenters()
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

        var fitnessCenters = await _fitnessCenterService.GetFitnessCentersAsync(email);
        return Ok(fitnessCenters);
    }
    
    [HttpGet("{fitnessCenterId}")]
    [ProducesResponseType(200, Type = typeof(FitnessCenterDto))]
    public async Task<IActionResult> GetFitnessCenter(int fitnessCenterId)
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

        var fitnessCenter = await _fitnessCenterService.GetFitnessCenterAsync(fitnessCenterId, email);
        return Ok(fitnessCenter);
    }
    
    [HttpPost("")]
    public async Task<IActionResult> AddAttendance([FromBody] FitnessCenterDto fitnessCenterDto)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }
        if (fitnessCenterDto == null)
        {
            return BadRequest("FitnessCenter object is null");
        }
        var result = await _fitnessCenterService.AddFitnessCenterAsync(fitnessCenterDto, email);
        if (result)
        {
            return Ok("FitnessCenter added successfully");
        }
        return BadRequest("FitnessCenter not added");
    }    
}
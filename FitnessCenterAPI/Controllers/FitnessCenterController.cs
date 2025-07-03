using System.Security.Claims;
using FitnessCenterApi.Dtos;
using FitnessCenterApi.Dtos.Coach;
using FitnessCenterApi.Models;
using FitnessCenterApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessCenterApi.Controllers;


[Route("api/FitnessCenter/")]
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
    
    [HttpGet("{fitnessCentarId}")]
    [ProducesResponseType(200, Type = typeof(FitnessCenterDto))]
    public async Task<IActionResult> GetFitnessCenter(int fitnessCentarId)
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

        var fitnessCenter = await _fitnessCenterService.GetFitnessCenterAsync(fitnessCentarId, email);
        return Ok(fitnessCenter);
    }
    
    [HttpGet("ClosestFitnessCentars")]
    [ProducesResponseType(200, Type = typeof(List<FitnessCenterDto>))]
    public async Task<IActionResult> GetClosestFitnessCentars([FromQuery] double userLat, [FromQuery] double userLng)
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

        var fitnessCenters = await _fitnessCenterService.GetClosestFitnessCentersAsync(userLat, userLng, email);
        return Ok(fitnessCenters);
    }
    
    
    [HttpGet("coaches/{fitnessCentarId}")]
    [ProducesResponseType(200, Type = typeof(List<CoachDto>))]
    public async Task<IActionResult> GetFitnessCentarsCoaches(int fitnessCentarId)
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

        var coaches = await _fitnessCenterService.GetFitnessCentarsCoaches(fitnessCentarId, email);
        return Ok(coaches);
    }

    
    [HttpGet("PromoFitnessCentars")]
    [ProducesResponseType(200, Type = typeof(List<FitnessCenterDto>))]
    public async Task<IActionResult> GetPromoFitnessCenter()
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

        var fitnessCenters = await _fitnessCenterService.GetPromoFitnessCenter(email);
        return Ok(fitnessCenters);
    }
    
    [HttpPost("AddFitnessCenter")]
    public async Task<IActionResult> AddFitnessCenter([FromBody] FitnessCenterDto fitnessCenterDto)
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
    
    [HttpPut("UpdateFitnessCentar/{fitnessCentarId}")]
    public async Task<IActionResult> UpdateFitnessCentar(int fitnessCentarId, [FromBody] FitnessCenterDto fitnessCentarDto)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }

        if (fitnessCentarDto == null)
        {
            return BadRequest("Fitness center object is null");
        }

        var result = await _fitnessCenterService.UpdateFitnessCentarAsync(fitnessCentarId, fitnessCentarDto, email);
        if (result)
        {
            return Ok("Fitness center updated successfully");
        }

        return BadRequest("Fitness center not updated");
    }

    [HttpDelete("DeleteFitnessCentar/{fitnessCentarId}")]
    public async Task<IActionResult> DeleteFitnessCentar(int fitnessCentarId)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }

        var result = await _fitnessCenterService.DeleteFitnessCentarAsync(fitnessCentarId, email);
        if (result)
        {
            return Ok("Fitness center deleted successfully");
        }

        return BadRequest("Fitness center not deleted");
    }

}
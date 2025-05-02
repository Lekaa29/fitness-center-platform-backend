using System.Security.Claims;
using FitnessCenterApi.Dtos;
using FitnessCenterApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessCenterApi.Controllers;


[Route("api/Membership")]
[ApiController]
public class MembershipController : ControllerBase
{
    private readonly MembershipService _membershipService;

    public MembershipController(MembershipService membershipService)
    {
        _membershipService = membershipService;
    }
    
    [HttpGet("/user/")]
    [ProducesResponseType(200, Type = typeof(List<MembershipDto>))]
    public async Task<IActionResult> GetUserMemberships()
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

        var memberships = await _membershipService.GetUserMemberships(email);
        return Ok(memberships);
    }
    [HttpGet("/user/{fitnessCenterId}")]
    [ProducesResponseType(200, Type = typeof(MembershipDto))]
    public async Task<IActionResult> GetUserMembershipByFitnessCenter(int fitnessCenterId)
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

        var membership = await _membershipService.GetUserMembershipByFitnessCenter(fitnessCenterId, email);
        return Ok(membership);
    }
    [HttpGet("/FitnessCenter/")]
    [ProducesResponseType(200, Type = typeof(List<MembershipDto>))]
    public async Task<IActionResult> GetFitnessCenterMemberships(int fitnessCenterId)
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

        var memberships = await _membershipService.GetFitnessCenterMemberships(fitnessCenterId, email);
        return Ok(memberships);
    }
    
    [HttpPost("")]
    public async Task<IActionResult> AddMembership([FromBody] MembershipDto membershipDto)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
     
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }
        if (membershipDto == null)
        {
            return BadRequest("FitnessCenter object is null");
        }
        var result = await _membershipService.AddMembership(membershipDto, email);
        if (result)
        {
            return Ok("FitnessCenter added successfully");
        }
        return BadRequest("FitnessCenter not added");
    }   
    
}
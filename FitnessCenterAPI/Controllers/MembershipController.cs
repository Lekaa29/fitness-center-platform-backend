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
    
    [HttpGet("user")]
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

        var memberships = await _membershipService.GetUserMembershipsAsync(email);
        return Ok(memberships);
    }
    
    [HttpGet("user/{fitnessCentarId}")]
    [ProducesResponseType(200, Type = typeof(MembershipDto))]
    public async Task<IActionResult> GetUserMembershipByFitnessCenter(int fitnessCentarId)
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

        var membership = await _membershipService.GetUserMembershipByFitnessCenterAsync(fitnessCentarId, email);
        return Ok(membership);
    }
    [HttpGet("FitnessCenter/{fitnessCentarId}")]
    [ProducesResponseType(200, Type = typeof(List<MembershipDto>))]
    public async Task<IActionResult> GetFitnessCenterMemberships(int fitnessCentarId)
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

        var memberships = await _membershipService.GetFitnessCenterMembershipsAsync(fitnessCentarId, email);
        return Ok(memberships);
    }
    
    [HttpGet("FitnessCenter/Leaderboard/{fitnessCentarId}")]
    [ProducesResponseType(200, Type = typeof(List<MembershipDto>))]
    public async Task<IActionResult> GetFitnessCenterLeaderboard(int fitnessCentarId)
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

        var memberships = await _membershipService.GetFitnessCenterLeaderboardAsync(fitnessCentarId, email);
        return Ok(memberships);
    }
    
    [HttpPost("AddMembership")]
    public async Task<IActionResult> AddMembership([FromBody] MembershipDto membershipDto)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
     
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }
        if (membershipDto == null)
        {
            return BadRequest("Membership object is null");
        }
        var result = await _membershipService.AddMembershipAsync(membershipDto, email);
        if (result)
        {
            return Ok("Membership added successfully");
        }
        return BadRequest("Membership not added");
    }  
    
    [HttpPost("AddMembershipPackage")]
    public async Task<IActionResult> AddMembershipPackage([FromBody] MembershipPackageDto membershipPackageDto)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
     
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }
        if (membershipPackageDto == null)
        {
            return BadRequest("Membership package object is null");
        }
        var result = await _membershipService.AddMembershipPackageAsync(membershipPackageDto, email);
        if (result)
        {
            return Ok("Membership package added successfully");
        }
        return BadRequest("Membership package not added");
    }  
    
    
    
    
    [HttpPost("UpdateMembership")]
    public async Task<IActionResult> UpdateMembership([FromBody] MembershipDto membershipDto)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
     
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }
        if (membershipDto == null)
        {
            return BadRequest("Membership object is null");
        }
        var result = await _membershipService.AddOrUpdateMembershipAsync(membershipDto, email);
        if (result)
        {
            return Ok("Membership updated successfully");
        }
        return BadRequest("Membership not updated");
    }   
    
    [HttpGet("MembershipPackage/{membershipPackageId}")]
    [ProducesResponseType(200, Type = typeof(MembershipPackageDto))]
    public async Task<IActionResult> GetFitnessCenterMembershipPackage(int membershipPackageId)
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

        var membershipPackage = await _membershipService.GetMembershipPackageAsync(membershipPackageId, email);
        return Ok(membershipPackage);
    }
    
    [HttpGet("MembershipPackages/{fitnessCentarId}")]
    [ProducesResponseType(200, Type = typeof(List<MembershipPackageDto>))]
    public async Task<IActionResult> GetFitnessCenterMembershipPackages(int fitnessCentarId)
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

        var membershipPackages = await _membershipService.GetMembershipPackagesAsync(fitnessCentarId, email);
        return Ok(membershipPackages);
    }
    
    [HttpPut("UpdateMembership/{membershipId}")]
    public async Task<IActionResult> UpdateMembership(int membershipId, [FromBody] MembershipDto membershipDto)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }

        if (membershipDto == null)
        {
            return BadRequest("Membership object is null");
        }

        var result = await _membershipService.UpdateMembershipAsync(membershipId, membershipDto, email);
        if (result)
        {
            return Ok("Membership updated successfully");
        }

        return BadRequest("Membership not updated");
    }

    [HttpDelete("DeleteMembership/{membershipId}")]
    public async Task<IActionResult> DeleteMembership(int membershipId)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }

        var result = await _membershipService.DeleteMembershipAsync(membershipId, email);
        if (result)
        {
            return Ok("Membership deleted successfully");
        }

        return BadRequest("Membership not deleted");
    }

    
    
    
}
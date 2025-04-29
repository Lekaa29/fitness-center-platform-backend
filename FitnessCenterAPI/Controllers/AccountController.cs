using System.Security.Claims;
using FitnessCenterApi.Dtos.Account;
using FitnessCenterApi.Models;
using FitnessCenterApi.Services.UserServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FitnessCenterApi.Controllers;

[Route("Api/Account/")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserService _userService;


    public AccountController(UserService userService)
    {
        this._userService = userService;
    }


    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            BadRequest(ModelState);
        }
        
        var result = await _userService.AddUserAsync(registerDto);
        if (result)
        {
            return Ok("User added successfully");
        }
        return BadRequest("Failed to add user");
    }
}
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
    private readonly TokenService _tokenService;
    private readonly AccountService _accountService;



    public AccountController(UserService userService, TokenService tokenService, AccountService accountService)
    {
        this._userService = userService;
        this._tokenService = tokenService;
        this._accountService = accountService;
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
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginUser)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _accountService.LoginUser(loginUser);

        if (result.Token != null)
            return Ok(result);

        return Unauthorized("Invalid credentials.");
    }
}
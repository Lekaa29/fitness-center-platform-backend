using FitnessCenterApi.Dtos.Account;
using FitnessCenterApi.Models;
using Microsoft.AspNetCore.Identity;

namespace FitnessCenterApi.Services.UserServices;

public class AccountService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly TokenService _tokenService;

    public AccountService(UserManager<User> userManager, TokenService tokenService, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
    }

    public async Task<UserTokenDto> LoginUser(LoginDto login)
    {
        var user = await _userManager.FindByEmailAsync(login.Email.ToLower());
        if (user == null || user.Status == 0) return new UserTokenDto();

        var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
        if (!result.Succeeded) return new UserTokenDto();
        
        return new UserTokenDto
        {
            Email = user.Email,
            UserName = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
    }
}
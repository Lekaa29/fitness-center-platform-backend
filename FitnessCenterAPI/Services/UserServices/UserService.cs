using AutoMapper;
using FitnessCenterApi.Dtos.Account;
using FitnessCenterApi.Dtos.Chat;
using FitnessCenterApi.Models;
using FitnessCenterApi.Repositories.UserRepositories;
using Microsoft.AspNetCore.Identity;

namespace FitnessCenterApi.Services.UserServices;

public class UserService
{
    private readonly UserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public UserService(UserRepository userRepository, IMapper mapper, UserManager<User> userManager)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _userManager = userManager;
    }
    
        
    public async Task<UserDto?> GetUserAsync(int userId, string email)
    {
        var user = await _userRepository.GetUserAsync(userId);
        if (user == null)
        {
            return null;
        }

        var userDto = _mapper.Map<UserDto>(user);
        
        return userDto;
    }
    public async Task<bool> AddUserAsync(RegisterDto user)
    {
        Console.WriteLine("USERNAME:");
        Console.WriteLine(user.Username);
        var emailExists = await _userRepository.CheckIfUserEmailExistsAsync(user.Email);
        if (emailExists)
        {
            return false;
        }

        var usernameExists = await _userRepository.CheckIfUserUsernameExistsAsync(
            user.Username);
        if (usernameExists)
        {
            return false;
        }

        string? imageUrl = null;
        /*
        if (user.ProfilePicture != null)
        {
            imageUrl = await _userPictureService.UploadImageAsync(user.ProfilePicture);
            if (imageUrl == null)
            {
                return false;
            }
        }
        */

        
        User newUser = new User
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.Username, 
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            PictureLink = imageUrl,
            Status = 1,
            EmailConfirmed = true
        };

        var createdUser = await _userManager.CreateAsync(newUser, user.Password);
        if (!createdUser.Succeeded)
        {
            foreach (var error in createdUser.Errors)
            {
                Console.WriteLine($"Error: {error.Code} - {error.Description}");
            }
            return false;
        }

      
        var roleResult = await _userManager.AddToRoleAsync(newUser, "Client"); 
        if (!roleResult.Succeeded)
        {
            return false;
        }
        var passwordUser = await _userManager.FindByEmailAsync(user.Email);
        if (passwordUser == null)
        {
            return false;
        }

        return true;

    }
    public async Task<bool> UpdateUserAsync(UserDto userDto, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null || userDto.Id != user.Id)
        {
            return false;
        }

      
        user.PictureLink = userDto.PictureLink;
        

        return await _userRepository.UpdateUserAsync(user);
    }
}


using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApi.Dtos.Account;

public class RegisterDto
{
     public string? Username { get; set; }

     public string FirstName { get; set; } = null!;
     public string LastName { get; set; } = null!;
     public string Email { get; set; } = null!;
     public string Password { get; set; } = null!;
     public string? PhoneNumber { get; set; }
     public IFormFile? ProfilePicture { get; set; }
}
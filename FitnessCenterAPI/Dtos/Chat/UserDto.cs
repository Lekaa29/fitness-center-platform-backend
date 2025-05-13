namespace FitnessCenterApi.Dtos.Chat;

public class UserDto
{

    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? PictureLink { get; set; }
    
}
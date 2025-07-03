using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApi.Models;

public class Coach
{
    [Key]
    public int IdCoach { get; set; }

    public int IdUser { get; set; }
    public User User { get; set; }

    public string Description { get; set; } = string.Empty;
    public string BannerPictureLink { get; set; } = string.Empty;
    
}

/*
data class Coach(
    val user: User,
    val description: String,
    val bannerPictureLink: String? = null,
    val programs: List<Program> = emptyList()
)


*/
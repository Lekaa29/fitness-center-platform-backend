using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApi.Models;

public class Coach
{
    [Key]
    public int IdCoach { get; set; }

    public int IdUser { get; set; }
    public User User { get; set; }

    public string Experience { get; set; } = string.Empty;
    public string PictureLink { get; set; } = string.Empty;
    public string VideoLink { get; set; } = string.Empty;
}

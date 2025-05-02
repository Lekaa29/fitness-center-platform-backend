namespace FitnessCenterApi.Dtos.Coach;

public class CoachDto
{
    public int IdCoach { get; set; }
    public int IdUser { get; set; }
    public string Experience { get; set; } = string.Empty;
    public string PictureLink { get; set; } = string.Empty;
    public string VideoLink { get; set; } = string.Empty;
}
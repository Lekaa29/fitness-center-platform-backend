using FitnessCenterApi.Dtos.Chat;
using FitnessCenterApi.Models;

namespace FitnessCenterApi.Dtos.Coach;

public class CoachDto
{
    public UserDto User { get; set; }
    public int IdCoach { get; set; }
    public string Description { get; set; } = string.Empty;
    public string BannerPictureLink { get; set; } = string.Empty;
    public ICollection<CoachProgramDto> Programs { get; set; } = new List<CoachProgramDto>();
}
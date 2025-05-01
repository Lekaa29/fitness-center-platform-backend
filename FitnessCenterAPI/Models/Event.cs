using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApi.Models;

public class Event
{
    [Key]
    public int IdEvent { get; set; }

    public int IdFitnessCentar { get; set; }
    public FitnessCentar FitnessCentar { get; set; }

    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string PictureUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int MaxParticipants { get; set; }

    public ICollection<EventParticipant> Participants { get; set; }
}

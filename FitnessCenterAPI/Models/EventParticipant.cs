namespace FitnessCenterApi.Models;

public class EventParticipant
{
    public int IdEvent { get; set; }
    public Event Event { get; set; }

    public int IdUser { get; set; }
    public User User { get; set; }
}

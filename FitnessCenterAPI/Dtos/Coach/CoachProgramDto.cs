namespace FitnessCenterApi.Dtos.Coach;

public class CoachProgramDto
{

    public int Id { get; set; }
    public int IdUser { get; set; }
    
    public string Title { get; set; }

    public float Price { get; set; }
    
    public string Description { get; set; }
    
    public int WeekDuration { get; set; }
    
    public int IdFitnessCentar { get; set; }
    public int IdCoach { get; set; }

}


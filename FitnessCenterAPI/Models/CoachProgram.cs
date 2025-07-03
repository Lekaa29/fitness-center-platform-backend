using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApi.Models;

public class CoachProgram
{
    public int Id { get; set; }

    public string Title { get; set; }

    public float Price { get; set; }
    
    public string Description { get; set; }
    
    public int WeekDuration { get; set; }
    
    public int IdFitnessCentar { get; set; }
    public int IdCoach { get; set; }
    public Coach Coach { get; set; }
    public FitnessCentar FitnessCentar { get; set; }

}



using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApi.Models;

public class CoachProgram
{
    [Key]
    public int IdCoachProgram { get; set; }

    public int IdCoach { get; set; }
    public Coach Coach { get; set; }

    public int IdFitnessCentar { get; set; }
    public FitnessCentar FitnessCentar { get; set; }

    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

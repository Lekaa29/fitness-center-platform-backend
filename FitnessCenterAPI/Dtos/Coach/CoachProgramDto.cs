namespace FitnessCenterApi.Dtos.Coach;

public class CoachProgramDto
{
    public int IdCoachProgram { get; set; }

    public int IdCoach { get; set; }

    public int IdFitnessCentar { get; set; }

    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }

}
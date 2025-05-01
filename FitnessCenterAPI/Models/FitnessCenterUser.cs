namespace FitnessCenterApi.Models;

public class FitnessCenterUser
{
    public int IdUser { get; set; }
    public User User { get; set; }

    public int IdFitnessCentar { get; set; }
    public FitnessCentar FitnessCentar { get; set; }
}

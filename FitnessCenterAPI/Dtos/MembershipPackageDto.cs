namespace FitnessCenterApi.Dtos;

public class MembershipPackageDto
{
    public int Id { get; set; }

    public string Title { get; set; }

    public float Price { get; set; }
    
    public int Days { get; set; }

    public int? Discount { get; set; }
    
    public string? FitnessCentarName { get; set; }


    public int IdFitnessCentar { get; set; }

}
namespace FitnessCenterApi.Dtos;

public class FitnessCenterDto
{
    public int IdFitnessCentar { get; set; }
    
    public double? DistanceInMeters { get; set; }
    public int? Promotion { get; set; }
    public double Latitude { get; set; }  
    public double Longitude { get; set; } 
    public string? PictureLink { get; set; }


    public string Name { get; set; } = null!;
}
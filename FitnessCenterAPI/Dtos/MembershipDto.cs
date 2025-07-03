namespace FitnessCenterApi.Dtos;

public class MembershipDto
{
    public int IdMembership { get; set; }

    public int IdUser { get; set; }
    public int? IdMembershipPackage { get; set; }


    public int IdFitnessCentar { get; set; }
    public int? Points { get; set; }


    public int? LoyaltyPoints { get; set; }

    public int? StreakRunCount { get; set; }
    public string? FitnessCentarName { get; set; }
    public string? FitnessCentarBannerUrl { get; set; }
    public string? FitnessCentarLogoUrl { get; set; }
    
    public DateTime? MembershipDeadline { get; set; }

    public string? Username { get; set; }

}
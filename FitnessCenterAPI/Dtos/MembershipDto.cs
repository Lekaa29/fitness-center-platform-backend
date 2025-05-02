namespace FitnessCenterApi.Dtos;

public class MembershipDto
{
    public int IdMembership { get; set; }

    public int IdUser { get; set; }

    public int IdFitnessCentar { get; set; }

    public int? LoyaltyPoints { get; set; }

    public int? StreakRunCount { get; set; }

    public int? MembershipCountdown { get; set; }
}
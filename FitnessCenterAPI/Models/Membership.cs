using System;
using System.Collections.Generic;

namespace FitnessCenterApi.Models;

public partial class Membership
{
    public int IdMembership { get; set; }

    public int IdUser { get; set; }

    public int IdFitnessCentar { get; set; }

    public int? LoyaltyPoints { get; set; }

    public int? StreakRunCount { get; set; }

    public DateTime? MembershipDeadline { get; set; }

    public virtual FitnessCentar IdFitnessCentarNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}

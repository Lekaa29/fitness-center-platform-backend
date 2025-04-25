using System;
using System.Collections.Generic;

namespace FitnessCenterApi.Models;

public partial class FitnessCentar
{
    public int IdFitnessCentar { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Membership> Memberships { get; set; } = new List<Membership>();
}

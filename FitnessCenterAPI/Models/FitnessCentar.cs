using System;
using System.Collections.Generic;

namespace FitnessCenterApi.Models;

public partial class FitnessCentar
{
    public int IdFitnessCentar { get; set; }

    public string Name { get; set; } = null!;
    public double Latitude { get; set; }  
    public double Longitude { get; set; } 
    
    public string? BannerUrl { get; set; }
    public string? LogoUrl { get; set; }
    
    public string? Color { get; set; }

    
    //color

    public virtual ICollection<Membership> Memberships { get; set; } = new List<Membership>();
    public ICollection<MembershipPackage> MembershipPackages { get; set; }

}

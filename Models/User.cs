using System;
using System.Collections.Generic;

namespace FitnessCenterApi.Models;

public partial class User
{
    public int IdUser { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int? IsStudent { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime Birthday { get; set; }

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string Username { get; set; } = null!;

    public int Status { get; set; }

    public string? PictureLink { get; set; }

    public string? SecretKey { get; set; }

    public virtual ICollection<Membership> Memberships { get; set; } = new List<Membership>();
}

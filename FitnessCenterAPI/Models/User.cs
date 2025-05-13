using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace FitnessCenterApi.Models;

public partial class User : IdentityUser<int>
{
    [Key]
    [Column("id_user")]
    public override  int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int? IsStudent { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime Birthday { get; set; }
    
    public string? Phone { get; set; }
    
    public int Status { get; set; }

    public string? PictureLink { get; set; }

    public string? SecretKey { get; set; }

    public virtual ICollection<Membership> Memberships { get; set; } = new List<Membership>();
    public ICollection<UserConversation> UserConversations { get; set; } = new List<UserConversation>();

}

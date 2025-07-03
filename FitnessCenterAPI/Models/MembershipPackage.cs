using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessCenterApi.Models;

public class MembershipPackage
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public float Price { get; set; }
    
    [Required]
    public int Days { get; set; }

    public int? Discount { get; set; }

    [ForeignKey("FitnessCenter")]
    public int IdFitnessCentar { get; set; }

    public FitnessCentar FitnessCentar { get; set; }
    
    
}

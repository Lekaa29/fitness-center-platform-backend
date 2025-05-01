using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessCenterApi.Models;

public class Attendance
{
    [Key]
    [Column("id_attendance")]
    public int IdAttendance { get; set; }

    [Column("id_user")]
    public int UserId { get; set; }

    [Column("id_fitness_centar")]
    public int FitnessCentarId { get; set; }

    [Column("timestamp")]
    public DateTime Timestamp { get; set; }

    // Navigation properties (optional but good practice)
    public User User { get; set; }
    public FitnessCentar FitnessCentar { get; set; }
}

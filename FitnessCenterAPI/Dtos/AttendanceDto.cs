namespace FitnessCenterApi.Dtos;

public class AttendanceDto
{
    public int IdAttendance { get; set; }

    public int UserId { get; set; }

    public int FitnessCentarId { get; set; }

    public DateTime Timestamp { get; set; }
}
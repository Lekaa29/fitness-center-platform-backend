using FitnessCenterApi.Data;
using FitnessCenterApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterApi.Repositories;

public class AttendanceRepository
{
    private readonly FitnessCenterDbContext _context;
    
    public AttendanceRepository(FitnessCenterDbContext context)
    {
        _context = context;
    }
    
    public async  Task<ICollection<Attendance>> GetAttendancesByUser(int userId)
    {
        var attendances = await _context.Attendances.Where(
            a => a.UserId == userId).ToListAsync();
        
        return attendances;
    }
    
    public async  Task<ICollection<Attendance>> GetAttendancesByUserAtFitnessCenterAsync(int userId, int fitnessCenterId)
    {
        var attendances = await _context.Attendances.Where(
            a => a.UserId == userId && a.FitnessCentarId == fitnessCenterId).ToListAsync();
        
        return attendances;
    }
    
    public async  Task<ICollection<Attendance>> GetAttendancesByFitnessCenter(int fitnessCenterId)
    {
        var attendances = await _context.Attendances.Where(
            a => a.FitnessCentarId == fitnessCenterId).ToListAsync();
        
        return attendances;
    }
    
    public async Task<Attendance?> GetLastAttendancesByFitnessCenter(int fitnessCenterId, int userId)
    {
        var attendances = await _context.Attendances
            .Where(a => a.FitnessCentarId == fitnessCenterId && a.UserId == userId)
            .ToListAsync();

        return attendances.LastOrDefault(); // returns null if list is empty
    }

    
    public async  Task<bool> ExtendStreakAsync(int userId, int fitnessCenterId)
    {
        var membership = await _context.Memberships.Where(
            m => m.IdFitnessCentar == fitnessCenterId && m.IdUser == userId).FirstOrDefaultAsync();
        if (membership == null) return false;
        
        membership.StreakRunCount += 1;
        
        return await SaveAsync();
    }
    
    
    
    public async Task<ICollection<Attendance>> GetRecentFitnessCenterAttendeesAsync(int fitnessCenterId)
    {
        var oneAndHalfHoursAgo = DateTime.UtcNow.AddHours(-1.5);

        var attendances = await _context.Attendances
            .Where(a => a.FitnessCentarId == fitnessCenterId && a.Timestamp >= oneAndHalfHoursAgo)
            .ToListAsync();

        return attendances;
    }

    
    public async Task<ICollection<Attendance>> GetLeavingFitnessCenterAttendeesAsync(int fitnessCenterId)
    {
        var oneHourAgo = DateTime.UtcNow.AddHours(-1);
        var oneAndHalfHoursAgo = DateTime.UtcNow.AddHours(-1.5);

        var attendances = await _context.Attendances
            .Where(a => a.FitnessCentarId == fitnessCenterId 
                        && a.Timestamp >= oneAndHalfHoursAgo 
                        && a.Timestamp < oneHourAgo)
            .ToListAsync();

        return attendances;
    }

    
    public async Task<Attendance?> GetAttendanceAsync(int attendanceId)
    {
        var attendance = await _context.Attendances.Where(
            a => a.IdAttendance == attendanceId).FirstOrDefaultAsync();
        
        return attendance;    
    }
    public async Task<bool> AddAttendanceAsync(Attendance attendance) 
    {
        await _context.Attendances.AddAsync(attendance);
        return await SaveAsync();
    }
    
    public async Task<bool> UpdateAttendanceAsync(Attendance attendance) 
    {
        _context.Attendances.Update(attendance);
        return await SaveAsync();
    }
    public async Task<bool> DeleteAttendanceAsync(Attendance attendance) 
    {
        _context.Attendances.Remove(attendance);
        return await SaveAsync();
    }
    
    private async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

}
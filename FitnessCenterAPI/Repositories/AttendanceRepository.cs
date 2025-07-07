using FitnessCenterApi.Data;
using FitnessCenterApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterApi.Repositories;

public class AttendanceRepository
{
    private readonly FitnessCenterDbContext _context;
    private readonly Random _random = new Random();

    
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
    
    public void SeedAttendances()
    {
        var userIds = Enumerable.Range(1, 10).ToList(); // User IDs 1-10
        var fitnessCenterIds = new List<int> { 2, 3 };

        var startDate = DateTime.Now.AddMonths(-1); // Start from 3 months ago
        var attendances = new List<Attendance>();

        foreach (var userId in userIds)
        {
            DateTime currentTimestamp = startDate.AddDays(_random.Next(0, 5)); // Small initial random offset

            for (int i = 0; i < 4; i++) // Roughly 4 attendances per user to total ~40 records
            {
                var fitnessCenterId = fitnessCenterIds[_random.Next(fitnessCenterIds.Count)];
                
                attendances.Add(new Attendance
                {
                    UserId = userId,
                    FitnessCentarId = fitnessCenterId,
                    Timestamp = currentTimestamp
                });

                // Increment date by 2 or 3 days randomly
                currentTimestamp = currentTimestamp.AddDays(_random.Next(2, 4));
            }
        }

        _context.Attendances.AddRange(attendances);
        _context.SaveChanges();
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
        SeedAttendances();
        SeedRecentAttendances();
        
        await _context.Attendances.AddAsync(attendance);
        return await SaveAsync();
    }

    public void SeedRecentAttendances()
    {
        var userIds = Enumerable.Range(1, 10).ToList(); // User IDs 1-10
        var fitnessCenterIds = new List<int> { 2, 3 };

        var now = DateTime.Now;
        var attendances = new List<Attendance>();

        foreach (var userId in userIds)
        {
            for (int i = 0; i < 2; i++) // 2 attendances per user
            {
                var fitnessCenterId = fitnessCenterIds[_random.Next(fitnessCenterIds.Count)];

                // Random timestamp within the last 2 hours
                var randomMinutesAgo = _random.Next(0, 120); // Up to 120 minutes ago
                var timestamp = now.AddMinutes(-randomMinutesAgo);

                attendances.Add(new Attendance
                {
                    UserId = userId,
                    FitnessCentarId = fitnessCenterId,
                    Timestamp = timestamp
                });
            }
        }
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
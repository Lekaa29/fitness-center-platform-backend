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
    
    public async Task<bool> AddAttendanceAsync(Attendance attendance) 
    {
        await _context.Attendances.AddAsync(attendance);
        return await SaveAsync();
    }
    
    private async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

}
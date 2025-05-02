using FitnessCenterApi.Data;
using FitnessCenterApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterApi.Repositories;

public class CoachRepository
{
    private readonly FitnessCenterDbContext _context;
    
    public CoachRepository(FitnessCenterDbContext context)
    {
        _context = context;
    }
    
  
    
    public async  Task<Coach?> GetCoachAsync(int coachId)
    {
        var coach = await _context.Coaches.Where(
            c => c.IdCoach == coachId
        ).FirstOrDefaultAsync();
        return coach;
    }
    
    public async  Task<CoachProgram?> GetCoachProgramAsync(int coachProgramId)
    {
        var coach = await _context.CoachPrograms.Where(
            c => c.IdCoachProgram == coachProgramId
        ).FirstOrDefaultAsync();
        return coach;
    }
    

    
    public async Task<bool> AddCoachAsync(Coach coach) 
    {
        await _context.Coaches.AddAsync(coach);
        return await SaveAsync();
    }
    
    public async Task<bool> AddCoachProgramAsync(CoachProgram coachProgram) 
    {
        await _context.CoachPrograms.AddAsync(coachProgram);
        return await SaveAsync();
    }

    
    private async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
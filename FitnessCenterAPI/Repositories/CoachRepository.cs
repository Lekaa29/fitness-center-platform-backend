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
            c => c.Id == coachProgramId
        ).FirstOrDefaultAsync();
        return coach;
    }
    
    public async  Task<ICollection<CoachProgram>> GetCoachProgramsAsync(int coachId)
    {

        var coachPrograms = await _context.CoachPrograms.Where(
            m => m.IdCoach == coachId).Include(m => m.Coach).ToListAsync();
      
        return coachPrograms;
    }
    
    public async Task<ICollection<Coach>> GetFitnessCentarsCoachesAsync(int fitnessCentarId)
    {
        var coachPrograms = await _context.CoachPrograms
            .Where(c => c.IdFitnessCentar == fitnessCentarId)
            .Include(m => m.Coach)
            .ThenInclude(c => c.User) // Dodaješ ovo za učitavanje povezanog Usera
            .ToListAsync();

        var coaches = coachPrograms.Select(m => m.Coach).Distinct().ToList();
        return coaches;
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

    public async Task<bool> UpdateCoachAsync(Coach coach) 
    {
        _context.Coaches.Update(coach);
        return await SaveAsync();
    }

    public async Task<bool> DeleteCoachAsync(Coach coach) 
    {
        _context.Coaches.Remove(coach);
        return await SaveAsync();
    }


    
    private async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
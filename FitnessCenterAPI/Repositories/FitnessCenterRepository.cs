using FitnessCenterApi.Data;
using FitnessCenterApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterApi.Repositories;

public class FitnessCenterRepository
{
    private readonly FitnessCenterDbContext _context;
    
    public FitnessCenterRepository(FitnessCenterDbContext context)
    {
        _context = context;
    }
    
    public async  Task<FitnessCentar> GetFitnessCenterAsync(int fitnessCenterId)
    {
        var fitnessCenter = await _context.FitnessCentars.Where(
            f => f.IdFitnessCentar == fitnessCenterId).FirstOrDefaultAsync();
        
        return fitnessCenter;
    }
    public async  Task<ICollection<FitnessCentar>> GetFitnessCentersAsync()
    {
        var fitnessCenter = await _context.FitnessCentars.ToListAsync();
        
        return fitnessCenter;
    }
    
    public async Task<bool> AddFitnessCenterAsync(FitnessCentar fitnessCentar) 
    {
        await _context.FitnessCentars.AddAsync(fitnessCentar);
        return await SaveAsync();
    }
    
    public async Task<bool> UpdateFitnessCenterAsync(FitnessCentar fitnessCentar)
    {
        _context.FitnessCentars.Update(fitnessCentar);
        return await SaveAsync();
    }

    public async Task<bool> DeleteFitnessCenterAsync(FitnessCentar fitnessCentar)
    {
        _context.FitnessCentars.Remove(fitnessCentar);
        return await SaveAsync();
    }

    
    private async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
    
}
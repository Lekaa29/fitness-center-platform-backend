using FitnessCenterApi.Data;
using FitnessCenterApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterApi.Repositories;

public class MembershipRepository
{
    private readonly FitnessCenterDbContext _context;
    
    public MembershipRepository(FitnessCenterDbContext context)
    {
        _context = context;
    }
    
    public async  Task<ICollection<Membership>> GetUserMembershipsAsync(int userId)
    {   

        var membership = await _context.Memberships.Where(
            m => m.IdUser == userId).ToListAsync();
      
        return membership;
    }
    
    public async  Task<Membership?> GetUserMembershipByFitnessCenterAsync(int userId, int fitnessCenterId)
    {

        var membership = await _context.Memberships.Where(
            m => m.IdUser == userId && m.IdFitnessCentar == fitnessCenterId).FirstOrDefaultAsync();
        return membership;
    }
    
    public async  Task<ICollection<Membership>> GetFitnessCenterMembershipsAsync(int fitnessCenterId)
    {

        var membership = await _context.Memberships.Where(
            m => m.IdFitnessCentar == fitnessCenterId).ToListAsync();
      
        return membership;
    }
    
    public async Task<bool> AddMembershipAsync(Membership membership) 
    {
        await _context.Memberships.AddAsync(membership);
        return await SaveAsync();
    }
    public async Task<bool> UpdateMembershipAsync(Membership membership) 
    {
        _context.Memberships.Update(membership);
        return await SaveAsync();
    }
        
        
    private async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
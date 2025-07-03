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
    
    public async Task<ICollection<Membership>> GetFitnessCenterLeaderboardAsync(int fitnessCenterId)
    {
        var membership = await _context.Memberships
            .Where(m => m.IdFitnessCentar == fitnessCenterId)
            .OrderByDescending(m => m.Points)
            .Take(50)
            .ToListAsync();

        return membership;
    }

    
    public async Task<bool> AddMembershipAsync(Membership membership) 
    {
        await _context.Memberships.AddAsync(membership);
        return await SaveAsync();
    }
    
    public async Task<bool> AddMembershipPackageAsync(MembershipPackage membershipPackage) 
    {
        await _context.MembershipPackages.AddAsync(membershipPackage);
        return await SaveAsync();
    }
    
    public async  Task<MembershipPackage> GetFitnessCenterMembershipPackageAsync(int? membershipPackageId)
    {

        var membershipPackage = await _context.MembershipPackages.Where(
            m => m.Id == membershipPackageId)
            .Include(m => m.FitnessCentar).FirstOrDefaultAsync();
      
        return membershipPackage;
    }
    
    public async  Task<ICollection<MembershipPackage>> GetFitnessCenterMembershipPackagesAsync(int fitnessCenterId)
    {

        var membershipPackages = await _context.MembershipPackages.Where(
            m => m.IdFitnessCentar == fitnessCenterId).ToListAsync();
      
        return membershipPackages;
    }
    
    public async  Task<ICollection<MembershipPackage>> GetAllMembershipPackagesAsync()
    {

        var membershipPackages = await _context.MembershipPackages
            .Include(mp => mp.FitnessCentar)
            .ToListAsync();
        
        return membershipPackages;
    }
    
    public async Task<bool> UpdateMembershipAsync(Membership membership) 
    {
        _context.Memberships.Update(membership);
        return await SaveAsync();
    }
    
    public async Task<Membership?> GetMembershipAsync(int membershipId)
    {
        var membership = await _context.Memberships
            .Where(m => m.IdMembership == membershipId)
            .FirstOrDefaultAsync();

        return membership;
    }


    public async Task<bool> DeleteMembershipAsync(Membership membership)
    {
        _context.Memberships.Remove(membership);
        return await SaveAsync();
    }

        
        
    private async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
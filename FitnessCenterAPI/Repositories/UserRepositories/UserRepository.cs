using FitnessCenterApi.Data;
using FitnessCenterApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterApi.Repositories.UserRepositories;

public class UserRepository
{
    private readonly FitnessCenterDbContext _context;

    public UserRepository(FitnessCenterDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> CheckIfUserEmailExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }
    
    public async Task<bool> CheckIfUserUsernameExistsAsync(string username)
    {
        return await _context.Users.AnyAsync(u => u.UserName == username);
    }
    
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
    }
    
    public async Task<User?> GetUserAsync(int userId)
    {
        return await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
    }
    
}
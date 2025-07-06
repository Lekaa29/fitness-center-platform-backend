using FitnessCenterApi.Data;
using FitnessCenterApi.Dtos.Chat;
using FitnessCenterApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterApi.Repositories;

public class ShopRepository
{
    private readonly FitnessCenterDbContext _context;
    
    public ShopRepository(FitnessCenterDbContext context)
    {
        _context = context;
    }
    
    public async  Task<ICollection<ShopItem>> GetFitnessCenterItemsAsync(int fitnessCenterId)
    {   
        var shopItems = await _context.ShopItems.Where(
            s => s.IdFitnessCentar == fitnessCenterId).ToListAsync();
      
        return shopItems;
    }
    
    public async  Task<ICollection<ShopItem>> GetUserItemsAsync(int userId)
    {
        var userItems = await _context.UserItems
            .Where(s => s.IdUser == userId)
            .ToListAsync();

        var shopItemIds = userItems.Select(ui => ui.IdShopItem).ToList();

        var shopItems = await _context.ShopItems
            .Where(s => shopItemIds.Contains(s.IdShopItem))
            .ToListAsync();

        return shopItems;
    }
    
    public async  Task<ShopItem?> GetShopItemAsync(int shopItemId)
    {

        var shopItem = await _context.ShopItems.Where(
            s => s.IdShopItem == shopItemId).FirstOrDefaultAsync();

        return shopItem;
    }
    
    public async  Task<ICollection<Membership>> GetFitnessCenterMembershipsAsync(int fitnessCenterId)
    {

        var membership = await _context.Memberships.Where(
            m => m.IdFitnessCentar == fitnessCenterId).ToListAsync();
      
        return membership;
    }
    
    public async Task<bool> AddShopItemAsync(ShopItem shopItem) 
    {
        await _context.ShopItems.AddAsync(shopItem);
        return await SaveAsync();
    }
    public async Task<bool> AddUserItem(UserItems userItems) 
    {
        _context.UserItems.Add(userItems);
        return await SaveAsync();
    }
    public async Task<bool> UpdateShopItemAsync(ShopItem shopItem)
    {
        _context.ShopItems.Update(shopItem);
        return await SaveAsync();
    }

    public async Task<bool> DeleteShopItemAsync(ShopItem shopItem)
    {
        _context.ShopItems.Remove(shopItem);
        return await SaveAsync();
    }

        
        
    private async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
namespace FitnessCenterApi.Models;

public class UserItems
{
    public int IdUser { get; set; }
    public User User { get; set; }

    public int IdShopItem { get; set; }
    public ShopItem ShopItem { get; set; }
}
namespace FitnessCenterApi.Dtos.Chat;

public class ShopItemDto
{
    public int IdShopItem { get; set; }

    public int IdFitnessCentar { get; set; }

    public string Name { get; set; } = string.Empty;
    public string PictureUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal LoyaltyPrice { get; set; }
}
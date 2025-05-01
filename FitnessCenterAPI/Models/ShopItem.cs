using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApi.Models;

public class ShopItem
{
    [Key]
    public int IdShopItem { get; set; }

    public int IdFitnessCentar { get; set; }
    public FitnessCentar FitnessCentar { get; set; }

    public string Name { get; set; } = string.Empty;
    public string PictureUrl { get; set; } = string.Empty;

    public decimal Price { get; set; }
    public decimal LoyaltyPrice { get; set; }
}
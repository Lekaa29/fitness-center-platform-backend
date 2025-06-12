using System.Security.Claims;
using FitnessCenterApi.Dtos;
using FitnessCenterApi.Dtos.Chat;
using FitnessCenterApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessCenterApi.Controllers;

[Route("api/Shop")]
[ApiController]
public class ShopController : ControllerBase
{
    private readonly ShopService _shopService;

    public ShopController(ShopService shopService)
    {
        _shopService = shopService;
    }
    
    [HttpGet("items/{fitnessCenterId}")]
    [ProducesResponseType(200, Type = typeof(List<ShopItemDto>))]
    public async Task<IActionResult> GetFitnessCenterItems(int fitnessCenterId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }

        var shopItems = await _shopService.GetFitnessCenterItemsAsync(email, fitnessCenterId);
        return Ok(shopItems);
    }
    
    [HttpGet("items/users")]
    [ProducesResponseType(200, Type = typeof(List<ShopItemDto>))]
    public async Task<IActionResult> GetUserItems()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }

        var shopItems = await _shopService.GetUserItemsAsync(email);
        return Ok(shopItems);
    }
    
    [HttpGet("item/{shopItemId}")]
    [ProducesResponseType(200, Type = typeof(ShopItemDto))]
    public async Task<IActionResult> GetShopItem(int shopItemId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }

        var shopItem = await _shopService.GetShopItemAsync(shopItemId, email);
        return Ok(shopItem);
    }
    
    [HttpPost("BuyShopItem/{shopItemId}")]
    public async Task<IActionResult> BuyShopItem(int shopItemId)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
     
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }

        var result = await _shopService.BuyShopItem(email, shopItemId);
        if (result)
        {
            return Ok("Item bought successfully");
        }
        return BadRequest("Item not bought");
    }   
    
    [HttpPost("AddShopItem")]
    public async Task<IActionResult> AddShopItem([FromBody] ShopItemDto shopItemDto)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
     
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }
        if (shopItemDto == null)
        {
            return BadRequest("shopItem object is null");
        }
        var result = await _shopService.AddShopItemAsync(shopItemDto, email);
        if (result)
        {
            return Ok("shopItem added successfully");
        }
        return BadRequest("shopItem not added");
    }   
}


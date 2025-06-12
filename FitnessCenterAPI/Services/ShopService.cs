using AutoMapper;
using FitnessCenterApi.Dtos;
using FitnessCenterApi.Dtos.Chat;
using FitnessCenterApi.Models;
using FitnessCenterApi.Repositories;
using FitnessCenterApi.Repositories.UserRepositories;

namespace FitnessCenterApi.Services;

public class ShopService
{
    private readonly IMapper _mapper;
    private readonly ShopRepository _shopRepository;
    private readonly UserRepository _userRepository;
    private readonly FitnessCenterRepository _fitnessCenterRepository;

    public ShopService(IMapper mapper, ShopRepository shopRepository, UserRepository userRepository, FitnessCenterRepository fitnessCenterRepository)
    {
        _mapper = mapper;
        _shopRepository = shopRepository;
        _userRepository = userRepository;
        _fitnessCenterRepository = fitnessCenterRepository;
    }
    
    public async Task<ICollection<ShopItemDto>?> GetFitnessCenterItemsAsync(string email, int fitnessCenterId)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return null;
        }
        var shopItems = await _shopRepository.GetFitnessCenterItemsAsync(fitnessCenterId);

        var shopItemsDtos = _mapper.Map<ICollection<ShopItemDto>>(shopItems);
        
        return shopItemsDtos;
    }
    
    public async Task<ICollection<ShopItemDto>?> GetUserItemsAsync(string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return null;
        }
        var shopItems = await _shopRepository.GetUserItemsAsync(user.Id);

        var shopItemsDtos = _mapper.Map<ICollection<ShopItemDto>>(shopItems);
        
        return shopItemsDtos;
    }
    
    public async Task<MembershipDto?> GetShopItemAsync(int shopItemId, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return null;
        }
        var shopItem = await _shopRepository.GetShopItemAsync(shopItemId);

        var shopItemDto = _mapper.Map<MembershipDto>(shopItem);
        
        return shopItemDto;
    }
    
    public async Task<bool> AddShopItemAsync(ShopItemDto shopItemDto, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return false;
        }
        var shopItem = _mapper.Map<ShopItem>(shopItemDto);
        
        shopItem.FitnessCentar = await _fitnessCenterRepository.GetFitnessCenterAsync(shopItemDto.IdFitnessCentar);

        return await _shopRepository.AddShopItemAsync(shopItem);
    }
    
    public async Task<bool> BuyShopItem(string email, int shopItemId)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return false;
        }
        
        var shopItem = await _shopRepository.GetShopItemAsync(shopItemId);
        if (shopItem == null)
        {
            return false;
        }
        
        UserItems userItems = new UserItems();
        userItems.IdUser = user.Id;
        userItems.IdShopItem = shopItemId;
        userItems.User = user;
        userItems.ShopItem = shopItem;


        return await _shopRepository.AddUserItem(userItems);
    }
    
    
}
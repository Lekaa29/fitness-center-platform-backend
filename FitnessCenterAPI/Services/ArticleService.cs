using AutoMapper;
using FitnessCenterApi.Dtos;
using FitnessCenterApi.Dtos.Chat;
using FitnessCenterApi.Models;
using FitnessCenterApi.Repositories;
using FitnessCenterApi.Repositories.UserRepositories;

namespace FitnessCenterApi.Services;

public class ArticleService
{
    private readonly IMapper _mapper;
    private readonly ShopRepository _shopRepository;
    private readonly UserRepository _userRepository;
    private readonly FitnessCenterRepository _fitnessCenterRepository;
    private readonly ArticleRepository _articleRepository;
    private readonly IConfiguration _configuration;


    public ArticleService(IMapper mapper, IConfiguration configuration, ArticleRepository articleRepository, ShopRepository shopRepository, UserRepository userRepository, FitnessCenterRepository fitnessCenterRepository)
    {
        _mapper = mapper;
        _shopRepository = shopRepository;
        _userRepository = userRepository;
        _fitnessCenterRepository = fitnessCenterRepository;
        _configuration = configuration;
        
        _articleRepository = articleRepository;
    }
    
    public async Task<ICollection<ArticleDto>?> GetFitnessCenterArticlesAsync(string email, int fitnessCenterId)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return null;
        }
        var articles = await _articleRepository.GetFitnessCenterArticlesAsync(fitnessCenterId);

        var articlesDtos = _mapper.Map<ICollection<ArticleDto>>(articles);
        
        return articlesDtos;
    }
    
    
    public async Task<bool> AddArticleAsync(ArticleDto articleDto, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return false;
        }
        var article = _mapper.Map<Article>(articleDto);
        
        article.FitnessCentar = await _fitnessCenterRepository.GetFitnessCenterAsync(articleDto.IdFitnessCentar);
        article.IdFitnessCentar = article.FitnessCentar.IdFitnessCentar;
        
        return await _articleRepository.AddArticleAsync(article);
    }
    
    public async Task<bool> UpdateArticleAsync(int articleId, ArticleDto articleDto, string email)
    {
        var article = await _articleRepository.GetArticleAsync(articleId);
        if (article == null)
        {
            return false; // Either not found or user is not the owner
        }
        if (email != _configuration["AdminSettings:AdminEmail"])
        {
            return false;
        }

        _mapper.Map(articleDto, article);

        return await _articleRepository.UpdateArticleAsync(article);
    }

    public async Task<bool> DeleteArticleAsync(int articleId, string email)
    {
        var article = await _articleRepository.GetArticleAsync(articleId);
        if (article == null)
        {
            return false; 
        }
        if (email != _configuration["AdminSettings:AdminEmail"])
        {
            return false;
        }

        return await _articleRepository.DeleteArticleAsync(article);
    }

    
  
    
    
}
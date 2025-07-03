using FitnessCenterApi.Data;
using FitnessCenterApi.Dtos.Chat;
using FitnessCenterApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterApi.Repositories;

public class ArticleRepository
{
    private readonly FitnessCenterDbContext _context;
    
    public ArticleRepository(FitnessCenterDbContext context)
    {
        _context = context;
    }
    
    public async  Task<ICollection<Article>> GetFitnessCenterArticlesAsync(int fitnessCenterId)
    {   
        var articles = await _context.Articles.Where(
            a => a.IdFitnessCentar == fitnessCenterId).ToListAsync();
      
        return articles;
    }
    
    public async  Task<Article?> GetArticleAsync(int articleId)
    {
        var article = await _context.Articles.Where(
            s => s.IdArticle == articleId).FirstOrDefaultAsync();

        return article;
    }
    
    public async Task<bool> AddArticleAsync(Article article) 
    {
        await _context.Articles.AddAsync(article);
        return await SaveAsync();
    }
    
    public async Task<bool> UpdateArticleAsync(Article article) 
    {
        _context.Articles.Update(article);
        return await SaveAsync();
    }
    public async Task<bool> DeleteArticleAsync(Article article) 
    {
        _context.Articles.Remove(article);
        return await SaveAsync();
    }
        
    private async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
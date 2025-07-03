using System.Security.Claims;
using FitnessCenterApi.Dtos;
using FitnessCenterApi.Dtos.Chat;
using FitnessCenterApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessCenterApi.Controllers;

[Route("api/Article")]
[ApiController]
public class ArticleController : ControllerBase
{
    private readonly ArticleService _articleService;

    public ArticleController(ArticleService articleService)
    {
        _articleService = articleService;
    }
    
    [HttpGet("articles/{fitnessCentarId}")]
    [ProducesResponseType(200, Type = typeof(List<ArticleDto>))]
    public async Task<IActionResult> GetFitnessCenterArticles(int fitnessCentarId)
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

        var articles = await _articleService.GetFitnessCenterArticlesAsync(email, fitnessCentarId);
        return Ok(articles);
    }
    
    [HttpPost("AddArticle")]
    public async Task<IActionResult> AddArticle([FromBody] ArticleDto articleDto)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
     
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }
        if (articleDto == null)
        {
            return BadRequest("Article object is null");
        }
        var result = await _articleService.AddArticleAsync(articleDto, email);
        if (result)
        {
            return Ok("Article added successfully");
        }
        return BadRequest("Article not added");
    }   
    
    [HttpPut("UpdateArticle/{articleId}")]
    public async Task<IActionResult> UpdateArticle(int articleId, [FromBody] ArticleDto articleDto)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }

        if (articleDto == null)
        {
            return BadRequest("Article object is null");
        }

        var result = await _articleService.UpdateArticleAsync(articleId, articleDto, email);
        if (result)
        {
            return Ok("Article updated successfully");
        }

        return BadRequest("Article not updated");
    }
    [HttpDelete("DeleteArticle/{articleId}")]
    public async Task<IActionResult> DeleteArticle(int articleId)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }

        var result = await _articleService.DeleteArticleAsync(articleId, email);
        if (result)
        {
            return Ok("Article deleted successfully");
        }

        return BadRequest("Article not deleted");
    }

}


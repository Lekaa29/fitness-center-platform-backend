namespace FitnessCenterApi.Dtos;

public class ArticleDto
{
    public int IdArticle { get; set; }

    public int IdFitnessCentar { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public string? CoverPictureLink { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.UtcNow;
}
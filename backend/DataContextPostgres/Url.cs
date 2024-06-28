namespace linkApi.Entities;
public class Url
{
    public int Id { get; set; }

    public string? Hash { get; set; }

    public string OriginalUrl { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public int? AccessCount { get; set; }

    public Url()
    {
        CreatedAt = DateTime.Now;
    }
}

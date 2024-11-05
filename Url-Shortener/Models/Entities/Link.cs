using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models.Entities;

public class Link(string originalUrl, string shortUrl, string shortUrlId)
{
    [Key]
    [Required]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    [Required] public string OriginalUrl { get; set; } = originalUrl;
    [Required] public string ShortUrl { get; set; } = shortUrl;
    [Required] public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.UtcNow;
    [Required] public string ShortUrlId { get; set; } = shortUrlId;
}

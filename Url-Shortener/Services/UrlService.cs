using System.Security.Cryptography;
using System.Text;
using UrlShortener.Data.Abstractions;
using UrlShortener.Models.Dtos;
using UrlShortener.Models.Entities;
using UrlShortener.Services.Abstractions;

namespace UrlShortener.Services;

public class UrlService(
    IConfiguration configuration,
    IRepository<Link> linkRepository) : IUrlService
{
    private readonly IRepository<Link> _linkRepository = linkRepository;

    public async Task<Result> CreateShortUrl(string url)
    {
        if (!IsValidUrl(url))
            return new Error[] { new("Url.Error", "Invalid URL format.") };

        var shortUrlId = await GenerateUniqueShortUrlId();

        var link = new Link(
            url,
            shortUrlId,
            shortUrlId);

        var shortUrl = link.ShortUrl;

        await _linkRepository.AddAsync(link);
        await _linkRepository.SaveChangesAsync();

        return Result<string>.Success(shortUrl);
    }

    private bool IsValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    private async Task<string> GenerateUniqueShortUrlId()
    {
        string shortUrlId;
        bool exists;

        do
        {
            shortUrlId = GenerateShortUrlId_v2();
            exists = await _linkRepository.ExistsAsync(l => l.ShortUrlId == shortUrlId);
        } while (exists);

        return shortUrlId;
    }

    private string GenerateShortUrlId()
    {
        var date = DateTime.Now;

        return $"{date:dd}{date:dd}{date:MM}{date:HH}{date:mm}{date:yy}";
    }

    private string GenerateShortUrlId_v2()
    {
        // Generate a unique identifier using a hashed value and encode it
        using var sha256 = SHA256.Create();
        var urlBytes = Encoding.UTF8.GetBytes(DateTime.Now.ToString("yyyyMMddHHmmssffff"));
        var hashBytes = sha256.ComputeHash(urlBytes);

        // Convert to Base64 and truncate for a short URL
        return Convert.ToBase64String(hashBytes)
            .Substring(0, 8)
            .Replace("/", "_")
            .Replace("+", "-");
    }

    public async Task<Result> GoToOriginalUrl(string id)
    {
        var link = await _linkRepository.GetSingleAsync( x => x.ShortUrlId == id);

        if (link != null) return Result<string>.Success(
            link.OriginalUrl);

        return new Error[] { new("Url.Error", "Url does not exist.") };
    }
}

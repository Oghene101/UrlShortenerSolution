using UrlShortener.Models.Dtos;

namespace UrlShortener.Services.Abstractions;

public interface IUrlService
{
    Task<Result> CreateShortUrl(string shortUrlRequest);
    Task<Result> GoToOriginalUrl(string id);
}

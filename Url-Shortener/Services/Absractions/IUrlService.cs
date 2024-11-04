using Url_Shortener.Models.Dtos;

namespace Url_Shortener.Services.Abstractions;

public interface IUrlService
{
    Task<Result> CreateShortUrl(CreateShortUrlDto shortUrlRequest);
    Task<Result> GoToOriginalUrl(string id);
}

using Microsoft.AspNetCore.Mvc;
using Url_Shortener.Models.Dtos;
using Url_Shortener.Services.Abstractions;

namespace Url_Shortener.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UrlController(IUrlService urlService) : ControllerBase
{
    private readonly IUrlService _urlService = urlService;

    [HttpPost("CreateShortUrl")]
    public async Task<IActionResult> CreateShortUrl([FromBody] CreateShortUrlDto shortUrlRequest)
    {
        var result = await _urlService.CreateShortUrl(shortUrlRequest);

        if (result.IsFailure) return BadRequest(
            ResponseDto<object>.Failure(result.Errors!));

        if (result is Result<LinkDto> success) return Ok(
            ResponseDto<LinkDto>.Success(
                "Short url successfully generated.",
                data: success.Data));

        return BadRequest();
    }

    [HttpGet("/OG/{id}")]
    public async Task<IActionResult> GoToOriginalUrl([FromRoute] string id)
    {
        var result = await _urlService.GoToOriginalUrl(id);

        if (result.IsFailure) return BadRequest(
            ResponseDto<object>.Failure(result.Errors!));

        if (result is Result<string> success) return Ok(
            ResponseDto<string>.Success(data: success.Data));

        return BadRequest();
    }
}

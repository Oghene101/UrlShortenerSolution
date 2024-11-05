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
    public async Task<IActionResult> CreateShortUrl([FromBody] string url)
    {
        var result = await _urlService.CreateShortUrl(url);

        if (result.IsFailure) return BadRequest(
            ResponseDto<object>.Failure(result.Errors!));

        if (result is Result<string> success) return Ok(
            ResponseDto<string>.Success(
                "Short url successfully generated.",
                data: success.Data));

        return BadRequest();
    }

    [HttpGet("/{id}")]
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

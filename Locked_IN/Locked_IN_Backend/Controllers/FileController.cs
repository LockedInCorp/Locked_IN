using Locked_IN_Backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Locked_IN_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly IFileUploadService _fileUploadService;
    
    public FileController(IFileUploadService fileUploadService)
    {
        _fileUploadService = fileUploadService;
    }
    
    [HttpGet("avatar/{filename}")]
    public async Task<IActionResult> GetUserAvatar(string filename)
    {
        var avatar = await _fileUploadService.GetUserAvatarAsync(filename);
        if (avatar == null)
        {
            return NotFound();
        }

        return File(avatar.OpenReadStream(), avatar.ContentType, avatar.FileName);
    }

    [HttpGet("teamicon/{filename}")]
    public async Task<IActionResult> GetTeamIcon(string filename)
    {
        var avatar = await _fileUploadService.GetTeamIconAsync(filename);
        if (avatar == null)
        {
            return NotFound();
        }

        return File(avatar.OpenReadStream(), avatar.ContentType, avatar.FileName);
    }
}
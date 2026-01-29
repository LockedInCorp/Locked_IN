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
    
    [HttpGet("{prefix}/{filename}")]
    public async Task<IActionResult> GetFile(string prefix, string filename)
    {
<<<<<<< Updated upstream
        var avatar =await  _fileUploadService.GetUserAvatarAsync(filename);
        return File(avatar.OpenReadStream(), avatar.ContentType, avatar.FileName);
    }
    
    [HttpGet("teamicon/{filename}")]
    public async Task<IActionResult> GetTeamIcon(string filename)
    {
        var avatar =await  _fileUploadService.GetUserAvatarAsync(filename);
        return File(avatar.OpenReadStream(), avatar.ContentType, avatar.FileName);
=======
        var file = await _fileUploadService.GetFileAsync(prefix, filename);
        if (file == null)
        {
            return NotFound();
        }

        return File(file.OpenReadStream(), file.ContentType, file.FileName);
>>>>>>> Stashed changes
    }
}
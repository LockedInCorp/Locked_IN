using Locked_IN_Backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Locked_IN_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommunicationServiceController : ControllerBase
{
    private readonly ICommunicationService _communicationService;

    public CommunicationServiceController(ICommunicationService communicationService)
    {
        _communicationService = communicationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCommunicationServicesAsync()
    {
        var result = await _communicationService.GetCommunicationServicesAsync();
        return Ok(result);
    }
}

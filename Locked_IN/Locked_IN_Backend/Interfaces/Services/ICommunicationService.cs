using Locked_IN_Backend.DTOs;

namespace Locked_IN_Backend.Interfaces;

public interface ICommunicationService
{
    Task<List<CommunicationServiceDto>> GetCommunicationServicesAsync();
}

using Locked_IN_Backend.Data;
using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Services;

public class CommunicationServiceImplementation : ICommunicationService
{
    private readonly AppDbContext _context;

    public CommunicationServiceImplementation(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CommunicationServiceDto>> GetCommunicationServicesAsync()
    {
        return await _context.CommunicationServices
            .Select(cs => new CommunicationServiceDto
            {
                Id = cs.Id,
                Name = cs.Name
            })
            .ToListAsync();
    }
}

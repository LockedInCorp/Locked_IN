using AutoMapper;
using Locked_IN_Backend.Data;
using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Services;

public class CommunicationService : ICommunicationService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CommunicationService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CommunicationServiceDto>> GetCommunicationServicesAsync()
    {
        var services = await _context.CommunicationServices.ToListAsync();
        return _mapper.Map<List<CommunicationServiceDto>>(services);
    }
}

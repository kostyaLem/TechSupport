using Microsoft.EntityFrameworkCore;
using TechSupport.BusinessLogic.Exceptions;
using TechSupport.BusinessLogic.Interfaces;
using TechSupport.BusinessLogic.Mapping;
using TechSupport.BusinessLogic.Models.RequestModels;
using TechSupport.DataAccess.Context;
using Domain = TechSupport.DataAccess.Models;

namespace TechSupport.BusinessLogic.Services;

internal class RequestService : IRequestService
{
    private readonly TechSupportContext _context;

    public RequestService(TechSupportContext context)
    {
        _context = context;
    }

    public async Task Create(CreateRequest request)
    {
        _context.Requests.Add(request.ToDomain());
        await _context.SaveChangesAsync();
    }

    public async Task<Request> GetRequestById(int id)
    {
        var request = await GetRequest(id);

        return request.ToBl();
    }

    public async Task<IReadOnlyList<Request>> GetRequests()
    {
        var requests = await _context.Requests
            //.AsNoTracking()
            .ToListAsync();

        return requests.Select(x => x.ToBl()).ToList();
    }

    public async Task Remove(int requestId)
    {
        var request = await GetRequest(requestId);

        _context.Requests.Remove(request);
        await _context.SaveChangesAsync();
    }

    public Task UpdateStatus(RequestStatus requestStatus)
    {
        throw new NotImplementedException();
    }

    private async Task<Domain.Request> GetRequest(int requestId)
    {
        var request = await _context.Requests.FindAsync(requestId);

        if (request is null)
        {
            throw new NotFoundException("Заявка не найдена.");
        }

        return request;
    }
}

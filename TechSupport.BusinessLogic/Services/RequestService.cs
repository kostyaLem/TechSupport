using TechSupport.BusinessLogic.Interfaces;
using TechSupport.BusinessLogic.Models.RequestModels;
using TechSupport.DataAccess.Context;

namespace TechSupport.BusinessLogic.Services;

internal class RequestService : IRequestService
{
    private readonly TechSupportContext _context;

    public RequestService(TechSupportContext context)
    {
        _context = context;
    }

    public Task Create(CreateRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<CreateRequest> GetRequestById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<CreateRequest>> GetRequests()
    {
        throw new NotImplementedException();
    }

    public Task Remove(int requestId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateStatus()
    {
        throw new NotImplementedException();
    }
}

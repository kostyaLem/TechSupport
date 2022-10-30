using TechSupport.BusinessLogic.Models.RequestModels;

namespace TechSupport.BusinessLogic.Interfaces;

public interface IRequestService
{
    Task<IReadOnlyList<ExtendedRequest>> GetRequests();
    Task<Request> GetRequestById(int id);
    Task Create(CreateRequest request);
    Task UpdateStatus(RequestStatus status);
    Task Remove(int requestId);
}
using TechSupport.BusinessLogic.Models.RequestModels;

namespace TechSupport.BusinessLogic.Interfaces;

public interface IRequestService
{
    Task<IReadOnlyList<CreateRequest>> GetRequests();
    Task<CreateRequest> GetRequestById(int id);
    Task Create(CreateRequest request);
    Task UpdateStatus();
    Task Remove(int requestId);
}
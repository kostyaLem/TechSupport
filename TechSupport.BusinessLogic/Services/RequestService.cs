﻿using Microsoft.EntityFrameworkCore;
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

    public async Task<IReadOnlyList<ExtendedRequest>> GetRequests()
    {
        var requests = await _context.Requests.ToListAsync();

        return requests.Select(x => x.ToExtendedBl()).ToList();
    }

    public async Task Remove(int requestId)
    {
        var request = await GetRequest(requestId);

        _context.Requests.Remove(request);
        await _context.SaveChangesAsync();
    }

    public async Task CompleteRequest(int requestId)
    {
        var request = await GetRequest(requestId);

        request.Status = Domain.RequestStatus.Completed;
        request.StatusUpdatedOn = DateTime.Now;

        await _context.SaveChangesAsync();
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

    public async Task Update(ExtendedRequest updateRequest)
    {
        var request = await GetRequest(updateRequest.Id);

        request.Title = updateRequest.Title;
        request.Description = updateRequest.Description;
        request.Computer = updateRequest.Computer;
        request.UserId = updateRequest.User.Id;
        request.DepartmentId = updateRequest.Department.Id;
        request.RequestCategoryId = updateRequest.Category.Id;

        var status = updateRequest.RequestStatus.ToDomain();
        if (request.Status != status)
        {
            request.Status = status;
            request.StatusUpdatedOn = DateTime.Now;
        }

        await _context.SaveChangesAsync();
    }
}

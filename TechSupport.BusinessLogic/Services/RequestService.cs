using Microsoft.EntityFrameworkCore;
using TechSupport.BusinessLogic.Exceptions;
using TechSupport.BusinessLogic.Interfaces;
using TechSupport.BusinessLogic.Mapping;
using TechSupport.BusinessLogic.Models.RequestModels;
using TechSupport.DataAccess.Context;
using Domain = TechSupport.DataAccess.Models;

namespace TechSupport.BusinessLogic.Services;

/// <summary>
/// Сервис для управления заявками технической поддержки
/// </summary>
internal class RequestService : IRequestService
{
    private readonly TechSupportContext _context;

    public RequestService(TechSupportContext context)
    {
        _context = context;
    }

    // Метод создания заявки
    public async Task Create(CreateRequest request)
    {
        // Добавить сущность в базу 
        _context.Requests.Add(request.ToDomain());
        // Сохранить изменения в базе
        await _context.SaveChangesAsync();
    }

    // Получить заявку по Id
    public async Task<Request> GetRequestById(int id)
    {
        var request = await GetRequest(id);

        // Преобразование модели
        return request.ToBl();
    }

    // Получить список заявок
    public async Task<IReadOnlyList<ExtendedRequest>> GetRequests()
    {
        var requests = await _context.Requests.ToListAsync();

        // Преобразование модели
        return requests.Select(x => x.ToExtendedBl()).ToList();
    }

    // Удалить заявку по Id
    public async Task Remove(int requestId)
    {
        var request = await GetRequest(requestId);

        // Пометить сущность на удаление
        _context.Requests.Remove(request);
        // Сохранить изменения в базе
        await _context.SaveChangesAsync();
    }

    // Установить зявке статус "Завершено"
    public async Task CompleteRequest(int requestId)
    {
        var request = await GetRequest(requestId);

        // Обновление некоторых полей сущности
        request.Status = Domain.RequestStatus.Completed;
        request.StatusUpdatedOn = DateTime.Now;

        // Сохранить изменения в базе
        await _context.SaveChangesAsync();
    }

    // Получить заявки по Id и выбросить ошибку, если сущности нет
    private async Task<Domain.Request> GetRequest(int requestId)
    {
        // Поиск сущности в базе по Id
        var request = await _context.Requests.FindAsync(requestId);

        if (request is null)
        {
            // Выбросить исключение, если сущность не найдена
            throw new NotFoundException("Заявка не найдена.");
        }

        return request;
    }

    // Обновление заявки
    public async Task Update(ExtendedRequest updateRequest)
    {
        var request = await GetRequest(updateRequest.Id);

        // Обновление некоторых полей сущности
        request.Title = updateRequest.Title;
        request.Description = updateRequest.Description;
        request.Computer = updateRequest.Computer;
        request.UserId = updateRequest.User?.Id ?? null;
        request.DepartmentId = updateRequest.Department.Id;
        request.RequestCategoryId = updateRequest.Category.Id;

        var status = updateRequest.RequestStatus.ToDomain();

        // Изменить время обновления статуса, если он поменялся
        if (request.Status != status)
        {
            request.Status = status;
            request.StatusUpdatedOn = DateTime.Now;
        }

        // Сохранить изменения в базе
        await _context.SaveChangesAsync();
    }
}

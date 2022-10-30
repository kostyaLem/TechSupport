using System.ComponentModel;

namespace TechSupport.BusinessLogic.Models.RequestModels;

public enum RequestStatus
{
    [Description("Создано")]
    Created,
    [Description("В работе")]
    InProgress,
    [Description("На согласовании/уточнении")]
    Paused,
    [Description("Завершено")]
    Completed
}
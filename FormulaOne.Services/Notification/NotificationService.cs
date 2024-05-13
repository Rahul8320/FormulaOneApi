using System.Text.Json;
using FormulaOne.Services.Common;
using FormulaOne.Services.Notification.Interface;

namespace FormulaOne.Services.Notification;

public class NotificationService : INotificationService
{
    public Task SendNotification(NotificationDto notification)
    {
        var data = JsonSerializer.Serialize(notification);
        Console.WriteLine("Sending notification....", data);

        return Task.FromResult(notification);
    }
}

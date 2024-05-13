using FormulaOne.Services.Common;

namespace FormulaOne.Services.Notification.Interface;

public interface INotificationService
{
    Task SendNotification(NotificationDto notification);
}

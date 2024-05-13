namespace FormulaOne.Services.Common;

public class NotificationDto
{
    public Guid DriverId { get; set; } = Guid.Empty;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime TimeStamp { get; set; } = DateTime.Now.ToLocalTime();
}

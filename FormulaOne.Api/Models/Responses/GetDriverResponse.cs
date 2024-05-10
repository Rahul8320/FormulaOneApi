namespace FormulaOne.Api.Models.Responses;

public class GetDriverResponse
{
    public Guid DriverId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int DriverNumber { get; set; }
    public string DateOfBirth { get; set; } = string.Empty;
}

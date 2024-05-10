namespace FormulaOne.DataService.Repositories.Interfaces;

public interface IUnitOfWork
{
    IDriverRepository DriverRepository { get; }
    IAchievementRepository AchievementRepository { get; }

    Task<bool> Complete();
}

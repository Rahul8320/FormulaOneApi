using FormulaOne.Entities.DbSet;

namespace FormulaOne.DataService.Repositories.Interfaces;

public interface IAchievementRepository : IGenericRepository<Achievement>
{
    Task<List<Achievement>> GetDriverAchievement(Guid driverId, CancellationToken cancellationToken = default);
}

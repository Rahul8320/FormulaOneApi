using FormulaOne.DataService.Data;
using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Entities.DbSet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FormulaOne.DataService.Repositories;

public class AchievementRepository(
    ILogger logger,
    AppDbContext context) : GenericRepository<Achievement>(logger, context), IAchievementRepository
{
    private readonly ILogger _logger = logger;
    private readonly DbSet<Achievement> _dbSet = context.Set<Achievement>();

    public async Task<List<Achievement>> GetDriverAchievement(Guid driverId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _dbSet.Where(x => x.DriverId == driverId).ToListAsync(cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                exception: ex,
                message: $"[Repo] GetDriverAchievement function error. Error: {ex.Message}",
                args: typeof(AchievementRepository));
            throw;
        }
    }

    public override async Task<IEnumerable<Achievement>> GetAll(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _dbSet.Where(x => x.Status == 1)
                .AsNoTracking()
                .AsSplitQuery()
                .OrderBy(x => x.AddedDate)
                .ToListAsync(cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                exception: ex,
                message: $"[Repo] Get All function error. Error: {ex.Message}",
                args: typeof(AchievementRepository));
            throw;
        }
    }

    public override async Task<bool> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            // get driver entity
            var existingAchievement = await _dbSet.FindAsync([id], cancellationToken: cancellationToken);

            if (existingAchievement is null)
            {
                return false;
            }

            existingAchievement.Status = 0;
            existingAchievement.UpdatedDate = DateTime.UtcNow;

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                exception: ex,
                message: $"[Repo] Delete function error. Error: {ex.Message}",
                args: typeof(AchievementRepository));
            throw;
        }
    }

    public override async Task<bool> Update(Achievement entity, CancellationToken cancellationToken = default)
    {
        try
        {
            // get driver entity
            var existingAchievement = await _dbSet.FindAsync([entity.Id], cancellationToken: cancellationToken);

            if (existingAchievement is null)
            {
                return false;
            }

            existingAchievement.UpdatedDate = DateTime.UtcNow;
            existingAchievement.FastestLap = entity.FastestLap;
            existingAchievement.RaceWins = entity.RaceWins;
            existingAchievement.PolePosition = entity.PolePosition;
            existingAchievement.WorldChampionship = entity.WorldChampionship;

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                exception: ex,
                message: $"[Repo] Delete function error. Error: {ex.Message}",
                args: typeof(DriverRepository));
            throw;
        }
    }
}

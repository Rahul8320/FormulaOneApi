using FormulaOne.DataService.Data;
using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Entities.DbSet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FormulaOne.DataService.Repositories;

public sealed class DriverRepository(
    ILogger logger,
    AppDbContext context) : GenericRepository<Driver>(logger, context), IDriverRepository
{
    private readonly ILogger _logger = logger;
    private readonly DbSet<Driver> _dbSet = context.Set<Driver>();

    public override async Task<IEnumerable<Driver>> GetAll(CancellationToken cancellationToken = default)
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
                args: typeof(DriverRepository));
            throw;
        }
    }

    public override async Task<bool> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            // get driver entity
            var existingDriver = await _dbSet.FindAsync([id], cancellationToken: cancellationToken);

            if (existingDriver is null)
            {
                return false;
            }

            existingDriver.Status = 0;
            existingDriver.UpdatedDate = DateTime.UtcNow;

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

    public override async Task<bool> Update(Driver entity, CancellationToken cancellationToken = default)
    {
        try
        {
            // get driver entity
            var existingDriver = await _dbSet.FindAsync([entity.Id], cancellationToken: cancellationToken);

            if (existingDriver is null)
            {
                return false;
            }

            existingDriver.UpdatedDate = DateTime.UtcNow;
            existingDriver.DriverNumber = entity.DriverNumber;
            existingDriver.FirstName = entity.FirstName;
            existingDriver.LastName = entity.LastName;
            existingDriver.DateOfBirth = entity.DateOfBirth;

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

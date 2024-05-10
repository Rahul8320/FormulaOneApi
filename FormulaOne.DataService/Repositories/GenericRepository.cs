using FormulaOne.DataService.Data;
using FormulaOne.DataService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FormulaOne.DataService.Repositories;

public class GenericRepository<T>(
    ILogger logger,
    AppDbContext context) : IGenericRepository<T> where T : class
{
    private readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<bool> Add(T entity, CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            logger.LogInformation($"Add new entity of type {entity.GetType().FullName}");
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(
                exception: ex,
                message: $"[Repo] Get By Id function error. Error: {ex.Message}",
                args: typeof(GenericRepository<T>));
            throw;
        }
    }

    public virtual Task<bool> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<T?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _dbSet.FindAsync([id], cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(
                exception: ex,
                message: $"[Repo] Get By Id function error. Error: {ex.Message}",
                args: typeof(GenericRepository<T>));
            throw;
        }
    }

    public virtual Task<bool> Update(T entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

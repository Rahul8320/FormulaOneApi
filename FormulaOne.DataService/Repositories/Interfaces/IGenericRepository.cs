namespace FormulaOne.DataService.Repositories.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken = default);
    Task<T?> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<bool> Add(T entity, CancellationToken cancellationToken = default);
    Task<bool> Update(T entity, CancellationToken cancellationToken = default);
    Task<bool> Delete(Guid id, CancellationToken cancellationToken = default);
}

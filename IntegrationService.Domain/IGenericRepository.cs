using System.Linq.Expressions;

namespace IntegrationServices.Domain
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Query();

        TEntity Get(Expression<Func<TEntity, bool>> expression);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression);
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TEntity>> selector);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TEntity>> selector, CancellationToken cancellationToken = default);
        Task<IEnumerable<TResult>> GetAllAsync<TResult>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken = default);

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);

        bool Exists(params object?[]? keyValues);
        bool Exists(Expression<Func<TEntity, bool>> expression);
        Task<bool> ExistsAsync(params object?[]? keyValues);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    }
}

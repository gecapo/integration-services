using IntegrationServices.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IntegrationServices.Infrastructure
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly IntegrationServiceDbContext _dbContext;
        private readonly DbSet<TEntity> _entitiySet;

        public GenericRepository(IntegrationServiceDbContext context)
        {
            _dbContext = context;
            _entitiySet = _dbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> Query() => _entitiySet.AsQueryable();

        #region Add
        public void Add(TEntity entity)
            => _dbContext.Add(entity);
        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
            => await _dbContext.AddAsync(entity, cancellationToken);
        public void AddRange(IEnumerable<TEntity> entities)
            => _dbContext.AddRange(entities);
        public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
            => await _dbContext.AddRangeAsync(entities, cancellationToken);
        #endregion

        #region Get
        public TEntity Get(Expression<Func<TEntity, bool>> expression) => _entitiySet.FirstOrDefault(expression);

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default) => await _entitiySet.FirstOrDefaultAsync(expression, cancellationToken);

        public IEnumerable<TEntity> GetAll() => _entitiySet.AsEnumerable();

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression) => _entitiySet.Where(expression).AsEnumerable();

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TEntity>> selector)
            => _entitiySet.Where(expression).Select(selector).AsEnumerable();

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _entitiySet.ToListAsync(cancellationToken);

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
            => await _entitiySet.Where(expression).ToListAsync(cancellationToken);

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TEntity>> selector, CancellationToken cancellationToken = default)
            => await _entitiySet.Where(expression).Select(selector).ToListAsync(cancellationToken);

        public async Task<IEnumerable<TResult>> GetAllAsync<TResult>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken = default)
            => await _entitiySet.Where(expression).Select(selector).ToListAsync(cancellationToken);
        #endregion

        #region Remove
        public void Remove(TEntity entity) => _dbContext.Remove(entity);
        public void RemoveRange(IEnumerable<TEntity> entities) => _dbContext.RemoveRange(entities);
        #endregion

        #region Update
        public void Update(TEntity entity) => _dbContext.Update(entity);
        public void UpdateRange(IEnumerable<TEntity> entities) => _dbContext.UpdateRange(entities);

        #endregion

        #region Exists
        public bool Exists(params object?[]? keyValues) => null != _entitiySet.Find(keyValues);
        public bool Exists(Expression<Func<TEntity, bool>> expression) => null != Get(expression);
        public async Task<bool> ExistsAsync(params object?[]? keyValues) => null != await _entitiySet.FindAsync(keyValues);
        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default) => null != await GetAsync(expression, cancellationToken);
        #endregion
    }
}
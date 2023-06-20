using IntegrationServices.Domain;

namespace IntegrationServices.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields
        private readonly IntegrationServiceDbContext _dbContext;
        #endregion

        #region Ctor
        public UnitOfWork(IntegrationServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Properties

        #endregion

        #region Public_Methods
        public void Commit()
            => _dbContext.SaveChanges();

        public async Task CommitAsync()
            => await _dbContext.SaveChangesAsync();

        public void Rollback()
            => _dbContext.Dispose();

        public async Task RollbackAsync()
            => await _dbContext.DisposeAsync();
        #endregion
    }
}
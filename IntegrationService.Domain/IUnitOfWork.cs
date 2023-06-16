namespace IntegrationServices.Domain
{
    public interface IUnitOfWork
    {
        void Commit();
        void Rollback();
        Task CommitAsync();
        Task RollbackAsync();
    }
}

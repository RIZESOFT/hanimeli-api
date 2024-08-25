using HanimeliApp.Domain.Repositories;

namespace HanimeliApp.Domain.UnitOfWorks
{
    public interface IUnitOfWork
    {
        public IGenericRepository<T> Repository<T>() where T : class;

        public Task BeginTransactionAsync();

        public Task CommitAsync();

        public Task RollbackAsync();

        public Task<int> SaveChangesAsync();
    }
}

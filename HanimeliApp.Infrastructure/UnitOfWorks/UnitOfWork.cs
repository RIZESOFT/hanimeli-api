using HanimeliApp.Domain.Repositories;
using HanimeliApp.Domain.UnitOfWorks;
using HanimeliApp.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace HanimeliApp.Infrastructure.UnitOfWorks
{
	public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly HanimeliDbContext _context;
        private Dictionary<string, object>? _repositories;
        private readonly IServiceProvider _serviceProvider;
        private IDbContextTransaction? _currentTransaction;

        public IGenericRepository<T> Repository<T>() where T : class
        {
            _repositories ??= new Dictionary<string, object>();

            var type = typeof(T).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repositoryInstance = _serviceProvider.GetRequiredService<IGenericRepository<T>>();
                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<T>)_repositories[type];
        }
        public UnitOfWork(HanimeliDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }


        public async Task<int> SaveChangesAsync()
        {
            if (_currentTransaction != null)
            {
                try
                {
                    int result = await _context.SaveChangesAsync();
                    await _currentTransaction.CommitAsync();
                    return result;
                }
                catch (Exception)
                {
                    await _currentTransaction.RollbackAsync();
                    throw;
                }
            }
            else
            {
                return await _context.SaveChangesAsync();
            }
        }

        public async Task BeginTransactionAsync()
        {
                _currentTransaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_currentTransaction != null)
            {
                await _context.SaveChangesAsync();
                await _currentTransaction.CommitAsync();
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
            else
            {
                throw new InvalidOperationException("No transaction is in progress.");
            }
        }

        public async Task RollbackAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync();
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
            else
            {
                throw new InvalidOperationException("No transaction is in progress.");
            }
        }

    }
}

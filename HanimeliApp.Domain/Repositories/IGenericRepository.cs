using System.Linq.Expressions;
using HanimeliApp.Domain.Models;

namespace HanimeliApp.Domain.Repositories
{
	public interface IGenericRepository<TEntity> : IGenericRepository<TEntity, int>
        where TEntity : class
    {
    }

    public interface IGenericRepository<TEntity, TPrimaryKey>
    {
        // CREATE operations
        Task<TEntity> InsertAsync(TEntity entity);
        TEntity Insert(TEntity entity);
        Task InsertRangeAsync(IEnumerable<TEntity> entities);

        // READ operations
        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null);

        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null,
            EntityPaging? paging = null);
        
        Task<List<TProjectType>> GetListAndProjectAsync<TProjectType>(Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TProjectType>> projection,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null,
            EntityPaging? paging = null);
        
        IQueryable<TEntity> AsQueryable();
        
        // UPDATE operations
        void Update(TEntity entity);

        // DELETE operations
        void HardDelete(TEntity entity);
        void HardDelete(IEnumerable<TEntity> entities);
        

        Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null);

        Task<bool> ExistAsync(Expression<Func<TEntity, bool>> filter);
    }

}
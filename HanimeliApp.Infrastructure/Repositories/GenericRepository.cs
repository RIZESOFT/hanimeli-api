using System.Linq.Expressions;
using HanimeliApp.Domain.Entities.Abstract;
using HanimeliApp.Domain.Models;
using HanimeliApp.Domain.Repositories;
using HanimeliApp.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace HanimeliApp.Infrastructure.Repositories
{
	public class GenericRepository<TEntity> : BaseRepository<TEntity, int>, IGenericRepository<TEntity>
        where TEntity : class, IEntity<int>
    {
        public GenericRepository(HanimeliDbContext context) : base(context)
        {
        }
    }

    public class BaseRepository<TEntity, TPrimaryKey> : IGenericRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected readonly HanimeliDbContext _context;
        protected readonly DbSet<TEntity> _entitySet;

        public BaseRepository(HanimeliDbContext context)
        {
            _context = context;
            _entitySet = _context.Set<TEntity>();
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return _entitySet.AsNoTracking();
        }
        
        public async Task<TEntity?> GetAsync(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null)
        {
            var query = Query(filter, includes: includes);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<TEntity>> GetListAsync(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null,
            EntityPaging? paging = null)
        {
            var query = Query(filter, orderBy, includes, paging);

            return await query.ToListAsync();
        }
        
        public async Task<List<TProjectType>> GetListAndProjectAsync<TProjectType>(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TProjectType>> projection,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null,
            EntityPaging? paging = null)
        {
            ArgumentNullException.ThrowIfNull(projection);
            var query = Query(filter, orderBy, includes, paging);
            
            var projectionResult = query.Select(projection);
            return await projectionResult.ToListAsync();
        }

        public TEntity Insert(TEntity entity)
        {
            _entitySet.Add(entity);
            return entity;
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            await _entitySet.AddAsync(entity);
            return entity;
        }
        
        public async Task InsertRangeAsync(IEnumerable<TEntity> entities)
        {
            await _entitySet.AddRangeAsync(entities);
        }

        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void HardDelete(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
        }

        public void HardDelete(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities) {
                this.HardDelete(entity);
            }
        }
        
        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            return await _entitySet.CountAsync(filter ?? (x => true));
        }

        public async Task<bool> ExistAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _entitySet.AnyAsync(filter);
        }
        
        #region Utilities

        protected IQueryable<TEntity> Query(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null,
            EntityPaging? paging = null)
        {
            IQueryable<TEntity> query = _entitySet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                query = includes(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (paging == null) return query;
            
            var itemCount = paging.ItemCount ?? 0;
            if (paging.PageNumber.HasValue)
            {
                var pageNumber = paging.PageNumber.Value <= 1 ? 1 : paging.PageNumber.Value;
                query = query.Skip((pageNumber - 1) * itemCount);
            }

            if (itemCount > 0)
            {
                query = query.Take(itemCount);
            }

            return query;
        }

        #endregion
    }
}

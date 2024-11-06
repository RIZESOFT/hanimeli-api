using AutoMapper;
using HanimeliApp.Application.Exceptions;
using HanimeliApp.Domain.Entities.Abstract;
using HanimeliApp.Domain.Models;
using HanimeliApp.Domain.UnitOfWorks;
using System.Linq.Expressions;

namespace HanimeliApp.Application.Services.Abstract
{
	public abstract class ServiceBase
	{
		protected readonly IUnitOfWork UnitOfWork;
		protected readonly IMapper Mapper;

		public ServiceBase(IUnitOfWork unitOfWork, IMapper mapper)
		{
			UnitOfWork = unitOfWork;
			Mapper = mapper;
		}
	}
	
	public class ServiceBase<TEntity, TModel, TCreateRequest, TUpdateRequest>
		where TEntity : BaseEntity<int>
	{
		protected readonly IUnitOfWork UnitOfWork;
		protected readonly IMapper Mapper;

		public Func<IQueryable<TEntity>, IQueryable<TEntity>>? GetByIdIncludes = null;
		public Func<IQueryable<TEntity>, IQueryable<TEntity>>? GetListIncludes = null;

        public ServiceBase(IUnitOfWork unitOfWork, IMapper mapper)
		{
			UnitOfWork = unitOfWork;
			Mapper = mapper;
		}
		
		public virtual async Task<TModel?> GetById(int id)
		{
			var entity = await UnitOfWork.Repository<TEntity>().GetAsync(x => x.Id == id, includes: GetByIdIncludes);
			var model = Mapper.Map<TModel>(entity);
			return model;
		}
		
		public virtual async Task<List<TModel>> GetList(Expression<Func<TEntity, bool>> filter, int pageNumber, int pageSize)
		{
			var paging = new EntityPaging();
			paging.PageNumber = pageNumber;
			paging.ItemCount = pageSize;
			var entities = await UnitOfWork.Repository<TEntity>().GetListAsync(filter, x => x.OrderBy(y => y.Id), paging: paging, includes: GetListIncludes);
			var models = Mapper.Map<List<TModel>>(entities);
			return models;
		}
		
		public virtual async Task<TModel> Create(TCreateRequest request)
		{
			var entity = Mapper.Map<TEntity>(request);
			await UnitOfWork.Repository<TEntity>().InsertAsync(entity);
			await UnitOfWork.SaveChangesAsync();
			var model = Mapper.Map<TModel>(entity);
			return model;
		}
		
		public virtual async Task<TModel> Update(int id, TUpdateRequest request)
		{
			var entity = await UnitOfWork.Repository<TEntity>().GetAsync(x => x.Id == id);

			if (entity == null)
				throw ValidationExceptions.RecordNotFound;
            
			Mapper.Map(request, entity);
			UnitOfWork.Repository<TEntity>().Update(entity);
			await UnitOfWork.SaveChangesAsync();
			var model = Mapper.Map<TModel>(entity);
			return model;
		}
    
		public virtual async Task Delete(int id)
		{
			var entity = await UnitOfWork.Repository<TEntity>().GetAsync(x => x.Id == id);

			if (entity == null)
				throw ValidationExceptions.RecordNotFound;
        
			UnitOfWork.Repository<TEntity>().HardDelete(entity);
			await UnitOfWork.SaveChangesAsync();
		}
	}

}

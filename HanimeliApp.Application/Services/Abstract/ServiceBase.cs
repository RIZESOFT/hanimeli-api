using AutoMapper;
using HanimeliApp.Application.Exceptions;
using HanimeliApp.Domain.Entities.Abstract;
using HanimeliApp.Domain.Models;
using HanimeliApp.Domain.UnitOfWorks;

namespace HanimeliApp.Application.Services.Abstract
{
	public class ServiceBase<TEntity, TModel, TCreateRequest, TUpdateRequest>
		where TEntity : BaseEntity<int>
	{
		protected readonly IUnitOfWork UnitOfWork;
		private readonly IMapper _mapper;

		public ServiceBase(IUnitOfWork unitOfWork, IMapper mapper)
		{
			UnitOfWork = unitOfWork;
			_mapper = mapper;
		}
		
		public virtual async Task<TModel?> GetById(int id)
		{
			var entity = await UnitOfWork.Repository<TEntity>().GetAsync(x => x.Id == id);
			var model = _mapper.Map<TModel>(entity);
			return model;
		}
		
		public virtual async Task<List<TModel>> GetList(int pageNumber, int pageSize)
		{
			var paging = new EntityPaging();
			paging.PageNumber = pageNumber;
			paging.ItemCount = pageSize;
			var entities = await UnitOfWork.Repository<TEntity>().GetListAsync(x => true, x => x.OrderBy(y => y.Id), paging: paging);
			var models = _mapper.Map<List<TModel>>(entities);
			return models;
		}
		
		public virtual async Task<TModel> Create(TCreateRequest request)
		{
			var entity = _mapper.Map<TEntity>(request);
			await UnitOfWork.Repository<TEntity>().InsertAsync(entity);
			await UnitOfWork.SaveChangesAsync();
			var model = _mapper.Map<TModel>(entity);
			return model;
		}
		
		public virtual async Task<TModel> Update(int id, TUpdateRequest request)
		{
			var entity = await UnitOfWork.Repository<TEntity>().GetAsync(x => x.Id == id);

			if (entity == null)
				throw ValidationExceptions.RecordNotFound;
            
			_mapper.Map(request, entity);
			UnitOfWork.Repository<TEntity>().Update(entity);
			await UnitOfWork.SaveChangesAsync();
			var model = _mapper.Map<TModel>(entity);
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

using AutoMapper;
using HanimeliApp.Application.Exceptions;
using HanimeliApp.Application.Models;
using HanimeliApp.Domain.Dtos.User;
using HanimeliApp.Domain.Entities.Abstract;
using HanimeliApp.Domain.Models;
using HanimeliApp.Domain.Repositories;
using HanimeliApp.Domain.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;

namespace HanimeliApp.Admin.Api.Controllers;

public class CrudBaseController<TEntity, TModel, TCreateRequest, TUpdateRequest> : AdminBaseController where TEntity : BaseEntity<int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<TEntity> _baseRepository;
    private readonly IMapper _mapper;

    public CrudBaseController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _baseRepository = unitOfWork.Repository<TEntity>();
        _unitOfWork = unitOfWork;
    }
    
    [HttpGet("{id}")]
    public virtual async Task<Result<TModel>> GetById(int id)
    {
        var entity = await _baseRepository.GetAsync(x => x.Id == id);
        var model = _mapper.Map<TModel>(entity);
        return Result.AsSuccess(model);
    }
    
    [HttpGet]
    public virtual async Task<Result<List<TModel>>> GetList([FromQuery] int pageNumber)
    {
        var paging = new EntityPaging();
        paging.PageNumber = pageNumber;
        paging.ItemCount = 25;
        var entity = await _baseRepository.GetListAsync(x => true, x => x.OrderBy(y => y.Id), paging: paging);
        var model = _mapper.Map<List<TModel>>(entity);
        return Result.AsSuccess(model);
    }
    
    [HttpPost]
    public virtual async Task<Result<TModel>> Create([FromBody] TCreateRequest request)
    {
        var entity = _mapper.Map<TEntity>(request);
        await _baseRepository.InsertAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        var model = _mapper.Map<TModel>(entity);
        return Result.AsSuccess(model);
    }
    
    [HttpPut("{id:int}")]
    public virtual async Task<Result<TModel>> Update(int id, [FromBody] TUpdateRequest request)
    {
        var entity = await _baseRepository.GetAsync(x => x.Id == id);

        if (entity == null)
            throw ValidationExceptions.RecordNotFound;
            
        _mapper.Map(request, entity);
        _baseRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        var model = _mapper.Map<TModel>(entity);
        return Result.AsSuccess(model);
    }
    
    [HttpDelete("{id:int}")]
    public virtual async Task<Result> Delete(int id)
    {
        var entity = await _baseRepository.GetAsync(x => x.Id == id);

        if (entity == null)
            throw ValidationExceptions.RecordNotFound;
        
        _baseRepository.HardDelete(entity);
        await _unitOfWork.SaveChangesAsync();
        return Result.AsSuccess();
    }
}
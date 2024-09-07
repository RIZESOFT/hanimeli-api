﻿using AutoMapper;
using HanimeliApp.Application.Exceptions;
using HanimeliApp.Application.Models;
using HanimeliApp.Application.Services.Abstract;
using HanimeliApp.Domain.Dtos.Menu;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models.Menu;
using HanimeliApp.Domain.UnitOfWorks;

namespace HanimeliApp.Application.Services;

public class MenuService : ServiceBase<Menu, MenuModel, CreateMenuRequest, UpdateMenuRequest>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    
    public MenuService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public override async Task<MenuModel> Create(CreateMenuRequest request)
    {
        var entity = _mapper.Map<Menu>(request);
        
        if (request.FoodIds != null && request.FoodIds.Any())
        {
            var foods = await _unitOfWork.Repository<Food>().GetListAsync(x => request.FoodIds.Contains(x.Id));
            entity.Foods = foods;
        }

        if (request.BeverageIds != null && request.BeverageIds.Any())
        {
            var beverages = await _unitOfWork.Repository<Beverage>().GetListAsync(x => request.BeverageIds.Contains(x.Id));
            entity.Beverages = beverages;
        }
        
        await _unitOfWork.Repository<Menu>().InsertAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        var model = _mapper.Map<MenuModel>(entity);
        return model;
    }

    public override async Task<MenuModel> Update(int id, UpdateMenuRequest request)
    {
        var entity = await _unitOfWork.Repository<Menu>().GetAsync(x => x.Id == id);

        if (entity == null)
            throw ValidationExceptions.RecordNotFound;
            
        _mapper.Map(request, entity);
        
        if (request.FoodIds != null && request.FoodIds.Any())
        {
            var foods = await _unitOfWork.Repository<Food>().GetListAsync(x => request.FoodIds.Contains(x.Id));
            entity.Foods = foods;
        }

        if (request.BeverageIds != null && request.BeverageIds.Any())
        {
            var beverages = await _unitOfWork.Repository<Beverage>().GetListAsync(x => request.BeverageIds.Contains(x.Id));
            entity.Beverages = beverages;
        }

        _unitOfWork.Repository<Menu>().Update(entity);
        await _unitOfWork.SaveChangesAsync();
        var model = _mapper.Map<MenuModel>(entity);
        return model;
    }
}
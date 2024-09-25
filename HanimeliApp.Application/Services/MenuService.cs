using AutoMapper;
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
    public MenuService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }
    
    public override async Task<MenuModel> Create(CreateMenuRequest request)
    {
        var entity = Mapper.Map<Menu>(request);
        
        if (request.FoodIds != null && request.FoodIds.Any())
        {
            var foods = await UnitOfWork.Repository<Food>().GetListAsync(x => request.FoodIds.Contains(x.Id));
            entity.Foods = foods;
        }

        if (request.BeverageIds != null && request.BeverageIds.Any())
        {
            var beverages = await UnitOfWork.Repository<Beverage>().GetListAsync(x => request.BeverageIds.Contains(x.Id));
            entity.Beverages = beverages;
        }
        
        await UnitOfWork.Repository<Menu>().InsertAsync(entity);
        await UnitOfWork.SaveChangesAsync();
        var model = Mapper.Map<MenuModel>(entity);
        return model;
    }

    public override async Task<MenuModel> Update(int id, UpdateMenuRequest request)
    {
        var entity = await UnitOfWork.Repository<Menu>().GetAsync(x => x.Id == id);

        if (entity == null)
            throw ValidationExceptions.RecordNotFound;
            
        Mapper.Map(request, entity);
        
        if (request.FoodIds != null && request.FoodIds.Any())
        {
            var foods = await UnitOfWork.Repository<Food>().GetListAsync(x => request.FoodIds.Contains(x.Id));
            entity.Foods = foods;
        }

        if (request.BeverageIds != null && request.BeverageIds.Any())
        {
            var beverages = await UnitOfWork.Repository<Beverage>().GetListAsync(x => request.BeverageIds.Contains(x.Id));
            entity.Beverages = beverages;
        }

        UnitOfWork.Repository<Menu>().Update(entity);
        await UnitOfWork.SaveChangesAsync();
        var model = Mapper.Map<MenuModel>(entity);
        return model;
    }
}
using AutoMapper;
using HanimeliApp.Application.Exceptions;
using HanimeliApp.Application.Models;
using HanimeliApp.Application.Services.Abstract;
using HanimeliApp.Domain.Dtos.Food;
using HanimeliApp.Domain.Dtos.Menu;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models.Food;
using HanimeliApp.Domain.Models.Menu;
using HanimeliApp.Domain.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HanimeliApp.Application.Services;

public class MenuService : ServiceBase<Menu, MenuModel, CreateMenuRequest, UpdateMenuRequest>
{
    private readonly ImageService _imageService;

    public MenuService(IUnitOfWork unitOfWork, IMapper mapper, ImageService imageService) : base(unitOfWork, mapper)
    {
        _imageService = imageService;

        GetByIdIncludes = x => x.Include(y => y.Foods).Include(y => y.Beverages);
        GetListIncludes = x => x.Include(y => y.Foods).Include(y => y.Beverages);
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

    public async Task<MenuModel> CreateWithImage(CreateMenuRequest request, IFormFile imageFile)
    {
        var imageUrl = await _imageService.UploadImageAsync(imageFile, "images");
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

        entity.ImageUrl = imageUrl;
        await UnitOfWork.Repository<Menu>().InsertAsync(entity);
        await UnitOfWork.SaveChangesAsync();
        var model = Mapper.Map<MenuModel>(entity);
        return model;
    }

    public async Task<MenuModel> UpdateWithImage(int id, UpdateMenuRequest request, IFormFile imageFile)
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

        if (imageFile != null)
        {
            var imageUrl = await _imageService.UploadImageAsync(imageFile, "images");
            entity.ImageUrl = imageUrl;
        }

        UnitOfWork.Repository<Menu>().Update(entity);
        await UnitOfWork.SaveChangesAsync();
        var model = Mapper.Map<MenuModel>(entity);
        return model;
    }
    
    public async Task ToggleActiveStatus(int id)
    {
        var entity = await UnitOfWork.Repository<Menu>().GetAsync(x => x.Id == id);
        if (entity == null)
            throw ValidationExceptions.RecordNotFound;

        entity.IsActive = !entity.IsActive;

        UnitOfWork.Repository<Menu>().Update(entity);
        await UnitOfWork.SaveChangesAsync();
    }

    
}
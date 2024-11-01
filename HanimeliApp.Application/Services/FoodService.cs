using AutoMapper;
using HanimeliApp.Application.Exceptions;
using HanimeliApp.Application.Services.Abstract;
using HanimeliApp.Application.Utilities;
using HanimeliApp.Domain.Dtos.Food;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Entities.Abstract;
using HanimeliApp.Domain.Models.Food;
using HanimeliApp.Domain.UnitOfWorks;
using Microsoft.AspNetCore.Http;

namespace HanimeliApp.Application.Services;

public class FoodService : ServiceBase<Food, FoodModel, CreateFoodRequest, UpdateFoodRequest>
{
    private readonly ImageService _imageService;

    public FoodService(IUnitOfWork unitOfWork, IMapper mapper, ImageService imageService)
        : base(unitOfWork, mapper)
    {
        _imageService = imageService;
    }

    public async Task<FoodModel> CreateWithImage(CreateFoodRequest request, IFormFile imageFile)
    {
        var imageUrl = await _imageService.UploadImageAsync(imageFile, "images");
        var entity = Mapper.Map<Food>(request);
        entity.ImageUrl = imageUrl;
        await UnitOfWork.Repository<Food>().InsertAsync(entity);
        await UnitOfWork.SaveChangesAsync();
        var model = Mapper.Map<FoodModel>(entity);
        return model;
    }

    public async Task<FoodModel> UpdateWithImage(int id, UpdateFoodRequest request, IFormFile imageFile)
    {
        var entity = await UnitOfWork.Repository<Food>().GetAsync(x => x.Id == id);

        if (entity == null)
            throw ValidationExceptions.RecordNotFound;

        Mapper.Map(request, entity);
        if (imageFile != null)
        {
            var imageUrl = await _imageService.UploadImageAsync(imageFile, "images");
            entity.ImageUrl = imageUrl;
        }
        UnitOfWork.Repository<Food>().Update(entity);
        await UnitOfWork.SaveChangesAsync();
        var model = Mapper.Map<FoodModel>(entity);
        return model;
    }
}
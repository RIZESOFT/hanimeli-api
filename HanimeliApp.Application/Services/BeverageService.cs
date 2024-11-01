using AutoMapper;
using HanimeliApp.Application.Exceptions;
using HanimeliApp.Application.Services.Abstract;
using HanimeliApp.Domain.Dtos.Beverage;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Entities.Abstract;
using HanimeliApp.Domain.Enums;
using HanimeliApp.Domain.Models.Beverage;
using HanimeliApp.Domain.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace HanimeliApp.Application.Services;

public class BeverageService : ServiceBase<Beverage, BeverageModel, CreateBeverageRequest, UpdateBeverageRequest>
{
    private readonly ImageService _imageService;
    public BeverageService(IUnitOfWork unitOfWork, IMapper mapper, ImageService imageService) : base(unitOfWork, mapper)
    {
        _imageService = imageService;
    }


    public async Task<BeverageModel> CreateWithImage(CreateBeverageRequest request, IFormFile imageFile)
    {
        var imageUrl = await _imageService.UploadImageAsync(imageFile, "images");
        var entity = Mapper.Map<Beverage>(request);
        entity.ImageUrl = imageUrl;
        await UnitOfWork.Repository<Beverage>().InsertAsync(entity);
        await UnitOfWork.SaveChangesAsync();
        var model = Mapper.Map<BeverageModel>(entity);
        return model;
    }

    public async Task<BeverageModel> UpdateWithImage(int id, UpdateBeverageRequest request, IFormFile imageFile)
    {
        var entity = await UnitOfWork.Repository<Beverage>().GetAsync(x => x.Id == id);

        if (entity == null)
            throw ValidationExceptions.RecordNotFound;

        Mapper.Map(request, entity);
        if (imageFile != null)
        {
            var imageUrl = await _imageService.UploadImageAsync(imageFile, "images");
            entity.ImageUrl = imageUrl;
        }
        UnitOfWork.Repository<Beverage>().Update(entity);
        await UnitOfWork.SaveChangesAsync();
        var model = Mapper.Map<BeverageModel>(entity);
        return model;
    }
}
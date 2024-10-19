using AutoMapper;
using HanimeliApp.Application.Exceptions;
using HanimeliApp.Application.Services.Abstract;
using HanimeliApp.Domain.Dtos.Address;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models;
using HanimeliApp.Domain.Models.Address;
using HanimeliApp.Domain.UnitOfWorks;

namespace HanimeliApp.Application.Services;

public class AddressService : ServiceBase<Address, AddressModel, CreateAddressRequest, UpdateAddressRequest>
{
    public AddressService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
        
    }


    public async Task<List<AddressModel>> GetListByUser(int userId, int pageNumber, int pageSize)
    {
        var paging = new EntityPaging
        {
            PageNumber = pageNumber,
            ItemCount = pageSize
        };
        var entities = await UnitOfWork.Repository<Address>().GetListAsync(x => x.UserId == userId, x => x.OrderBy(y => y.Id), paging: paging);
        var models = Mapper.Map<List<AddressModel>>(entities);
        return models;
    }
    public override async Task<AddressModel> Create(CreateAddressRequest request)
    {
        return await SaveAddress(null, request);
    }

    public override async Task<AddressModel> Update(int id, UpdateAddressRequest request)
    {
        return await SaveAddress(id, request);
    }
    
    private async Task<AddressModel> SaveAddress(int? id, CreateAddressRequest request)
    {
        var hasState = await UnitOfWork.Repository<State>().ExistAsync(x => x.Id == request.StateId);
        var hasCity = await UnitOfWork.Repository<City>().ExistAsync(x => x.Id == request.CityId);
        var hasCountry = await UnitOfWork.Repository<Country>().ExistAsync(x => x.Id == request.CountryId);
        if (!hasState || !hasCity || !hasCountry)
        {
            throw ValidationExceptions.RecordNotFound;
        }
            
        var hasUser = await UnitOfWork.Repository<User>().ExistAsync(x => x.Id == request.UserId);
        if (!hasUser)
        {
            throw ValidationExceptions.InvalidUser;
        }
            
        var addressRepository = UnitOfWork.Repository<Address>();
        if (id.HasValue)
        {
            var address = await addressRepository.GetAsync(x => x.Id == id.Value && x.UserId == request.UserId);
            if (address == null)
            {
                throw ValidationExceptions.RecordNotFound;
            }
            Mapper.Map(request, address);
            addressRepository.Update(address);
            await UnitOfWork.SaveChangesAsync();
            var model = Mapper.Map<AddressModel>(address);
            return model;
        }
        else
        {
            var address = Mapper.Map<Address>(request);
            await addressRepository.InsertAsync(address);
            await UnitOfWork.SaveChangesAsync();
            var model = Mapper.Map<AddressModel>(address);
            return model;
        }
    }
}
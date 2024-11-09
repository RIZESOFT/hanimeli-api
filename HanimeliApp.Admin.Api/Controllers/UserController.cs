using System.Security.Claims;
using HanimeliApp.Admin.Api.Controllers.BaseControllers;
using HanimeliApp.Application.Models;
using HanimeliApp.Application.Services;
using HanimeliApp.Domain.Dtos.User;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Enums;
using HanimeliApp.Domain.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HanimeliApp.Admin.Api.Controllers;

public class UserController : CrudBaseController<UserService, User, UserModel, CreateUserRequest, UpdateUserRequest>
{
    public UserController(UserService userService) : base(userService)
    {
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<Result<UserLoginResultModel>> Login([FromBody] UserLoginRequest request)
    {
        var result = await Service.Login(request);
        return Result.AsSuccess(result);
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpGet]
    public async Task<Result<List<UserModel>>> GetB2BUserList([FromQuery] int pageNumber)
    {
        var result = await Service.GetB2BUserList(pageNumber, 25);
        return Result.AsSuccess(result);
    }

    [HttpGet]
    public async Task<Result<UserModel>> GetB2BUserSettings([FromQuery] int userId)
    {
        if (User.IsInRole(Roles.B2B))
        {
            userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
        var result = await Service.GetB2BUserSettings(userId);
        return Result.AsSuccess(result);
    }
    
    [HttpPost]
    public async Task<Result<UserModel>> UpdateB2BUserSettings([FromBody] UpdateB2BUserSettingsRequest request)
    {
        if (User.IsInRole(Roles.B2B))
        {
            request.UserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
        var result = await Service.UpdateB2BUserSettings(request);
        return Result.AsSuccess(result);
    }
}
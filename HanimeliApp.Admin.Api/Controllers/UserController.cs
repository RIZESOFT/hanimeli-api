﻿using HanimeliApp.Application.Models;
using HanimeliApp.Application.Services;
using HanimeliApp.Domain.Dtos.User;
using HanimeliApp.Domain.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HanimeliApp.Admin.Api.Controllers;

public class UserController : AdminBaseController
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<Result<UserLoginResultModel>> Login([FromBody] UserLoginRequest request)
    {
        var result = await _userService.Login(request);
        return Result.AsSuccess(result);
    }
    
    [HttpPost]
    public async Task<Result<UserModel>> CreateUser([FromBody] CreateUserRequest request)
    {
        var result = await _userService.Create(request);
        return Result.AsSuccess(result);
    }
}
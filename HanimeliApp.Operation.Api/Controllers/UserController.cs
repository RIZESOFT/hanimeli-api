using HanimeliApp.Application.Models;
using HanimeliApp.Application.Services;
using HanimeliApp.Domain.Dtos.User;
using HanimeliApp.Domain.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HanimeliApp.Operation.Api.Controllers;

[Route("api/{culture}/[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<Result<OperationUserLoginResultModel>> Login([FromBody] UserLoginRequest request)
    {
        var result = await _userService.OperationLogin(request);
        return Result.AsSuccess(result);
    }
}
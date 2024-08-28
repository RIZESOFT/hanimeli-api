using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HanimeliApp.API.Controllers;

[Authorize(Policy = "UserPolicy")]
[Route("api/{culture}/[controller]/[action]")]
public class BaseController : ControllerBase
{
    
}
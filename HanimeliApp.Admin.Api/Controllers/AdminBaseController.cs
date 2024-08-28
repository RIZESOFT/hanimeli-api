using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HanimeliApp.Admin.Api.Controllers;

[Authorize(Policy = "AdminPolicy")]
[Route("api/{culture}/[controller]/[action]")]
public class AdminBaseController : ControllerBase
{
    
}
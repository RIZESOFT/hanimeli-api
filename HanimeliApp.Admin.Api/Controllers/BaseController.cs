﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HanimeliApp.Admin.Api.Controllers;

[Authorize(Policy = "B2BPolicy")]
[Route("api/{culture}/[controller]/[action]")]
public class BaseController : ControllerBase
{
    
}
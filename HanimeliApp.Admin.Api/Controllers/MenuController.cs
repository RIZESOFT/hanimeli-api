using AutoMapper;
using HanimeliApp.Application.Exceptions;
using HanimeliApp.Application.Models;
using HanimeliApp.Application.Services;
using HanimeliApp.Domain.Dtos.Menu;
using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models.Food;
using HanimeliApp.Domain.Models.Menu;
using HanimeliApp.Domain.UnitOfWorks;

namespace HanimeliApp.Admin.Api.Controllers;

public class MenuController : CrudBaseController<MenuService, Menu, MenuModel, CreateMenuRequest, UpdateMenuRequest>
{
    public MenuController(MenuService service) : base(service)
    {
    }
}
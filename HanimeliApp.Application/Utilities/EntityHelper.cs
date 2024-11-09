using HanimeliApp.Domain.Entities;
using HanimeliApp.Domain.Models.Beverage;
using HanimeliApp.Domain.Models.Cook;
using HanimeliApp.Domain.Models.Food;
using HanimeliApp.Domain.Models.Menu;
using HanimeliApp.Domain.Models.Order;

namespace HanimeliApp.Application.Utilities;

public static class EntityHelper
{
    public static List<OrderItemModel> GroupOrderItemsForUi(this IEnumerable<OrderItem>? orderItems)
    {
        if (orderItems == null)
        {
            return new List<OrderItemModel>();
        }
        var orderItemList = orderItems.ToList();
        if (orderItemList.Count == 0)
        {
            return new List<OrderItemModel>();
        }

        var groupedItems = orderItemList.GroupBy(oi => oi.MenuId).Select(oi =>
        {
            var orderItem = oi.First();
            return new OrderItemModel
            {
                MenuId = orderItem.MenuId,
                Menu = orderItem.Menu != null
                    ? new MenuModel
                    {
                        Id = orderItem.Menu.Id,
                        Name = orderItem.Menu.Name,
                        Description = orderItem.Menu.Description,
                        Price = orderItem.Menu.Price,
                        ImageUrl = orderItem.Menu.ImageUrl,
                        Beverages = orderItem.Menu.Beverages != null
                            ? orderItem.Menu.Beverages.Select(b => new BeverageModel
                            {
                                Id = b.Id,
                                Name = b.Name,
                                Description = b.Description,
                                Price = b.Price,
                                ImageUrl = b.ImageUrl
                            }).ToList()
                            : null,
                        Foods = orderItem.Menu.Foods != null
                            ? orderItem.Menu.Foods.Select(f => new FoodModel
                            {
                                Id = f.Id,
                                Name = f.Name,
                                Description = f.Description,
                                Price = f.Price,
                                ImageUrl = f.ImageUrl
                            }).ToList()
                            : null
                    }
                    : new MenuModel(),
                Quantity = oi.Count(),
                Amount = orderItem.Amount,
                CookId = orderItem.CookId,
                Cook = orderItem.Cook != null
                    ? new CookModel
                    {
                        Id = orderItem.Cook.Id,
                        Name = orderItem.Cook.Name,
                        ImageUrl = orderItem.Cook.ImageUrl,
                        Address = orderItem.Cook.Address,
                        Bio = orderItem.Cook.Bio,
                        Iban = orderItem.Cook.Iban,
                        Rating = orderItem.Cook.Rating
                    }
                    : null,
                BeverageId = orderItem.BeverageId,
                Beverage = orderItem.Beverage != null
                    ? new BeverageModel
                    {
                        Id = orderItem.Beverage.Id,
                        Name = orderItem.Beverage.Name,
                        Description = orderItem.Beverage.Description,
                        Price = orderItem.Beverage.Price,
                        ImageUrl = orderItem.Beverage.ImageUrl
                    }
                    : null,
                FoodId = orderItem.FoodId,
                Food = orderItem.Food != null
                    ? new FoodModel
                    {
                        Id = orderItem.Food.Id,
                        Name = orderItem.Food.Name,
                        Description = orderItem.Food.Description,
                        Price = orderItem.Food.Price,
                        ImageUrl = orderItem.Food.ImageUrl
                    }
                    : null
            };
        });

        return groupedItems.ToList();
    }
}
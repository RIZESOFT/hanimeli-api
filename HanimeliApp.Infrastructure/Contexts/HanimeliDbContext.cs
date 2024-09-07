using HanimeliApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HanimeliApp.Infrastructure.Contexts;

public class HanimeliDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<Beverage> Beverages { get; set; }
    public DbSet<Cook> Cooks { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<Food> Foods { get; set; }
    
    public HanimeliDbContext(DbContextOptions<HanimeliDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
}
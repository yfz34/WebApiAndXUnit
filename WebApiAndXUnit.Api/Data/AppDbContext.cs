using Microsoft.EntityFrameworkCore;
using WebApiAndXUnit.Api.Entities;

namespace WebApiAndXUnit.Api.Data;

public class AppDbContext : DbContext
{
    public DbSet<Item> Items { get; set; } = default!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
}
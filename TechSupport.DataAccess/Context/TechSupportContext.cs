using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TechSupport.DataAccess.Models;

namespace TechSupport.DataAccess.Context;

public class TechSupportContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<RequestCategory> RequestCategories { get; set; }
    public DbSet<Department> Departments { get; set; }

    public DbSet<Request> Requests { get; set; }

    public TechSupportContext(DbContextOptions<TechSupportContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        foreach (var foreignKey in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            foreignKey.DeleteBehavior = DeleteBehavior.Cascade;
        }
    }
}

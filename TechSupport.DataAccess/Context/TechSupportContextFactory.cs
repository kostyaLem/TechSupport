using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TechSupport.DataAccess.Context;

/// <summary>
/// Класс для создания контекста базы данных
/// </summary>
internal class TechSupportContextFactory : IDesignTimeDbContextFactory<TechSupportContext>
{
    public TechSupportContext CreateDbContext() => CreateDbContext(Array.Empty<string>());

    public TechSupportContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TechSupportContext>();
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TechSupportDb;Trusted_Connection=True;");
        optionsBuilder.UseLazyLoadingProxies();

        return new TechSupportContext(optionsBuilder.Options);
    }
}

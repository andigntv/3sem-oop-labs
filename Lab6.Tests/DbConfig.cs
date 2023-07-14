using BusinessLogic.Extensions;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Lab6.Tests;

public class DbConfig
{
    public DbConfig()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddApplication();
        serviceCollection.AddDbContext<DatabaseContext>(x =>
            x.UseLazyLoadingProxies().UseSqlite("Data Source = DB.db"));

        ServiceProvider = serviceCollection.BuildServiceProvider();
        ServiceProvider.GetRequiredService<DatabaseContext>().Database.EnsureCreated();
    }
    public ServiceProvider ServiceProvider { get; }
}
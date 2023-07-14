using BusinessLogic.Services;
using BusinessLogic.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogic.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection collection)
    {
        collection.AddScoped<IMessageService, MessageService>();
        collection.AddScoped<IEmployeeService, EmployeeService>();
        collection.AddScoped<ISourceService, SourceService>();
        collection.AddScoped<IAuthService, AuthService>();
        return collection;
    }
}
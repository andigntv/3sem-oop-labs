using BusinessLogic.Dto;
using BusinessLogic.Extensions;
using BusinessLogic.Services.Implementations;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Models;
using DataAccess.Models.Messages;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
[assembly: CollectionBehavior(DisableTestParallelization = true)]


namespace Lab6.Tests;

public class Tests : IClassFixture<DbConfig>
{
    private DbConfig _config;
    private ServiceProvider _serviceProvider => _config.ServiceProvider;

    public Tests(DbConfig config)
    {
        _config = config;
    }
    
    [Fact]
    public async void EmployeeTesting()
    {
        DatabaseContext? context = _serviceProvider.GetService<DatabaseContext>();
        var employeeService = new EmployeeService(context);

        EmployeeDto chiefDto =
            await employeeService.CreateEmployeeAsync("Chief", null, AccessLevel.Manager, "Chief", "qwerty",
                CancellationToken.None);
        EmployeeDto firstSubordinateDto = 
            await employeeService.CreateEmployeeAsync("SubordinateA", chiefDto.Id, AccessLevel.OrdinaryEmployee, "SubordinateA", "qwerty123",
                CancellationToken.None);
        EmployeeDto secondSubordinateDto = 
            await employeeService.CreateEmployeeAsync("SubordinateB", null, AccessLevel.OrdinaryEmployee, "SubordinateB", "qwerty321",
                CancellationToken.None);

        Employee chief = await context.Employees.GetEntityAsync(chiefDto.Id, CancellationToken.None);
        
        Assert.Equal(3, context.Employees.Count());
        Assert.Single(chief.Subordinates!);
        Assert.Equal(chiefDto.Id, firstSubordinateDto.ChiefId);
        Assert.Null(secondSubordinateDto.ChiefId);

        secondSubordinateDto =
            await employeeService.AddSubordinateAsync(chiefDto.Id, secondSubordinateDto.Id, CancellationToken.None);
        
        Assert.Equal(chiefDto.Id, secondSubordinateDto.ChiefId);
        Assert.Equal(2, chief.Subordinates!.Count);
    }

    [Fact]
    public async void SourceAndMessagesTesting_AndReport()
    {
        DatabaseContext? context = _serviceProvider.GetService<DatabaseContext>();
        var sourceService = new SourceService(context);
        var messageService = new MessageService(context);
        
        Employee chief = context.Employees.ToList()[0];
        Employee subordinateA = context.Employees.ToList()[1]; // using employees from the first test

        SourceDto firstSource =
            await sourceService.CreateSourceAsync(chief.Id, "Email", "a@mail.ru", CancellationToken.None);
        SourceDto secondSource =
            await sourceService.CreateSourceAsync(subordinateA.Id, "Email", "s@mail.ru", CancellationToken.None);
        
        Assert.Single(chief.Sources);
        Assert.Single(subordinateA.Sources);
        Assert.Equal(2, context.Sources.Count());

        MessageDto firstMessageDto =
            await messageService.CreateMessageAsync(firstSource.Id, secondSource.Id, "go kurit",
                CancellationToken.None);
        MessageDto secondMessageDto =
            await messageService.ReplyToMessageAsync(firstMessageDto.Id,"go" ,CancellationToken.None);

        Message firstMessage = await context.Messages.GetEntityAsync(firstMessageDto.Id, CancellationToken.None);
        Message secondMessage = await context.Messages.GetEntityAsync(secondMessageDto.Id, CancellationToken.None);
        
        Assert.Equal(2, context.Messages.Count());
        Assert.Equal(MessageStatus.Processed, firstMessage.Status);
        Assert.Equal(MessageStatus.Sent, secondMessage.Status);
        
        ReportDto reportDto =
            await messageService.CreateReportAsync(DateTime.Now.Subtract(TimeSpan.FromDays(1)), CancellationToken.None);
        const string expectedString = "Source Type : Email\n" +
                                      "Messages Processed 1 Delivered 1 Sent 2\n" +
                                      "-------------------------------------------------\n";
        Assert.Equal(expectedString, reportDto.Info);
    }
}
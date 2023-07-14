using Microsoft.EntityFrameworkCore;
using BusinessLogic.Extensions;
using DataAccess.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplication();
builder.Services.AddDataAccess(x => x.UseLazyLoadingProxies().UseSqlite("Data Source = DB.db"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
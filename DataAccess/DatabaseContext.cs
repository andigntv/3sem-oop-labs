using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Models.Messages;
using DataAccess.Models.Sources;

namespace DataAccess;
using Microsoft.EntityFrameworkCore;
public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    public DbSet<Log> Logs { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Source> Sources { get; set; }
    public DbSet<Report> Reports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(builder =>
        {
            builder.HasOne(x => x.Chief).WithMany(x => x.Subordinates);
            builder.HasMany(x => x.Sources).WithOne(x => x.Owner);
        });

        modelBuilder.Entity<Source>(builder =>
        {
            builder.HasOne(x => x.Owner).WithMany(x => x.Sources);
        });

        modelBuilder.Entity<Message>(builder =>
        {
            builder.HasOne(x => x.SenderSource);
            builder.HasOne(x => x.RecipientSource);
        });
    }
}
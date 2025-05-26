using Contacts.Domain.Accounts;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Contacts.Infrastructure.Persistance;

internal class AppDbContext(DbContextOptions<AppDbContext> options,
                            IConfiguration configuration) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connection string is not exists.");

        optionsBuilder.UseNpgsql(connectionString, opt =>{
            opt.MigrationsHistoryTable("ef_migrations");

            opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        });

        optionsBuilder.UseSnakeCaseNamingConvention();

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = typeof(AppDbContext).Assembly;

        modelBuilder.ApplyConfigurationsFromAssembly(assembly);

        modelBuilder.AddIsDeletedQueryFilters();
        modelBuilder.AddCreatedAuditableForeignKey<Account, int>();
        modelBuilder.AddUpdatedAuditableForeignKey<Account, int>();
        modelBuilder.AddDeletedAuditableForeignKey<Account, int>();

        base.OnModelCreating(modelBuilder);
    }
}
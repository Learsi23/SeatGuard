using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SeatGuard.Api.Data;

public static class DatabaseOptions
{
    /// <summary>
    /// Extension method that centralizes database configuration.
    /// It is used across the API, EF Core CLI tools, and unit/integration tests
    /// so all environments share the exact same configuration rules.
    /// </summary>
    public static TBuilder UseSeatGuardDatabase<TBuilder>(this TBuilder options, string connectionString)
        where TBuilder : DbContextOptionsBuilder
    {
        options
            // 1. PostgreSQL Provider:
            // Configures EF Core to connect to PostgreSQL instead of SQL Server.
            .UseNpgsql(connectionString)

            // 2. Naming Convention:
            // Automatically converts C# PascalCase properties and classes to PostgreSQL snake_case table and column names.
            .UseSnakeCaseNamingConvention();

        return options;
    }

    /// <summary>
    /// Design-time factory used exclusively by 'dotnet-ef' CLI tools.
    /// </summary>
    public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // Reads configuration specifically for CLI execution including User Secrets and Environment Variables.
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets(typeof(DesignTimeDbContextFactory).Assembly, optional: true)
                .AddEnvironmentVariables()
                .Build();

            // Retrieves the connection string configured in User Secrets or Environment Variables.
            var connectionString = configuration.GetConnectionString("SeatGuard");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Connection string 'ConnectionStrings:SeatGuard' was not found in User Secrets or Environment Variables.");
            }

            return new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
                .UseSeatGuardDatabase(connectionString)
                .Options);
        }
    }
}
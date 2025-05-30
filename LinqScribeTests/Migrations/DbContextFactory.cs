using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LinqScribeTests.Migrations;

/// <summary>
/// Needed for the migrations command to work: dotnet ef migrations add [MIGRATION_NAME]
/// </summary>
public class DbContextFactory : IDesignTimeDbContextFactory<LinqScribeClientDbContext>
{
    public LinqScribeClientDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<LinqScribeClientDbContext>();
        optionsBuilder.UseSqlServer("Test");

        return new LinqScribeClientDbContext(optionsBuilder.Options);
    }
}
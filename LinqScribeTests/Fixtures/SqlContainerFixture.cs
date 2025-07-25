using LinqScribeTests.Migrations;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace LinqScribeTests.Fixtures;

[CollectionDefinition(nameof(CollectionDefinitions.SharedDatabase))]    
public class DatabaseCollection : ICollectionFixture<SqlContainerFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}

public class SqlContainerFixture : IAsyncLifetime
{
    public MsSqlContainer Container { get; set; }
    public DbContextOptions<LinqScribeClientDbContext> DbOptions { get; set; }

    public async Task InitializeAsync()
    {
        Container = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("yourStrong(!)Password")
            .Build();

        await Container.StartAsync();
        
        DbOptions = new DbContextOptionsBuilder<LinqScribeClientDbContext>()
            .UseSqlServer(Container.GetConnectionString())
            .Options;
        
        await using var context = new LinqScribeClientDbContext(DbOptions);
        await context.Database.MigrateAsync();
        await context.Database.EnsureCreatedAsync();
        await TestDataSeeder.SeedAsync(context);
        await TestDataSeeder.SeedCustomersAsync(context);
    }

    public async Task DisposeAsync() => await Container.DisposeAsync();

}
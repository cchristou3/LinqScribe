using LinqScribeTests.Fixtures;
using LinqScribeTests.Migrations;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using static LinqScribe.IQueryableExtensions.DynamicOrderExtensions;

namespace LinqScribeTests;

public class OrderByTests(SqlContainerFixture fixture) : IClassFixture<SqlContainerFixture>
{
    private DbContextOptions<LinqScribeClientDbContext> DbOptions { get; } = fixture.DbOptions;
    
    [Fact]
    public async Task OrderBy_WithNameProperty_OrdersEntitiesAlphabetically()
    {
        // Arrange
        await using var context = new LinqScribeClientDbContext(DbOptions);
        
        // Act
        var list = await context.Entities
            .AsNoTracking()
            .OrderBy("Name")
            .ToListAsync();
        
        // Assert
        Assert.True(list[0].Name == "Alpha");
        Assert.True(list[1].Name == "Beta");
        Assert.True(list[2].Name == "Gamma");
    }
    
    [Fact]
    public async Task OrderBy_WithNonExistingNameProperty_ThrowsArgumentException()
    {
        // Arrange
        await using var context = new LinqScribeClientDbContext(DbOptions);
        
        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            context.Entities
                .AsNoTracking()
                .OrderBy(Constants.InvalidPropertyName)
        );
        
        // Assert
        exception.Message.ShouldBe($"Type [Entity] does not have the following property [{Constants.InvalidPropertyName}].");
    }
    
    [Fact]
    public async Task ThenBy_WithNameAndId_OrdersEntitiesByNameThenById()
    {
        // Arrange
        await using var context = new LinqScribeClientDbContext(DbOptions);
        
        // Act
        var list = await context.Entities
            .AsNoTracking()
            .Where(x => x.Name == "Gamma")
            .OrderBy("Name")
            .ThenBy("Id")
            .ToListAsync();
        
        // Assert
        Assert.True(list[0].Id < list[1].Id);
    }
    
    [Fact]
    public async Task ThenBy_WithNonExistingNameProperty_ThrowsArgumentException()
    {
        // Arrange
        await using var context = new LinqScribeClientDbContext(DbOptions);
        
        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            context.Entities
                .AsNoTracking()
                .OrderBy("Name")
                .ThenBy(Constants.InvalidPropertyName)
        );

        // Assert
        exception.Message.ShouldBe($"Type [Entity] does not have the following property [{Constants.InvalidPropertyName}].");
    }
}
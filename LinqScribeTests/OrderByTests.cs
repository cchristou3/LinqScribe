using LinqScribeTests.Fixtures;
using LinqScribeTests.Migrations;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using static LinqScribe.IQueryableExtensions.DynamicOrderExtensions;
using static LinqScribe.IQueryableExtensions.DynamicSelectExtensions;

namespace LinqScribeTests;

[Collection(nameof(CollectionDefinitions.SharedDatabase))]
public class OrderByTests(SqlContainerFixture fixture) 
{
    private DbContextOptions<LinqScribeClientDbContext> DbOptions { get; } = fixture.DbOptions;
    
    [Fact]
    public async Task OrderBy_WithNameProperty_OrdersEntitiesAlphabetically()
    {
        // Arrange
        await using var context = new LinqScribeClientDbContext(DbOptions);
        
        // Act
        var names = await context.Entities
            .AsNoTracking()
            .OrderBy("Name")
            .Select("Name")
            .ToListAsync();
        
        // Assert
        names.ShouldBeInOrder();
    }
    
    [Fact]
    public async Task OrderBy_WithNameProperty_OrdersEntitiesBackwardsAlphabetically()
    {
        // Arrange
        await using var context = new LinqScribeClientDbContext(DbOptions);
        
        // Act
        var names = await context.Entities
            .AsNoTracking()
            .OrderByDescending("Name")
            .Select("Name")
            .ToListAsync();
        
        // Assert
        names.ShouldBeInOrder(SortDirection.Descending);
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
    public async Task ThenBy_WithNameAndId_OrdersEntitiesByNameThenByIdDescending()
    {
        // Arrange
        await using var context = new LinqScribeClientDbContext(DbOptions);
        
        // Act
        var list = await context.Entities
            .AsNoTracking()
            .Where(x => x.Name == "Gamma")
            .OrderBy("Name")
            .ThenByDescending("Id")
            .ToListAsync();
        
        // Assert
        Assert.True(list[0].Id > list[1].Id);
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
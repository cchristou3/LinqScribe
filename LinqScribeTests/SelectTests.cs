using LinqScribeTests.Fixtures;
using LinqScribeTests.Migrations;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using static LinqScribe.IQueryableExtensions.DynamicSelectExtensions;

namespace LinqScribeTests;

[Collection(nameof(CollectionDefinitions.SharedDatabase))]
public class SelectTests(SqlContainerFixture fixture) 
{
    private DbContextOptions<LinqScribeClientDbContext> DbOptions { get; } = fixture.DbOptions;
    
    [Fact]
    public async Task Select_WithSinglePropertyName_ReturnsNonNullProjection()
    {
        // Arrange
        await using var context = new LinqScribeClientDbContext(DbOptions);
        
        // Act
        var projected = await context.Entities.Select("Name").FirstOrDefaultAsync();
        
        // Assert
        Assert.True(projected != null);
        Assert.IsType<string>(projected);
    }
    
    [Fact]
    public async Task Select_WithNonExistingPropertyName_ThrowsArgumentException()
    {
        // Arrange
        await using var context = new LinqScribeClientDbContext(DbOptions);
        
        // Act
        var exception = Assert.Throws<ArgumentException>(() => context.Entities.Select(Constants.InvalidPropertyName));

        // Assert
        exception.Message.ShouldBe($"Type [Entity] does not have the following property [{Constants.InvalidPropertyName}].");
    }
    
    [Fact]
    public async Task Select_WithMultiplePropertyNames_ReturnsNonNullProjection()
    {
        // Arrange
        await using var context = new LinqScribeClientDbContext(DbOptions);
        
        // Act
        var projected = await context.Entities.Select("Id", "Name").FirstAsync();
        
        // Assert
        Assert.True(projected.Id != null);
        Assert.True(projected.Name != null);
    }
    
    [Fact]
    public async Task Select_WithMultiplePropertyNames_ThrowsArgumentException()
    {
        // Arrange
        await using var context = new LinqScribeClientDbContext(DbOptions);
        
        // Act
        var exception = Assert.Throws<ArgumentException>(() => context.Entities.Select("Name", Constants.InvalidPropertyName));

        // Assert
        exception.Message.ShouldBe($"Type [Entity] does not have the following property [{Constants.InvalidPropertyName}].");
    }
}
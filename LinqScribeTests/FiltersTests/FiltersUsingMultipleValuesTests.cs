using System.Linq.Expressions;
using LinqScribe.IQueryableExtensions;
using LinqScribeTests.Fixtures;
using LinqScribeTests.Migrations;
using LinqScribeTests.TestDtos;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace LinqScribeTests.FiltersTests;

public class FiltersUsingMultipleValuesTests(SqlContainerFixture fixture) : IClassFixture<SqlContainerFixture>
{
    private DbContextOptions<LinqScribeClientDbContext> DbOptions { get; } = fixture.DbOptions;
    
    [Fact]
    public async Task FilterWith_FiltersWithMultipleEmails_ReturnsMatchingEmails()
    {
        // Arrange
        await using var context = new LinqScribeClientDbContext(DbOptions);
        var filters = new MultipleValueFilter
        {
            EmailList = ["alice@example.com", "bruno@example.com"]
        };
        
        // Act
        var customers = await context.Customers
            .AsNoTracking()
            .FilterWith(filters)
            .Select(x => x.Email)
            .Distinct()
            .ToListAsync();
        
        // Assert
        customers.Count.ShouldBe(2);
        customers.ShouldContain("alice@example.com");
        customers.ShouldContain("bruno@example.com");
    }
    
    [Fact]
    public async Task FilterWith_FiltersWithMultipleRegisteredAts_ReturnsMatchingIds()
    {
        // Arrange
        await using var context = new LinqScribeClientDbContext(DbOptions);
        var filters = new MultipleValueFilter
        {
            IdList = [1, 2]
        };
        
        // Act
        var customers = await context.Customers
            .AsNoTracking()
            .FilterWith(filters)
            .Select(x => x.Id)
            .Distinct()
            .ToListAsync();
        
        // Assert
        customers.Count.ShouldBe(2);
        customers.ShouldContain(1);
        customers.ShouldContain(2);
    }
}
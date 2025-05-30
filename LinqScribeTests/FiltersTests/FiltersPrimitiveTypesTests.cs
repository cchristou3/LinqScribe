using System.Linq.Expressions;
using LinqScribe.IQueryableExtensions;
using LinqScribeTests.Fixtures;
using LinqScribeTests.Migrations;
using LinqScribeTests.TestDtos;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace LinqScribeTests.FiltersTests;

public class FiltersPrimitiveTypesTests(SqlContainerFixture fixture) : IClassFixture<SqlContainerFixture>
{
    private DbContextOptions<LinqScribeClientDbContext> DbOptions { get; } = fixture.DbOptions;
    
    [Fact]
    public async Task FilterWith_StringProperty_FiltersStringValues()
    {
        var filters = new TestFilter { Name = "Gamma" };
        await TestCase(filters, x => x.Name == "Gamma");
    }
    
    [Fact]
    public async Task FilterWith_IntProperty_FiltersIntValues()
    {
        var filters = new TestFilter { Id = 1 };
        await TestCase(filters, x => x.Id == 1);
    }
    
    [Fact]
    public async Task FilterWith_BooleanProperty_FiltersBooleanValues()
    {
        var filters = new TestFilter { IsActive = true };
        await TestCase(filters, x => x.IsActive);
    }
    
    [Fact]
    public async Task FilterWith_ByteProperty_FiltersByteValues()
    {
        var filters = new TestFilter { NumberOfRelatives = byte.MinValue };
        await TestCase(filters, x => x.NumberOfRelatives == byte.MinValue);
    }
    
    [Fact]
    public async Task FilterWith_SByteProperty_FiltersSByteValues()
    {
        var filters = new TestFilter { NumberOfSiblings = sbyte.MinValue };
        await TestCase(filters, x => x.NumberOfSiblings == sbyte.MinValue);
    }
    
    [Fact]
    public async Task FilterWith_ShortProperty_FiltersShortValues()
    {
        var filters = new TestFilter { Salary = short.MinValue };
        await TestCase(filters, x => x.Salary == short.MinValue);
    }
    
    [Fact]
    public async Task FilterWith_UShortProperty_FiltersUShortValues()
    {
        var filters = new TestFilter { ExpectedSalary = ushort.MinValue };
        await TestCase(filters, x => x.ExpectedSalary == ushort.MinValue);
    }
    
    [Fact]
    public async Task FilterWith_UIntProperty_FiltersUIntValues()
    {
        var filters = new TestFilter { NumberOfFriends = uint.MinValue };
        await TestCase(filters, x => x.NumberOfFriends == uint.MinValue);
    }
    
    [Fact]
    public async Task FilterWith_LongProperty_FiltersLongValues()
    {
        var filters = new TestFilter { NumberOfDays = long.MinValue };
        await TestCase(filters, x => x.NumberOfDays == long.MinValue);
    }
    
    [Fact]
    public async Task FilterWith_ULongProperty_FiltersULongValues()
    {
        var filters = new TestFilter { NumberOfSeconds = ulong.MinValue };
        await TestCase(filters, x => x.NumberOfSeconds == ulong.MinValue);
    }
    
    [Fact]
    public async Task FilterWith_FloatProperty_FiltersFloatValues()
    {
        var filters = new TestFilter { TaxRate = float.MinValue };
        await TestCase(filters, x => x.TaxRate == float.MinValue);
    }
    
    [Fact]
    public async Task FilterWith_DoubleProperty_FiltersDoubleValues()
    {
        var filters = new TestFilter { SocialInsuranceRate = double.MinValue };
        await TestCase(filters, x => x.SocialInsuranceRate == double.MinValue);
    }
    
    [Fact]
    public async Task FilterWith_DecimalProperty_FiltersDecimalValues()
    {
        var filters = new TestFilter { IndexFundsRate = -1111111111111111.22m };
        await TestCase(filters, x => x.IndexFundsRate == -1111111111111111.22m);
    }
    
    [Fact]
    public async Task FilterWith_CharProperty_FiltersCharValues()
    {
        var filters = new TestFilter { Gender = char.MinValue };
        await TestCase(filters, x => x.Gender == char.MinValue);
    }

    private async Task TestCase(TestFilter filters, Expression<Func<LinqScribeClientDbContext.Entity, bool>> assertThat)
    {
        // Arrange
        await using var context = new LinqScribeClientDbContext(DbOptions);
        
        // Act
        var list = await context.Entities
            .AsNoTracking()
            .FilterWith(filters)
            .ToListAsync();
        
        // Assert
        list.ShouldAllBe(assertThat);
    }
}
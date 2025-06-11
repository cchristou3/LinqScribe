using System.Linq.Expressions;
using LinqScribe.IQueryableExtensions;
using LinqScribeTests.Fixtures;
using LinqScribeTests.Migrations;
using LinqScribeTests.TestDtos;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace LinqScribeTests.FiltersTests;

public class FiltersComplexTypesTests(SqlContainerFixture fixture) : IClassFixture<SqlContainerFixture>
{
    private DbContextOptions<LinqScribeClientDbContext> DbOptions { get; } = fixture.DbOptions;
    
    [Fact]
    public async Task FilterWith_FiltersByNestedAddressProperties_ReturnsMatchingCustomer()
    {
        // Arrange
        await using var context = new LinqScribeClientDbContext(DbOptions);
        var filters = new CustomerFilter
        {
            Address = new AddressFilter
            {
                Street = "123 Broadway", City = "New York", State = "NY", ZipCode = "10001"
            }
        };
        
        // Act
        var customer = await context.Customers
            .AsNoTracking()
            .Include(c => c.Address)
            .FilterWith(filters)
            .FirstOrDefaultAsync();
        
        // Assert
        customer.ShouldNotBeNull();
        customer.Address.ShouldNotBeNull();
        customer.Address.Street.ShouldBe(filters.Address.Street);
        customer.Address.City.ShouldBe(filters.Address.City);
        customer.Address.State.ShouldBe(filters.Address.State);
        customer.Address.ZipCode.ShouldBe(filters.Address.ZipCode);
    }

    [Fact]
    public async Task FilterWith_EmailOnlyFilter_ReturnsMatchingCustomer()
    {
        // Arrange
        await using var context = new LinqScribeClientDbContext(DbOptions);
        var filters = new CustomerFilter
        {
            Email = "alice@example.com",
            Address = null
        };

        // Act
        var customer = await context.Customers
            .AsNoTracking()
            .FilterWith(filters)
            .FirstOrDefaultAsync();

        // Assert
        customer.ShouldNotBeNull();
        customer.Email.ShouldBe(filters.Email);
    }
    
    [Fact]
    public async Task FilterWith_FiltersBy2ndLevelNestedGeoCoordinateProperties_ReturnsMatchingCustomer()
    {
        // Arrange
        await using var context = new LinqScribeClientDbContext(DbOptions);
        var filters = new CustomerFilter
        {
            Address = new AddressFilter
            {
                GeoCoordinate = new GeoCoordinateFilter
                {
                    Latitude = 40.71,
                    Longitude = -74.00
                }
            }
        };
        
        // Act
        var customer = await context.Customers
            .AsNoTracking()
            .Include(c => c.Address).ThenInclude(x => x.GeoCoordinate)
            .FilterWith(filters)
            .FirstOrDefaultAsync();
        
        // Assert
        customer.ShouldNotBeNull();
        customer.Address.ShouldNotBeNull();
        customer.Address.GeoCoordinate.ShouldNotBeNull();
        customer.Address.GeoCoordinate.Latitude.ShouldBe((double)filters.Address.GeoCoordinate.Latitude);
        customer.Address.GeoCoordinate.Longitude.ShouldBe((double)filters.Address.GeoCoordinate.Longitude);
    }
    
    [Fact]
    public async Task FilterWith_FiltersBy3rdLevelNestedRegionProperties_ReturnsMatchingCustomer()
    {
        // Arrange
        await using var context = new LinqScribeClientDbContext(DbOptions);
        var filters = new CustomerFilter
        {
            Address = new AddressFilter
            {
                GeoCoordinate = new GeoCoordinateFilter
                {
                    Region = new RegionFilter
                    {
                        Name = "North America", CountryCode = "US"
                    }
                }
            }
        };
        
        // Act
        var customer = await context.Customers
            .AsNoTracking()
            .Include(c => c.Address).ThenInclude(x => x.GeoCoordinate).ThenInclude(x => x.Region)
            .FilterWith(filters)
            .FirstOrDefaultAsync();
        
        // Assert
        customer.ShouldNotBeNull();
        customer.Address.ShouldNotBeNull();
        customer.Address.GeoCoordinate.ShouldNotBeNull();
        customer.Address.GeoCoordinate.Region.ShouldNotBeNull();
        customer.Address.GeoCoordinate.Region.Name.ShouldBe(filters.Address.GeoCoordinate.Region.Name);
        customer.Address.GeoCoordinate.Region.CountryCode.ShouldBe(filters.Address.GeoCoordinate.Region.CountryCode);
    }
}
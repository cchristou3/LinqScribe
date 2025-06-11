using Docker.DotNet.Models;
using Microsoft.EntityFrameworkCore;

namespace LinqScribeTests.Migrations;

public static class TestDataSeeder
{
    public static async Task SeedAsync(LinqScribeClientDbContext context)
    {
        var data = new List<LinqScribeClientDbContext.Entity>
        {
            new () { Name = "Gamma", IsActive = true, NumberOfRelatives = byte.MaxValue, NumberOfSiblings = sbyte.MaxValue, Salary = short.MaxValue, ExpectedSalary = ushort.MaxValue, NumberOfFriends = uint.MaxValue, NumberOfDays = long.MaxValue, NumberOfSeconds = ulong.MaxValue, TaxRate = float.MaxValue, SocialInsuranceRate = double.MaxValue, IndexFundsRate = 1111111111111111.22m, Gender = char.MaxValue, },
            new () { Name = "Alpha", IsActive = false, NumberOfRelatives = byte.MinValue, NumberOfSiblings = sbyte.MinValue, Salary = short.MinValue, ExpectedSalary = ushort.MinValue, NumberOfFriends = uint.MinValue, NumberOfDays = long.MinValue, NumberOfSeconds = ulong.MinValue, TaxRate = float.MinValue, SocialInsuranceRate = double.MinValue, IndexFundsRate = -1111111111111111.22m, Gender = char.MinValue, },
            new () { Name = "Beta", IsActive = false, NumberOfRelatives = byte.MinValue, NumberOfSiblings = sbyte.MinValue, Salary = short.MinValue, ExpectedSalary = ushort.MinValue, NumberOfFriends = uint.MinValue, NumberOfDays = long.MinValue, NumberOfSeconds = ulong.MinValue, TaxRate = float.MinValue, SocialInsuranceRate = double.MinValue, IndexFundsRate = -1111111111111111.22m, Gender = char.MinValue, },
            new () { Name = "Gamma", IsActive = true, NumberOfRelatives = byte.MinValue, NumberOfSiblings = sbyte.MinValue, Salary = short.MinValue, ExpectedSalary = ushort.MinValue, NumberOfFriends = uint.MinValue, NumberOfDays = long.MinValue, NumberOfSeconds = ulong.MinValue, TaxRate = float.MinValue, SocialInsuranceRate = double.MinValue, IndexFundsRate = -1111111111111111.22m, Gender = char.MinValue, }
        };

        await context.Entities.AddRangeAsync(data);
        await context.SaveChangesAsync();
    }

    public static async Task SeedCustomersAsync(LinqScribeClientDbContext context)
    {
        var regions = new[]
        {
            new LinqScribeClientDbContext.Region { Name = "North America", CountryCode = "US" },
            new LinqScribeClientDbContext.Region { Name = "South America", CountryCode = "BR" },
            new LinqScribeClientDbContext.Region { Name = "Europe", CountryCode = "DE" },
            new LinqScribeClientDbContext.Region { Name = "Asia", CountryCode = "JP" },
            new LinqScribeClientDbContext.Region { Name = "Oceania", CountryCode = "AU" },
            new LinqScribeClientDbContext.Region { Name = "Africa", CountryCode = "ZA" },
            new LinqScribeClientDbContext.Region { Name = "Middle East", CountryCode = "AE" },
            new LinqScribeClientDbContext.Region { Name = "Scandinavia", CountryCode = "SE" },
            new LinqScribeClientDbContext.Region { Name = "Eastern Europe", CountryCode = "PL" },
            new LinqScribeClientDbContext.Region { Name = "South Asia", CountryCode = "IN" },
        };
        
        var coordinates = new[]
        {
            new LinqScribeClientDbContext.GeoCoordinate { Latitude = 40.71, Longitude = -74.00, Region = regions[0] },
            new LinqScribeClientDbContext.GeoCoordinate { Latitude = -23.55, Longitude = -46.63, Region = regions[1] },
            new LinqScribeClientDbContext.GeoCoordinate { Latitude = 52.52, Longitude = 13.40, Region = regions[2] },
            new LinqScribeClientDbContext.GeoCoordinate { Latitude = 35.68, Longitude = 139.69, Region = regions[3] },
            new LinqScribeClientDbContext.GeoCoordinate { Latitude = -33.87, Longitude = 151.21, Region = regions[4] },
            new LinqScribeClientDbContext.GeoCoordinate { Latitude = -26.20, Longitude = 28.04, Region = regions[5] },
            new LinqScribeClientDbContext.GeoCoordinate { Latitude = 25.20, Longitude = 55.27, Region = regions[6] },
            new LinqScribeClientDbContext.GeoCoordinate { Latitude = 59.33, Longitude = 18.06, Region = regions[7] },
            new LinqScribeClientDbContext.GeoCoordinate { Latitude = 52.23, Longitude = 21.01, Region = regions[8] },
            new LinqScribeClientDbContext.GeoCoordinate { Latitude = 28.61, Longitude = 77.20, Region = regions[9] },
        };
        
        var addresses = new[]
        {
            new LinqScribeClientDbContext.Address { Street = "123 Broadway", City = "New York", State = "NY", ZipCode = "10001", GeoCoordinate = coordinates[0] },
            new LinqScribeClientDbContext.Address { Street = "456 Paulista Ave", City = "São Paulo", State = "SP", ZipCode = "01311-000", GeoCoordinate = coordinates[1] },
            new LinqScribeClientDbContext.Address { Street = "789 Unter den Linden", City = "Berlin", State = "BE", ZipCode = "10117", GeoCoordinate = coordinates[2] },
            new LinqScribeClientDbContext.Address { Street = "101 Shibuya", City = "Tokyo", State = "Tokyo", ZipCode = "150-0002", GeoCoordinate = coordinates[3] },
            new LinqScribeClientDbContext.Address { Street = "102 George St", City = "Sydney", State = "NSW", ZipCode = "2000", GeoCoordinate = coordinates[4] },
            new LinqScribeClientDbContext.Address { Street = "103 Nelson Mandela Dr", City = "Johannesburg", State = "Gauteng", ZipCode = "2001", GeoCoordinate = coordinates[5] },
            new LinqScribeClientDbContext.Address { Street = "104 Sheikh Zayed Rd", City = "Dubai", State = "Dubai", ZipCode = "00000", GeoCoordinate = coordinates[6] },
            new LinqScribeClientDbContext.Address { Street = "105 Drottninggatan", City = "Stockholm", State = "ST", ZipCode = "11160", GeoCoordinate = coordinates[7] },
            new LinqScribeClientDbContext.Address { Street = "106 Nowy Świat", City = "Warsaw", State = "MZ", ZipCode = "00-001", GeoCoordinate = coordinates[8] },
            new LinqScribeClientDbContext.Address { Street = "107 Connaught Place", City = "Delhi", State = "DL", ZipCode = "110001", GeoCoordinate = coordinates[9] },
        };
        
        var customers = new[]
        {
            new LinqScribeClientDbContext.Customer { FullName = "Alice Smith", Email = "alice@example.com", RegisteredAt = DateTime.Now.AddYears(-5), Address = addresses[0] },
            new LinqScribeClientDbContext.Customer { FullName = "Bruno Costa", Email = "bruno@example.com", RegisteredAt = DateTime.Now.AddYears(-3), Address = addresses[1] },
            new LinqScribeClientDbContext.Customer { FullName = "Clara Fischer", Email = "clara@example.com", RegisteredAt = DateTime.Now.AddYears(-2), Address = addresses[2] },
            new LinqScribeClientDbContext.Customer { FullName = "Daichi Tanaka", Email = "daichi@example.com", RegisteredAt = DateTime.Now.AddYears(-4), Address = addresses[3] },
            new LinqScribeClientDbContext.Customer { FullName = "Emily Brown", Email = "emily@example.com", RegisteredAt = DateTime.Now.AddYears(-1), Address = addresses[4] },
            new LinqScribeClientDbContext.Customer { FullName = "Fikile Khumalo", Email = "fikile@example.com", RegisteredAt = DateTime.Now.AddMonths(-18), Address = addresses[5] },
            new LinqScribeClientDbContext.Customer { FullName = "Ghaith Al-Maktoum", Email = "ghaith@example.com", RegisteredAt = DateTime.Now.AddMonths(-8), Address = addresses[6] },
            new LinqScribeClientDbContext.Customer { FullName = "Helena Borg", Email = "helena@example.com", RegisteredAt = DateTime.Now.AddYears(-6), Address = addresses[7] },
            new LinqScribeClientDbContext.Customer { FullName = "Igor Kowalski", Email = "igor@example.com", RegisteredAt = DateTime.Now.AddYears(-2), Address = addresses[8] },
            new LinqScribeClientDbContext.Customer { FullName = "Jyoti Patel", Email = "jyoti@example.com", RegisteredAt = DateTime.Now.AddYears(-3), Address = addresses[9] },
        };
        
        await context.Customers.AddRangeAsync(customers);
        await context.SaveChangesAsync();
    }
}
using LinqScribeTests.Migrations;
using Microsoft.EntityFrameworkCore;

namespace LinqScribeTests;

public static class TestDataSeeder
{
    public static async Task SeedAsync(LinqScribeClientDbContext context)
    {
        if (await context.Entities.AnyAsync()) return;

        var data = new List<LinqScribeClientDbContext.Entity>
        {
            new () { Name = "Gamma", IsActive = true, NumberOfRelatives = byte.MaxValue, NumberOfSiblings = sbyte.MaxValue, Salary = short.MaxValue, ExpectedSalary = ushort.MaxValue, NumberOfFriends = uint.MaxValue, NumberOfDays = long.MaxValue, NumberOfSeconds = ulong.MaxValue, TaxRate = float.MaxValue, SocialInsuranceRate = double.MaxValue, IndexFundsRate = 1111111111111111.22m, Gender = char.MaxValue, },
            new () { Name = "Alpha", IsActive = false, NumberOfRelatives = byte.MinValue, NumberOfSiblings = sbyte.MinValue, Salary = short.MinValue, ExpectedSalary = ushort.MinValue, NumberOfFriends = uint.MinValue, NumberOfDays = long.MinValue, NumberOfSeconds = ulong.MinValue, TaxRate = float.MinValue, SocialInsuranceRate = double.MinValue, IndexFundsRate = -1111111111111111.22m, Gender = char.MinValue, },
            new () { Name = "Beta", IsActive = false, NumberOfRelatives = byte.MinValue, NumberOfSiblings = sbyte.MinValue, Salary = short.MinValue, ExpectedSalary = ushort.MinValue, NumberOfFriends = uint.MinValue, NumberOfDays = long.MinValue, NumberOfSeconds = ulong.MinValue, TaxRate = float.MinValue, SocialInsuranceRate = double.MinValue, IndexFundsRate = -1111111111111111.22m, Gender = char.MinValue, },
            new () { Name = "Gamma", IsActive = true, NumberOfRelatives = byte.MinValue, NumberOfSiblings = sbyte.MinValue, Salary = short.MinValue, ExpectedSalary = ushort.MinValue, NumberOfFriends = uint.MinValue, NumberOfDays = long.MinValue, NumberOfSeconds = ulong.MinValue, TaxRate = float.MinValue, SocialInsuranceRate = double.MinValue, IndexFundsRate = -1111111111111111.22m, Gender = char.MinValue, }
        };

        await context.Entities.AddRangeAsync(data);
        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine(ex.InnerException);
            throw;
        }
    }
}
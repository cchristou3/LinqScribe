namespace LinqScribeTests.TestDtos;

public class TestFilter
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public byte NumberOfRelatives { get; set; }
    public sbyte NumberOfSiblings { get; set; }
    public short Salary { get; set; }
    public ushort ExpectedSalary { get; set; }
    public uint NumberOfFriends { get; set; }
    public long NumberOfDays { get; set; }
    public ulong NumberOfSeconds { get; set; }
        
    public float TaxRate { get; set; }
    public double SocialInsuranceRate { get; set; }
    public decimal IndexFundsRate { get; set; }
        
    public char  Gender { get; set; }
}
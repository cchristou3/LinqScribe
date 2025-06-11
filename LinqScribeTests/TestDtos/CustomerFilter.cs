namespace LinqScribeTests.TestDtos;

public class CustomerFilter
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public DateTime? RegisteredAt { get; set; }

    public AddressFilter? Address { get; set; }
}

public class AddressFilter
{
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    
    public GeoCoordinateFilter GeoCoordinate { get; set; }
}


public class GeoCoordinateFilter
{
    public int? Id { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public  RegionFilter? Region { get; set; }
}
    
public class RegionFilter
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? CountryCode { get; set; }
}
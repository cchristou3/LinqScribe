using Microsoft.EntityFrameworkCore;

namespace LinqScribeTests.Migrations;

public class LinqScribeClientDbContext : DbContext
{
    public DbSet<Entity> Entities => Set<Entity>();
    
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Region> Regions => Set<Region>();
    public DbSet<GeoCoordinate> GeoCoordinates => Set<GeoCoordinate>();
    public DbSet<Address> Addresses => Set<Address>();
    
    public LinqScribeClientDbContext(DbContextOptions<LinqScribeClientDbContext> options) : base(options)
    {}
    
    public LinqScribeClientDbContext()
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Entity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
        });
        
        modelBuilder.Entity<Customer>()
            .HasOne(c => c.Address);
        
        modelBuilder.Entity<Address>()
            .HasOne(c => c.GeoCoordinate);
        
        modelBuilder.Entity<GeoCoordinate>()
            .HasOne(c => c.Region);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId);
    }
    
    public class Entity
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
    
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public DateTime RegisteredAt { get; set; }

        public Address Address { get; set; } = new();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }

    public class Order
    {
        public Guid Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = default!;

        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = default!;
    }

    public class Address
    {
        public Guid Id { get; set; }
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string State { get; set; } = default!;
        public string ZipCode { get; set; } = default!;
        
        public GeoCoordinate GeoCoordinate { get; set; }
    }
    
    public class GeoCoordinate
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public required Region Region { get; set; }
    }
    
    public class Region
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string CountryCode { get; set; }
    }
}
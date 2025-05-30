using Microsoft.EntityFrameworkCore;

namespace LinqScribeTests.Migrations;

public class LinqScribeClientDbContext : DbContext
{
    public DbSet<Entity> Entities { get; set; }
    
    public LinqScribeClientDbContext(DbContextOptions<LinqScribeClientDbContext> options) : base(options)
    {}
    
    public LinqScribeClientDbContext() : base()
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Entity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
        });
    }

    public async Task AddEntitiesAsync(params Entity[] entities)
    {
        await Entities.AddRangeAsync(entities);
        await SaveChangesAsync();
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
}
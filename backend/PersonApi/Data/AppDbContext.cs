using Microsoft.EntityFrameworkCore;

namespace PersonApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Person> Persons => Set<Person>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(e =>
        {
            e.ToTable("persons");
            e.HasKey(p => p.Id);
            e.Property(p => p.Id).HasColumnName("id");
            e.Property(p => p.FirstName).HasColumnName("first_name").HasMaxLength(100).IsRequired();
            e.Property(p => p.LastName).HasColumnName("last_name").HasMaxLength(100).IsRequired();
            e.Property(p => p.BirthDate).HasColumnName("birth_date").IsRequired();
            e.Property(p => p.Address).HasColumnName("address").HasMaxLength(500);
            e.Property(p => p.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");
        });
    }
}

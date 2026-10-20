using Microsoft.EntityFrameworkCore;

namespace FitnessEf;

public sealed class FitnessContext : DbContext
{
    public DbSet<Client> Clients => Set<Client>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string connectionString = "Host=localhost;Port=5432;Database=FitnessEfTraining;Username=postgres;Password=123";
            optionsBuilder.UseNpgsql(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("clients");
            entity.HasKey(c => c.ClientId);
            entity.Property(c => c.ClientId).HasColumnName("client_id").ValueGeneratedOnAdd();
            entity.Property(c => c.FirstName).HasColumnName("first_name").HasMaxLength(100).IsRequired();
            entity.Property(c => c.LastName).HasColumnName("last_name").HasMaxLength(100).IsRequired();
            entity.Property(c => c.BirthDate).HasColumnName("birth_date").HasColumnType("date");
            entity.Property(c => c.Phone).HasColumnName("phone").HasMaxLength(30);
            entity.Property(c => c.Email).HasColumnName("email").HasMaxLength(254);
            entity.Property(c => c.RegistrationDate).HasColumnName("registration_date").HasColumnType("date").IsRequired();
            entity.Property(c => c.Status).HasColumnName("status").HasMaxLength(20).IsRequired();
        });
    }
}

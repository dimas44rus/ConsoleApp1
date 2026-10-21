using Microsoft.EntityFrameworkCore;

namespace FitnessEf;

public sealed class FitnessContext : DbContext
{
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Visit> Visits => Set<Visit>();


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
        modelBuilder.Entity<Visit>(entity =>
        {
            entity.Property(c => c.Note).HasColumnName("note").HasMaxLength(500);

            entity.ToTable("visits");
            entity.HasKey(v => v.VisitId);

            entity.Property(v => v.VisitId)
                .HasColumnName("visit_id")
                .ValueGeneratedOnAdd();

            entity.Property(v => v.ClientId)
                .HasColumnName("client_id")
                .IsRequired();

            entity.Property(v => v.VisitDate)
                .HasColumnName("visit_date")
                .HasColumnType("date")
                .IsRequired();

            entity.Property(v => v.Note)
                .HasColumnName("note")
                .HasMaxLength(500);

            entity.HasOne(v => v.Client)
                .WithMany(c => c.Visits)
                .HasForeignKey(v => v.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
        });

    }
}

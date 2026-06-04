using Microsoft.EntityFrameworkCore;
using SpaceCare.Domain.Comportamentos;
using SpaceCare.Domain.Telemetrias;
using SpaceCare.Domain.Turistas;

namespace SpaceCare.Infra.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Turista> Turistas { get; set; }
        public DbSet<Telemetria> Telemetrias { get; set; }
        public DbSet<Comportamento> Comportamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Turista>()
                .HasIndex(t => t.PassaporteEspacial)
                .IsUnique();

            modelBuilder.Entity<Turista>()
                .HasIndex(t => t.Email)
                .IsUnique();

            modelBuilder.Entity<Turista>()
                .Property(t => t.Ativo)
                .HasColumnType("CHAR(1)")
                .HasDefaultValue("1");

            modelBuilder.Entity<Telemetria>(b =>
            {
                b.ToTable("SC_TELEMETRIAS");
                b.HasKey(t => t.Id);
                b.Property(t => t.Id).HasColumnName("ID");

                b.HasOne(t => t.Turista)
                    .WithMany()
                    .HasForeignKey(t => t.TuristaId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Comportamento>(b =>
            {
                b.ToTable("SC_COMPORTAMENTOS");
                b.HasKey(c => c.Id);
                b.Property(c => c.Id).HasColumnName("ID");

                b.Property(c => c.GestoDetectado).HasConversion<int>();
                b.Property(c => c.NivelAlerta).HasConversion<int>();

                b.HasOne(c => c.Turista)
                    .WithMany()
                    .HasForeignKey(c => c.TuristaId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}

using Alert.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Alert.API.Data
{
    public partial class AlertDbContext : DbContext
    {
        public virtual DbSet<Models.Alert> Alert { get; set; }
        public virtual DbSet<AlertConclusion> AlertConclusion { get; set; }
        public virtual DbSet<AlertExplanation> AlertExplanation { get; set; }
        public virtual DbSet<AlertSolution> AlertSolution { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<MonitoredDevice> MonitoredDevice { get; set; }
        public virtual DbSet<SystemAdministator> SystemAdministator { get; set; }


        public AlertDbContext(DbContextOptions<AlertDbContext> options) : base((DbContextOptions) options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Alert>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.TimeGenerated).HasColumnType("datetime");

                entity.HasOne(d => d.AlertConclusion)
                    .WithMany(p => p.Alert)
                    .HasForeignKey(d => d.AlertConclusionId)
                    .HasConstraintName("FK_Alert_AlertConclusion");

                entity.HasOne(d => d.AlertExplanation)
                    .WithMany(p => p.Alert)
                    .HasForeignKey(d => d.AlertExplanationId)
                    .HasConstraintName("FK_Alert_AlertExplanation");

                entity.HasOne(d => d.AlertSolution)
                    .WithMany(p => p.Alert)
                    .HasForeignKey(d => d.AlertSolutionId)
                    .HasConstraintName("FK_Alert_AlertSolution");

                entity.HasOne(d => d.MonitoredDevice)
                    .WithMany(p => p.Alert)
                    .HasForeignKey(d => d.MonitoredDeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Alert_MonitoredDevice");
            });

            modelBuilder.Entity<AlertConclusion>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<AlertExplanation>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<AlertSolution>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });


            modelBuilder.Entity<MonitoredDevice>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.MonitoredDevice)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MonitoredDevice_Company");
            });

            modelBuilder.Entity<SystemAdministator>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.SystemAdministator)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SystemAdministator_Company");
            });
        }
    }
}

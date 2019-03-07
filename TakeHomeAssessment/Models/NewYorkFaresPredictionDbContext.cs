using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TakeHomeAssessment.Models
{
    public partial class NewYorkFaresPredictionDbContext : DbContext
    {
        //public NewYorkFaresPredictionDbContext()
        //{
        //}

        public NewYorkFaresPredictionDbContext(DbContextOptions<NewYorkFaresPredictionDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TaxiZones> TaxiZones { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=tcp:takehomeassessment.database.windows.net,1433;Initial Catalog=NewYorkFaresPrediction;Persist Security Info=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity<TaxiZones>(entity =>
            {
                entity.HasKey(e => e.LocationId);

                entity.ToTable("TaxiZones", "ref");

                entity.Property(e => e.LocationId).ValueGeneratedNever();

                entity.Property(e => e.Borough)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceZone)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Zone)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}

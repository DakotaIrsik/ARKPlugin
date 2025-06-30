using ArkFury.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace ArkFury.Entities.Models
{
    public partial class ArkShopContext : DbContext
    {

        public virtual DbSet<Arkshopplayers> Arkshopplayers { get; set; }

        private AppSettings _settings { get; set; }

        public ArkShopContext(DbContextOptions<ArkShopContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Arkshopplayers>(entity =>
            {
                entity.ToTable("arkshopplayers");

                entity.HasIndex(e => e.SteamId)
                    .HasName("SteamId_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Kits)
                    .IsRequired()
                    .HasColumnType("varchar(768)")
                    .HasDefaultValueSql("'{}'")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Points).HasDefaultValueSql("'0'");

                entity.Property(e => e.TotalSpent).HasDefaultValueSql("'0'");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
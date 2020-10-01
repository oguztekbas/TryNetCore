using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TryNetCore.ORM.Entity
{
    public partial class TryNetCoreContext : DbContext
    {
        public TryNetCoreContext()
        {
        }

        public TryNetCoreContext(DbContextOptions<TryNetCoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Tablo1> Tablo1 { get; set; }
        public virtual DbSet<Tablo2> Tablo2 { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-CHGIFVH;Database=TryNetCoreDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tablo1>(entity =>
            {
                entity.HasIndex(e => e.Tablo1Name)
                    .HasName("DenemeIndex");

                entity.Property(e => e.Tablo1Name).HasMaxLength(200);
            });

            modelBuilder.Entity<Tablo2>(entity =>
            {
                entity.HasOne(d => d.Tablo1)
                    .WithMany(p => p.Tablo2)
                    .HasForeignKey(d => d.Tablo1Id)
                    .HasConstraintName("FK_Tablo2_Tablo1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

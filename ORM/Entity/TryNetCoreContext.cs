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

        public virtual DbSet<Blog> Blog { get; set; }
        public virtual DbSet<BlogImages> BlogImages { get; set; }
        public virtual DbSet<BlogTags> BlogTags { get; set; }

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
            modelBuilder.Entity<Blog>(entity =>
            {
                entity.HasIndex(e => e.BlogRouteUrl)
                    .HasName("NonClusteredIndex-20201002-223754")
                    .IsUnique();

                entity.Property(e => e.BlogAuthor).IsRequired();

                entity.Property(e => e.BlogCategoryName).IsRequired();

                entity.Property(e => e.BlogContent).IsRequired();

                entity.Property(e => e.BlogImagePath).IsRequired();

                entity.Property(e => e.BlogName)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.BlogPostDate).IsRequired();

                entity.Property(e => e.BlogRouteUrl)
                    .IsRequired()
                    .HasMaxLength(300);
            });

            modelBuilder.Entity<BlogImages>(entity =>
            {
                entity.Property(e => e.ImagePath).IsRequired();

                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.BlogImages)
                    .HasForeignKey(d => d.BlogId)
                    .HasConstraintName("FK_BlogImages_Blog");
            });

            modelBuilder.Entity<BlogTags>(entity =>
            {
                entity.Property(e => e.BlogTagName).IsRequired();

                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.BlogTags)
                    .HasForeignKey(d => d.BlogId)
                    .HasConstraintName("FK_BlogTags_Blog");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

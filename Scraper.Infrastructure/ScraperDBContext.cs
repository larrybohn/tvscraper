using System;
using Microsoft.EntityFrameworkCore;
using Scraper.Infrastructure.Models;

namespace Scraper.Infrastructure
{
    public class ScraperDBContext : DbContext
    {
        public ScraperDBContext()
        {
        }

        public ScraperDBContext(string connectionString)
            : base(GetOptions(connectionString))
        {
        }

        public ScraperDBContext(DbContextOptions<ScraperDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Actor> Actors { get; set; }
        public virtual DbSet<ActorShow> ActorShow { get; set; }
        public virtual DbSet<Show> Shows { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new InvalidOperationException("ScraperDBContext is not configured");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Actor>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<ActorShow>(entity =>
            {
                entity.HasKey(e => new { e.ActorId, e.ShowId });

                entity.HasOne(d => d.Actor)
                    .WithMany(p => p.ActorShow)
                    .HasForeignKey(d => d.ActorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ActorShow_Actor");

                entity.HasOne(d => d.Show)
                    .WithMany(p => p.ActorShow)
                    .HasForeignKey(d => d.ShowId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ActorShow_Show");
            });

            modelBuilder.Entity<Show>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Title).IsRequired();
            });
        }

        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        }
    }
}

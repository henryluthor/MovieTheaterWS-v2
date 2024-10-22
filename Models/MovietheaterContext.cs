using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MovieTheaterWS_v2.Models;

public partial class MovietheaterContext : DbContext
{
    public MovietheaterContext()
    {
    }

    public MovietheaterContext(DbContextOptions<MovietheaterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Movie> Movies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-R601H3RA\\SQLEXPRESS;Database=movietheater;Trusted_Connection=true;TrustServerCertificate=true;Persist Security Info=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>(entity =>
        {
            entity.ToTable("Movie");

            entity.Property(e => e.Genre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Runtime).HasComment("Runtime in minutes.");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Year).HasComment("Year of release of the movie.");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

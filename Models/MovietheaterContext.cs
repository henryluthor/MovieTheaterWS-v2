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

    public virtual DbSet<Complex> Complexes { get; set; }

    public virtual DbSet<ComplexMovie> ComplexMovies { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Systemuser> Systemusers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=HENRYLAPTOP\\SQLEXPRESS;Database=movietheater;Trusted_Connection=True;TrustServerCertificate=true;Persist Security Info=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AI");

        modelBuilder.Entity<Complex>(entity =>
        {
            entity.HasKey(e => e.IdComplex);

            entity.ToTable("Complex");

            entity.Property(e => e.IdComplex).HasColumnName("idComplex");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<ComplexMovie>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ComplexMovie");

            entity.Property(e => e.IdComplex).HasColumnName("idComplex");
            entity.Property(e => e.IdMovie).HasColumnName("idMovie");

            entity.HasOne(d => d.IdMovieNavigation).WithMany()
                .HasForeignKey(d => d.IdMovie)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComplexMovie_Movie");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.IdMovie);

            entity.ToTable("Movie");

            entity.Property(e => e.IdMovie).HasColumnName("idMovie");
            entity.Property(e => e.Genre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Imdbid)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Movie id from www.imdb.com")
                .HasColumnName("IMDBId");
            entity.Property(e => e.Runtime).HasComment("Runtime in minutes.");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Year).HasComment("Year of release of the movie.");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole);

            entity.ToTable("Role");

            entity.Property(e => e.IdRole).HasColumnName("idRole");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("description");
        });

        modelBuilder.Entity<Systemuser>(entity =>
        {
            entity.ToTable("Systemuser");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("firstName");
            entity.Property(e => e.IdRole).HasColumnName("idRole");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("lastName");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("passwordHash");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Systemusers)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Systemuser_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

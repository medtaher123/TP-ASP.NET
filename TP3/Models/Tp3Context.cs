using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TP3.Models;

public partial class Tp3Context : DbContext
{
    public Tp3Context()
    {
    }

    public Tp3Context(DbContextOptions<Tp3Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Membershiptype> Membershiptypes { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

//     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//         => optionsBuilder.UseSqlite("Data Source=./Database/tp3.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.MembershiptypeId).HasColumnName("membershiptype_id");

            entity.HasOne(d => d.Membershiptype).WithMany(p => p.Customers).HasForeignKey(d => d.MembershiptypeId);

            entity.HasMany(d => d.Movies).WithMany(p => p.Customers)
                .UsingEntity<Dictionary<string, object>>(
                    "CustomerMovie",
                    r => r.HasOne<Movie>().WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    l => l.HasOne<Customer>().WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    j =>
                    {
                        j.HasKey("CustomerId", "MovieId");
                        j.ToTable("CustomerMovies");
                    });
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Membershiptype>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DiscountRate).HasColumnType("NUMERIC");
            entity.Property(e => e.SignUpFee).HasColumnType("NUMERIC");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.GenreId).HasColumnName("genre_id");

            entity.HasOne(d => d.Genre).WithMany(p => p.Movies).HasForeignKey(d => d.GenreId);
        });

        // Seed Membershiptype
        modelBuilder.Entity<Membershiptype>().HasData(
            new Membershiptype { Id = 1, DurationInMonth = 6, DiscountRate = 10, SignUpFee = 50 },
            new Membershiptype { Id = 2, DurationInMonth = 12, DiscountRate = 20, SignUpFee = 100 }
        );

        // Seed Genre
        modelBuilder.Entity<Genre>().HasData(
            new Genre { Id = 1, GenreName = "Action" },
            new Genre { Id = 2, GenreName = "Comedy" },
            new Genre { Id = 3, GenreName = "Drama" }
        );

        // Seed Movies
        modelBuilder.Entity<Movie>().HasData(
            new Movie { Id = 1, GenreId = 1, Name = "Movie 1" },
            new Movie { Id = 2, GenreId = 2, Name = "Movie 2" },
            new Movie { Id = 3, GenreId = 3, Name = "Movie 3" }
        );

        // Seed Customers
        modelBuilder.Entity<Customer>().HasData(
            new Customer { Id = 1, Name = "Customer1", MembershiptypeId = 1 },
            new Customer { Id = 2, Name = "Customer2", MembershiptypeId = 2 }
        );

        // Seed the Customer-Movie Many-to-Many Relationship
        modelBuilder.Entity("CustomerMovie").HasData(
            new { CustomerId = 1, MovieId = 1 },
            new { CustomerId = 1, MovieId = 2 },
            new { CustomerId = 2, MovieId = 2 }
        );
    
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

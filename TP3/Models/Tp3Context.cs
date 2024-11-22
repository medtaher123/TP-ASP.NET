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

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        // => optionsBuilder.UseSqlite("Data Source=./Database/tp3.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired();
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
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Membershiptype>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.DiscountRate).HasColumnType("NUMERIC");
            entity.Property(e => e.SignUpFee).HasColumnType("NUMERIC");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.GenreId).HasColumnName("genre_id");
            entity.Property(e => e.ReleaseDate).HasColumnType("datetime");
            entity.Property(e => e.ImagePath).HasMaxLength(255);               

            entity.HasOne(d => d.Genre).WithMany(p => p.Movies).HasForeignKey(d => d.GenreId);
        });
        Guid m1 = Guid.NewGuid();
        Guid m2 = Guid.NewGuid();
        // Seed Membershiptype
        modelBuilder.Entity<Membershiptype>().HasData(
            new Membershiptype { Id = m1, Name = "Basic", DurationInMonth = 6, DiscountRate = 10, SignUpFee = 50 },
            new Membershiptype { Id = m2, Name = "Premium", DurationInMonth = 12, DiscountRate = 20, SignUpFee = 100 }
        );

        Guid g1 = Guid.NewGuid();
        Guid g2 = Guid.NewGuid();
        Guid g3 = Guid.NewGuid();

        // Seed Genre
        modelBuilder.Entity<Genre>().HasData(
            new Genre { Id = g1, GenreName = "Action" },
            new Genre { Id = g2, GenreName = "Comedy" },
            new Genre { Id = g3, GenreName = "Drama" }
        );

        Guid mv1 = Guid.NewGuid();
        Guid mv2 = Guid.NewGuid();
        Guid mv3 = Guid.NewGuid();
        // Seed Movies
        modelBuilder.Entity<Movie>().HasData(
            new Movie { Id = mv1, GenreId = g1, Name = "Movie 1", ReleaseDate = DateTime.Now.AddMonths(-3)},
            new Movie { Id = mv2, GenreId = g2, Name = "Movie 2", ReleaseDate = DateTime.Now.AddMonths(-1)},
            new Movie { Id = mv3, GenreId = g3, Name = "Movie 3", ReleaseDate = DateTime.Now }

        );

        Guid c1 = Guid.NewGuid();
        Guid c2 = Guid.NewGuid();
        // Seed Customers
        modelBuilder.Entity<Customer>().HasData(
            new Customer { Id = c1, Name = "Customer1", MembershiptypeId = m1 },
            new Customer { Id = c2, Name = "Customer2", MembershiptypeId = m2 }
        );

        // Seed the Customer-Movie Many-to-Many Relationship
        modelBuilder.Entity("CustomerMovie").HasData(
            new { CustomerId = c1, MovieId = mv1 },
            new { CustomerId = c1, MovieId = mv2 },
            new { CustomerId = c2, MovieId = mv2 }
        );
    
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TP4.Models;

#nullable disable

namespace TP4.Migrations
{
    [DbContext(typeof(Tp4Context))]
    [Migration("20241122025303_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("CustomerMovie", b =>
                {
                    b.Property<Guid>("CustomerId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("MovieId")
                        .HasColumnType("TEXT");

                    b.HasKey("CustomerId", "MovieId");

                    b.HasIndex("MovieId");

                    b.ToTable("CustomerMovies", (string)null);

                    b.HasData(
                        new
                        {
                            CustomerId = new Guid("61c18cb0-aaae-4ed1-b66b-991082b306a0"),
                            MovieId = new Guid("48f00b1c-0d7a-44f6-9f05-bfff280642e3")
                        },
                        new
                        {
                            CustomerId = new Guid("61c18cb0-aaae-4ed1-b66b-991082b306a0"),
                            MovieId = new Guid("3a4baf45-1065-46ec-9d10-add7cd0d7155")
                        },
                        new
                        {
                            CustomerId = new Guid("31e31368-776a-42f2-9aa6-b4f59d8bc4c6"),
                            MovieId = new Guid("3a4baf45-1065-46ec-9d10-add7cd0d7155")
                        });
                });

            modelBuilder.Entity("TP4.Models.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("MembershiptypeId")
                        .HasColumnType("TEXT")
                        .HasColumnName("membershiptype_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MembershiptypeId");

                    b.ToTable("Customers");

                    b.HasData(
                        new
                        {
                            Id = new Guid("61c18cb0-aaae-4ed1-b66b-991082b306a0"),
                            MembershiptypeId = new Guid("4cc42dd0-0365-4f33-9e10-88da09a8685a"),
                            Name = "Customer1"
                        },
                        new
                        {
                            Id = new Guid("31e31368-776a-42f2-9aa6-b4f59d8bc4c6"),
                            MembershiptypeId = new Guid("7bcbf1b5-07cc-449d-b35c-a649c73a519d"),
                            Name = "Customer2"
                        });
                });

            modelBuilder.Entity("TP4.Models.Genre", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("GenreName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Genres");

                    b.HasData(
                        new
                        {
                            Id = new Guid("536f48d5-40a8-4807-8bec-19c14d6dec5b"),
                            GenreName = "Action"
                        },
                        new
                        {
                            Id = new Guid("d2a7d9b5-ab00-450c-ae7c-03ea333ee534"),
                            GenreName = "Comedy"
                        },
                        new
                        {
                            Id = new Guid("60a037c3-137e-4bba-bfee-f3978ba7aeaf"),
                            GenreName = "Drama"
                        });
                });

            modelBuilder.Entity("TP4.Models.Membershiptype", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("DiscountRate")
                        .HasColumnType("NUMERIC");

                    b.Property<int?>("DurationInMonth")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("SignUpFee")
                        .HasColumnType("NUMERIC");

                    b.HasKey("Id");

                    b.ToTable("Membershiptypes");

                    b.HasData(
                        new
                        {
                            Id = new Guid("4cc42dd0-0365-4f33-9e10-88da09a8685a"),
                            DiscountRate = 10m,
                            DurationInMonth = 6,
                            Name = "Basic",
                            SignUpFee = 50m
                        },
                        new
                        {
                            Id = new Guid("7bcbf1b5-07cc-449d-b35c-a649c73a519d"),
                            DiscountRate = 20m,
                            DurationInMonth = 12,
                            Name = "Premium",
                            SignUpFee = 100m
                        });
                });

            modelBuilder.Entity("TP4.Models.Movie", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("GenreId")
                        .HasColumnType("TEXT")
                        .HasColumnName("genre_id");

                    b.Property<string>("ImagePath")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("GenreId");

                    b.ToTable("Movies");

                    b.HasData(
                        new
                        {
                            Id = new Guid("48f00b1c-0d7a-44f6-9f05-bfff280642e3"),
                            GenreId = new Guid("536f48d5-40a8-4807-8bec-19c14d6dec5b"),
                            Name = "Movie 1",
                            ReleaseDate = new DateTime(2024, 8, 22, 3, 53, 3, 347, DateTimeKind.Local).AddTicks(3038)
                        },
                        new
                        {
                            Id = new Guid("3a4baf45-1065-46ec-9d10-add7cd0d7155"),
                            GenreId = new Guid("d2a7d9b5-ab00-450c-ae7c-03ea333ee534"),
                            Name = "Movie 2",
                            ReleaseDate = new DateTime(2024, 10, 22, 3, 53, 3, 347, DateTimeKind.Local).AddTicks(3081)
                        },
                        new
                        {
                            Id = new Guid("20a4d230-a0c9-4ef6-b19c-0e3e5d33cb90"),
                            GenreId = new Guid("60a037c3-137e-4bba-bfee-f3978ba7aeaf"),
                            Name = "Movie 3",
                            ReleaseDate = new DateTime(2024, 11, 22, 3, 53, 3, 347, DateTimeKind.Local).AddTicks(3085)
                        });
                });

            modelBuilder.Entity("CustomerMovie", b =>
                {
                    b.HasOne("TP4.Models.Customer", null)
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .IsRequired();

                    b.HasOne("TP4.Models.Movie", null)
                        .WithMany()
                        .HasForeignKey("MovieId")
                        .IsRequired();
                });

            modelBuilder.Entity("TP4.Models.Customer", b =>
                {
                    b.HasOne("TP4.Models.Membershiptype", "Membershiptype")
                        .WithMany("Customers")
                        .HasForeignKey("MembershiptypeId");

                    b.Navigation("Membershiptype");
                });

            modelBuilder.Entity("TP4.Models.Movie", b =>
                {
                    b.HasOne("TP4.Models.Genre", "Genre")
                        .WithMany("Movies")
                        .HasForeignKey("GenreId");

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("TP4.Models.Genre", b =>
                {
                    b.Navigation("Movies");
                });

            modelBuilder.Entity("TP4.Models.Membershiptype", b =>
                {
                    b.Navigation("Customers");
                });
#pragma warning restore 612, 618
        }
    }
}

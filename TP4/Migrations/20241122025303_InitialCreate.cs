using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TP4.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    GenreName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Membershiptypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    SignUpFee = table.Column<decimal>(type: "NUMERIC", nullable: true),
                    DurationInMonth = table.Column<int>(type: "INTEGER", nullable: true),
                    DiscountRate = table.Column<decimal>(type: "NUMERIC", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Membershiptypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    genre_id = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movies_Genres_genre_id",
                        column: x => x.genre_id,
                        principalTable: "Genres",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    membershiptype_id = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Membershiptypes_membershiptype_id",
                        column: x => x.membershiptype_id,
                        principalTable: "Membershiptypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerMovies",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MovieId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerMovies", x => new { x.CustomerId, x.MovieId });
                    table.ForeignKey(
                        name: "FK_CustomerMovies_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerMovies_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "GenreName" },
                values: new object[,]
                {
                    { new Guid("536f48d5-40a8-4807-8bec-19c14d6dec5b"), "Action" },
                    { new Guid("60a037c3-137e-4bba-bfee-f3978ba7aeaf"), "Drama" },
                    { new Guid("d2a7d9b5-ab00-450c-ae7c-03ea333ee534"), "Comedy" }
                });

            migrationBuilder.InsertData(
                table: "Membershiptypes",
                columns: new[] { "Id", "DiscountRate", "DurationInMonth", "Name", "SignUpFee" },
                values: new object[,]
                {
                    { new Guid("4cc42dd0-0365-4f33-9e10-88da09a8685a"), 10m, 6, "Basic", 50m },
                    { new Guid("7bcbf1b5-07cc-449d-b35c-a649c73a519d"), 20m, 12, "Premium", 100m }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "membershiptype_id", "Name" },
                values: new object[,]
                {
                    { new Guid("31e31368-776a-42f2-9aa6-b4f59d8bc4c6"), new Guid("7bcbf1b5-07cc-449d-b35c-a649c73a519d"), "Customer2" },
                    { new Guid("61c18cb0-aaae-4ed1-b66b-991082b306a0"), new Guid("4cc42dd0-0365-4f33-9e10-88da09a8685a"), "Customer1" }
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "genre_id", "ImagePath", "Name", "ReleaseDate" },
                values: new object[,]
                {
                    { new Guid("20a4d230-a0c9-4ef6-b19c-0e3e5d33cb90"), new Guid("60a037c3-137e-4bba-bfee-f3978ba7aeaf"), null, "Movie 3", new DateTime(2024, 11, 22, 3, 53, 3, 347, DateTimeKind.Local).AddTicks(3085) },
                    { new Guid("3a4baf45-1065-46ec-9d10-add7cd0d7155"), new Guid("d2a7d9b5-ab00-450c-ae7c-03ea333ee534"), null, "Movie 2", new DateTime(2024, 10, 22, 3, 53, 3, 347, DateTimeKind.Local).AddTicks(3081) },
                    { new Guid("48f00b1c-0d7a-44f6-9f05-bfff280642e3"), new Guid("536f48d5-40a8-4807-8bec-19c14d6dec5b"), null, "Movie 1", new DateTime(2024, 8, 22, 3, 53, 3, 347, DateTimeKind.Local).AddTicks(3038) }
                });

            migrationBuilder.InsertData(
                table: "CustomerMovies",
                columns: new[] { "CustomerId", "MovieId" },
                values: new object[,]
                {
                    { new Guid("31e31368-776a-42f2-9aa6-b4f59d8bc4c6"), new Guid("3a4baf45-1065-46ec-9d10-add7cd0d7155") },
                    { new Guid("61c18cb0-aaae-4ed1-b66b-991082b306a0"), new Guid("3a4baf45-1065-46ec-9d10-add7cd0d7155") },
                    { new Guid("61c18cb0-aaae-4ed1-b66b-991082b306a0"), new Guid("48f00b1c-0d7a-44f6-9f05-bfff280642e3") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerMovies_MovieId",
                table: "CustomerMovies",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_membershiptype_id",
                table: "Customers",
                column: "membershiptype_id");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_genre_id",
                table: "Movies",
                column: "genre_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerMovies");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Membershiptypes");

            migrationBuilder.DropTable(
                name: "Genres");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TP7.Migrations
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
                    { new Guid("30d52886-c2d4-48f5-acf8-5702023674a0"), "Action" },
                    { new Guid("37fb9b54-8f4f-44fe-9cad-c30f21e91b51"), "Drama" },
                    { new Guid("e18fab3d-78f5-4ade-a3c8-c365e9602c62"), "Comedy" }
                });

            migrationBuilder.InsertData(
                table: "Membershiptypes",
                columns: new[] { "Id", "DiscountRate", "DurationInMonth", "Name", "SignUpFee" },
                values: new object[,]
                {
                    { new Guid("48332a5e-6cf8-4c92-9255-3579431ec739"), 10m, 6, "Basic", 50m },
                    { new Guid("4aa675bf-b31a-4964-8ad3-094db9f95f5d"), 20m, 12, "Premium", 100m }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "membershiptype_id", "Name" },
                values: new object[,]
                {
                    { new Guid("02d25b7b-aa5a-4fea-b40c-fa843a788f28"), new Guid("4aa675bf-b31a-4964-8ad3-094db9f95f5d"), "Customer2" },
                    { new Guid("1ce81473-96ad-43ad-ab4d-4b79ea54480a"), new Guid("48332a5e-6cf8-4c92-9255-3579431ec739"), "Customer1" }
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "genre_id", "ImagePath", "Name", "ReleaseDate" },
                values: new object[,]
                {
                    { new Guid("10b99739-3add-4de2-9535-d2e02008f942"), new Guid("30d52886-c2d4-48f5-acf8-5702023674a0"), null, "Movie 1", new DateTime(2024, 9, 27, 23, 58, 50, 378, DateTimeKind.Local).AddTicks(4521) },
                    { new Guid("1354da16-62a8-428e-9820-c001ef5868f0"), new Guid("e18fab3d-78f5-4ade-a3c8-c365e9602c62"), null, "Movie 2", new DateTime(2024, 11, 27, 23, 58, 50, 378, DateTimeKind.Local).AddTicks(4541) },
                    { new Guid("64bd5188-70b6-4a19-add3-e95fc1d04db7"), new Guid("37fb9b54-8f4f-44fe-9cad-c30f21e91b51"), null, "Movie 3", new DateTime(2024, 12, 27, 23, 58, 50, 378, DateTimeKind.Local).AddTicks(4544) }
                });

            migrationBuilder.InsertData(
                table: "CustomerMovies",
                columns: new[] { "CustomerId", "MovieId" },
                values: new object[,]
                {
                    { new Guid("02d25b7b-aa5a-4fea-b40c-fa843a788f28"), new Guid("1354da16-62a8-428e-9820-c001ef5868f0") },
                    { new Guid("1ce81473-96ad-43ad-ab4d-4b79ea54480a"), new Guid("10b99739-3add-4de2-9535-d2e02008f942") },
                    { new Guid("1ce81473-96ad-43ad-ab4d-4b79ea54480a"), new Guid("1354da16-62a8-428e-9820-c001ef5868f0") }
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

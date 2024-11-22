using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TP3.Migrations
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
                    { new Guid("92311c05-b31e-4197-8b0e-5dfca27f511a"), "Comedy" },
                    { new Guid("aa9ee57c-0ef4-405c-99a1-ca70835dc053"), "Drama" },
                    { new Guid("fda6b224-84e2-4214-94ec-ab6cb671ccf9"), "Action" }
                });

            migrationBuilder.InsertData(
                table: "Membershiptypes",
                columns: new[] { "Id", "DiscountRate", "DurationInMonth", "Name", "SignUpFee" },
                values: new object[,]
                {
                    { new Guid("1b78f2e4-a838-4f2b-83e0-cb02f49ec1a9"), 10m, 6, "Basic", 50m },
                    { new Guid("31fc5653-750b-4d7c-8dcb-740028211f90"), 20m, 12, "Premium", 100m }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "membershiptype_id", "Name" },
                values: new object[,]
                {
                    { new Guid("08e29ae7-79b1-4960-9580-86d6fc37c370"), new Guid("31fc5653-750b-4d7c-8dcb-740028211f90"), "Customer2" },
                    { new Guid("7d3b6b62-f330-4f7c-8481-47307c980e7f"), new Guid("1b78f2e4-a838-4f2b-83e0-cb02f49ec1a9"), "Customer1" }
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "genre_id", "ImagePath", "Name", "ReleaseDate" },
                values: new object[,]
                {
                    { new Guid("2d053bc3-e2d0-4a93-bc65-5d4f81dbaabb"), new Guid("fda6b224-84e2-4214-94ec-ab6cb671ccf9"), null, "Movie 1", new DateTime(2024, 8, 22, 2, 13, 47, 158, DateTimeKind.Local).AddTicks(4488) },
                    { new Guid("a8bb74ab-be59-47a5-9e7c-c75948efa1ea"), new Guid("92311c05-b31e-4197-8b0e-5dfca27f511a"), null, "Movie 2", new DateTime(2024, 10, 22, 2, 13, 47, 158, DateTimeKind.Local).AddTicks(4532) },
                    { new Guid("bd7a36d7-3262-49ea-97cd-72ef543cb37b"), new Guid("aa9ee57c-0ef4-405c-99a1-ca70835dc053"), null, "Movie 3", new DateTime(2024, 11, 22, 2, 13, 47, 158, DateTimeKind.Local).AddTicks(4536) }
                });

            migrationBuilder.InsertData(
                table: "CustomerMovies",
                columns: new[] { "CustomerId", "MovieId" },
                values: new object[,]
                {
                    { new Guid("08e29ae7-79b1-4960-9580-86d6fc37c370"), new Guid("a8bb74ab-be59-47a5-9e7c-c75948efa1ea") },
                    { new Guid("7d3b6b62-f330-4f7c-8481-47307c980e7f"), new Guid("2d053bc3-e2d0-4a93-bc65-5d4f81dbaabb") },
                    { new Guid("7d3b6b62-f330-4f7c-8481-47307c980e7f"), new Guid("a8bb74ab-be59-47a5-9e7c-c75948efa1ea") }
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

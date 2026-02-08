using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projekt_ASP.Migrations
{
    /// <inheritdoc />
    public partial class InitSqlite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ConfirmPassword = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    VehicleBrand = table.Column<string>(type: "TEXT", nullable: true),
                    VehicleModel = table.Column<string>(type: "TEXT", nullable: true),
                    VehicleYear = table.Column<int>(type: "INTEGER", nullable: true),
                    VehicleMileage = table.Column<int>(type: "INTEGER", nullable: true),
                    PropertyType = table.Column<string>(type: "TEXT", nullable: true),
                    PropertyArea = table.Column<int>(type: "INTEGER", nullable: true),
                    PropertyRooms = table.Column<int>(type: "INTEGER", nullable: true),
                    JobTitle = table.Column<string>(type: "TEXT", nullable: true),
                    JobCompany = table.Column<string>(type: "TEXT", nullable: true),
                    JobEmploymentType = table.Column<string>(type: "TEXT", nullable: true),
                    ElectronicsType = table.Column<string>(type: "TEXT", nullable: true),
                    ElectronicsBrand = table.Column<string>(type: "TEXT", nullable: true),
                    ElectronicsModel = table.Column<string>(type: "TEXT", nullable: true),
                    ServiceType = table.Column<string>(type: "TEXT", nullable: true),
                    ServiceDescription = table.Column<string>(type: "TEXT", nullable: true),
                    HomeAndGardenType = table.Column<string>(type: "TEXT", nullable: true),
                    HomeAndGardenCondition = table.Column<string>(type: "TEXT", nullable: true),
                    FashionType = table.Column<string>(type: "TEXT", nullable: true),
                    FashionSize = table.Column<string>(type: "TEXT", nullable: true),
                    FashionColor = table.Column<string>(type: "TEXT", nullable: true),
                    KidsItemType = table.Column<string>(type: "TEXT", nullable: true),
                    KidsAgeRange = table.Column<string>(type: "TEXT", nullable: true),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    Location = table.Column<string>(type: "TEXT", nullable: false),
                    PostedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ads_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImageData = table.Column<byte[]>(type: "BLOB", nullable: false),
                    AdId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdImages_Ads_AdId",
                        column: x => x.AdId,
                        principalTable: "Ads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdImages_AdId",
                table: "AdImages",
                column: "AdId");

            migrationBuilder.CreateIndex(
                name: "IX_Ads_UserId",
                table: "Ads",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdImages");

            migrationBuilder.DropTable(
                name: "Ads");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

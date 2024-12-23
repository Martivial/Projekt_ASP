using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projekt_ASP.Migrations
{
    /// <inheritdoc />
    public partial class AddUserToAd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Ads",
                newName: "UserName");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Ads",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Ads",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Ads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Ads");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Ads",
                newName: "UserId");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projekt_ASP.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToAds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Ads");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Ads",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Ads_UserId",
                table: "Ads",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ads_Users_UserId",
                table: "Ads",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ads_Users_UserId",
                table: "Ads");

            migrationBuilder.DropIndex(
                name: "IX_Ads_UserId",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Ads");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Ads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

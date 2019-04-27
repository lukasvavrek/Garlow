using Microsoft.EntityFrameworkCore.Migrations;

namespace Garlow.API.Migrations
{
    public partial class SecretKeyAndUniqueId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "Locations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecretKey",
                table: "Locations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_PublicId",
                table: "Locations",
                column: "PublicId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Locations_PublicId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "SecretKey",
                table: "Locations");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace GameInspectorWeb.Data.Migrations
{
    public partial class PhotoPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentPhotosPaths",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverPhotoPath",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentPhotosPaths",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "CoverPhotoPath",
                table: "Articles");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace TestSite.Data.Migrations
{
    public partial class Validated_Users : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Validated",
                table: "Simple_User",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Validated",
                table: "Simple_User");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTicketBookingApp.Data.Migrations
{
    public partial class moviepicimageextension : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "imageExtension",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imageExtension",
                table: "Movies");
        }
    }
}

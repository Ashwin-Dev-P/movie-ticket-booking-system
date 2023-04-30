using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTicketBookingApp.Data.Migrations
{
    public partial class userbooking2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShowId",
                table: "BookingModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BookingModel_ShowId",
                table: "BookingModel",
                column: "ShowId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingModel_Show_ShowId",
                table: "BookingModel",
                column: "ShowId",
                principalTable: "Show",
                principalColumn: "ShowId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingModel_Show_ShowId",
                table: "BookingModel");

            migrationBuilder.DropIndex(
                name: "IX_BookingModel_ShowId",
                table: "BookingModel");

            migrationBuilder.DropColumn(
                name: "ShowId",
                table: "BookingModel");
        }
    }
}

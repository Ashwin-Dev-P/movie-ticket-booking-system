using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTicketBookingApp.Data.Migrations
{
    public partial class bookings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingModel_AspNetUsers_UserId",
                table: "BookingModel");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingModel_Show_ShowId",
                table: "BookingModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingModel",
                table: "BookingModel");

            migrationBuilder.RenameTable(
                name: "BookingModel",
                newName: "Bookings");

            migrationBuilder.RenameIndex(
                name: "IX_BookingModel_UserId",
                table: "Bookings",
                newName: "IX_Bookings_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BookingModel_ShowId",
                table: "Bookings",
                newName: "IX_Bookings_ShowId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId",
                table: "Bookings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Show_ShowId",
                table: "Bookings",
                column: "ShowId",
                principalTable: "Show",
                principalColumn: "ShowId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Show_ShowId",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings");

            migrationBuilder.RenameTable(
                name: "Bookings",
                newName: "BookingModel");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_UserId",
                table: "BookingModel",
                newName: "IX_BookingModel_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_ShowId",
                table: "BookingModel",
                newName: "IX_BookingModel_ShowId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingModel",
                table: "BookingModel",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingModel_AspNetUsers_UserId",
                table: "BookingModel",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingModel_Show_ShowId",
                table: "BookingModel",
                column: "ShowId",
                principalTable: "Show",
                principalColumn: "ShowId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

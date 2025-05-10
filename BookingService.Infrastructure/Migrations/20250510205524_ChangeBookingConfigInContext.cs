using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeBookingConfigInContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookings_AspNetUsers_UserId",
                table: "bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_bookings_rooms_RoomId",
                table: "bookings");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "bookings",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_bookings_AspNetUsers_UserId",
                table: "bookings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_bookings_rooms_RoomId",
                table: "bookings",
                column: "RoomId",
                principalTable: "rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookings_AspNetUsers_UserId",
                table: "bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_bookings_rooms_RoomId",
                table: "bookings");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "bookings",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_bookings_AspNetUsers_UserId",
                table: "bookings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_bookings_rooms_RoomId",
                table: "bookings",
                column: "RoomId",
                principalTable: "rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

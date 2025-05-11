using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHotelToBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HotelId",
                table: "bookings",
                type: "integer",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_bookings_HotelId",
                table: "bookings",
                column: "HotelId");

            migrationBuilder.AddForeignKey(
                name: "FK_bookings_hotels_HotelId",
                table: "bookings",
                column: "HotelId",
                principalTable: "hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookings_hotels_HotelId",
                table: "bookings");

            migrationBuilder.DropIndex(
                name: "IX_bookings_HotelId",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "HotelId",
                table: "bookings");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2026_siperuk_backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStatusSeederDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BookingStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "Booking ditolak");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BookingStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "Booking ditolah");
        }
    }
}

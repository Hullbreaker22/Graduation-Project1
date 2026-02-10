using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyLine.Migrations
{
    /// <inheritdoc />
    public partial class NewFeatures23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Flight_Id",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_Flight_Id",
                table: "Bookings",
                column: "Flight_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Flights_Flight_Id",
                table: "Bookings",
                column: "Flight_Id",
                principalTable: "Flights",
                principalColumn: "Flight_Id_PK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Flights_Flight_Id",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_Flight_Id",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Flight_Id",
                table: "Bookings");
        }
    }
}

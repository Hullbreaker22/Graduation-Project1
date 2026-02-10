using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyLine.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "Fares");

            migrationBuilder.AddColumn<string>(
                name: "Flight_Number",
                table: "FlightSegments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "cabin",
                table: "FlightSegments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Baggage_Allowance",
                table: "Fares",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Changeable",
                table: "Fares",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Refundable",
                table: "Fares",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Seat_Selection",
                table: "Fares",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "terminal",
                table: "AirPorts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Flight_Number",
                table: "FlightSegments");

            migrationBuilder.DropColumn(
                name: "cabin",
                table: "FlightSegments");

            migrationBuilder.DropColumn(
                name: "Baggage_Allowance",
                table: "Fares");

            migrationBuilder.DropColumn(
                name: "Changeable",
                table: "Fares");

            migrationBuilder.DropColumn(
                name: "Refundable",
                table: "Fares");

            migrationBuilder.DropColumn(
                name: "Seat_Selection",
                table: "Fares");

            migrationBuilder.DropColumn(
                name: "terminal",
                table: "AirPorts");

            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "Fares",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

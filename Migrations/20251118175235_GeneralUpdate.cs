using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyLine.Migrations
{
    /// <inheritdoc />
    public partial class GeneralUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoungeAccess",
                table: "Fares");

            migrationBuilder.DropColumn(
                name: "MealsIncluded",
                table: "Fares");

            migrationBuilder.DropColumn(
                name: "Wifi",
                table: "Fares");

            migrationBuilder.AddColumn<bool>(
                name: "LoungeAccess",
                table: "FlightSegments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MealsIncluded",
                table: "FlightSegments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Wifi",
                table: "FlightSegments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoungeAccess",
                table: "FlightSegments");

            migrationBuilder.DropColumn(
                name: "MealsIncluded",
                table: "FlightSegments");

            migrationBuilder.DropColumn(
                name: "Wifi",
                table: "FlightSegments");

            migrationBuilder.AddColumn<bool>(
                name: "LoungeAccess",
                table: "Fares",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MealsIncluded",
                table: "Fares",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Wifi",
                table: "Fares",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyLine.Migrations
{
    /// <inheritdoc />
    public partial class Newmigro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Fare_ID",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fare_ID",
                table: "Bookings");
        }
    }
}

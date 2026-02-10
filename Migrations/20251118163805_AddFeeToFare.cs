using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyLine.Migrations
{
    /// <inheritdoc />
    public partial class AddFeeToFare : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "price",
                table: "Fares",
                newName: "Price");

            migrationBuilder.AddColumn<double>(
                name: "ChangeFee",
                table: "Fares",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Fees",
                table: "Fares",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

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

            migrationBuilder.AddColumn<double>(
                name: "RefundFee",
                table: "Fares",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Taxes",
                table: "Fares",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "Wifi",
                table: "Fares",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangeFee",
                table: "Fares");

            migrationBuilder.DropColumn(
                name: "Fees",
                table: "Fares");

            migrationBuilder.DropColumn(
                name: "LoungeAccess",
                table: "Fares");

            migrationBuilder.DropColumn(
                name: "MealsIncluded",
                table: "Fares");

            migrationBuilder.DropColumn(
                name: "RefundFee",
                table: "Fares");

            migrationBuilder.DropColumn(
                name: "Taxes",
                table: "Fares");

            migrationBuilder.DropColumn(
                name: "Wifi",
                table: "Fares");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Fares",
                newName: "price");
        }
    }
}

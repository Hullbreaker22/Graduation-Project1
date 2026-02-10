using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyLine.Migrations
{
    /// <inheritdoc />
    public partial class Addseatsrevison : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Seat_Reservations",
                columns: table => new
                {
                    SeatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlightId = table.Column<int>(type: "int", nullable: false),
                    SeatCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CabinClass = table.Column<int>(type: "int", nullable: false),
                    IsOccupied = table.Column<bool>(type: "bit", nullable: false),
                    OccupiedBy = table.Column<int>(type: "int", nullable: true),
                    BookingId = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seat_Reservations", x => x.SeatId);
                    table.ForeignKey(
                        name: "FK_Seat_Reservations_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Booking_Id_PK");
                    table.ForeignKey(
                        name: "FK_Seat_Reservations_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Flight_Id_PK",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Seat_Reservations_Passengers_OccupiedBy",
                        column: x => x.OccupiedBy,
                        principalTable: "Passengers",
                        principalColumn: "Passenger_Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Seat_Reservations_BookingId",
                table: "Seat_Reservations",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_Reservations_FlightId",
                table: "Seat_Reservations",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_Reservations_OccupiedBy",
                table: "Seat_Reservations",
                column: "OccupiedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Seat_Reservations");
        }
    }
}

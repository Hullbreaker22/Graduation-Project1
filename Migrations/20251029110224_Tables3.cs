using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyLine.Migrations
{
    /// <inheritdoc />
    public partial class Tables3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AirLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirLines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Booking_Id_PK = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Id_FK = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PNR = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Booking_Id_PK);
                    table.ForeignKey(
                        name: "FK_Bookings_AspNetUsers_User_Id_FK",
                        column: x => x.User_Id_FK,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeZone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoyalitySystems",
                columns: table => new
                {
                    Loyality_Id_PK = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Id_FK = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    TierLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoyalitySystems", x => x.Loyality_Id_PK);
                    table.ForeignKey(
                        name: "FK_LoyalitySystems_AspNetUsers_User_Id_FK",
                        column: x => x.User_Id_FK,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Passengers",
                columns: table => new
                {
                    Passenger_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Full_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Passport_Num = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Passenger_Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passengers", x => x.Passenger_Id);
                    table.ForeignKey(
                        name: "FK_Passengers_AspNetUsers_User_Id",
                        column: x => x.User_Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Payment_Id_PK = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Booking_Id_FK = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Payment_Ref_Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Payment_Id_PK);
                    table.ForeignKey(
                        name: "FK_Payments_Bookings_Booking_Id_FK",
                        column: x => x.Booking_Id_FK,
                        principalTable: "Bookings",
                        principalColumn: "Booking_Id_PK",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AirPorts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirPorts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AirPorts_Cities_cityId",
                        column: x => x.cityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Flight_Id_PK = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Flight_Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Transate_FlightCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Duration = table.Column<double>(type: "float", nullable: false),
                    Leaving_Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Arriving_Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Leaving_Airport_ID = table.Column<int>(type: "int", nullable: false),
                    Arrive_Airport_ID = table.Column<int>(type: "int", nullable: false),
                    AirLine_ID = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AvailableSeats = table.Column<int>(type: "int", nullable: false),
                    StopPoints = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Flight_Id_PK);
                    table.ForeignKey(
                        name: "FK_Flights_AirLines_AirLine_ID",
                        column: x => x.AirLine_ID,
                        principalTable: "AirLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Flights_AirPorts_Arrive_Airport_ID",
                        column: x => x.Arrive_Airport_ID,
                        principalTable: "AirPorts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Flights_AirPorts_Leaving_Airport_ID",
                        column: x => x.Leaving_Airport_ID,
                        principalTable: "AirPorts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Fares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    flightId = table.Column<int>(type: "int", nullable: false),
                    CabinClass = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false),
                    MyProperty = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fares_Flights_flightId",
                        column: x => x.flightId,
                        principalTable: "Flights",
                        principalColumn: "Flight_Id_PK",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlightSegments",
                columns: table => new
                {
                    Segment_ID_Pk = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Flight_ID_Fk = table.Column<int>(type: "int", nullable: false),
                    Segment_Order = table.Column<int>(type: "int", nullable: false),
                    Departure_Airport_ID_Fk = table.Column<int>(type: "int", nullable: false),
                    Arrival_Airport_ID_Fk = table.Column<int>(type: "int", nullable: false),
                    Departure_Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Arrival_Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Distance_KM = table.Column<double>(type: "float", nullable: false),
                    Duration_Min = table.Column<double>(type: "float", nullable: false),
                    Aircraft_Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Layover_Duration_Min = table.Column<double>(type: "float", nullable: false),
                    Distance_From_Origin = table.Column<double>(type: "float", nullable: false),
                    Distance_To_Destination = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightSegments", x => x.Segment_ID_Pk);
                    table.ForeignKey(
                        name: "FK_FlightSegments_AirPorts_Arrival_Airport_ID_Fk",
                        column: x => x.Arrival_Airport_ID_Fk,
                        principalTable: "AirPorts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FlightSegments_AirPorts_Departure_Airport_ID_Fk",
                        column: x => x.Departure_Airport_ID_Fk,
                        principalTable: "AirPorts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FlightSegments_Flights_Flight_ID_Fk",
                        column: x => x.Flight_ID_Fk,
                        principalTable: "Flights",
                        principalColumn: "Flight_Id_PK",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingPassengers",
                columns: table => new
                {
                    Id_PK = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Booking_Id_FK = table.Column<int>(type: "int", nullable: false),
                    Passenger_Id_FK = table.Column<int>(type: "int", nullable: false),
                    Fare_Id = table.Column<int>(type: "int", nullable: false),
                    Seat_Num = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingPassengers", x => x.Id_PK);
                    table.ForeignKey(
                        name: "FK_BookingPassengers_Bookings_Booking_Id_FK",
                        column: x => x.Booking_Id_FK,
                        principalTable: "Bookings",
                        principalColumn: "Booking_Id_PK",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingPassengers_Fares_Fare_Id",
                        column: x => x.Fare_Id,
                        principalTable: "Fares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingPassengers_Passengers_Passenger_Id_FK",
                        column: x => x.Passenger_Id_FK,
                        principalTable: "Passengers",
                        principalColumn: "Passenger_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AirPorts_cityId",
                table: "AirPorts",
                column: "cityId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingPassengers_Booking_Id_FK",
                table: "BookingPassengers",
                column: "Booking_Id_FK");

            migrationBuilder.CreateIndex(
                name: "IX_BookingPassengers_Fare_Id",
                table: "BookingPassengers",
                column: "Fare_Id");

            migrationBuilder.CreateIndex(
                name: "IX_BookingPassengers_Passenger_Id_FK",
                table: "BookingPassengers",
                column: "Passenger_Id_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_User_Id_FK",
                table: "Bookings",
                column: "User_Id_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Fares_flightId",
                table: "Fares",
                column: "flightId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_AirLine_ID",
                table: "Flights",
                column: "AirLine_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_Arrive_Airport_ID",
                table: "Flights",
                column: "Arrive_Airport_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_Leaving_Airport_ID",
                table: "Flights",
                column: "Leaving_Airport_ID");

            migrationBuilder.CreateIndex(
                name: "IX_FlightSegments_Arrival_Airport_ID_Fk",
                table: "FlightSegments",
                column: "Arrival_Airport_ID_Fk");

            migrationBuilder.CreateIndex(
                name: "IX_FlightSegments_Departure_Airport_ID_Fk",
                table: "FlightSegments",
                column: "Departure_Airport_ID_Fk");

            migrationBuilder.CreateIndex(
                name: "IX_FlightSegments_Flight_ID_Fk",
                table: "FlightSegments",
                column: "Flight_ID_Fk");

            migrationBuilder.CreateIndex(
                name: "IX_LoyalitySystems_User_Id_FK",
                table: "LoyalitySystems",
                column: "User_Id_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Passengers_User_Id",
                table: "Passengers",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Booking_Id_FK",
                table: "Payments",
                column: "Booking_Id_FK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingPassengers");

            migrationBuilder.DropTable(
                name: "FlightSegments");

            migrationBuilder.DropTable(
                name: "LoyalitySystems");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Fares");

            migrationBuilder.DropTable(
                name: "Passengers");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "AirLines");

            migrationBuilder.DropTable(
                name: "AirPorts");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}

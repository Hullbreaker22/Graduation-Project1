using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NPoco.Expressions;
using SkyLine.Models;
using SkyLine.Repositories;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Umbraco.Core.Models.Entities;

namespace SkyLine.Areas.Customer.Controllers;

[Area(SD.CustomerArea)]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepository<Flight> _flight;
    private readonly IRepository<Booking> _Booking;
    private readonly IRepository<City> _City;
    private readonly IRepository<FlightSegment> _Segment;
    public readonly IRepository<Fare> _Fair;
    public readonly IRepository<Seat_Reservation> _Seat_Reservation;
    public readonly IRepository<Passenger> _Passenger;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ILogger<HomeController> logger, IRepository<Flight> flight1, IRepository<FlightSegment> segment, IRepository<City> city, IRepository<Fare> fare, IRepository<Seat_Reservation> seat_Reservation, IRepository<Passenger> passenger, UserManager<ApplicationUser> userManager, IRepository<Booking> booking)
    {
        _logger = logger;
        _flight = flight1;
        _Segment = segment;
        _City = city;
        _Fair = fare;
        _Seat_Reservation = seat_Reservation;
        _Passenger = passenger;
        _userManager = userManager;
        _Booking = booking;
    }

    public async Task<IActionResult> Index()
    {
        var cities = await _City.GetAsync();

        return View(cities);
    }

    public async Task<IActionResult> Results(Filters flightFilter, int page = 1)
    {

        var flights = (await _flight.GetAsync(includes: [e => e.AirLine!, e => e.LeavingAirport!, e => e.ArriveAirport!])).AsQueryable();

        if (flightFilter.LeavingCity != null)
        {
            flights = flights.Where(e => e.LeavingAirport!.cityId == flightFilter.LeavingCity);
        }

        if (flightFilter.ArrivingCity is not null)
        {
            flights = flights.Where(e => e.ArriveAirport!.cityId == flightFilter.ArrivingCity);
        }

        if (flightFilter.dates.HasValue)
        {
            var startDate = flightFilter.dates.Value.Date;
            var endDate = startDate.AddDays(1);

            flights = flights.Where(e =>
                e.Leaving_Time >= startDate &&
                e.Leaving_Time < endDate);
        }
        var cities = await _City.GetAsync();

        ViewBag.Citiess = cities;

        double totalPages = Math.Ceiling(flights.Count() / 6.0); 
        int currentPage = page;

        ViewBag.TotalPages = totalPages;
        ViewBag.CurrentPage = currentPage;

        flights = flights.Skip((page - 1) * 6).Take(6);



        
        return View(flights.ToList());
    }


    public async Task<IActionResult> Details([FromRoute]int id)
    {

       
        var flights22 = await _flight.GetOneAsync(includes: [e => e.AirLine!, e => e.LeavingAirport!.city, e => e.ArriveAirport!.city, e=>e.Fares!],expression: e=>e.Flight_Id_PK == id);
        var Segment = await _Segment.GetAsync(expression: e => e.Flight_ID_Fk == id, includes: [e=>e.DepartureAirport!.city, e=>e.ArrivalAirport!.city]);

        InitialClass Initial = new InitialClass()
        {
            flights = flights22!,
            flightSegment = Segment

        };
        return View(Initial);
    }

    [HttpGet]
    public async Task<IActionResult> Booking( int fareId)
    {

        var theFare = await _Fair.GetOneAsync(expression: e => e.Id == fareId);

        var theFlight  = await _flight.GetOneAsync(expression: e => e.Flight_Id_PK == theFare!.flightId, includes: [e=>e.Fares!, e=>e.ArriveAirport!.city, e=>e.LeavingAirport!.city, e=>e.AirLine!]);

        FareBooking Newbooking = new FareBooking()
        {
            flight = theFlight,
            fare = theFare
        };
        return View(Newbooking);
    }

    [Authorize]
    public async Task<IActionResult> Checkout(int flightId, int fareId)
    {

        var occupiedSeats = await _Seat_Reservation.GetAsync(
        expression: e => e.FlightId == flightId && e.IsOccupied
        );


        ViewBag.theFair = fareId;
       

        var Nflight = await _flight.GetOneAsync(expression: e => e.Flight_Id_PK == flightId);

        ViewBag.Flight = Nflight;
        ViewBag.OccupiedSeats = occupiedSeats.Select(s => s.SeatCode).ToList();

        return View();
    }

    public static string GeneratePNR()
    {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        var pnr = new char[6];

        for (int i = 0; i < 6; i++)
        {
            pnr[i] = chars[random.Next(chars.Length)];
        }

        return new string(pnr);
    }

    [HttpPost]
    public async Task<ActionResult> SubmitBooking(BookingRequest Request)
    {



        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return Unauthorized();
        }

        var userId = user.Id;

        string pnr;
        bool exists;

        do
        {
            pnr = GeneratePNR();
            var existing = await _Booking.GetAsync(b => b.PNR == pnr);
            exists = existing.Any();
        } while (exists);


        var booking = new Booking
        {
            PNR = pnr,
            User_Id_FK = userId,
            status = Status.Pending,
            TotalPrice = Request.TotalPrice,
            CreatedAt = DateTime.Now,
            Flight_Id = Request.Flight_Id_PK,
            Fare_ID = Request.Fare_ID
           
        };


        await _Booking.CreateAsync(booking);
        await _Booking.CommitAsync();


        foreach (var Passang in Request.Passengers)
        {
            Passang.User_Id = userId;
        }

        foreach (var item in Request.Passengers)
        {
            await _Passenger.CreateAsync(item);
            await _Passenger.CommitAsync();
        }

        if (!string.IsNullOrEmpty(Request.SelectedSeats))
        {
            var seats = Request.SelectedSeats.Split(',');

            foreach (var seatCode in seats)
            {
                var seat = await _Seat_Reservation.GetOneAsync(e => e.SeatCode == seatCode);
                var Owner = await _Passenger.GetOneAsync(expression: e => e.SeatCode == seatCode);

                if (seat != null)
                {
                    seat.IsOccupied = true;
                    seat.OccupiedBy = Owner!.Passenger_Id;
                }
            }
            await _Seat_Reservation.CommitAsync();
        }

        TempData["success-notification"] = "Added to Flights Successfully";


        return RedirectToAction(
            actionName: "Index",
            controllerName: "Home",
            new { area = "Customer" }
        );  

    }
    public IActionResult NotFoundPage()
    {



        return View();
    }



}

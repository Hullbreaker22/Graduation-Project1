using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyLine.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SkyLine.Areas.Admin.Controllers
{
    [Area(SD.AdminArea)]
    [Authorize(Roles = "AdminRole,SuperAdminRole")]

    public class HomeController : Controller
    {
        private readonly IRepository<Flight> _flight;

        public HomeController(IRepository<Flight> flight)
        {
            _flight = flight;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var allFlights = (await _flight.GetAsync(
                includes: new System.Linq.Expressions.Expression<Func<Flight, object>>[]
                {
                    f => f.AirLine!,
                    f => f.LeavingAirport!,
                    f => f.ArriveAirport!
                })).ToList();

      
            ViewBag.TotalFlights = allFlights.Count;

            ViewBag.LowSeatsFlights = allFlights.Count(f => f.AvailableSeats < 100);

            ViewBag.FullFlights = allFlights.Count(f => f.AvailableSeats == 0);

            var now = DateTime.Now;
            ViewBag.UpcomingFlights = allFlights.Count(f => f.Leaving_Time > now && f.Leaving_Time <= now.AddDays(30));

    
            double totalPages = Math.Ceiling(allFlights.Count / 6.0);
            int currentPage = page;

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = currentPage;

            var flightsPage = allFlights.Skip((page - 1) * 6).Take(6).ToList();
            ViewBag.Flights = flightsPage;

         
            var flightsByAirline = allFlights
                .GroupBy(f => f.AirLine!.Name)
                .Select(g => new
                {
                    Airline = g.Key,
                    Count = g.Count()
                })
                .ToList();

            ViewBag.ChartLabelsAirline = flightsByAirline.Select(x => x.Airline).ToList();
            ViewBag.ChartDataAirline = flightsByAirline.Select(x => x.Count).ToList();

            var flightsByDay = allFlights
                .GroupBy(f => f.Leaving_Time.Date)
                .Select(g => new
                {
                    Day = g.Key.ToString("yyyy-MM-dd"),
                    Count = g.Count()
                })
                .OrderBy(g => g.Day)
                .ToList();

            ViewBag.ChartLabelsDays = flightsByDay.Select(x => x.Day).ToList();
            ViewBag.ChartDataDays = flightsByDay.Select(x => x.Count).ToList();

            return View();
        }
    }
}

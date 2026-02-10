using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyLine.Models;
using System.Threading.Tasks;

namespace SkyLine.Areas.Admin.Controllers
{

    [Area(SD.AdminArea)]
    [Authorize(Roles = "AdminRole,SuperAdminRole")]

    public class SegmentController : Controller
    {

        private readonly IRepository<FlightSegment> _Segment;
        private readonly IRepository<Flight> _flight;
        private readonly IRepository<AirPort> _Airport;

        public SegmentController(IRepository<FlightSegment> segment, IRepository<Flight> flight, IRepository<AirPort> airport)
        {
            _Segment = segment;
            _flight = flight;
            _Airport = airport;
        }
        public async Task<IActionResult> Segments()
        {
            var Segments = await _Segment.GetAsync(includes: [e=>e.ArrivalAirport! ,e=>e.DepartureAirport!, e=>e.Flight]);


            return View(Segments);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateSegment([FromRoute] int id)
        {
            var segment =  await _Segment.GetOneAsync(expression: e => e.Segment_ID_Pk == id);
            var flights = await _flight.GetAsync(includes: [e => e.AirLine!, e => e.LeavingAirport!, e => e.ArriveAirport!]);
            var airports = await _Airport.GetAsync();
            ViewBag.Airports = airports;

            ViewBag.Flights = flights;

            return View(segment);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSegment(FlightSegment flightSeg)
        {
            if (!ModelState.IsValid)
            {
                var flights = await _flight.GetAsync(includes: [e => e.AirLine!, e => e.LeavingAirport!, e => e.ArriveAirport!]);
                var airports = await _Airport.GetAsync();
                ViewBag.Airports = airports.ToList();

                ViewBag.Flights = flights.ToList();


                return View(flightSeg);
            }

            _Segment.Update(flightSeg);
            await _Segment.CommitAsync();
            TempData["success-notification"] = "Updated Successfully";


            return RedirectToAction("Segments");

        }


        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var Fare = await _Segment.GetOneAsync(e => e.Segment_ID_Pk == id);

            if (Fare is null)
                return BadRequest();

            _Segment.Delete(Fare);
            await _Segment.CommitAsync();

            TempData["success-notification"] = "Deleted Successfully";

            return RedirectToAction("Segments");

        }


        [HttpGet]
        public async Task<IActionResult> CreateSegment()
        {

            var flights = await _flight.GetAsync(includes: [e => e.AirLine!, e => e.LeavingAirport!, e => e.ArriveAirport!]);
            var airports = await _Airport.GetAsync();
            ViewBag.Airports = airports.ToList();

            ViewBag.Flights = flights.ToList();

            return View(new FlightSegment());
        }

        [HttpPost]
        public async Task<IActionResult> CreateSegment(CreateSegmentVM CreateVM)
        {


            if (!ModelState.IsValid)
            {
                var flights = await _flight.GetAsync(includes: [e => e.AirLine!, e => e.LeavingAirport!, e => e.ArriveAirport!]);
                var airports = await _Airport.GetAsync();
                ViewBag.Airports = airports.ToList();

                ViewBag.Flights = flights.ToList();


                return View(CreateVM);
            }

            var NewSegment = CreateVM.Adapt<FlightSegment>();
            await _Segment.CreateAsync(NewSegment);
            await _Segment.CommitAsync();
            TempData["success-notification"] = "Created Successfully";


            return RedirectToAction("Segments");
        }

    }
}

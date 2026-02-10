using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyLine.Models;
using System.Threading.Tasks;

namespace SkyLine.Areas.Admin.Controllers
{
    [Area(SD.AdminArea)]
    [Authorize(Roles = "AdminRole,SuperAdminRole")]


    public class FlightController : Controller
    {
        
        private readonly IRepository<Flight> _flight;
        private readonly IRepository<AirPort> _AirPort;
        private readonly IRepository<Airline> _AirLine;


        public FlightController( IRepository<Flight> flight1, IRepository<AirPort> airport1, IRepository<Airline> AirLine1)
        {
            
            _flight = flight1;
            _AirPort = airport1;
            _AirLine = AirLine1;

        }



        public async Task<IActionResult> Flights()
        {

            var flights = await _flight.GetAsync(includes: [e => e.AirLine!, e => e.LeavingAirport!, e => e.ArriveAirport!]);
            return View(flights);
        }

        [HttpGet]
        public async Task<IActionResult> FlightEdit([FromRoute] int id)
        {

            var flights = await _flight.GetOneAsync(expression: e => e.Flight_Id_PK == id);
            var airports = await _AirPort.GetAsync();
            var airLines = await _AirLine.GetAsync();

            MixedClass mixedclass = new MixedClass()
            {
                airline = airLines,
                airport = airports,
                flight = flights,

            };
            return View(mixedclass);
        }

        [HttpPost]
        public async Task<IActionResult> FlightEdit(EditFlightVM EditVM)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(e => e.Errors);
                TempData["error-notification"] = String.Join(", ", errors.Select(e => e.ErrorMessage));

                return View(EditVM);
            }

            var flights = await _flight.GetOneAsync(expression: e => e.Flight_Id_PK == EditVM.Flight_Id_PK, tracked: false);

            flights = EditVM.Adapt<Flight>();

            _flight.Update(flights);
            await _flight.CommitAsync();

            TempData["success-notification"] = "Updated Successfully";


            return RedirectToAction("Flights");
        }


        [HttpGet]
        public async Task<IActionResult> CreateFlight()
        {
            var airports = await _AirPort.GetAsync();
            var airLines = await _AirLine.GetAsync();

            MixedClass createVM = new MixedClass()
            {

                airline = airLines,
                airport = airports,
                flight = new Flight()

            };

            return View(createVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFlight(EditFlightVM EditVM)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(e => e.Errors);
                TempData["error-notification"] = String.Join(", ", errors.Select(e => e.ErrorMessage));

                return View(EditVM);
            }

            var flight = EditVM.Adapt<Flight>();
            await _flight.CreateAsync(flight);
            await _flight.CommitAsync();

            TempData["success-notification"] = "Created Successfully";

            return RedirectToAction("Flights");
        }

        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var flight = await _flight.GetOneAsync(e => e.Flight_Id_PK == id);

            if (flight is null)
                return BadRequest();

            _flight.Delete(flight);
            await _flight.CommitAsync();

            TempData["success-notification"] = "Deleted  Successfully";

            return RedirectToAction("Flights");
           
        }
    }
}

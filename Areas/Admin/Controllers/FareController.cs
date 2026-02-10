using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyLine.Models;
using System.Threading.Tasks;

namespace SkyLine.Areas.Admin.Controllers
{
    [Area(SD.AdminArea)]
    [Authorize(Roles = "AdminRole,SuperAdminRole")]


    public class FareController : Controller
    {

        public readonly IRepository<Fare> _Fair;
        public readonly IRepository<Flight> _flight;
        public FareController(IRepository<Fare> fair, IRepository<Flight> flight)
        {
            _Fair = fair;
            _flight = flight;
        }

        public async Task<IActionResult> AllFair()
        {
            var fares = await _Fair.GetAsync(includes: [e=>e.flight!]);

            return View(fares);
        }

        [HttpGet]
        public async Task<IActionResult> FareEdit([FromRoute]int id)
        {
            var fares = await _Fair.GetOneAsync(expression: e=>e.Id == id);
            var flights = await _flight.GetAsync(includes: [e => e.AirLine!, e => e.LeavingAirport!, e => e.ArriveAirport!]);


            ViewBag.Flights = flights;

            return View(fares);
        }

        [HttpPost]
        public async Task<IActionResult> FareEdit(Fare fair)
        {
            if(!ModelState.IsValid)
            {


                return View(fair);
            }

            _Fair.Update(fair);
            await _Fair.CommitAsync();
            TempData["success-notification"] = "Updated Successfully";


            return RedirectToAction("AllFair");

        }

        [HttpGet]
        public async Task<IActionResult> CreateFare()
        {
            var flights = await _flight.GetAsync(includes: [e => e.AirLine!, e => e.LeavingAirport!, e => e.ArriveAirport!]);

            ViewBag.Flights = flights;

            return View(new Fare());
        }




        [HttpPost]
        public async Task<IActionResult> CreateFare(CreateFareVM CreateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(CreateVM);
            }

            var NewFare = CreateVM.Adapt<Fare>();
           await _Fair.CreateAsync(NewFare);
            await _Fair.CommitAsync();
            TempData["success-notification"] = "Created Successfully";


            return RedirectToAction("AllFair");
        }

        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var Fare = await _Fair.GetOneAsync(e => e.Id == id);

            if (Fare is null)
                return BadRequest();

            _Fair.Delete(Fare);
            await _Fair.CommitAsync();

            TempData["success-notification"] = "Deleted  Successfully";

            return RedirectToAction("AllFair");

        }

    }
}

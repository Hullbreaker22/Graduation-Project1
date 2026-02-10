using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyLine.Models;
using System.Threading.Tasks;

namespace SkyLine.Areas.Admin.Controllers
{
    [Area(SD.AdminArea)]
    [Authorize(Roles = "AdminRole,SuperAdminRole")]
    public class AirportController : Controller
    {

        private readonly IRepository<AirPort> _Airport;
        public readonly IRepository<City> _City;

        public AirportController(IRepository<AirPort> airport, IRepository<City> city)
        {
            _Airport = airport;
            _City = city;
        }
        public async Task<IActionResult> Airports()
        {

            var airports = await _Airport.GetAsync(includes: [e=>e.city!]);

            return View(airports);
        }



        [HttpGet]
        public async Task<IActionResult> AirportUpdate([FromRoute] int id)
        {
            var Air = await _Airport.GetOneAsync(expression: e => e.Id == id , includes: [e=>e.city]);
            var cities = await _City.GetAsync();
            ViewBag.Cities = cities;

            return View(Air);
        }

        [HttpPost]
        public async Task<IActionResult> AirportUpdate(CreateAirportVM CreateAirVM)
        {

            var Airport22 = CreateAirVM.Adapt<AirPort>();

            if (!ModelState.IsValid)
            {
                var cities = await _City.GetAsync();
                ViewBag.Cities = cities;
                return View(Airport22);
            }

            _Airport.Update(Airport22);
            await _Airport.CommitAsync();

            TempData["success-notification"] = "Updated Successfully";

            return RedirectToAction("Airports");
        }

        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var airpo = await _Airport.GetOneAsync(expression: e => e.Id == id);

            _Airport.Delete(airpo!);
            await _Airport.CommitAsync();

            TempData["success-notification"] = "Deleted Successfully";

            return RedirectToAction("Airports");
        }


        [HttpGet]
        public async Task<IActionResult> CreateAirport()
        {
            var cities = await _City.GetAsync();
            ViewBag.Cities = cities;

            return View(new CreateAirportVM());

        }


        [HttpPost]
        public async Task<IActionResult> CreateAirport(CreateAirportVM CreateAirVM)
        {


            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(e => e.Errors);
                TempData["error-notification"] = String.Join(", ", errors.Select(e => e.ErrorMessage));

                var cities = await _City.GetAsync();
                ViewBag.Cities = cities;

                return View(CreateAirVM);
            }

            var newAirport = CreateAirVM.Adapt<AirPort>();


            await _Airport.CreateAsync(newAirport);
            await _Airport.CommitAsync();


            TempData["success-notification"] = "Created Successfully";

            return RedirectToAction("Airports");

        }





    }
}

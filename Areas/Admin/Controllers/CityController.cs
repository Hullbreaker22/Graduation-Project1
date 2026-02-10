using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SkyLine.Areas.Admin.Controllers
{
    [Area(SD.AdminArea)]
    [Authorize(Roles = "AdminRole,SuperAdminRole")]

    public class CityController : Controller
    {
        public readonly IRepository<City> _City;


        public CityController(IRepository<City> city1)
        {
            _City = city1;
        }


        public async Task<IActionResult> Cites()
        {
            var cities = await _City.GetAsync();
            return View(cities);
        }

        [HttpGet]
        public async Task<IActionResult> CityUpdate([FromRoute] int id)
        {
            var cities = await _City.GetOneAsync(expression: e => e.Id == id);
            return View(cities);
        }

        [HttpPost]
        public async Task<IActionResult> CityUpdate(City city)
        {
            if (!ModelState.IsValid)
            {
                return View(city);
            }

            _City.Update(city);
            await _City.CommitAsync();

            TempData["success-notification"] = "Updated Successfully";

            return RedirectToAction("Cites");
        }


        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var city = await _City.GetOneAsync(expression: e => e.Id == id);

            _City.Delete(city!);
            await _City.CommitAsync();

            TempData["success-notification"] = "Deleted Successfully";

            return RedirectToAction("Cites");
        }

        [HttpGet]
        public IActionResult CreateCity()
        {

            return View(new City());

        }


        [HttpPost]
        public async Task<IActionResult> CreateCity(City city)
        {
            if(!ModelState.IsValid)
            {
                return View(city);
            }

            await _City.CreateAsync(city);
            await _City.CommitAsync();


            TempData["success-notification"] = "Created Successfully";

            return RedirectToAction("Cites");

        }
    }
}

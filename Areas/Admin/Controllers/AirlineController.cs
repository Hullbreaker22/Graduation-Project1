using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SkyLine.Models;
using System.Threading.Tasks;

namespace SkyLine.Areas.Admin.Controllers
{
    [Area(SD.AdminArea)]
    [Authorize(Roles = "AdminRole,SuperAdminRole")]
    public class AirlineController : Controller
    {

        private readonly IRepository<Airline> _airline;

        public AirlineController(IRepository<Airline> repo)
        {
            _airline = repo;
        }
        public async Task<IActionResult> AirLines()
        {
            var airlines = await _airline.GetAsync(); 
            return View(airlines);
        }

        [HttpGet]
        public async Task<IActionResult> EditAirline([FromRoute] int id)
        {
            var air = await _airline.GetOneAsync(expression: e=>e.Id == id);

            return View(air);
        }




        [HttpPost]
        public async Task<IActionResult> EditAirline(Airline newair, IFormFile? logo)
        {
            var LogoInDb = await _airline.GetOneAsync(e => e.Id == newair.Id, tracked: false);

            if (LogoInDb is null)
                return BadRequest();

            if (newair.logo is not null && newair.logo.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(logo.FileName);

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Logos", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    logo.CopyTo(stream);
                }

                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Logos", LogoInDb.logo);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }

                var newAir = newair.Adapt<Airline>();
                newAir.logo = fileName;
            }
            else
            {
                newair.logo = LogoInDb.logo;
            }

            _airline.Update(newair);
            await _airline.CommitAsync();

            TempData["success-notification"] = "Updated Successfully";


            return RedirectToAction("AirLines");
        }

        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var flight = await _airline.GetOneAsync(e => e.Id == id);

            if (flight is null)
                return BadRequest();

            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Logos", flight.logo!);
            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }

            _airline.Delete(flight);
            await _airline.CommitAsync();

            TempData["success-notification"] = "Deleted Successfully";

            return RedirectToAction("AirLines");

        }


        [HttpGet]
        public async Task<IActionResult> CreateAirline()
        {


            return View(new Airline());
        }


        [HttpPost]
        public async Task<IActionResult> CreateAirline(EditAirlineVM EditVM, IFormFile? logo)
        {

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(e => e.Errors);
                TempData["error-notification"] = String.Join(", ", errors.Select(e => e.ErrorMessage));

                return View(EditVM);
            }
          
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(logo.FileName);

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Logos", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    logo.CopyTo(stream);
                }

             EditVM.logo = fileName;

              var newAirline = EditVM.Adapt<Airline>();
              
            
          

            _airline.Update(newAirline);
            await _airline.CommitAsync();
            TempData["success-notification"] = "Created Successfully";


            return RedirectToAction("AirLines");
        }
    }



}

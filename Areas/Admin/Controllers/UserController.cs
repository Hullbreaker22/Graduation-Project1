using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SkyLine.Models;
using System.Threading.Tasks;
using Umbraco.Core.Models.Membership;
using static System.Net.Mime.MediaTypeNames;

namespace SkyLine.Areas.Admin.Controllers
{

    [Area(SD.AdminArea)]
    [Authorize(Roles = "AdminRole,SuperAdminRole")]

    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRepository<UserOTP> _userOTP;
        private readonly IRepository<ApplicationUser> _ApplicatiRepo;

        public UserController(UserManager<ApplicationUser> userManager, IEmailSender emailSender, SignInManager<ApplicationUser> signInManager, IRepository<UserOTP> userOTP, IRepository<ApplicationUser> _ApplicatiRepo22)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _userOTP = userOTP;
            _ApplicatiRepo = _ApplicatiRepo22;
        }



        public async Task<IActionResult> AllUsers()
        {
            var users = _userManager.Users;


            return View(users.ToList());
        }


        [HttpGet]
        public IActionResult CreateUser([FromRoute] string id)
        {

            return View(new CreateUserVM22());
        }

        [HttpPost]

        public async Task<IActionResult> CreateUser(CreateUserVM22 CreateVM, IFormFile? Image)
        {
            
            if (!ModelState.IsValid)
            {
                return View(CreateVM);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\UserImg", fileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                Image.CopyTo(stream);
            }

            CreateVM.Image = fileName;


            ApplicationUser applicationUser = CreateVM.Adapt<ApplicationUser>();

            var result = await _userManager.CreateAsync(applicationUser, CreateVM.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }

                return View(CreateVM);
            }

            await _userManager.AddToRoleAsync(applicationUser, CreateVM.Role);


            var token = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
            var link = Url.Action("ConfirmEmail", "User", new { area = "Admin", token = token, userId = applicationUser.Id }, Request.Scheme);

            await _emailSender.SendEmailAsync(applicationUser.Email!, $"Confirm Your Email!", $"<h1>Confirm Your Email to Our Website ==> <a href='{link}'>Here</a></h1>");

            TempData["success-notification"] = "Create User successfully, Confirm Your Email!";
            return RedirectToAction("AllUsers", "User", new { area = "Admin" });

        }

        public async Task<IActionResult> ConfirmEmail(string token, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return NotFound();

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
                TempData["error-notification"] = "Link Expired!, Resend Email Confirmation";
            else
                TempData["success-notification"] = "Confirm Email successfully";

            return RedirectToAction("AllUsers", "User", new { area = "Admin" });
        }

        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            var DelUser = await _userManager.FindByIdAsync(id);

            if (DelUser is null)
                return BadRequest();

            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\UserImg", DelUser.Image!);
            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }

           await  _userManager.DeleteAsync(DelUser);
           

            TempData["success-notification"] = "Deleted Successfully";

            return RedirectToAction("AllUsers");

        }

        public async Task<IActionResult> LockUnLock([FromRoute] string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return NotFound();

            user.LockoutEnabled = !user.LockoutEnabled;

            if (!user.LockoutEnabled)
            {
                user.LockoutEnd = DateTime.UtcNow.AddDays(2);
            }
            else
            {
                user.LockoutEnd = null;
            }

            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(AllUsers));
        }


        [HttpGet]
        public async Task<IActionResult> EditUser([FromRoute] string id)
        {
            var EUser = await _userManager.FindByIdAsync(id);

          var updateduser =  EUser.Adapt<CreateUser>();

            return View(updateduser);
        }




        [HttpPost]
        public async Task<IActionResult> EditUser(CreateUser CreateV, IFormFile? UserImage)
        {
            var userInDb = await _userManager.FindByIdAsync(CreateV.Id!);

            if (userInDb == null)
                return BadRequest();

            userInDb.Name = CreateV.Name;
            userInDb.UserName = CreateV.UserName;
            userInDb.Email = CreateV.Email;
            userInDb.PhoneNumber = CreateV.PhoneNumber;
            userInDb.City = CreateV.City;
            userInDb.State = CreateV.State;
            userInDb.Street = CreateV.Street;
            userInDb.ZipCode = CreateV.ZipCode;

            if(CreateV.Role is  not null)
            {
                var oldRoles = await _userManager.GetRolesAsync(userInDb);

                if (oldRoles.Any())
                {
                    await _userManager.RemoveFromRolesAsync(userInDb, oldRoles);
                }

                await _userManager.AddToRoleAsync(userInDb, CreateV.Role);
            }

            if (UserImage != null && UserImage.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(UserImage.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserImg", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await UserImage.CopyToAsync(stream);
                }

                if (!string.IsNullOrEmpty(userInDb.Image))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserImg", userInDb.Image);
                    if (System.IO.File.Exists(oldFilePath))
                        System.IO.File.Delete(oldFilePath);
                }

                userInDb.Image = fileName;
            }

            var result = await _userManager.UpdateAsync(userInDb);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(CreateV);
            }

            TempData["success-notification"] = "Updated Successfully";
            return RedirectToAction("AllUsers");
        }


    }
}

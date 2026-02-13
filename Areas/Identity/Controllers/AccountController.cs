using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SkyLine.Models;
using SkyLine.ViewModels;
using System.Threading.Tasks;

namespace SkyLine.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRepository<UserOTP> _userOTP;

        public AccountController(UserManager<ApplicationUser> userManager, IEmailSender emailSender, SignInManager<ApplicationUser> signInManager, IRepository<UserOTP> userOTP)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _userOTP = userOTP;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegister userRegister)
        {
            if (!ModelState.IsValid)
            {
                return View(userRegister);
            }



            ApplicationUser applicationUser = userRegister.Adapt<ApplicationUser>();

            var result = await _userManager.CreateAsync(applicationUser, userRegister.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }

                return View(userRegister);
            }

            await _userManager.AddToRoleAsync(applicationUser, SD.CustomerRole);


            var token = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
            var link = Url.Action("ConfirmEmail", "Account", new { area = "Identity", token = token, userId = applicationUser.Id }, Request.Scheme);

            await _emailSender.SendEmailAsync(applicationUser.Email!, $"Confirm Your Email!", $"<h1>Confirm Your Email to Our Website ==> <a href='{link}'>Here</a></h1>");

            TempData["success-notification"] = "Create User successfully, Confirm Your Email!";

            return RedirectToAction("Index", "Home", new { area = "Customer" });
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

            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            if (!ModelState.IsValid)
            {
                return View(userLogin);
            }

            var user = await _userManager.FindByEmailAsync(userLogin.EmailOrUserName) ?? await _userManager.FindByNameAsync(userLogin.EmailOrUserName);

            if (user is null)
            {
                TempData["error-notification"] = "Invalid User name Or password";
                return View(userLogin);
            }

            //_userManager.CheckPasswordAsync();
            var result = await _signInManager.PasswordSignInAsync(user, userLogin.Password, userLogin.RememberMe, true);

            if (!result.Succeeded)
            {
                TempData["error-notification"] = "Invalid User name Or password";
                return View(userLogin);
            }

            if (!user.EmailConfirmed)
            {
                TempData["error-notification"] = "Confirm Your Email First!";
                return View(userLogin);
            }

            if (!user.LockoutEnabled)
            {
                TempData["error-notification"] = $"You have a block till {user.LockoutEnd}";
                return View(userLogin);
            }

            TempData["success-notification"] = "Login successfully";
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        [HttpGet]
        public IActionResult ResendConfirmation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResendConfirmation(ResentConfirmEmail resentConfirmEmail)
        {
            if (!ModelState.IsValid)
            {
                return View(resentConfirmEmail);
            }

            var user = await _userManager.FindByEmailAsync(resentConfirmEmail.EmailOrUserName) ?? await _userManager.FindByNameAsync(resentConfirmEmail.EmailOrUserName);

            if (user is null)
            {
                TempData["error-notification"] = "Invalid User name Or Email";
                return View(resentConfirmEmail);
            }

            if (user.EmailConfirmed)
            {
                TempData["error-notification"] = "Already Confirmed!";
                return View(resentConfirmEmail);
            }

            // Send confirmation msg
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action("ConfirmEmail", "Account", new { area = "Identity", token = token, userId = user.Id }, Request.Scheme);

            await _emailSender.SendEmailAsync(user.Email!, $"Confirm Your Email!", $"<h1>Confirm Your account on Our website <a href='{link}'>Here</a></h1>");

            TempData["success-notification"] = "Send Email successfully, Confirm Your Email!";
            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPassword forgetPassword)
        {
            if (!ModelState.IsValid)
            {
                return View(forgetPassword);
            }

            var user = await _userManager.FindByEmailAsync(forgetPassword.EmailOrUserName) ?? await _userManager.FindByNameAsync(forgetPassword.EmailOrUserName);

            if (user is null)
            {
                TempData["error-notification"] = "Invalid User name Or Email";
                return View(forgetPassword);
            }

            var OTPNumber = new Random().Next(1000, 9999);
            var link = Url.Action("ResetPassword", "Account", new { area = "Identity", userId = user.Id }, Request.Scheme);

            await _emailSender.SendEmailAsync(user.Email!, $"Reset Password!", $"<h1>Reset Password Using {OTPNumber}. Don't share it!</h1>");

            await _userOTP.CreateAsync(new()
            {
                ApplicationUserId = user.Id,
                OTPNumber = OTPNumber.ToString(),
                ValidTo = DateTime.UtcNow.AddDays(1)
            });
            await _userOTP.CommitAsync();

            TempData["success-notification"] = "Send OTP Number to Your Email successfully";
            return RedirectToAction("ResetPassword", "Account", new { area = "Identity", UserId = user.Id });
        }

        [HttpGet]
        public IActionResult ResetPassword(string UserId)
        {
            return View(new ResetPassword()
            {
                ApplicationUserId = UserId
            });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPassword);
            }

            var user = await _userManager.FindByIdAsync(resetPassword.ApplicationUserId);

            if (user is null)
            {
                TempData["error-notification"] = "Invalid User name Or Email";
                return View(resetPassword);
            }

            var userOTP = (await _userOTP.GetAsync(e => e.ApplicationUserId == resetPassword.ApplicationUserId)).OrderBy(e => e.Id).LastOrDefault();

            if (userOTP is null)
                return NotFound();

            if (userOTP.OTPNumber != resetPassword.OTPNumber)
            {
                TempData["error-notification"] = "Invalid OTP";
                return View(resetPassword);
            }

            if (DateTime.UtcNow > userOTP.ValidTo)
            {
                TempData["error-notification"] = "Expired OTP";
                return View(resetPassword);
            }

            TempData["success-notification"] = "Success OTP";
            return RedirectToAction("NewPassword", "Account", new { area = "Identity", UserId = user.Id });
        }

        [HttpGet]
        public IActionResult NewPassword(string UserId)
        {
            return View(new NewPass()
            {
                ApplicationUserId = UserId
            });
        }

        [HttpPost]
        public async Task<IActionResult> NewPassword(NewPass newPassword)
        {
            if (!ModelState.IsValid)
            {
                return View(newPassword);
            }

            var user = await _userManager.FindByIdAsync(newPassword.ApplicationUserId);

            if (user is null)
            {
                TempData["error-notification"] = "Invalid User name Or Email";
                return View(newPassword);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, token, newPassword.Password);

            TempData["success-notification"] = "Change Password Successfully!";
            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile(string userId)
        {

            var Users = await _userManager.FindByIdAsync(userId);


            var newuser = Users.Adapt<ProfileDM>();

            if (Users == null)
                return BadRequest();


            return View(newuser);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Profile(ProfileDM Appuser, IFormFile Image)
        {
            var userInDb = await _userManager.FindByIdAsync(Appuser.Id!);
            if (userInDb == null)
                return BadRequest();

            userInDb.Name = Appuser.Name;
            userInDb.UserName = Appuser.UserName;
            userInDb.Email = Appuser.Email;
            userInDb.PhoneNumber = Appuser.PhoneNumber;
            userInDb.City = Appuser.City;
            userInDb.State = Appuser.State;
            userInDb.Street = Appuser.Street;
            userInDb.ZipCode = Appuser.ZipCode;

            if (!string.IsNullOrWhiteSpace(Appuser.Password))
            {
                if (Appuser.Password != Appuser.ConfirmPassword)
                {
                    ModelState.AddModelError("", "Passwords do not match");
                    return View(Appuser);
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(userInDb);
                var passwordResult = await _userManager.ResetPasswordAsync(
                    userInDb,
                    token,
                    Appuser.Password
                );

                if (!passwordResult.Succeeded)
                {
                    foreach (var error in passwordResult.Errors)
                        ModelState.AddModelError("", error.Description);

                    return View(Appuser);
                }
            }

            if (Image != null && Image.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(Image.FileName);
                var filePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/UserImg",
                    fileName
                );

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }

                if (!string.IsNullOrEmpty(userInDb.Image))
                {
                    var oldFilePath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot/UserImg",
                        userInDb.Image
                    );

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

                return View(Appuser);
            }

            TempData["success-notification"] = "Updated Successfully";
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

    }
}

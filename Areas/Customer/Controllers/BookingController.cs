using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SkyLine.Models;
using Stripe.Checkout;


namespace SkyLine.Areas.Customer.Controllers
{

    [Area(SD.CustomerArea)]
    [Authorize]

    public class BookingController : Controller
    {
        private readonly IRepository<Booking> _Booking;
        private readonly IRepository<Flight> _flight;
        private readonly IRepository<Fare> _fare;
        private readonly UserManager<ApplicationUser> _userManager;


        public BookingController(IRepository<Booking> booking, IRepository<Flight> flight, UserManager<ApplicationUser> userManager, IRepository<Fare> fare)
        {
            _Booking = booking;
            _flight = flight;
            _userManager = userManager;
            _fare = fare;
        }

        [HttpGet]
        public async Task<IActionResult> MyBooking(string userId)
        {
           

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            var bookings = await _Booking.GetAsync(expression: e => e.User_Id_FK == userId, includes: [e => e.Flights!]);

            var Flights = await _flight.GetAsync(includes: [e => e.LeavingAirport!, e => e.ArriveAirport!]);

            ViewBag.Fligh = Flights;

            var fares22 = await _fare.GetAsync();

            ViewBag.Allfares = fares22;


            return View(bookings);
        }


        public async Task<IActionResult> Delete(int Booking_id)
         {
            var Books = await _Booking.GetOneAsync(expression: e => e.Booking_Id_PK == Booking_id);

            _Booking.Delete(Books!);
            await _Booking.CommitAsync();

            TempData["success-notification"] = "Deleted Successfully";

            return RedirectToAction(
                    actionName: "Index",
                    controllerName: "Home",
                    new { area = "Customer" });
        }






        public async Task<IActionResult> Pay()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null)
                return NotFound();

            var Flights22 = await _Booking.GetAsync(e => e.User_Id_FK == user.Id, includes: [e => e.Flights!]);

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/Customer/Booking/Success?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/Customer/Booking/cancel",
            };

           
            foreach (var item in Flights22)
            {
                if (item.status == 0)
                {

                    options.LineItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "egp",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.PNR,
                                Description = "Created At:" + item.CreatedAt.ToString("yyyy-MM-dd HH:mm")
                            },
                            UnitAmount = (long)item.TotalPrice * 100,
                        },
                        Quantity = 1,
                    });
                }
         
            }

          

            var service = new SessionService();
            var session = service.Create(options);
         
            return Redirect(session.Url);
        }


        public async Task<IActionResult> Success(string session_id)
        {
            if (string.IsNullOrEmpty(session_id))
                return BadRequest();

            var service = new SessionService();
            var session = service.Get(session_id);

            string transactionId = session.PaymentIntentId;

         
            var user = await _userManager.GetUserAsync(User);
            var bookings = await _Booking.GetAsync(e => e.User_Id_FK == user!.Id);

            foreach (var booking in bookings)
            {
                booking.status = Status.Completed;

                decimal Points = booking.TotalPrice;

                int points = (int)(Points / 10);

                user!.LoyaltyPoints += points;
                await _userManager.UpdateAsync(user);

            }

            await _Booking.CommitAsync();

     
            return View();
        }



    }
}

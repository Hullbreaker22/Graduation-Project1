using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SkyLine.Data_Access;
using SkyLine.Repositories;
using SkyLine.Utility.DBInitializer;
using Stripe;

namespace SkyLine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(option =>
            {
                option.Password.RequiredLength = 6;
                option.Password.RequireNonAlphanumeric = false;
                option.User.RequireUniqueEmail = true;
                option.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Customer/Home/NotFoundPage";
            });

            builder.Services.AddScoped<IRepository<UserOTP>, Repository<UserOTP>>();
            builder.Services.AddScoped<IRepository<Flight>, Repository<Flight>>();
            builder.Services.AddScoped<IRepository<Booking>, Repository<Booking>>();
            builder.Services.AddScoped<IRepository<Passenger>, Repository<Passenger>>();
            builder.Services.AddScoped<IRepository<ApplicationUser>, Repository<ApplicationUser>>();
            builder.Services.AddScoped<IRepository<FlightSegment>, Repository<FlightSegment>>();
            builder.Services.AddScoped<IRepository<Fare>, Repository<Fare>>();
            builder.Services.AddScoped<IRepository<Seat_Reservation>, Repository<Seat_Reservation>>();
            builder.Services.AddScoped<IRepository<City>, Repository<City>>();
            builder.Services.AddScoped<IRepository<AirPort>, Repository<AirPort>>();
            builder.Services.AddScoped<IRepository<Airline>, Repository<Airline>>();
            builder.Services.AddScoped<IRepository<City>, Repository<City>>();
            builder.Services.AddScoped<IRepository<FlightSegment>, Repository<FlightSegment>>();
            builder.Services.AddScoped<IDBInitializer, DBInitializer>();
            builder.Services.AddTransient<IEmailSender, EmailSender>();
          


            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

            var app = builder.Build();




            
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            //app.UseSession();

            var scope = app.Services.CreateScope();
            var service = scope.ServiceProvider.GetService<IDBInitializer>();

            service.Initialize();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using asp_projekt.Data;
using asp_projekt.Models;
using Microsoft.AspNetCore.Authorization;
// Mail
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using MimeKit.Text;
using Microsoft.AspNetCore.Http;

namespace asp_projekt.Controllers
{
    public class RestaurantBookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RestaurantBookingController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: RestaurantBooking
        [Authorize]
        public async Task<IActionResult> Index(string sortOrder)
        {
            // Sortering
            ViewData["DateParm"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["EmailParm"] = sortOrder == "Email" ? "email_desc" : "Email";

            var restaurantBookings = from r in _context.Restaurantbooking
                                     select r;

            switch (sortOrder)
            {
                case "email_desc":
                    restaurantBookings = restaurantBookings.OrderByDescending(h => h.Email);
                    break;
                case "Email":
                    restaurantBookings = restaurantBookings.OrderBy(h => h.Email);
                    break;
                case "date_desc":
                    restaurantBookings = restaurantBookings.OrderByDescending(h => h.Date);
                    break;
                default:
                    restaurantBookings = restaurantBookings.OrderBy(h => h.Date);
                    break;
            }

            return View(await restaurantBookings.AsNoTracking().ToListAsync());
        }

        // Partial som hanterar live-sökning via AJAX
        public PartialViewResult Ajax(string searchString)
        {
            List<RestaurantBooking> restaurantBookings;

            if (!String.IsNullOrEmpty(searchString))
            {
                restaurantBookings = _context.Restaurantbooking.Where(s => s.Email.Contains(searchString)
                                       || s.FirstName.Contains(searchString)
                                       || s.LastName.Contains(searchString)).ToList();
            }
            else
            {
                restaurantBookings = _context.Restaurantbooking.ToList();
            }
            return PartialView("_RestaurantBookingPartial", restaurantBookings);
        }

        // GET: RestaurantBooking/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurantBooking = await _context.Restaurantbooking
                .FirstOrDefaultAsync(m => m.Id == id);
            if (restaurantBooking == null)
            {
                return NotFound();
            }

            return View(restaurantBooking);
        }

        // GET: RestaurantBooking/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RestaurantBooking/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Time,NoOfPersons,FirstName,LastName,PhoneNumber,Email,ExtraInformation")] RestaurantBooking restaurantBooking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(restaurantBooking);
                await _context.SaveChangesAsync();

                // Lägger bokningsinfon i variabler
                var customerName = restaurantBooking.FirstName + " " + restaurantBooking.LastName;
                var customerEmail = restaurantBooking.Email;
                var customerDate = restaurantBooking.Date.ToString("d");
                var customerTime = restaurantBooking.Time.ToString("HH:mm");
                var customerNoOfPersons = restaurantBooking.NoOfPersons;
                //var customerExtraInfo = restaurantBooking.ExtraInformation;

                // Mail
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Pelican Palace", "pelicanpalace.ab@gmail.com"));
                message.To.Add(new MailboxAddress(customerName, customerEmail));
                message.Subject = "Bokningsbekräftelse";
                message.Body = new TextPart(TextFormat.Html)
                {
                    Text = "<h1> Hej, " + customerName + " </h1 >" +
                    "<p>Tack för att du bokar bord på Pelican Palace!</p>" +
                    "<p>Information om din bokning finner du nedan</p>" +
                    "<p>Datum: " + customerDate + "</p>" +
                    "<p>Tid: " + customerTime + "</p>" +
                    "<p>Antal personer: " + customerNoOfPersons + "</ p >" +
                    "<h4>Välkomna! </h4>"
                };

                // Konfigurering av email
                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("pelicanpalace.ab@gmail.com", "Core3Projekt");
                    client.Send(message);
                    client.Disconnect(true);
                };

                //Skapar sessionsvariabler för att skicka bokningsinfo till bokningsbekräftelsen
                HttpContext.Session.SetString("customer-name", customerName);

                return RedirectToAction(nameof(BookingConfirmation));
            }



            return View(restaurantBooking);
        }



        // Bokningsbekräftelse
        public IActionResult BookingConfirmation()
        {
            // Hämtar sessionsvariabeln med bokningsnamnet
            string sess = HttpContext.Session.GetString("customer-name");
            // Sparar i viewbag
            ViewBag.CustomerName = sess;
            return View();
        }



        // GET: RestaurantBooking/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurantBooking = await _context.Restaurantbooking.FindAsync(id);
            if (restaurantBooking == null)
            {
                return NotFound();
            }
            return View(restaurantBooking);
        }

        // POST: RestaurantBooking/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Time,NoOfPersons,FirstName,LastName,PhoneNumber,Email,ExtraInformation")] RestaurantBooking restaurantBooking)
        {
            if (id != restaurantBooking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(restaurantBooking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantBookingExists(restaurantBooking.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(restaurantBooking);
        }

        // GET: RestaurantBooking/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurantBooking = await _context.Restaurantbooking
                .FirstOrDefaultAsync(m => m.Id == id);
            if (restaurantBooking == null)
            {
                return NotFound();
            }

            return View(restaurantBooking);
        }

        // POST: RestaurantBooking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var restaurantBooking = await _context.Restaurantbooking.FindAsync(id);
            _context.Restaurantbooking.Remove(restaurantBooking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantBookingExists(int id)
        {
            return _context.Restaurantbooking.Any(e => e.Id == id);
        }
    }
}

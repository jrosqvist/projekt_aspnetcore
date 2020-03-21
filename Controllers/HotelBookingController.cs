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
    public class HotelBookingController : Controller
    {

        private readonly ApplicationDbContext _context;

        public HotelBookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HotelBooking
        [Authorize]
        public async Task<IActionResult> Index(string sortOrder)
        {
            var applicationDbContext = _context.HotelBooking.Include(h => h.Room);

            // Sortering
            ViewData["CheckInParm"] = String.IsNullOrEmpty(sortOrder) ? "checkin_desc" : "";
            ViewData["EmailParm"] = sortOrder == "Email" ? "email_desc" : "Email";

            var hotelBookings = from h in _context.HotelBooking.Include(r => r.Room)
                                select h;

            switch (sortOrder)
            {
                case "email_desc":
                    hotelBookings = hotelBookings.OrderByDescending(h => h.Email);
                    break;
                case "Email":
                    hotelBookings = hotelBookings.OrderBy(h => h.Email);
                    break;
                case "checkin_desc":
                    hotelBookings = hotelBookings.OrderByDescending(h => h.CheckIn);
                    break;
                default:
                    hotelBookings = hotelBookings.OrderBy(h => h.CheckIn);
                    break;
            }
            return View(await hotelBookings.AsNoTracking().ToListAsync());
        }

        // Partial som hanterar live-sökning via AJAX
        public PartialViewResult Ajax(string searchString)
        {
            List<HotelBooking> hotelBookings;

            if (!String.IsNullOrEmpty(searchString))
            {
                hotelBookings = _context.HotelBooking.Where(s => s.Email.Contains(searchString)
                                       || s.FirstName.Contains(searchString)
                                       || s.LastName.Contains(searchString)).ToList();
            } else
            {
                hotelBookings = _context.HotelBooking.ToList();
            }
            return PartialView("_HotelBookingPartial", hotelBookings);
        }

        // GET: HotelBooking/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotelBooking = await _context.HotelBooking
                .Include(h => h.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotelBooking == null)
            {
                return NotFound();
            }

            return View(hotelBooking);
        }

        // GET: HotelBooking/Create
        public IActionResult Create()
        {
            ViewData["RoomId"] = new SelectList(_context.Room, "Id", "RoomName");
            return View();
        }

        // POST: HotelBooking/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CheckIn,CheckOut,NoOfPersons,RoomId,FirstName,LastName,PhoneNumber,Email,ExtraInformation")] HotelBooking hotelBooking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hotelBooking);
                await _context.SaveChangesAsync();

                // Lägger bokningsinfon i variabler
                var customerName = hotelBooking.FirstName + " " + hotelBooking.LastName;
                var customerEmail = hotelBooking.Email;
                var customerCheckIn = hotelBooking.CheckIn.ToString("d");
                var customerCheckOut = hotelBooking.CheckOut.ToString("d");
                var customerNoOfPersons = hotelBooking.NoOfPersons;
                //var customerExtraInfo = restaurantBooking.ExtraInformation;

                // Hämtar namnet på rummet efter rumID
                var query =
                   from c in _context.Room
                   where c.Id == hotelBooking.RoomId
                   select c;

                var customerRoom = "";
                // Sätter rumsnamnet
                foreach (var c in query)
                {
                    customerRoom = c.RoomName;
                }

                // Mail
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Pelican Palace", "pelicanpalace.ab@gmail.com"));
                message.To.Add(new MailboxAddress(customerName, customerEmail));
                message.Subject = "Bokningsbekräftelse";
                message.Body = new TextPart(TextFormat.Html)
                {
                    Text = "<h1> Hej, " + customerName + " </h1 >" +
                    "<p>Tack för att du bokar din hotellvistelse på Pelican Palace!</p>" +
                    "<p>Information om din bokning finner du nedan</p>" +
                    "<p>Datum för incheckning: " + customerCheckIn + "</p>" +
                    "<p>Datum för utcheckning: " + customerCheckOut + "</p>" +
                    "<p>Bokat rum: " + customerRoom + "</p>" +
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
                //return View("BookingConfirmation", hotelBooking);
            }
            ViewData["RoomId"] = new SelectList(_context.Room, "Id", "RoomName", hotelBooking.RoomId);
            return View(hotelBooking);
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



        // GET: HotelBooking/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotelBooking = await _context.HotelBooking.FindAsync(id);
            if (hotelBooking == null)
            {
                return NotFound();
            }
            ViewData["RoomId"] = new SelectList(_context.Room, "Id", "RoomName", hotelBooking.RoomId);
            return View(hotelBooking);
        }

        // POST: HotelBooking/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CheckIn,CheckOut,NoOfPersons,RoomId,FirstName,LastName,PhoneNumber,Email,ExtraInformation")] HotelBooking hotelBooking)
        {
            if (id != hotelBooking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hotelBooking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelBookingExists(hotelBooking.Id))
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
            ViewData["RoomId"] = new SelectList(_context.Room, "Id", "RoomName", hotelBooking.RoomId);
            return View(hotelBooking);
        }

        // GET: HotelBooking/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotelBooking = await _context.HotelBooking
                .Include(h => h.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotelBooking == null)
            {
                return NotFound();
            }

            return View(hotelBooking);
        }

        // POST: HotelBooking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hotelBooking = await _context.HotelBooking.FindAsync(id);
            _context.HotelBooking.Remove(hotelBooking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HotelBookingExists(int id)
        {
            return _context.HotelBooking.Any(e => e.Id == id);
        }
    }
}

using AutoServis.Models;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace AutoServis.Controllers
{
    public class KorisnikController : Controller
    {
        private readonly BazaContext _context;

        public KorisnikController(BazaContext context)
        {
            _context = context;
        }

        public IActionResult Registracija()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registracija(Korisnik korisnik)
        {
            if (ModelState.IsValid)
            {
                korisnik.Aktiviran = false;
                _context.Korisnici.Add(korisnik);
                _context.SaveChanges();

                PosaljiAktivacijskiEmail(korisnik);

                ViewBag.Poruka = "Registracija uspješna! Provjerite e-mail za aktivaciju.";
                return View("Potvrda");
            }

            return View(korisnik);
        }

        public IActionResult Potvrda()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string lozinka)
        {
            
            HttpContext.Session.Remove("admin");

            var korisnik = _context.Korisnici
                .FirstOrDefault(k => k.Email == email && k.Lozinka == lozinka && k.Aktiviran);

            if (korisnik != null)
            {
                return RedirectToAction("Nova", "Servis", new { email = korisnik.Email });
            }

            ViewBag.Greska = "Neispravni podaci ili korisnik nije aktiviran.";
            return View();
        }



        public IActionResult Aktiviraj(string id)
        {
            var korisnik = _context.Korisnici.FirstOrDefault(k => k.Email == id);
            if (korisnik != null)
            {
                korisnik.Aktiviran = true;
                _context.SaveChanges();
                return Content("Korisnički račun je uspješno aktiviran!");
            }

            return Content("Neispravan aktivacijski link.");
        }

        private void PosaljiAktivacijskiEmail(Korisnik korisnik)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Auto Servis", "autoservis254@gmail.com"));
            message.To.Add(new MailboxAddress($"{korisnik.Ime} {korisnik.Prezime}", korisnik.Email));
            message.Subject = "Aktivacija korisničkog računa";

            var aktivacijskiLink = Url.Action("Aktiviraj", "Korisnik", new { id = korisnik.Email }, Request.Scheme);

            message.Body = new TextPart("plain")
            {
                Text = $"Pozdrav {korisnik.Ime},\n\nZa aktivaciju svog korisničkog računa klikni na link:\n{aktivacijskiLink}\n\nHvala!"
            };

            using var client = new SmtpClient();
            client.Connect("smtp.gmail.com", 587, false);
            client.Authenticate("autoservis254@gmail.com", "txct ehkw qcyr ezud");
            client.Send(message);
            client.Disconnect(true);
        }
    }
}

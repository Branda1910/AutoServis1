using AutoServis.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace AutoServis.Controllers
{
    public class AdminController : Controller
    {
        private const string adminEmail = "admin@autoservis.com";
        private const string adminLozinka = "admin123";
        private readonly BazaContext _context;

        public AdminController(BazaContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string lozinka)
        {
            if (email == adminEmail && lozinka == adminLozinka)
            {
                HttpContext.Session.SetString("admin", "true");
                return RedirectToAction("PopisKorisnika");
            }

            ViewBag.Greska = "Neispravni podaci za administratora.";
            return View();
        }

        public IActionResult Odjava()
        {
            HttpContext.Session.Remove("admin");
            return RedirectToAction("Login");
        }

        
        public IActionResult PopisKorisnika(string pojam)
        {
            if (HttpContext.Session.GetString("admin") != "true")
                return RedirectToAction("Login");

            var korisnici = _context.Korisnici.AsQueryable();

            if (!string.IsNullOrEmpty(pojam))
            {
                korisnici = korisnici.Where(k =>
                    k.Ime.Contains(pojam) ||
                    k.Prezime.Contains(pojam) ||
                    k.Email.Contains(pojam));
            }

            ViewBag.Pojam = pojam;
            return View(korisnici.ToList());
        }

        
        public IActionResult Uredi(int id)
        {
            if (HttpContext.Session.GetString("admin") != "true")
                return RedirectToAction("Login");

            var korisnik = _context.Korisnici.FirstOrDefault(k => k.Id == id);
            if (korisnik == null) return NotFound();
            return View(korisnik);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Uredi(Korisnik korisnik)
        {
            if (HttpContext.Session.GetString("admin") != "true")
                return RedirectToAction("Login");

            if (ModelState.IsValid)
            {
                _context.Korisnici.Update(korisnik);
                _context.SaveChanges();
                return RedirectToAction("PopisKorisnika");
            }

            return View(korisnik);
        }

        
        public IActionResult PotvrdaBrisanja(int id)
        {
            if (HttpContext.Session.GetString("admin") != "true")
                return RedirectToAction("Login");

            var korisnik = _context.Korisnici.Find(id);
            if (korisnik == null) return NotFound();
            return View(korisnik);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Obrisi(int id)
        {
            if (HttpContext.Session.GetString("admin") != "true")
                return RedirectToAction("Login");

            var korisnik = _context.Korisnici.FirstOrDefault(k => k.Id == id);
            if (korisnik == null) return NotFound();

            _context.Korisnici.Remove(korisnik);
            _context.SaveChanges();

            return RedirectToAction("PopisKorisnika");
        }

        
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("admin");
            return RedirectToAction("Index", "Home");
        }
    }
}

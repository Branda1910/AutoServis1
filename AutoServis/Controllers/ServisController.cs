using AutoServis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.IO;
using System.Linq;

namespace AutoServis.Controllers
{
    public class ServisController : Controller
    {
        private readonly BazaContext _context;

        public ServisController(BazaContext context)
        {
            _context = context;
        }

        public IActionResult Nova(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        public IActionResult Nova(NarudzbaServisa narudzba)
        {
            if (narudzba.PredlozeniDatum < DateTime.Today)
            {
                ModelState.AddModelError("PredlozeniDatum", "Datum ne može biti u prošlosti.");
            }

            if (ModelState.IsValid)
            {
                narudzba.Status = "Čeka potvrdu";
                narudzba.DatumNarudzbe = DateTime.Now;

                _context.NarudzbeServisa.Add(narudzba);
                _context.SaveChanges();

                return RedirectToAction("MojeNarudzbe", new { email = narudzba.EmailKorisnika });
            }

            ViewBag.Email = narudzba.EmailKorisnika;
            return View(narudzba);
        }


        public IActionResult MojeNarudzbe(string email)
        {
            var moje = _context.NarudzbeServisa
                .Where(n => n.EmailKorisnika == email)
                .OrderByDescending(n => n.Id)
                .ToList();

            ViewBag.Email = email;
            return View(moje);
        }

        public IActionResult Popis()
        {
            var sve = _context.NarudzbeServisa.OrderByDescending(n => n.Id).ToList();
            return View(sve);
        }

        public IActionResult NarudzbeZaPotvrdu()
        {
            var narudzbe = _context.NarudzbeServisa
                .Where(n => n.Status == "Čeka potvrdu")
                .OrderByDescending(n => n.DatumNarudzbe)
                .ToList();

            return View(narudzbe);
        }

        [HttpGet]
        public IActionResult Potvrdi(int id)
        {
            var narudzba = _context.NarudzbeServisa.FirstOrDefault(n => n.Id == id);
            if (narudzba == null) return NotFound();
            return View(narudzba); // View mora biti Potvrdi.cshtml
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Potvrdi(NarudzbaServisa narudzba)
        {
            Console.WriteLine(">>> POZVAN POST Potvrdi");

            // Učitavamo postojeću narudžbu
            var postojeca = _context.NarudzbeServisa.FirstOrDefault(n => n.Id == narudzba.Id);
            if (postojeca == null) return NotFound();

            // Dodaj podatke koji NEDOSTAJU u formi, ali su REQUIRED
            narudzba.OpisProblema = postojeca.OpisProblema;
            narudzba.EmailKorisnika = postojeca.EmailKorisnika;
            narudzba.PredlozeniDatum = postojeca.PredlozeniDatum;
            narudzba.DatumNarudzbe = postojeca.DatumNarudzbe;

            // Makni greške iz ModelState za te fieldove
            ModelState.Remove("OpisProblema");
            ModelState.Remove("EmailKorisnika");
            ModelState.Remove("PredlozeniDatum");
            ModelState.Remove("DatumNarudzbe");

            if (!ModelState.IsValid)
            {
                Console.WriteLine(">>> MODEL NIJE VALIDAN");
                foreach (var e in ModelState)
                {
                    Console.WriteLine($"{e.Key} => {string.Join(", ", e.Value.Errors.Select(er => er.ErrorMessage))}");
                }

                return View("Potvrdi", narudzba);
            }

            // Spremi potvrdu
            postojeca.VrstaUsluge = narudzba.VrstaUsluge;
            postojeca.Cijena = narudzba.Cijena;
            postojeca.PotvrdjeniDatum = narudzba.PotvrdjeniDatum;
            postojeca.Status = "Potvrđeno";

            _context.SaveChanges();

            Console.WriteLine(">>> POTVRĐENO I SPREMLJENO");
            return RedirectToAction("Popis");
        }





        public IActionResult Odbij(int id)
        {
            var narudzba = _context.NarudzbeServisa.FirstOrDefault(n => n.Id == id);
            if (narudzba != null)
            {
                narudzba.Status = "Odbijeno";
                _context.SaveChanges();
            }
            return RedirectToAction("Popis");
        }


        public IActionResult Uredi(int id)
        {
            var narudzba = _context.NarudzbeServisa.FirstOrDefault(n => n.Id == id);
            if (narudzba == null) return NotFound();
            return View(narudzba);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Uredi(NarudzbaServisa narudzba)
        {
            if (ModelState.IsValid)
            {
                _context.NarudzbeServisa.Update(narudzba);
                _context.SaveChanges();
                return RedirectToAction("Popis");
            }
            return View(narudzba);
        }

        public IActionResult GenerirajRacun(int id)
        {
            var narudzba = _context.NarudzbeServisa.FirstOrDefault(n => n.Id == id && n.Status == "Potvrđeno");
            if (narudzba == null)
            {
                return NotFound("Račun se može generirati samo za potvrđene zahtjeve.");
            }

            var dokument = new PdfDocument();
            dokument.Info.Title = "Račun za servis";

            var stranica = dokument.AddPage();
            var gfx = XGraphics.FromPdfPage(stranica);
            var font = new XFont("Verdana", 12, XFontStyle.Regular);

            gfx.DrawString("Račun za servis vozila", new XFont("Verdana", 16, XFontStyle.Bold), XBrushes.Black,
                new XRect(0, 0, stranica.Width, 40), XStringFormats.TopCenter);

            gfx.DrawString($"Email korisnika: {narudzba.EmailKorisnika}", font, XBrushes.Black, 40, 80);
            gfx.DrawString($"Opis problema: {narudzba.OpisProblema}", font, XBrushes.Black, 40, 110);
            gfx.DrawString($"Datum servisa: {narudzba.PotvrdjeniDatum?.ToShortDateString()}", font, XBrushes.Black, 40, 140);
            gfx.DrawString($"Vrsta usluge: {narudzba.VrstaUsluge}", font, XBrushes.Black, 40, 170);
            gfx.DrawString($"Cijena usluge: {narudzba.Cijena:F2} €", font, XBrushes.Black, 40, 200);
            gfx.DrawString($"PDV (25%): {(narudzba.Cijena * 0.25m):F2} €", font, XBrushes.Black, 40, 230);
            gfx.DrawString($"Ukupno za platiti: {(narudzba.Cijena * 1.25m):F2} €", new XFont("Verdana", 12, XFontStyle.Bold), XBrushes.Black, 40, 260);

            using var stream = new MemoryStream();
            dokument.Save(stream, false);
            return File(stream.ToArray(), "application/pdf", $"Racun_{narudzba.Id}.pdf");
        }
    }
}

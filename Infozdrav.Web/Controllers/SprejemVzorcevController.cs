using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Infozdrav.Web.Models.Labena;
using Microsoft.AspNetCore.Mvc;
using Infozdrav.Web.Data;
using Microsoft.AspNetCore.Identity;

namespace Infozdrav.Web.Controllers
{
    public class SprejemVzorcevController : Controller
    {
        private readonly AppDbContext _dbContext;

        public SprejemVzorcevController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Sprejem()
        {
            return View(new SprejemVzorcevViewModel());
        }


        [HttpPost]
        public IActionResult Sprejem([FromForm] SprejemVzorcevViewModel sprejem)
        {
            if (sprejem == null) // popravi, ker ne preverja zares, ali so polja prazna
            {
                return RedirectToAction("Sprejem");
            }

            var narocnik = _dbContext.Subscribers.FirstOrDefault(o => o.Name == sprejem.SupplierName);
            var kontakt = _dbContext.ContactPeople.FirstOrDefault(o => o.Name == sprejem.ContactName);

            if (narocnik == null)
            {
                narocnik = new Subscriber
                {
                    Name = sprejem.SupplierName,
                    IdNumber = sprejem.IdDDV,
                    TaxNumber = sprejem.TaxNumber,
                    Address = sprejem.StreetNum,
                    City = sprejem.City,
                    Country = sprejem.Country,
                    PostalCode = sprejem.ZipNumber
                };
                _dbContext.Subscribers.Add(narocnik);
            }

            if (kontakt == null)
            {
                kontakt = new ContactPerson
                {
                    Name = sprejem.ContactName,
                    Email = sprejem.Mail,
                    PhoneNumber = sprejem.TelNumber
                };
                _dbContext.ContactPeople.Add(kontakt);
            }

            _dbContext.Acceptances.Add(new Acceptance
            {
                Date = sprejem.DateOfReception ?? DateTime.Now,
                SubscriberName = narocnik,
                Contact = kontakt,
            });
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult DodajanjeVzorca()
        {
            return View(new DodajanjeVzorcaViewModel());
        }

        [HttpPost]
        public IActionResult DodajanjeVzorca([FromForm] DodajanjeVzorcaViewModel vzorec)
        {
            if (vzorec == null)
                return RedirectToAction("Index");

            _dbContext.Samples.Add(new Sample
            {
                Type = _dbContext.SampleTypes.FirstOrDefault(m => m.SampleTypeName == vzorec.Type),
                ProjectName = _dbContext.Projects.FirstOrDefault(m => m.Name == vzorec.Project),
                Stored = new Storage
                {
                    BoxName = _dbContext.Boxes.FirstOrDefault(m => m.BoxName == vzorec.Box),
                    FridgeName = _dbContext.Fridges.FirstOrDefault(m => m.Name == vzorec.Fridge),
                    PositionColumn = (int) char.GetNumericValue(vzorec.Location[1]),
                    PositionRow = vzorec.Location[0],
                    RoomName = _dbContext.Rooms.FirstOrDefault(m => m.RoomName == vzorec.Room),
                    Temperature = vzorec.Temp
                },
                Comments = vzorec.Notes,
                SubscriberName = vzorec.IdProvider,
                Time = vzorec.Date ?? DateTime.Now,
                Volume = vzorec.Volume
            });
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult DodajanjeAlikvotov()
        {
            return View(new DodajanjeAlikvotovViewModel());
        }

        public IActionResult UrejanjeSkatel()
        {
            return View(new SkatlaViewModel());
        }

        public IActionResult PoveziVzorec()
        {
            return View(new PoveziVzorecViewModel());
        }

        public IActionResult DodajTip()
        {
            return View(new DodajTipViewModel());
        }

        [HttpPost]
        public IActionResult DodajTip([FromForm] DodajTipViewModel tip)
        {
            if (tip == null)
                return RedirectToAction("DodajanjeVzorca");

            var exists = _dbContext.SampleTypes.FirstOrDefault(m => m.SampleTypeName == tip.TipVzorca);

            if (exists == null)
            {
                _dbContext.SampleTypes.Add(new SampleType {SampleTypeName = tip.TipVzorca});
                _dbContext.SaveChanges();
            }

            return RedirectToAction("DodajanjeVzorca");
        }

        public IActionResult VnosHladilnika()
        {
            return View(new VnosHladilnikaViewModel());
        }

        [HttpPost]
        public IActionResult VnosHladilnika([FromForm] VnosHladilnikaViewModel hladilnik)
        {
            if (hladilnik == null)
                return RedirectToAction("DodajanjeVzorca");

            var fridge = new Fridge
            {
                Name = hladilnik.Fridge,
                Place = _dbContext.Rooms.First(o => o.RoomName == hladilnik.Room)
            };
            _dbContext.Fridges.Add(fridge);
            _dbContext.SaveChanges();

            return RedirectToAction("DodajanjeVzorca");
        }

        public IActionResult Skatle()
        {
            return View(new SkatlaViewModel());
        }

        public IActionResult DodajanjeSkatel()
        {
            return View(new SkatlaViewModel());
        }

        [HttpPost]
        public IActionResult DodajanjeSkatel([FromForm] SkatlaViewModel skatla)
        {
            Console.WriteLine("here");
            if (skatla == null)
                return RedirectToAction("DodajanjeVzorca");

            Console.WriteLine(skatla);

            Console.WriteLine(skatla.ImeSkatle);
            Console.WriteLine(skatla.Velikost);
            Console.WriteLine(skatla.Tip);
            _dbContext.Boxes.Add(new Box
            {
                BoxName = skatla.ImeSkatle,
                Size = skatla.Velikost,
                Type = skatla.Tip
            });

            _dbContext.SaveChanges();

            return RedirectToAction("DodajanjeVzorca");
        }
    }
}
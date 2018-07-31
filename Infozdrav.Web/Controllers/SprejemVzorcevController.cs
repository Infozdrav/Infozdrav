using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Infozdrav.Web.Models.Labena;
using Microsoft.AspNetCore.Mvc;
using Infozdrav.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using System.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Infozdrav.Web.Helpers;

namespace Infozdrav.Web.Controllers
{
    public class SprejemVzorcevController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly Dictionary<char, int> _rowNames;
        private static int _sampleValue;

        public SprejemVzorcevController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _rowNames = new Dictionary<char, int>
            {
                {'A', 0},
                {'B', 1},
                {'C', 2},
                {'D', 3},
                {'E', 4},
                {'F', 5},
                {'G', 6},
                {'H', 7},
                {'I', 8},
                {'J', 9},
                {'K', 10},
                {'L', 11}
            };
//            _sampleValue = 0;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DodajanjeNarocnikov()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DodajanjeNarocnikov([FromForm] SprejemVzorcevViewModel sprejem)
        {
            if (sprejem.SupplierName == null || sprejem.TaxNumber == null || sprejem.StreetNum == null
                || sprejem.City == null || sprejem.Country == null || !sprejem.ZipNumber.HasValue)
                return View(sprejem);

            var narocnik = new Subscriber
            {
                Name = sprejem.SupplierName,
                IdNumber = sprejem.IdDDV,
                TaxNumber = sprejem.TaxNumber,
                Address = sprejem.StreetNum,
                City = sprejem.City,
                Country = sprejem.Country,
                PostalCode = sprejem.ZipNumber.Value
            };
            _dbContext.Subscribers.Add(narocnik);
            _dbContext.SaveChanges();
            
            TempData.Put("narocnik", sprejem);
            
            return RedirectToAction("Sprejem");
        }

        public IActionResult DodajanjeKontaktov()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DodajanjeKontaktov([FromForm] SprejemVzorcevViewModel sprejem)
        {
            if (sprejem.ContactName == null)
                return View(sprejem);

            var kontakt = new ContactPerson
            {
                Name = sprejem.ContactName,
                Email = sprejem.Mail,
                PhoneNumber = sprejem.TelNumber
            };
            _dbContext.ContactPeople.Add(kontakt);
            _dbContext.SaveChanges();
            
            TempData.Put("kontakt", sprejem);
            
            return RedirectToAction("Sprejem");
        }


        public IActionResult Sprejem()
        {
            ViewBag.suppliers = _dbContext.Subscribers.Select(m => m.Name);
            ViewBag.people = _dbContext.ContactPeople.Select(m => m.Name);

            if (TempData.ContainsKey("kontakt"))
            {
                var sprejem = TempData.Get<SprejemVzorcevViewModel>("kontakt");
                TempData.Remove("kontakt");
                return View(sprejem);
            }
            
            if (TempData.ContainsKey("narocnik"))
            {
                var sprejem = TempData.Get<SprejemVzorcevViewModel>("narocnik");
                TempData.Remove("narocnik");
                return View(sprejem);
            }
            
            return View();
        }


        [HttpPost]
        public IActionResult Sprejem([FromForm] SprejemVzorcevViewModel sprejem)
        {
            if (sprejem?.SupplierName == null || sprejem.ContactName == null)
                return View(sprejem);

            var narocnik = _dbContext.Subscribers.FirstOrDefault(o => o.Name == sprejem.SupplierName);
            var kontakt = _dbContext.ContactPeople.FirstOrDefault(o => o.Name == sprejem.ContactName);

            _dbContext.Acceptances.Add(new Acceptance
            {
                Date = sprejem.DateOfReception ?? DateTime.Now,
                SubscriberName = narocnik,
                Contact = kontakt
            });
            _dbContext.SaveChanges();
            return RedirectToAction("DodajanjeVzorca");
        }

        public IActionResult DodajanjeVzorca()
        {
            ViewBag.Fridges = _dbContext.Fridges.Select(m => m.Name);
            ViewBag.Supplier = _dbContext.Subscribers.Select(m => m.Name);
            ViewBag.SupplierDates = _dbContext.Acceptances.Select(m => m.Date);
            ViewBag.Boxes = _dbContext.Boxes.Select(m => m.BoxName);
            ViewBag.Types = _dbContext.SampleTypes.Select(m => m.SampleTypeName);
            ViewBag.Projects = _dbContext.Projects.Select(m => m.Name);
            return View();
        }

        [HttpPost]
        public IActionResult DodajanjeVzorca([FromForm] DodajanjeVzorcaViewModel vzorec)
        {
            if (vzorec == null)
                return RedirectToAction("Index");

            if (vzorec.Fridge == null || !vzorec.Razdelek.HasValue)
                return View(vzorec);

            Storage stored = new Storage
            {
                FridgeName = _dbContext.Fridges.FirstOrDefault(m => m.Name == vzorec.Fridge),
                Temperature = vzorec.Temp,
                Section = vzorec.Razdelek.Value
            };


            if (vzorec.Location != null && vzorec.Box != null)
            {
                stored.BoxName = _dbContext.Boxes.First(m => m.BoxName == vzorec.Box);
                var column = (int) char.GetNumericValue(vzorec.Location[1]);
                if (vzorec.Location.Length == 3)
                    column = 10 * column + (int) char.GetNumericValue(vzorec.Location[2]);
                stored.Position = _rowNames[vzorec.Location[0]] * stored.BoxName.Size + column;
            }

            var sampleType = _dbContext.SampleTypes.FirstOrDefault(m => m.SampleTypeName == vzorec.Type);
            
            if (_dbContext.Samples.FirstOrDefault(m =>
                    m.NewId.Year == (vzorec.DateReception ?? DateTime.Now).Year) == null)
                _sampleValue = 1;
            else
                _sampleValue++;
            
            var id = new SampleId
            {
                AliquotSequenceNumber = 0,
                ProcessedAliquotSequenceNumber = 0,
                Year = (vzorec.DateReception ?? DateTime.Now).Year,
                SequenceNumber = _sampleValue
            };

            if (sampleType != null)
                id.IsolateType = sampleType.ShortName;

            if (!vzorec.Volume.HasValue)
                return View(vzorec);

            var sample = new Sample
            {
                SubscriberName = vzorec.IdProvider,
                Type = sampleType,
                Stored = stored,
                Accepted = _dbContext.Acceptances.FirstOrDefault(m => m.Date == vzorec.DateReception &&
                                                                      m.SubscriberName ==
                                                                      _dbContext.Subscribers.FirstOrDefault(n =>
                                                                          n.Name == vzorec.Provider)),
                NewId = id,
                Volume = vzorec.VolType == "ml" ? vzorec.Volume.Value : vzorec.Volume.Value / 1000000
            };

            if (vzorec.Project != null)
                sample.ProjectName = _dbContext.Projects.FirstOrDefault(m => m.Name == vzorec.Project);

            if (vzorec.Notes != null)
                sample.Comments = vzorec.Notes;

            if (vzorec.Date != null)
                sample.Time = vzorec.Date ?? DateTime.Now;

            _dbContext.Samples.Add(sample);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult DodajanjeAlikvotov()
        {
            ViewBag.Samples = _dbContext.Samples.Select(m => m.NewId.ToString());
            ViewBag.Fridges = _dbContext.Fridges.Select(m => m.Name);
            ViewBag.Boxes = _dbContext.Boxes.Select(m => m.BoxName);
            ViewBag.Projects = _dbContext.Projects.Select(m => m.Name);
            return View();
        }

        [HttpPost]
        public IActionResult DodajanjeAlikvotov([FromForm] DodajanjeAlikvotovViewModel alikvot)
        {
            if (alikvot == null)
                return RedirectToAction("Index");

            if (alikvot.IdVzorca == null || alikvot.Box == null)
                return View(alikvot);
            
            Console.WriteLine(alikvot.IdVzorca);              

            var originalSample = _dbContext.Samples.Include(m => m.NewId).First(m => m.NewId.ToString() == alikvot.IdVzorca);

            var startPosition = 0;
            var boxName = _dbContext.Boxes.First(m => m.BoxName == alikvot.Box);
            
            if (alikvot.Location != null)
            {
                var column = (int) char.GetNumericValue(alikvot.Location[1]);
                if (alikvot.Location.Length == 3)
                    column = 10 * column + (int) char.GetNumericValue(alikvot.Location[2]);
                startPosition = _rowNames[alikvot.Location[0]] * boxName.Size + column;
            }

            for (var i = 0; i < alikvot.StAlikvotov; i++)
            {
                var id = new SampleId
                {
                    AliquotSequenceNumber = originalSample.NewId.AliquotSequenceNumber,
                    IsolateType = originalSample.NewId.IsolateType,
                    ProcessedAliquotSequenceNumber = originalSample.NewId.ProcessedAliquotSequenceNumber,
                    ProcessedIsolateType = originalSample.NewId.ProcessedIsolateType,
                    SequenceNumber = originalSample.NewId.SequenceNumber,
                    Year = originalSample.NewId.Year
                };
                if (id.AliquotSequenceNumber == 0)
                    id.AliquotSequenceNumber = i + 1;
                else
                    id.ProcessedAliquotSequenceNumber = i + 1;

                if (!alikvot.Volume.HasValue)
                    return View(alikvot);

                var sample = new Aliquot
                {
                    Volume = alikvot.VolType == "ml" ? alikvot.Volume.Value : alikvot.Volume.Value / 1000000,
                    AliquotationDate = alikvot.Date ?? DateTime.Now,
                    OriginalSample = originalSample,
                    NewId = id
                };
                if (alikvot.Project != null)
                    sample.ProjectName = _dbContext.Projects.FirstOrDefault(m => m.Name == alikvot.Project);

                if (alikvot.Notes != null)
                    sample.Comments = alikvot.Notes;

                if (!alikvot.Razdelek.HasValue)
                    return View(alikvot);

                var stored = new Storage
                {
                    FridgeName = _dbContext.Fridges.FirstOrDefault(m => m.Name == alikvot.Fridge),
                    Temperature = alikvot.Temp,
                    Section = alikvot.Razdelek.Value,
                    BoxName = boxName,
                    Position = startPosition + i
                };
                sample.Stored = stored;

                _dbContext.Samples.Add(sample);
            }

            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult UrejanjeSkatel()
        {
            ViewBag.Boxes = _dbContext.Boxes.Select(m => m.BoxName);
            return View();
        }

        public IActionResult PoveziVzorec()
        {
            return View(new PoveziVzorecViewModel());
        }

        public IActionResult DodajTip()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DodajTip([FromForm] DodajTipViewModel tip)
        {
            if (tip == null)
                return RedirectToAction("DodajanjeVzorca");

            var exists = _dbContext.SampleTypes.FirstOrDefault(m => m.SampleTypeName == tip.TipVzorca);

            if (exists == null)
            {
                _dbContext.SampleTypes.Add(new SampleType {SampleTypeName = tip.TipVzorca, ShortName = "OTH"});
                _dbContext.SaveChanges();
            }

            return RedirectToAction("DodajanjeVzorca");
        }

        public IActionResult VnosHladilnika()
        {
            ViewBag.Rooms = _dbContext.Rooms.Select(m => m.RoomName);
            return View();
        }

        [HttpPost]
        public IActionResult VnosHladilnika([FromForm] VnosHladilnikaViewModel hladilnik)
        {
            if (hladilnik.Fridge == null || hladilnik.Room == null)
                return RedirectToAction("VnosHladilnika");

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
            if (skatla == null)
                return RedirectToAction("DodajanjeVzorca");

            if (!skatla.Velikost.HasValue)
                return View(skatla);

            _dbContext.Boxes.Add(new Box
            {
                BoxName = skatla.ImeSkatle,
                Size = skatla.Velikost.Value,
                Type = skatla.Tip
            });

            _dbContext.SaveChanges();

            return RedirectToAction("DodajanjeVzorca");
        }
    }
}
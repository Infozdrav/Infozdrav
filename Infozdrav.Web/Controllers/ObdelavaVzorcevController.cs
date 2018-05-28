using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infozdrav.Web.Data;
using Infozdrav.Web.Models.Labena;
using Microsoft.AspNetCore.Mvc;

namespace Infozdrav.Web.Controllers
{
    public class ObdelavaVzorcevController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ObdelavaVzorcevController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult IzberiVzorec()
        {
            ViewBag.DataSource = _dbContext.Samples;
            return View();
        }

        public IActionResult Obdelava()
        {
            ViewBag.DataSource = _dbContext.Samples;
            ViewBag.Types = _dbContext.SampleTypes.Select(m => m.SampleTypeName);
            return View();
        }

        [HttpPost]
        public IActionResult Obdelava(ObdelavaVzorcevViewModel obdelava)
        {
            var process = new Processing
            {
                Aparature = obdelava.Aparatura,
                Chemicals = obdelava.Kemikalije,
                Date = obdelava.DatumObdelave ?? DateTime.Today,
                IsolateName = _dbContext.SampleTypes.First( m => m.SampleTypeName == obdelava.Izolat),
                Protocole = obdelava.Protokol
            };

            _dbContext.Add(process);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Rezultati()
        {
            return View();
        }

        public IActionResult VzorciVObdelavi()
        {
            ViewBag.DataSource = _dbContext.Processings;
            return View();
        }
    }
}
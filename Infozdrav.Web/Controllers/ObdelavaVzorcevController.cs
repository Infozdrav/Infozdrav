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
            return View(new ObdelavaVzorcevViewModel());
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
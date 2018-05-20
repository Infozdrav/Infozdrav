using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infozdrav.Web.Models.Labena;
using Microsoft.AspNetCore.Mvc;

namespace Infozdrav.Web.Controllers
{
    public class ObdelavaVzorcevController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult IzberiVzorec()
        {
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
            return View();
        }
    }
}
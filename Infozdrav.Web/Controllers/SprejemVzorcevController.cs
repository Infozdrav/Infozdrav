using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infozdrav.Web.Models.Labena;
using Microsoft.AspNetCore.Mvc;

namespace Infozdrav.Web.Controllers
{
    public class SprejemVzorcevController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Sprejem()
        {
            return View(new SprejemVzorcevViewModel());
        }

        public IActionResult DodajanjeVzorca()
        {
            return View(new DodajanjeVzorcaViewModel());
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
            return View(new DodajanjeVzorcaViewModel());
        }

        public IActionResult VnosHladilnika()
        {
            return View(new VnosHladilnikaViewModel());
        }

        public IActionResult Skatle()
        {
            return View(new SkatlaViewModel());
        }

        public IActionResult DodajanjeSkatel()
        {
            return View(new SkatlaViewModel());
        }


    }
}
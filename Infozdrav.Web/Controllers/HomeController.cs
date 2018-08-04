using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Infozdrav.Web.Models;

namespace Infozdrav.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Labena()
        {
            return View();
        }
    }
}
using Microsoft.AspNetCore.Mvc;

namespace Infozdrav.Web.Controllers
{
    public class ScannerController : Controller
    {
        public IActionResult Camera()
        {
            return View();
        }
    }
}
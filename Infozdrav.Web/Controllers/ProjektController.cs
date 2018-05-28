using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Infozdrav.Web.Models.Labena;
using Infozdrav.Web.Data;

namespace Infozdrav.Web.Controllers
{
    public class ProjektController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ProjektController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DodajanjeProjektov()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DodajanjeProjektov([FromForm] ProjektViewModel projekt)
        {
            _dbContext.Projects.Add(new Project
            {
                Name = projekt.Name,
                Description = projekt.Description
            });
            
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
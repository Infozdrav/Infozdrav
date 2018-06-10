using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Infozdrav.Web.Data;
using Infozdrav.Web.Data.Trbovlje;
using Infozdrav.Web.Models.Manage;
using Infozdrav.Web.Models.Trbovlje;
using Infozdrav.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Infozdrav.Web.Controllers
{
    public class WorkLocationController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public WorkLocationController(AppDbContext dbContext, IMapper mapper, UserService userService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            ViewBag.DataSource = _mapper.Map<ICollection<Models.Trbovlje.WorkLocationViewModel>>(_dbContext.WorkLocations);

            return View();
        }

        public IActionResult WorkLocation(int id)
        {
            var workLocation = _dbContext.WorkLocations.FirstOrDefault(u => u.Id == id);
            if (workLocation == null)
                return RedirectToAction("Index");

            return base.View(_mapper.Map<Models.Trbovlje.WorkLocationViewModel>(workLocation));
        }

        [HttpPost]
        public IActionResult WorkLocation([FromForm] Models.Trbovlje.WorkLocationViewModel workLocation)
        {
            if (!ModelState.IsValid)
                return View(workLocation);

            var dbWorkLocation = _dbContext.WorkLocations.FirstOrDefault(u => u.Id == workLocation.Id);
            if (dbWorkLocation == null)
                return RedirectToAction("Index");

            _mapper.Map(workLocation, dbWorkLocation);
            _dbContext.WorkLocations.Update(dbWorkLocation);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add([FromForm] Models.Trbovlje.WorkLocationViewModel workLocation)
        {
            if (!ModelState.IsValid)
                return View(workLocation);

            WorkLocation dbWorkLocation = new WorkLocation();
            
            _mapper.Map(workLocation, dbWorkLocation);
            _dbContext.WorkLocations.Add(dbWorkLocation);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var dbWorkLocation = _dbContext.WorkLocations.FirstOrDefault(l => l.Id == id);
            if (dbWorkLocation == null)
                RedirectToAction("Index");

            return View(_mapper.Map<WorkLocationViewModel>(dbWorkLocation));
        }

        [HttpPost]
        public IActionResult Remove([FromForm] WorkLocationViewModel viewModel)
        {
            var dbWorkLocation = _dbContext.WorkLocations.FirstOrDefault(l => l.Id == viewModel.Id);
            if (dbWorkLocation != null)
            {
                _dbContext.WorkLocations.Remove(dbWorkLocation);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
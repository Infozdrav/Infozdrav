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
    public class ManufacturerController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public ManufacturerController(AppDbContext dbContext, IMapper mapper, UserService userService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            ViewBag.DataSource = _mapper.Map<ICollection<Models.Trbovlje.ManufacturerViewModel>>(_dbContext.Manufacturers);

            return View();
        }

        public IActionResult Manufacturer(int id)
        {
            var manufacturer = _dbContext.Manufacturers.FirstOrDefault(u => u.Id == id);
            if (manufacturer == null)
                return RedirectToAction("Index");

            return base.View(_mapper.Map<Models.Trbovlje.ManufacturerViewModel>(manufacturer));
        }

        [HttpPost]
        public IActionResult Manufacturer([FromForm] Models.Trbovlje.ManufacturerViewModel manufacturer)
        {
            if (!ModelState.IsValid)
                return View(manufacturer);

            var dbManufacturer = _dbContext.Manufacturers.FirstOrDefault(u => u.Id == manufacturer.Id);
            if (dbManufacturer == null)
                return RedirectToAction("Index");

            _mapper.Map(manufacturer, dbManufacturer);
            _dbContext.Manufacturers.Update(dbManufacturer);
            _dbContext.SaveChanges();

            return View(manufacturer);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add([FromForm] Models.Trbovlje.ManufacturerViewModel manufacturer)
        {
            if (!ModelState.IsValid)
                return View(manufacturer);

            Manufacturer dbManufacturer = new Manufacturer();

            _mapper.Map(manufacturer, dbManufacturer);
            _dbContext.Manufacturers.Add(dbManufacturer);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var dbManufacturer = _dbContext.Manufacturers.FirstOrDefault(l => l.Id == id);
            if (dbManufacturer == null)
                RedirectToAction("Index");

            return View(_mapper.Map<ManufacturerViewModel>(dbManufacturer));
        }

        [HttpPost]
        public IActionResult Remove([FromForm] ManufacturerViewModel viewModel)
        {
            var dbManufacturer = _dbContext.Manufacturers.FirstOrDefault(l => l.Id == viewModel.Id);
            if (dbManufacturer != null)
            {
                _dbContext.Manufacturers.Remove(dbManufacturer);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }


    }
}
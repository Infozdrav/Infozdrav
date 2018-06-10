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
    public class StorageLocationController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public StorageLocationController(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            ViewBag.DataSource = _mapper.Map<ICollection<Models.Trbovlje.StorageLocationViewModel>>(_dbContext.StorageLocations);

            return View();
        }

        public IActionResult StorageLocation(int id)
        {
            var storageLocation = _dbContext.StorageLocations.FirstOrDefault(u => u.Id == id);
            if (storageLocation == null)
                return RedirectToAction("Index");

            return base.View(_mapper.Map<Models.Trbovlje.StorageLocationViewModel>(storageLocation));
        }

        [HttpPost]
        public IActionResult StorageLocation([FromForm] Models.Trbovlje.StorageLocationViewModel storageLocation)
        {
            if (!ModelState.IsValid)
                return View(storageLocation);

            var dbStorageLocation = _dbContext.StorageLocations.FirstOrDefault(u => u.Id == storageLocation.Id);
            if (dbStorageLocation == null)
                return RedirectToAction("Index");

            _mapper.Map(storageLocation, dbStorageLocation);
            _dbContext.StorageLocations.Update(dbStorageLocation);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add([FromForm] Models.Trbovlje.StorageLocationViewModel storageLocation)
        {
            if (!ModelState.IsValid)
                return View(storageLocation);

            StorageLocation dbStorageLocation = new StorageLocation();
            
            _mapper.Map(storageLocation, dbStorageLocation);
            _dbContext.StorageLocations.Add(dbStorageLocation);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var dbStorageLocation = _dbContext.StorageLocations.FirstOrDefault(l => l.Id == id);
            if (dbStorageLocation == null)
                RedirectToAction("Index");

            return View(_mapper.Map<StorageLocationViewModel>(dbStorageLocation));
        }

        [HttpPost]
        public IActionResult Remove([FromForm] StorageLocationViewModel viewModel)
        {
            var dbStorageLocation = _dbContext.StorageLocations.FirstOrDefault(l => l.Id == viewModel.Id);
            if (dbStorageLocation != null)
            {
                _dbContext.StorageLocations.Remove(dbStorageLocation);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
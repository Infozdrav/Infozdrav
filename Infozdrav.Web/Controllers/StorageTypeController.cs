using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Infozdrav.Web.Data;
using Infozdrav.Web.Models.Manage;
using Infozdrav.Web.Models.Trbovlje;
using Infozdrav.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Infozdrav.Web.Controllers
{
    public class StorageTypeController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public StorageTypeController(AppDbContext dbContext, IMapper mapper, UserService userService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            ViewBag.DataSource = _mapper.Map<ICollection<Models.Trbovlje.StorageTypeViewModel>>(_dbContext.StorageTypes);

            return View();
        }

        public IActionResult StorageType(int id)
        {
            var storageType = _dbContext.StorageTypes.FirstOrDefault(u => u.Id == id);
            if (storageType == null)
                return RedirectToAction("Index");

            return base.View(_mapper.Map<Models.Trbovlje.StorageTypeViewModel>(storageType));
        }

        [HttpPost]
        public IActionResult StorageType([FromForm] Models.Trbovlje.StorageTypeViewModel storageType)
        {
            if (!ModelState.IsValid)
                return View(storageType);

            var dbStorageType = _dbContext.StorageTypes.FirstOrDefault(u => u.Id == storageType.Id);
            if (storageType == null)
                return RedirectToAction("Index");

            _mapper.Map(storageType, dbStorageType);
            _dbContext.StorageTypes.Update(dbStorageType);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add([FromForm] Models.Trbovlje.StorageTypeViewModel storageType)
        {
            if (!ModelState.IsValid)
                return View(storageType);

            StorageType dbStorageType = new StorageType();
            
            _mapper.Map(storageType, dbStorageType);
            _dbContext.StorageTypes.Add(dbStorageType);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var dbStorageType = _dbContext.StorageTypes.FirstOrDefault(l => l.Id == id);
            if (dbStorageType == null)
                RedirectToAction("Index");

            return View(_mapper.Map<StorageTypeViewModel>(dbStorageType));
        }

        [HttpPost]
        public IActionResult Remove([FromForm] StorageTypeViewModel viewModel)
        {
            var dbStorageType = _dbContext.StorageTypes.FirstOrDefault(l => l.Id == viewModel.Id);
            if (dbStorageType != null)
            {
                _dbContext.StorageTypes.Remove(dbStorageType);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
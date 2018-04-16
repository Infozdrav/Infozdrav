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
    public class SupplierController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public SupplierController(AppDbContext dbContext, IMapper mapper, UserService userService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            ViewBag.DataSource = _mapper.Map<ICollection<Models.Trbovlje.SupplierViewModel>>(_dbContext.Suppliers);

            return View();
        }

        public IActionResult Supplier(int id)
        {
            var supplier = _dbContext.Suppliers.FirstOrDefault(u => u.Id == id);
            if (supplier == null)
                return RedirectToAction("Index");

            return base.View(_mapper.Map<Models.Trbovlje.SupplierViewModel>(supplier));
        }

        [HttpPost]
        public IActionResult Supplier([FromForm] Models.Trbovlje.SupplierViewModel supplier)
        {
            if (!ModelState.IsValid)
                return View(supplier);

            var dbSupplier = _dbContext.Suppliers.FirstOrDefault(u => u.Id == supplier.Id);
            if (supplier == null)
                return RedirectToAction("Index");

            _mapper.Map(supplier, dbSupplier);
            _dbContext.Suppliers.Update(dbSupplier);
            _dbContext.SaveChanges();

            return View(supplier);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add([FromForm] Models.Trbovlje.SupplierViewModel supplier)
        {
            if (!ModelState.IsValid)
                return View(supplier);

            Supplier dbSupplier = new Supplier();

            _mapper.Map(supplier, dbSupplier);
            _dbContext.Suppliers.Add(dbSupplier);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var dbSupplier = _dbContext.Suppliers.FirstOrDefault(l => l.Id == id);
            if (dbSupplier == null)
                RedirectToAction("Index");

            return View(_mapper.Map<SupplierViewModel>(dbSupplier));
        }

        [HttpPost]
        public IActionResult Remove([FromForm] SupplierViewModel viewModel)
        {
            var dbSupplier = _dbContext.Suppliers.FirstOrDefault(l => l.Id == viewModel.Id);
            if (dbSupplier != null)
            {
                _dbContext.Suppliers.Remove(dbSupplier);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }


    }
}
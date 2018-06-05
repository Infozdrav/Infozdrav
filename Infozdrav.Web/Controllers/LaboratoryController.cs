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
    public class LaboratoryController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public LaboratoryController(AppDbContext dbContext, IMapper mapper, UserService userService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            ViewBag.DataSource = _mapper.Map<ICollection<Models.Trbovlje.LaboratoryViewModel>>(_dbContext.Laboratories);

            return View();
        }

        public IActionResult Laboratory(int id)
        {
            var analayser = _dbContext.Laboratories.FirstOrDefault(u => u.Id == id);
            if (analayser == null)
                return RedirectToAction("Index");

            return base.View(_mapper.Map<Models.Trbovlje.LaboratoryViewModel>(analayser));
        }

        [HttpPost]
        public IActionResult Laboratory([FromForm] Models.Trbovlje.LaboratoryViewModel analayser)
        {
            if (!ModelState.IsValid)
                return View(analayser);

            var dbLaboratory = _dbContext.Laboratories.FirstOrDefault(u => u.Id == analayser.Id);
            if (dbLaboratory == null)
                return RedirectToAction("Index");

            _mapper.Map(analayser, dbLaboratory);
            _dbContext.Laboratories.Update(dbLaboratory);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add([FromForm] Models.Trbovlje.LaboratoryViewModel analayser)
        {
            if (!ModelState.IsValid)
                return View(analayser);

            Laboratory dbLaboratory = new Laboratory();
            
            _mapper.Map(analayser, dbLaboratory);
            _dbContext.Laboratories.Add(dbLaboratory);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var dbLaboratory = _dbContext.Laboratories.FirstOrDefault(l => l.Id == id);
            if (dbLaboratory == null)
                RedirectToAction("Index");

            return View(_mapper.Map<LaboratoryViewModel>(dbLaboratory));
        }

        [HttpPost]
        public IActionResult Remove([FromForm] LaboratoryViewModel viewModel)
        {
            var dbLaboratory = _dbContext.Laboratories.FirstOrDefault(l => l.Id == viewModel.Id);
            if (dbLaboratory != null)
            {
                _dbContext.Laboratories.Remove(dbLaboratory);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
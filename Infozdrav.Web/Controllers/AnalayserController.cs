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
    public class AnalyserController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public AnalyserController(AppDbContext dbContext, IMapper mapper, UserService userService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            ViewBag.DataSource = _mapper.Map<ICollection<Models.Trbovlje.AnalyserViewModel>>(_dbContext.Analysers);

            return View();
        }

        public IActionResult Analyser(int id)
        {
            var analayser = _dbContext.Analysers.FirstOrDefault(u => u.Id == id);
            if (analayser == null)
                return RedirectToAction("Index");

            return base.View(_mapper.Map<Models.Trbovlje.AnalyserViewModel>(analayser));
        }

        [HttpPost]
        public IActionResult Analyser([FromForm] Models.Trbovlje.AnalyserViewModel analayser)
        {
            if (!ModelState.IsValid)
                return View(analayser);

            var dbAnalyser = _dbContext.Analysers.FirstOrDefault(u => u.Id == analayser.Id);
            if (dbAnalyser == null)
                return RedirectToAction("Index");

            _mapper.Map(analayser, dbAnalyser);
            _dbContext.Analysers.Update(dbAnalyser);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add([FromForm] Models.Trbovlje.AnalyserViewModel analayser)
        {
            if (!ModelState.IsValid)
                return View(analayser);

            Analyser dbAnalyser = new Analyser();
            
            _mapper.Map(analayser, dbAnalyser);
            _dbContext.Analysers.Add(dbAnalyser);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var dbAnalyser = _dbContext.Analysers.FirstOrDefault(l => l.Id == id);
            if (dbAnalyser == null)
                RedirectToAction("Index");

            return View(_mapper.Map<AnalyserViewModel>(dbAnalyser));
        }

        [HttpPost]
        public IActionResult Remove([FromForm] AnalyserViewModel viewModel)
        {
            var dbAnalyser = _dbContext.Analysers.FirstOrDefault(l => l.Id == viewModel.Id);
            if (dbAnalyser != null)
            {
                _dbContext.Analysers.Remove(dbAnalyser);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
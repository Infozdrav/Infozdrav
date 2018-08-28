using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Infozdrav.Web.Data;
using Infozdrav.Web.Data.Manage;
using Infozdrav.Web.Data.Trbovlje;
using Infozdrav.Web.Models.Trbovlje;
using Infozdrav.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Infozdrav.Web.Controllers
{
    [Authorize(Roles = Roles.Administrator + "," + Roles.BufferCreate)]
    public class BufferController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public BufferController(AppDbContext dbContext, IMapper mapper, UserService userService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            ViewBag.DataSource = _mapper.Map<ICollection<BufferViewModel>>(_dbContext.Buffers);

            var data = _dbContext.Buffers
                .Include(a => a.Article)
                .Include(b => b.StorageType)
                .Include(c => c.StorageLocation)
                .Include(d => d.WorkLocation)
                .Include(e => e.Analyser)
                .ToList();

            return View(_mapper.Map<List<Models.Trbovlje.BufferViewModel>>(data));
        }

        public IActionResult Buffer(int id)
        {
            var buffer = _dbContext.Buffers.FirstOrDefault(u => u.Id == id);
            if (buffer == null)
                return RedirectToAction("Index");

            return base.View(_mapper.Map<BufferViewModel>(buffer));
        }

        private IEnumerable<SelectListItem> GetArticles()
        {
            return new SelectList(_dbContext.Articles, "Id", "Lot");
        }

        private IEnumerable<SelectListItem> GetStorageTypes()
        {
            return new SelectList(_dbContext.StorageTypes, "Id", "Name");
        }

        private IEnumerable<SelectListItem> GetStorageLocations()
        {
            return new SelectList(_dbContext.StorageLocations, "Id", "Name");
        }

        private IEnumerable<SelectListItem> GetWorkLocations()
        {
            return new SelectList(_dbContext.WorkLocations, "Id", "Name");
        }

        private IEnumerable<SelectListItem> GetAnalysers()
        {
            return new SelectList(_dbContext.Analysers, "Id", "Name");
        }

        private BufferViewModel GetBufferViewModel()
        {
            return new BufferViewModel
            {
                Articles = GetArticles(), 
                StorageTypes = GetStorageTypes(),
                StorageLocations = GetStorageLocations(),
                WorkLocations = GetWorkLocations(),
                Analysers = GetAnalysers()
            };
        }

       /*public IActionResult Buffer()
       {
          return View(GetBufferViewModel());
       }*/

        [HttpPost]
        public IActionResult Buffer([FromForm] BufferViewModel buffer)
        {
            if (!ModelState.IsValid)
            {
                return View(buffer);
            }

            var dbBuffer = _dbContext.Buffers.FirstOrDefault(a => a.Id == buffer.Id);
            if (dbBuffer == null)
                return RedirectToAction("Index");

            _mapper.Map(buffer, dbBuffer);
            _dbContext.Buffers.Update(dbBuffer);
            _dbContext.SaveChanges();

            return View(buffer);
        }

        public IActionResult Add()
        {
            return View(GetBufferViewModel());
        }

        [HttpPost]
        public IActionResult Add([FromForm] BufferViewModel buffer)
        {
            if (!ModelState.IsValid)
                return View(buffer);

            Buffer dbBuffer = new Buffer();

            _mapper.Map(buffer, dbBuffer);
            _dbContext.Buffers.Add(dbBuffer);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var dbBuffer = _dbContext.Buffers.FirstOrDefault(l => l.Id == id);
            if (dbBuffer == null)
                RedirectToAction("Index");

            return View(_mapper.Map<BufferViewModel>(dbBuffer));
        }

        [HttpPost]
        public IActionResult Remove([FromForm] BufferViewModel viewModel)
        {
            var dbBuffer = _dbContext.Buffers.FirstOrDefault(l => l.Id == viewModel.Id);
            if (dbBuffer != null)
            {
                _dbContext.Buffers.Remove(dbBuffer);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }


    }
}
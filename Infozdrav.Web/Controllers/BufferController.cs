using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Infozdrav.Web.Data;
using Infozdrav.Web.Models.Manage;
using Infozdrav.Web.Models.Trbovlje;
using Infozdrav.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Infozdrav.Web.Controllers
{
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
            var data = _dbContext.Buffers
                .Include(s => s.WorkLocation)
                .ToList();

            return View(_mapper.Map<List<BufferViewModel>>(data));
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
            return new SelectList(_dbContext.Articles, "Id", "Name");
        }

        private BufferViewModel GetBufferViewModel()
        {
            return new BufferViewModel
            {
                Articles = GetArticles()
            };
        }

        public IActionResult Buffer()
        {
            return View(GetBufferViewModel());
        }

        [HttpPost]
        public IActionResult Buffer([FromForm] BufferViewModel buffer)
        {
            if (!ModelState.IsValid)
                return View(buffer);

            var dbBuffer = _dbContext.Buffers.FirstOrDefault(u => u.Id == buffer.Id);
            if (buffer == null)
                return RedirectToAction("Index");

            _mapper.Map(buffer, dbBuffer);
            _dbContext.Buffers.Update(dbBuffer);
            _dbContext.SaveChanges();

            return View(buffer);
        }

        public IActionResult Add()
        {
            return View();
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
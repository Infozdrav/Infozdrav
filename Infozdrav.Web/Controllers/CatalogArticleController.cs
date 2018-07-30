using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Infozdrav.Web.Data;
using Infozdrav.Web.Data.Trbovlje;
using Infozdrav.Web.Models.Trbovlje;
using Infozdrav.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Infozdrav.Web.Controllers
{
    public class CatalogArticleController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public CatalogArticleController(AppDbContext dbContext, IMapper mapper, UserService userService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            ViewBag.DataSource = _mapper.Map<ICollection<CatalogArticleViewModel>>(_dbContext.CatalogArticles);

            var data = _dbContext.CatalogArticles
                .Include(s => s.Manufacturer)
                .Include(p => p.Supplier)
                .ToList();

            return View(_mapper.Map<List<Models.Trbovlje.CatalogArticleViewModel>>(data.ToList()));
        }

        public IActionResult CatalogArticle(int id)
        {
            var catalogArticle = _dbContext.CatalogArticles.FirstOrDefault(u => u.Id == id);
            if (catalogArticle == null)
                return RedirectToAction("Index");

            return base.View(_mapper.Map<Models.Trbovlje.CatalogArticleViewModel>(catalogArticle));
        }

        private IEnumerable<SelectListItem> GetManufacturers()
        {
            return new SelectList(_dbContext.Manufacturers, "Id", "Name");
        }

        private IEnumerable<SelectListItem> GetSuppliers()
        {
            return new SelectList(_dbContext.Suppliers, "Id", "Name");
        }

        private CatalogArticleViewModel GetCatalogArticleViewModel()
        {
            return new CatalogArticleViewModel
            {
                Manufacturers = GetManufacturers(),
                Suppliers = GetSuppliers()
            };
        }

        [HttpPost]
        public IActionResult CatalogArticle([FromForm] Models.Trbovlje.CatalogArticleViewModel catalogArticle)
        {
            if (!ModelState.IsValid)
                return View(catalogArticle);

            var dbCatalogArticle = _dbContext.CatalogArticles.FirstOrDefault(u => u.Id == catalogArticle.Id);
            if (dbCatalogArticle == null)
                return RedirectToAction("Index");

            _mapper.Map(catalogArticle, dbCatalogArticle);
            _dbContext.CatalogArticles.Update(dbCatalogArticle);
            _dbContext.SaveChanges();

            return View(catalogArticle);
        }

        public IActionResult Add()
        {
            return View(GetCatalogArticleViewModel());
        }

        [HttpPost]
        public IActionResult Add([FromForm] Models.Trbovlje.CatalogArticleViewModel catalogArticle)
        {
            if (!ModelState.IsValid)
                return View(catalogArticle);

            CatalogArticle dbCatalogArticle = new CatalogArticle();

            _mapper.Map(catalogArticle, dbCatalogArticle);
            _dbContext.CatalogArticles.Add(dbCatalogArticle);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var dbCatalogArticle = _dbContext.CatalogArticles.FirstOrDefault(l => l.Id == id);
            if (dbCatalogArticle == null)
                RedirectToAction("Index");

            return View(_mapper.Map<CatalogArticleViewModel>(dbCatalogArticle));
        }

        [HttpPost]
        public IActionResult Remove([FromForm] CatalogArticleViewModel viewModel)
        {
            var dbCatalogArticle = _dbContext.CatalogArticles.FirstOrDefault(l => l.Id == viewModel.Id);
            if (dbCatalogArticle != null)
            {
                _dbContext.CatalogArticles.Remove(dbCatalogArticle);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }


    }
}
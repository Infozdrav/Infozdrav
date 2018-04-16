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
            ViewBag.DataSource = _mapper.Map<ICollection<Models.Trbovlje.CatalogArticleViewModel>>(_dbContext.CatalogArticles);

            return View();
        }

        public IActionResult CatalogArticle(int id)
        {
            var catalogArticle = _dbContext.CatalogArticles.FirstOrDefault(u => u.Id == id);
            if (catalogArticle == null)
                return RedirectToAction("Index");

            return base.View(_mapper.Map<Models.Trbovlje.CatalogArticleViewModel>(catalogArticle));
        }

        [HttpPost]
        public IActionResult CatalogArticle([FromForm] Models.Trbovlje.CatalogArticleViewModel catalogArticle)
        {
            if (!ModelState.IsValid)
                return View(catalogArticle);

            var dbCatalogArticle = _dbContext.CatalogArticles.FirstOrDefault(u => u.Id == catalogArticle.Id);
            if (catalogArticle == null)
                return RedirectToAction("Index");

            _mapper.Map(catalogArticle, dbCatalogArticle);
            _dbContext.CatalogArticles.Update(dbCatalogArticle);
            _dbContext.SaveChanges();

            return View(catalogArticle);
        }

        public IActionResult Add()
        {
            return View();
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
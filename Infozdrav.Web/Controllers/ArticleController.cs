using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Google.Protobuf.Collections;
using Infozdrav.Web.Data;
using Infozdrav.Web.Models.Manage;
using Infozdrav.Web.Models.Trbovlje;
using Infozdrav.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Infozdrav.Web.Controllers
{
    public class ArticleController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public ArticleController(AppDbContext dbContext, IMapper mapper, UserService userService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var data = _dbContext.Articles.Include(s => s.WorkLocation).ToList();

            return View(
                _mapper.Map<List<Models.Trbovlje.ArticleFullViewModel>>(
                    _dbContext.Articles.Include(s => s.WorkLocation)));
        }

        public IActionResult Article(int id)
        {
            var article = _dbContext.Articles.FirstOrDefault(u => u.Id == id);
            if (article == null)
                return RedirectToAction("Index");

            return base.View(_mapper.Map<Models.Trbovlje.ArticleFullViewModel>(article));
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

        private IEnumerable<SelectListItem> GetCatalogArticles()
        {
            return new SelectList(_dbContext.CatalogArticles, "Id", "CatalogNumber");
        }

        private ArticleReceptionViewModel GetReceptionViewModel()
        {
            return new ArticleReceptionViewModel
            {
                StorageTypes = GetStorageTypes(),
                StorageLocations = GetStorageLocations(),
                WorkLocations = GetWorkLocations(),
                Analysers = GetAnalysers(),
                CatalogArticles = GetCatalogArticles()
            };
        }

        public IActionResult Reception()
        {
            return View(GetReceptionViewModel());
        }

        [HttpPost]
        public IActionResult Reception([FromForm] Models.Trbovlje.ArticleReceptionViewModel article,
            bool repeat = false)
        {
            if (article == null)
            {
                return RedirectToAction("Index");
            }

            // TODO: Set user and stuff

            if (article.Rejected)
            {
                if (ModelState.GetFieldValidationState("CatalogArticleId") == ModelValidationState.Invalid ||
                    ModelState.GetFieldValidationState("Lot") == ModelValidationState.Invalid ||
                    ModelState.GetFieldValidationState("NumberOfUnits") == ModelValidationState.Invalid )
                {
                    return View(_mapper.Map(GetReceptionViewModel(), article));
                }

                Article dbArticle = new Article
                {
                    CatalogArticleId = article.CatalogArticleId,
                    Lot = article.Lot,
                    NumberOfUnits = article.NumberOfUnits,
                    Rejected = true,
                };
                _dbContext.Articles.Add(dbArticle);
                _dbContext.SaveChanges();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(_mapper.Map(GetReceptionViewModel(), article));
                }

                // TODO: Iz kataloga dobiti koliko dni mora biti artikle vsaj uporaben oz. minimanel rok uporab

                if (article.UseByDate <= DateTime.Today)
                {
                    if (!article.Rejected)
                    {
                        ModelState.AddModelError("UseByDate",
                            "Izdelek ni več uporabne oz. ni v skladu s pogodbo. Prosim te, da izdelek zavrneš.");
                    }
                }

                if (!article.IgnoreBadLot && _dbContext.Articles.Count(queryArticle =>
                        queryArticle.Rejected && queryArticle.Lot == article.Lot) > 0)
                {
                    ModelState.AddModelError("Lot",
                        "Izdelek s tem lotom je že bil zavrnjen. Prosim te, da izdelek zavrneš.");
                    article.ShowIgnoreBadLot = true;
                }

                if (!ModelState.IsValid)
                {
                    return View(_mapper.Map(GetReceptionViewModel(), article));
                }

                Article dbArticle = new Article();
                _mapper.Map(article, dbArticle);
                _dbContext.Articles.Add(dbArticle);
                _dbContext.SaveChanges();

                if (repeat)
                {
                    ModelState.Clear();
                    return View(GetReceptionViewModel());
                }
            }
            return RedirectToAction("Index");
        }

        //[HttpPost]
        //public IActionResult Article([FromForm] Models.Trbovlje.ArticleFullViewModel article)
        //{
        //    if (!ModelState.IsValid)
        //        return View(article);

        //    var dbArticle = _dbContext.Articles.FirstOrDefault(u => u.Id == article.Id);
        //    if (article == null)
        //        return RedirectToAction("Index");

        //    _mapper.Map(article, dbArticle);
        //    _dbContext.Articles.Update(dbArticle);
        //    _dbContext.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //public IActionResult Add()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult Add([FromForm] Models.Trbovlje.ArticleViewModel article)
        //{
        //    if (!ModelState.IsValid)
        //        return View(article);

        //    Article dbArticle = new Article();

        //    _mapper.Map(article, dbArticle);
        //    _dbContext.Articles.Add(dbArticle);
        //    _dbContext.SaveChanges();

        //    return RedirectToAction("Index");
        //}

        //public IActionResult Remove(int id)
        //{
        //    var dbArticle = _dbContext.Articles.FirstOrDefault(l => l.Id == id);
        //    if (dbArticle == null)
        //        RedirectToAction("Index");

        //    return View(_mapper.Map<ArticleViewModel>(dbArticle));
        //}

        //[HttpPost]
        //public IActionResult Remove([FromForm] ArticleViewModel viewModel)
        //{
        //    var dbArticle = _dbContext.Articles.FirstOrDefault(l => l.Id == viewModel.Id);
        //    if (dbArticle != null)
        //    {
        //        _dbContext.Articles.Remove(dbArticle);
        //        _dbContext.SaveChanges();
        //    }
        //    return RedirectToAction("Index");
        //}
    }
}
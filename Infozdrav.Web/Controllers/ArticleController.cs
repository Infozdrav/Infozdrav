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
            var data = _dbContext.Articles
                .Where(a => !a.Rejected && a.WriteOffReason == null)
                .Include(s => s.WorkLocation)
                .ToList();

            return View(
                _mapper.Map<List<Models.Trbovlje.ArticleFullViewModel>>(data));
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
                    ReceptionTime = DateTime.Now,
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

                // TODO: File upload...
                Article dbArticle = _mapper.Map<Article>(article);
                dbArticle.ReceptionTime = DateTime.Now;
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

        public IActionResult UseArticle(int id) // article id
        {
            var article = _dbContext.Articles.Include(a => a.CatalogArticle).FirstOrDefault(a => a.Id == id);
            if (article == null)
            {
                var data = _dbContext.Articles
                    .Where(a => !a.Rejected && a.WriteOffReason == null)
                    .Include(s => s.WorkLocation)
                    .ToList();
                // TODO

                ViewData["Title"] = "Uporaba artikla";
                return View("Table", _mapper.Map<List<Models.Trbovlje.ArticleFullViewModel>>(data));
            }

            return View(new ArticleUseViewModel
            {
                Article = article,
                ArticleId = article.Id
            });
        }

        [HttpPost]
        public IActionResult UseArticle([FromForm]  ArticleUseViewModel articleUse)
        {
            if (articleUse.NumberOfUnits < 1)
            {
                ModelState.AddModelError("NumberOfUnits", "Število uporabljenih enot mora biti več kot 0");
            }

            var article = _dbContext.Articles.FirstOrDefault(a => a.Id == articleUse.ArticleId);
            if (article == null)
            {
                return RedirectToAction("Index");
            }

            var articleUses = _dbContext.ArticleUses.Where(use => use.ArticleId == articleUse.ArticleId).Select(use => use.NumberOfUnits).Sum();
            var articlesLeft = article.NumberOfUnits - articleUses;
            if (articlesLeft - articleUses < 0)
            {
                ModelState.AddModelError("NumberOfUnits", "Na voljo je samo še " + articlesLeft + " enot");
            }

            if (!ModelState.IsValid)
            {
                return View(articleUse);
            }

            // TODO: set user
            ArticleUse dbArticleUse = _mapper.Map<ArticleUse>(articleUse);
            dbArticleUse.UseTime = DateTime.Now;

            _dbContext.ArticleUses.Add(dbArticleUse);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult WriteOff(int id)
        {
            var article = _dbContext.Articles.Include( a => a.CatalogArticle ).FirstOrDefault(a => a.Id == id);

            if (article == null)
            {
                var data = _dbContext.Articles
                    .Where(a => !a.Rejected && a.WriteOffReason == null)
                    .Include(s => s.WorkLocation)
                    .ToList();
                // TODO

                ViewData["Title"] = "Odpis artikla";
                return View("Table", _mapper.Map<List<Models.Trbovlje.ArticleFullViewModel>>(data));
            }

            return View(_mapper.Map<ArticleWriteOffViewModel>(article));
        }

        [HttpPost]
        public IActionResult WriteOff([FromForm] ArticleWriteOffViewModel article)
        {
            if (article == null)
            {
                return RedirectToAction("Index");
            }

            if (article.WriteOffReason == WriteOffReason.Other && (String.IsNullOrEmpty(article.WriteOffNote) || String.IsNullOrWhiteSpace(article.WriteOffNote)))
                ModelState.AddModelError("WriteOffNote", "Opomba je potrebna");

            if (!ModelState.IsValid)
            {
                return View(article);
            }

            // TODO: Set user and stuff

            var dbArticle = _dbContext.Articles.Include(a => a.CatalogArticle).FirstOrDefault(a => a.Id == article.Id);

            if (dbArticle == null)
            {
                return RedirectToAction("Index");
            }

            dbArticle.WriteOffReason = article.WriteOffReason;
            dbArticle.WriteOffNote = article.WriteOffNote;
            dbArticle.WriteOffTime = DateTime.Now;

            _dbContext.Articles.Update(dbArticle);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var dbArticle = _dbContext.Articles.FirstOrDefault(l => l.Id == id);
            if (dbArticle == null)
                RedirectToAction("Index");

            return View(_mapper.Map<ArticleFullViewModel>(dbArticle));
        }

        [HttpPost]
        public IActionResult Remove([FromForm] ArticleFullViewModel viewModel)
        {
            var dbArticle = _dbContext.Articles.FirstOrDefault(l => l.Id == viewModel.Id);
            if (dbArticle != null)
            {
                _dbContext.Articles.Remove(dbArticle);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
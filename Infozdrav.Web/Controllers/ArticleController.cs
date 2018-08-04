using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Infozdrav.Web.Data;
using Infozdrav.Web.Data.Manage;
using Infozdrav.Web.Data.Trbovlje;
using Infozdrav.Web.Helpers;
using Infozdrav.Web.Models.Trbovlje;
using Infozdrav.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Infozdrav.Web.Controllers
{
    [Authorize]
    public class ArticleController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly FileService _fileService;

        public ArticleController(AppDbContext dbContext, IMapper mapper, UserService userService,
            UserManager<User> userManager, FileService fileService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mapper = mapper;
            _fileService = fileService;
        }


        [Authorize(Roles = Roles.Administrator + "," + Roles.StockView)]
        public IActionResult Index()
        {
            var data = _dbContext.Articles
                .Include(s => s.CatalogArticle)
                .Include(s => s.ArticleUses)
                .Include(s => s.WorkLocation)
                .Include(s => s.StorageType)
                .Include(s => s.StorageLocation)
                .Include(s => s.WorkLocation)
                .Include(s => s.Analyser)
                .Include(s => s.Certificate)
                .Include(s => s.SafteyList)
                .ToList();

            return View(_mapper.Map<List<Models.Trbovlje.ArticleFullViewModel>>(data));
        }

        [Authorize(Roles = Roles.Administrator + "," + Roles.ArticleView)]
        public IActionResult Article(int id)
        {
            var article = _dbContext.Articles
                .Include(s => s.CatalogArticle)
                .Include(s => s.ArticleUses)
                .Include(s => s.WorkLocation)
                .Include(s => s.StorageType)
                .Include(s => s.StorageLocation)
                .Include(s => s.WorkLocation)
                .Include(s => s.Analyser)
                .Include(s => s.Certificate)
                .Include(s => s.SafteyList)
                .Include(s => s.WriteOfUser)
                .Include(s => s.ReceptionUser)
                .Include(s => s.Lends)
                .FirstOrDefault(u => u.Id == id);

            if (article == null)
                return RedirectToAction("Index");

            return base.View(_mapper.Map<Models.Trbovlje.ArticleFullViewModel>(article));
        }

        [Authorize(Roles = Roles.Administrator + "," + Roles.ArticleReception)]
        public IActionResult Reception()
        {
            return View(GetReceptionViewModel());
        }

        [Authorize(Roles = Roles.Administrator + "," + Roles.ArticleReception)]
        [HttpPost]
        public async Task<IActionResult> Reception([FromForm] Models.Trbovlje.ArticleReceptionViewModel article,
            bool repeat = false)
        {
            if (article == null)
            {
                return RedirectToAction("Index");
            }

            if (article.Rejected)
            {
                if (ModelState.GetFieldValidationState("CatalogArticleId") == ModelValidationState.Invalid ||
                    ModelState.GetFieldValidationState("Lot") == ModelValidationState.Invalid ||
                    ModelState.GetFieldValidationState("NumberOfUnits") == ModelValidationState.Invalid)
                {
                    return View(_mapper.Map(GetReceptionViewModel(), article));
                }

                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                Article dbArticle = new Article
                {
                    CatalogArticleId = article.CatalogArticleId,
                    Lot = article.Lot,
                    NumberOfUnits = article.NumberOfUnits,

                    ReceptionTime = DateTime.Now,
                    ReceptionUser = user,

                    Rejected = true,

                    WriteOfUser = user,
                    WriteOffTime = DateTime.Now,
                    WriteOffReason = WriteOffReason.Rejected,
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

                var catalogArticle =
                    _dbContext.CatalogArticles.FirstOrDefault(c => c.Id == article.CatalogArticleId);
                if (article.UseByDate <= DateTime.Today 
                    || (catalogArticle != null && ((DateTime)article.UseByDate - DateTime.Today).TotalDays >= catalogArticle.UseByDaysLimit))
                {
                    if (!article.Rejected)
                    {
                        ModelState.AddModelError("UseByDate",
                            "Izdelek ni več uporabne oz. ni v skladu s pogodbo. Prosim te, da izdelek zavrneš.");
                    }
                }

                if (!article.IgnoreBadLot &&
                    _dbContext.Articles.Count(queryArticle =>
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

                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                Article dbArticle = _mapper.Map<Article>(article);
                dbArticle.ReceptionTime = DateTime.Now;
                dbArticle.ReceptionUser = user;
                dbArticle.Certificate = _fileService.SaveFile(article.Certificate);
                dbArticle.SafteyList = _fileService.SaveFile(article.SafteyList);
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

        [Authorize(Roles = Roles.Administrator + "," + Roles.ArticleUse)]
        public IActionResult UseArticleTable()
        {
            return GetTableView("Uporaba artikla", "UseArticle", "Uporaba",
                a => !a.Rejected && a.WriteOffReason == null && a.NumberOfUnits - (a.ArticleUses.Count() + a.Lends.Sum( lend => lend.UnitsUsed)) > 0 && !(a.Lends.Any(lend => lend.LendReciveTime == null)) );
        }

        [Authorize(Roles = Roles.Administrator + "," + Roles.ArticleUse)]
        public IActionResult UseArticle(int id) // article id
        {
            var article = _dbContext.Articles.Include(a => a.CatalogArticle).FirstOrDefault(a => a.Id == id);

            if (article == null)
            {
                return RedirectToAction("UseArticleTable");
            }

            return View(new ArticleUseViewModel
            {
                Article = article,
                ArticleId = article.Id
            });
        }

        [Authorize(Roles = Roles.Administrator + "," + Roles.ArticleUse)]
        [HttpPost]
        public async Task<IActionResult> UseArticle([FromForm] ArticleUseViewModel articleUse)
        {
            var article = _dbContext.Articles.Include(s => s.Lends).FirstOrDefault(a => a.Id == articleUse.ArticleId);
            if (article == null)
            {
                return RedirectToAction("UseArticleTable");
            }

            var articleUses = _dbContext.ArticleUses.Count(use => use.ArticleId == articleUse.ArticleId) + article.Lends.Sum(lend => lend.UnitsUsed);
            var articlesLeft = article.NumberOfUnits - articleUses;
            if (articlesLeft < 0)
            {
                return RedirectToAction("UseArticleTable");
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            ArticleUse dbArticleUse = _mapper.Map<ArticleUse>(articleUse);
            dbArticleUse.UseTime = DateTime.Now;
            dbArticleUse.User = user;
            if (articleUse.UnitNumber == 0)
            {
                var lastUse = _dbContext.ArticleUses.OrderByDescending(use => use.UseTime)
                    .LastOrDefault(use => use.ArticleId == articleUse.ArticleId);
                if (lastUse != null)
                {
                    dbArticleUse.UnitNumber = lastUse.UnitNumber + 1;
                }
            }

            _dbContext.ArticleUses.Add(dbArticleUse);
            _dbContext.SaveChanges();

            return RedirectToAction("UseArticleTable");
        }

        [Authorize(Roles = Roles.Administrator + "," + Roles.ArticleWriteOff)]
        public IActionResult WriteOffTable()
        {
            return GetTableView("Odpis artikla", "WriteOff", "Odpis", 
                a => a.WriteOffReason == null);
        }

        [Authorize(Roles = Roles.Administrator + "," + Roles.ArticleWriteOff)]
        public IActionResult WriteOff(int id)
        {
            var article = _dbContext.Articles.Include(a => a.CatalogArticle).FirstOrDefault(a => a.Id == id);

            if (article == null)
            {
                RedirectToAction("WriteOffTable");
            }

            return View(_mapper.Map<ArticleWriteOffViewModel>(article));
        }

        [Authorize(Roles = Roles.Administrator + "," + Roles.ArticleWriteOff)]
        [HttpPost]
        public async Task<IActionResult> WriteOff([FromForm] ArticleWriteOffViewModel article)
        {
            if (article == null)
            {
                return RedirectToAction("WriteOffTable");
            }

            if (article.WriteOffReason == WriteOffReason.Other &&
                (String.IsNullOrEmpty(article.WriteOffNote) || String.IsNullOrWhiteSpace(article.WriteOffNote)))
                ModelState.AddModelError("WriteOffNote", "Opomba je potrebna");

            if (!ModelState.IsValid)
            {
                return View(article);
            }


            var dbArticle = _dbContext.Articles.Include(a => a.CatalogArticle).FirstOrDefault(a => a.Id == article.Id);

            if (dbArticle == null)
            {
                return RedirectToAction("WriteOffTable");
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            dbArticle.WriteOffReason = article.WriteOffReason;
            dbArticle.WriteOffNote = article.WriteOffNote;
            dbArticle.WriteOffTime = DateTime.Now;
            dbArticle.WriteOfUser = user;

            _dbContext.Articles.Update(dbArticle);
            _dbContext.SaveChanges();

            return RedirectToAction("WriteOffTable");
        }

        [Authorize(Roles = Roles.Administrator)]
        public IActionResult Remove(int id)
        {
            var dbArticle = _dbContext.Articles.FirstOrDefault(l => l.Id == id);
            if (dbArticle == null)
                RedirectToAction("Index");

            return View(_mapper.Map<ArticleFullViewModel>(dbArticle));
        }

        [Authorize(Roles = Roles.Administrator)]
        [HttpPost]
        public IActionResult Remove([FromForm] ArticleFullViewModel viewModel)
        {
            var dbArticle = _dbContext.Articles.FirstOrDefault(l => l.Id == viewModel.Id);
            if (dbArticle != null)
            {
                _fileService.DeleteFile(dbArticle.Certificate);
                _fileService.DeleteFile(dbArticle.SafteyList);
                _dbContext.Articles.Remove(dbArticle);
                _dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = Roles.Administrator)]
        public IActionResult Edit(int id)
        {
            var dbArticle = _dbContext.Articles
                .Include(s => s.Certificate)
                .Include(s => s.SafteyList)
                .FirstOrDefault(l => l.Id == id);
            if (dbArticle == null)
                RedirectToAction("Index");

            var viewModel = new ArticleEditViewModel
            {
                StorageTypes = GetStorageTypes(),
                StorageLocations = GetStorageLocations(),
                WorkLocations = GetWorkLocations(),
                Analysers = GetAnalysers(),
                CatalogArticles = GetCatalogArticles()
            };

            return View(_mapper.Map(dbArticle, viewModel));
        }

        [Authorize(Roles = Roles.Administrator)]
        [HttpPost]
        public IActionResult Edit([FromForm] ArticleEditViewModel viewModel)
        {
            var dbArticle = _dbContext.Articles.FirstOrDefault(l => l.Id == viewModel.Id);
            if (dbArticle == null)
                RedirectToAction("Index");

            if (!ModelState.IsValid)
                return View(viewModel);

            _mapper.Map(viewModel, dbArticle);

            if (viewModel.CertificateUpload != null)
            {
                _fileService.DeleteFile(dbArticle.Certificate);
                dbArticle.Certificate = _fileService.SaveFile(viewModel.CertificateUpload);
            }

            if (viewModel.SafteyListUpload != null)
            {
                _fileService.DeleteFile(dbArticle.SafteyList);
                dbArticle.SafteyList = _fileService.SaveFile(viewModel.SafteyListUpload);
            }

            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = Roles.Administrator + "," + Roles.ArticleLend)]
        public IActionResult LendGiveTable()
        {
            return GetTableView("Odprema artikla", "LendGive", "Odprema",
                a => !a.Rejected && a.WriteOffReason == null && a.NumberOfUnits - (a.ArticleUses.Count() + a.Lends.Sum(lend => lend.UnitsUsed)) > 0 && !(a.Lends.Any(lend => lend.LendReciveTime == null)));
        }

        [Authorize(Roles = Roles.Administrator + "," + Roles.ArticleLend)]
        public IActionResult LendGive(int id) // article id
        {
            var article = _dbContext.Articles.Include(a => a.CatalogArticle).FirstOrDefault(a => a.Id == id);

            if (article == null)
            {
                return RedirectToAction("LendGiveTable");
            }

            return View(new ArticleLendGiveViewModel
            {
                Id = article.Id,
                Laboratories = GetLaboratories()
            });
        }

        [Authorize(Roles = Roles.Administrator + "," + Roles.ArticleLend)]
        [HttpPost]
        public async Task<IActionResult> LendGive([FromForm] ArticleLendGiveViewModel articleLendGive)
        {
            var article = _dbContext.Articles.Include(s => s.Lends).FirstOrDefault(a => a.Id == articleLendGive.Id);
            if (article == null || articleLendGive.LaboratoryId == null || !ModelState.IsValid)
            {
                return RedirectToAction("LendGiveTable");
            }

            var articleUses = _dbContext.ArticleUses.Count(use => use.ArticleId == articleLendGive.Id) + article.Lends.Sum(lend => lend.UnitsUsed);
            var articlesLeft = article.NumberOfUnits - articleUses;
            if (articlesLeft < 0)
            {
                return RedirectToAction("LendGiveTable");
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            Lend dbarticleLendGive = new Lend();
            dbarticleLendGive.Article = article;
            dbarticleLendGive.LaboratoryId = (int) articleLendGive.LaboratoryId;
            dbarticleLendGive.LendGiveTime = DateTime.Now;
            dbarticleLendGive.LendGiveUser = user;

            _dbContext.Lends.Add(dbarticleLendGive);
            _dbContext.SaveChanges();

            return RedirectToAction("LendGiveTable");
        }

        [Authorize(Roles = Roles.Administrator + "," + Roles.ArticleLendRecive)]
        public IActionResult LendReciveTable()
        {
            return GetTableView("Re-odprema artikla", "LendRecive", "Re-odprema",
                a => !a.Rejected && a.WriteOffReason == null && (a.Lends.Any(lend => lend.LendReciveTime == null)));
        }

        [Authorize(Roles = Roles.Administrator + "," + Roles.ArticleLendRecive)]
        public IActionResult LendRecive(int id) // article id
        {
            var article = _dbContext.Articles.Include(a => a.CatalogArticle).FirstOrDefault(a => a.Id == id);

            if (article == null)
            {
                return RedirectToAction("LendReciveTable");
            }

            var lastLend = _dbContext.Lends.FirstOrDefault(l => l.Article == article && l.LendReciveTime == null);

            if (lastLend == null)
            {
                return RedirectToAction("LendReciveTable");
            }

            var articleUses = _dbContext.ArticleUses.Count(use => use.ArticleId == id) + article.Lends.Sum(l => l.UnitsUsed);
            var articlesLeft = article.NumberOfUnits - articleUses;

            return View(new ArticleLendReciveViewModel
            {
                LendId = lastLend.Id,
                ArticleId = article.Id,
                UnitsLeft = articlesLeft
            });
        }

        [Authorize(Roles = Roles.Administrator + "," + Roles.ArticleLendRecive)]
        [HttpPost]
        public async Task<IActionResult> LendRecive([FromForm] ArticleLendReciveViewModel lendRecive)
        {
            var article = _dbContext.Articles.Include(s => s.Lends).FirstOrDefault(a => a.Id == lendRecive.ArticleId);
            var lend = _dbContext.Lends.FirstOrDefault(a => a.Id == lendRecive.LendId);
            if (article == null || lend == null || !ModelState.IsValid)
            {
                return RedirectToAction("LendReciveTable");
            }

            var articleUses = _dbContext.ArticleUses.Count(use => use.ArticleId == lendRecive.ArticleId) + article.Lends.Sum(l => l.UnitsUsed);
            var articlesLeft = article.NumberOfUnits - articleUses;
            if (articlesLeft < 0)
            {
                return RedirectToAction("LendReciveTable");
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            lend.LendReciveTime = DateTime.Now;
            lend.LendReciveUser = user;
            lend.UnitsUsed = (int) lendRecive.UnitsUsed;
            _dbContext.SaveChanges();

            return RedirectToAction("LendReciveTable");
        }

        private IActionResult GetTableView(string title, string action, string actionName, Expression<Func<Article, bool>> predicate)
        {
            var data = _dbContext.Articles
                .Where(predicate)
                .Include(s => s.CatalogArticle)
                .Include(s => s.ArticleUses)
                .Include(s => s.WorkLocation)
                .Include(s => s.StorageType)
                .Include(s => s.StorageLocation)
                .Include(s => s.WorkLocation)
                .Include(s => s.Analyser)
                .Include(s => s.Certificate)
                .Include(s => s.SafteyList)
                .Include(s => s.Lends)
                .ToList();

            ViewData["Title"] = title;
            ViewData["Action"] = action;
            ViewData["ActionName"] = actionName;

            return View("Table", _mapper.Map<List<Models.Trbovlje.ArticleTableViewModel>>(data));
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

        private IEnumerable<SelectListItem> GetLaboratories()
        {
            return new SelectList(_dbContext.Laboratories, "Id", "Name");
        }

        private IEnumerable<SelectListItem> GetCatalogArticles()
        {
            // TOOO: Samo "aktiven" artikle
            return new SelectList(_dbContext.CatalogArticles, "Id", "Name");
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

    }
}
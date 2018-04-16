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
            ViewBag.DataSource = _mapper.Map<ICollection<Models.Trbovlje.ArticleFullViewModel>>(_dbContext.Articles);

            return View();
        }

        public IActionResult Article(int id)
        {
            var article = _dbContext.Articles.FirstOrDefault(u => u.Id == id);
            if (article == null)
                return RedirectToAction("Index");

            return base.View(_mapper.Map<Models.Trbovlje.ArticleFullViewModel>(article));
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
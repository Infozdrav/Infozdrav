using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Infozdrav.Web.Data;
using Infozdrav.Web.Data.Manage;
using Infozdrav.Web.Data.Trbovlje;
using Infozdrav.Web.Models.Manage;
using Infozdrav.Web.Models.Trbovlje;
using Infozdrav.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Infozdrav.Web.Controllers
{
    [Authorize(Roles = Roles.Administrator + "," + Roles.CatalogArticleOrder)]

    public class OrderCatalogArticleController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public OrderCatalogArticleController(AppDbContext dbContext, IMapper mapper, UserService userService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            ViewBag.DataSource = _mapper.Map<ICollection<OrderCatalogArticleViewModel>>(_dbContext.OrderCatalogArticles);

            var data = _dbContext.OrderCatalogArticles
                .Include(s => s.CatalogArticle)
                .ToList();

            return View(_mapper.Map<List<Models.Trbovlje.OrderCatalogArticleViewModel>>(data));
        }

        public IActionResult OrderCatalogArticle(int id)
        {
            var orderCatalogArticle = _dbContext.OrderCatalogArticles.FirstOrDefault(u => u.Id == id);
            if (orderCatalogArticle == null)
                return RedirectToAction("Index");

            return base.View(_mapper.Map<Models.Trbovlje.OrderCatalogArticleViewModel>(orderCatalogArticle));
        }

        private IEnumerable<SelectListItem> GetCatalogArticles()
        {
            return new SelectList(_dbContext.CatalogArticles, "Id", "Name");
        }

        private OrderCatalogArticleViewModel GetOrderCatalogArticleViewModel()
        {
            return new OrderCatalogArticleViewModel
            {
                CatalogArticles = GetCatalogArticles()
            };
        }

        public IActionResult OrderCatalogArticle()
        {
            return View(GetOrderCatalogArticleViewModel());
        }

        [HttpPost]
        public IActionResult OrderCatalogArticle([FromForm] Models.Trbovlje.OrderCatalogArticleViewModel orderCatalogArticle)
        {
            if (!ModelState.IsValid)
                return View(orderCatalogArticle);

            var dbOrderCatalogArticle = _dbContext.OrderCatalogArticles.FirstOrDefault(u => u.Id == orderCatalogArticle.Id);
            if (dbOrderCatalogArticle == null)
                return RedirectToAction("Index");

            _mapper.Map(orderCatalogArticle, dbOrderCatalogArticle);
            _dbContext.OrderCatalogArticles.Update(dbOrderCatalogArticle);
            _dbContext.SaveChanges();

            return View(orderCatalogArticle);
        }

        public IActionResult Order()
        {
            return View(GetOrderCatalogArticleViewModel());
        }

        [HttpPost]
        public IActionResult Order([FromForm] Models.Trbovlje.OrderCatalogArticleViewModel orderCatalogArticle)
        {
            if (!ModelState.IsValid)
                return View(orderCatalogArticle);

            OrderCatalogArticle dbOrderCatalogArticle = new OrderCatalogArticle();

            _mapper.Map(orderCatalogArticle, dbOrderCatalogArticle);
            _dbContext.OrderCatalogArticles.Add(dbOrderCatalogArticle);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var dbOrderCatalogArticle = _dbContext.OrderCatalogArticles.FirstOrDefault(l => l.Id == id);
            if (dbOrderCatalogArticle == null)
                RedirectToAction("Index");

            return View(_mapper.Map<OrderCatalogArticleViewModel>(dbOrderCatalogArticle));
        }

        [HttpPost]
        public IActionResult Remove([FromForm] OrderCatalogArticleViewModel viewModel)
        {
            var dbCatalogArticle = _dbContext.CatalogArticles.FirstOrDefault(l => l.Id == viewModel.Id);
            if (dbCatalogArticle != null)
            {
                _dbContext.CatalogArticles.Remove(dbCatalogArticle);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult ConfirmIndex()
        {
            ViewBag.DataSource = _mapper.Map<ICollection<OrderCatalogArticleViewModel>>(_dbContext.OrderCatalogArticles);

            var data = _dbContext.OrderCatalogArticles
                .Include(s => s.CatalogArticle)
                .ToList();

            return View(_mapper.Map<List<Models.Trbovlje.OrderCatalogArticleViewModel>>(data).First());
        }

        public IActionResult Confirm()
        {
            return View(GetOrderCatalogArticleViewModel());
        }

        [HttpPost]
        public IActionResult Confirm([FromForm] Models.Trbovlje.OrderCatalogArticleViewModel orderCatalogArticle)
        {
            if (!ModelState.IsValid)
                return View(orderCatalogArticle);

            OrderCatalogArticle dbOrderCatalogArticle = new OrderCatalogArticle();

            _mapper.Map(orderCatalogArticle, dbOrderCatalogArticle);
            _dbContext.OrderCatalogArticles.Add(dbOrderCatalogArticle);
            _dbContext.SaveChanges();

            return RedirectToAction("ConfirmIndex");
        }
    }
}
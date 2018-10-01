using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infozdrav.Web.Data;
using Infozdrav.Web.Data.Manage;
using Infozdrav.Web.Data.Trbovlje;
using Infozdrav.Web.Models.Manage;
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
    [Authorize(Roles = Roles.Administrator + "," + Roles.CatalogArticleOrder)]

    public class OrderCatalogArticleController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly FileService _fileService;

        public OrderCatalogArticleController(AppDbContext dbContext, IMapper mapper, UserService userService,
            UserManager<User> userManager, FileService fileService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mapper = mapper;
            _fileService = fileService;
        }
        
        [Authorize(Roles = Roles.Administrator + "," + Roles.CatalogArticleOrder)]
        public IActionResult Index()
        {
            var data = _dbContext.OrderCatalogArticles
                .Include(s => s.CatalogArticle)
                .ToList();

            return View(_mapper.Map<List<Models.Trbovlje.OrderCatalogArticleFullViewModel>>(data));
        }

        [Authorize(Roles = Roles.Administrator + "," + Roles.CatalogArticleOrder)]
        public IActionResult Edit(int id)
        {
            var article = _dbContext.OrderCatalogArticles
                .Include(s => s.CatalogArticle)
                .FirstOrDefault(u => u.Id == id);

            if (article == null)
                return RedirectToAction("Index");

            return base.View(_mapper.Map<Models.Trbovlje.OrderCatalogArticleFullViewModel>(article));
        }

        private IEnumerable<SelectListItem> GetCatalogArticles()
        {
            return new SelectList(_dbContext.CatalogArticles, "Id", "Name", "Price");
        }

        private OrderCatalogArticleFullViewModel GetOrderCatalogArticleFullViewModel()
        {
            return new OrderCatalogArticleFullViewModel
            {
                CatalogArticles = GetCatalogArticles()
            };
        }

        public IActionResult OrderCatalogArticle()
        {
            return View(GetOrderCatalogArticleFullViewModel());
        }

        [Authorize(Roles = Roles.Administrator + "," + Roles.CatalogArticleOrder)]
        [HttpPost]
          public IActionResult OrderCatalogArticle([FromForm] Models.Trbovlje.OrderCatalogArticleFullViewModel orderCatalogArticle)
          {
            var dbOrderCatalogArticle = _dbContext.OrderCatalogArticles
                .Include(s => s.CatalogArticle)
                .Include(s => s.UrgencyDegree)
                .Include(s => s.ReceptionUser)
                .Include(s => s.ReceptionTime)
                .Include(s => s.Quantity)
                .FirstOrDefault(u => u.Id == orderCatalogArticle.Id);

            if (!ModelState.IsValid)
                  return View(orderCatalogArticle);

              if (dbOrderCatalogArticle == null)
                  return RedirectToAction("Index");

              _mapper.Map(orderCatalogArticle, dbOrderCatalogArticle);
              _dbContext.OrderCatalogArticles.Update(dbOrderCatalogArticle);
              _dbContext.SaveChanges();

              return View(orderCatalogArticle);
          }

        private NewOrderCatalogArticleViewModel GetOrderCatalogArticleViewModel()
        {
            return new NewOrderCatalogArticleViewModel
            {
                CatalogArticles = GetCatalogArticles()
            };
        }

        public IActionResult NewOrder()
        {
            return View(GetOrderCatalogArticleViewModel());
        }

        [HttpPost]
        public IActionResult NewOrder([FromForm] Models.Trbovlje.NewOrderCatalogArticleViewModel orderCatalogArticle)
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

            return View(_mapper.Map<OrderCatalogArticleFullViewModel>(dbOrderCatalogArticle));
        }

        [Authorize(Roles = Roles.Administrator)]
        [HttpPost]
        public IActionResult Remove([FromForm] OrderCatalogArticleFullViewModel viewModel)
        {
            var dbOrderCatalogArticle = _dbContext.OrderCatalogArticles.FirstOrDefault(l => l.Id == viewModel.Id);
            if (dbOrderCatalogArticle != null)
            {
                _dbContext.OrderCatalogArticles.Remove(dbOrderCatalogArticle);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        private OrderCatalogArticleEditViewModel GetOrderCatalogArticleEditViewModel()
        {
            return new OrderCatalogArticleEditViewModel
            {
                CatalogArticles = GetCatalogArticles()
            };
        }

        public IActionResult Edit()
        {
            return View(GetOrderCatalogArticleEditViewModel());
        }

        [Authorize(Roles = Roles.Administrator)]
        [HttpPost]
        public IActionResult Edit([FromForm] OrderCatalogArticleEditViewModel viewModel)
        {
            var dbOrderCatalogArticle= _dbContext.OrderCatalogArticles.FirstOrDefault(l => l.Id == viewModel.Id);
            if (dbOrderCatalogArticle == null)
                RedirectToAction("Index");

            if (!ModelState.IsValid)
                return View(viewModel);

            _mapper.Map(viewModel, dbOrderCatalogArticle);

            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = Roles.Administrator + "," + Roles.CatalogArticleConfirmOrder)]
        public IActionResult ConfirmIndex()
        {
            ViewBag.DataSource = _mapper.Map<ICollection<OrderCatalogArticleFullViewModel>>(_dbContext.OrderCatalogArticles);

            var data = _dbContext.OrderCatalogArticles
                .Include(s => s.CatalogArticle)
                .ToList();

            return View(_mapper.Map<List<Models.Trbovlje.OrderCatalogArticleFullViewModel>>(data).First());
        }

        [Authorize(Roles = Roles.Administrator + "," + Roles.CatalogArticleConfirmOrder)]
        public IActionResult Confirm()
        {
            return View(GetOrderCatalogArticleViewModel());
        }

        [Authorize(Roles = Roles.Administrator + "," + Roles.CatalogArticleConfirmOrder)]
        [HttpPost]
        public IActionResult Confirm([FromForm] Models.Trbovlje.OrderCatalogArticleFullViewModel orderCatalogArticle)
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
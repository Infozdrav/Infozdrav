using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Infozdrav.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infozdrav.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public DashboardController(AppDbContext dbContext, IMapper mapper
        )
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Trbovlje()
        {
            var data = _dbContext.Articles
                .Where(a => a.UseByDate != null & a.CatalogArticle.UseByDaysLimit <= ((DateTime)a.UseByDate - DateTime.Now).TotalDays)
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

            return View(_mapper.Map < List < Models.Trbovlje.ArticleTableViewModel >> (data));
        }
    }
}
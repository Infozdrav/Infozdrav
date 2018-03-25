using System.Collections.Generic;
using AutoMapper;
using Infozdrav.Web.Data;
using Infozdrav.Web.Models.Manage;
using Microsoft.AspNetCore.Mvc;

namespace Infozdrav.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public UsersController(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            ViewBag.DataSource = _mapper.Map<ICollection<UserViewModel>>(_dbContext.Users);

            return View();
        }
    }
}
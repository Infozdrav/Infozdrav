using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Infozdrav.Web.Data;
using Infozdrav.Web.Models.Manage;
using Infozdrav.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Infozdrav.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserService _userService;

        public UsersController(AppDbContext dbContext, IMapper mapper, UserService userService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userService = userService;
        }

        public IActionResult Index()
        {
            ViewBag.DataSource = _mapper.Map<ICollection<UserViewModel>>(_dbContext.Users);

            return View();
        }

        public IActionResult User(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return RedirectToAction("Index");

            return View(_mapper.Map<UserViewModel>(user));
        }

        [HttpPost]
        public IActionResult User([FromForm] UserViewModel user)
        {
            if (!ModelState.IsValid)
                return View(user);

            var dbUser = _dbContext.Users.FirstOrDefault(u => u.Id == user.Id);
            if (user == null)
                return RedirectToAction("Index");

            if (!_userService.UpdateEmail(dbUser, user.Email, out var error))
            {
                ModelState.AddModelError("Email", error);
                return View(user);
            }

            _mapper.Map(user, dbUser);
            _dbContext.Users.Update(dbUser);
            _dbContext.SaveChanges();

            return View(user);
        }
    }
}
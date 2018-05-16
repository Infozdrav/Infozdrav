using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infozdrav.Web.Data;
using Infozdrav.Web.Data.Manage;
using Infozdrav.Web.Models.Manage;
using Infozdrav.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Infozdrav.Web.Controllers
{
    [Authorize(Roles = Roles.Administrator)]

    public class UsersController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserService _userService;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public UsersController(AppDbContext dbContext,
            IMapper mapper,
            UserService userService,
            RoleManager<Role> roleManager,
            UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userService = userService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            ViewBag.DataSource = _mapper.Map<ICollection<UserViewModel>>(_dbContext.Users);

            return View();
        }

        public async Task<IActionResult> User(int id)
        {
            if (id == 0)
            {
                var userVm = new UserEditViewModel();
                userVm.Roles = _mapper.Map<List<RoleViewModel>>(_roleManager.Roles.ToList());
                return View(userVm);
            }
            else
            {
                var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                    return RedirectToAction("Index");

                var userVm = _mapper.Map<UserEditViewModel>(user);
                userVm.Roles = _mapper.Map<List<RoleViewModel>>(_roleManager.Roles.ToList());

                foreach (var role in userVm.Roles)
                    role.Selected = await _userManager.IsInRoleAsync(user, role.Name);

                return View(userVm);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> User([FromForm] UserEditViewModel user)
        {
            var newUser = user.Id == 0;
            if (newUser && string.IsNullOrWhiteSpace(user.Password))
                ModelState.AddModelError(nameof(user.Password), "Password field is requried when creating a new user.");

            if (!ModelState.IsValid)
                return View(user);

            
            var dbUser = _mapper.Map<User>(user);
            IdentityResult result = null;


            if (user.Id == 0)
            {
                dbUser.EmailConfirmed = true;
                dbUser.Enabled = true;
                result = await _userManager.CreateAsync(dbUser);
            }

            else
            {

                dbUser = _dbContext.Users.FirstOrDefault(u => u.Id == user.Id);
                if (dbUser == null)
                    return RedirectToAction("Index");
                _mapper.Map(user, dbUser);


                result = await _userManager.UpdateAsync(dbUser);
            }

            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError(err.Code, err.Description);

                return View(user);
            }

            foreach (var role in user.Roles)
            {
                if (role.Selected)
                {
                    if (!await _userManager.IsInRoleAsync(dbUser, role.Name))
                        await _userManager.AddToRoleAsync(dbUser, role.Name);
                }
                else // not selected
                {
                    if (await _userManager.IsInRoleAsync(dbUser, role.Name))
                        await _userManager.RemoveFromRoleAsync(dbUser, role.Name);

                }
            }

            _dbContext.SaveChanges();

            if (newUser)
                return RedirectToAction("User", new {id = dbUser.Id});
            else
                return View(user);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Infozdrav.Web.Abstractions;
using Infozdrav.Web.Data.Manage;
using Infozdrav.Web.Helpers;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace Infozdrav.Web.Data
{
    public class DbInitializer : ISingletonDependency
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public DbInitializer(AppDbContext appDbContext,
                             UserManager<User> userManager,
                             RoleManager<Role> roleManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            UpdateDatabaseOnModelChange();

            InitRoles().Wait();
            InitUsers().Wait();
        }

        private void UpdateDatabaseOnModelChange()
        {
            var dbModels = GetDbModels();
            var currModels = Assembly.GetEntryAssembly()
                                .GetAllTypesWithBase<IEntity>()
                                .Select(e => new ModelHash
                                {
                                    Name = e.FullName,
                                    Hash = GetSimpleHash(e)
                                })
                                .ToList();

            // TODO Could be optimized
            if (currModels.Any(m => dbModels.Count(o => o.Name == m.Name && o.Hash == m.Hash) != 1))
                _appDbContext.Database.EnsureDeleted();


            _appDbContext.Database.EnsureCreated();
            var hashTable = _appDbContext.Set<ModelHash>();
            hashTable.AddRange(currModels);

            _appDbContext.SaveChanges();
        }

        private List<ModelHash> GetDbModels()
        {
            try
            {
                var hashTable = _appDbContext.Set<ModelHash>();
                return hashTable.ToList();
            }
            catch (Exception e)
            {
                return new List<ModelHash>();
            }
        }


        private static string GetSimpleHash(Type t)
        {
            return JsonConvert.SerializeObject(t.GetProperties().Select(prop => (type: prop.PropertyType.Name, name: prop.Name))).ToSHA1();
        }

        private async Task InitRoles()
        {
            //if (_appDbContext.Roles.Any())
            //    return;

            foreach (var field in typeof(Roles).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                await _roleManager.CreateAsync(new Role
                {
                    Name = field.GetValue(null) as string
                });
            }

            _appDbContext.SaveChanges();
        }

        private async Task InitUsers()
        {
            if (_appDbContext.Users.Any())
                return;

            var user = new User
            {
                UserName = "Administrator",
                Email = "admin@infozdrav.si",
                EmailConfirmed = true,
            };

            await _userManager.CreateAsync(user, "$Demo1001");
            await _userManager.AddToRoleAsync(user, Roles.Administrator);

            _appDbContext.SaveChanges();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using Infozdrav.Web.Abstractions;
using Infozdrav.Web.Helpers;
using Newtonsoft.Json;

namespace Infozdrav.Web.Data
{
    public class DbInitializer : ISingletonDependency
    {
        private readonly AppDbContext _appDbContext;

        public DbInitializer(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            UpdateDatabaseOnModelChange();

            InitRoles();
            InitUsers();
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

        private void InitRoles()
        {
            if (this._appDbContext.Roles.Any())
                return;

            foreach (var field in typeof(Roles).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                this._appDbContext.Roles.Add(new Role
                {
                    Name = field.GetValue(null) as string
                });
            }

            _appDbContext.SaveChanges();
        }

        private void InitUsers()
        {
            if (this._appDbContext.Users.Any())
                return;

            var user = new User
            {
                Email = "admin@infozdrav.si",
                Name = "Administrator",
                Password = "infozdrav".ToSHA512(),
            };

            user.Roles = new List<UserRole> {
                new UserRole
                {
                    User = user,
                    Role = this._appDbContext.Roles.First(o => o.Name == Roles.Administrator)
                },
            };

            this._appDbContext.Add(user);
            _appDbContext.SaveChanges();
        }
    }
}
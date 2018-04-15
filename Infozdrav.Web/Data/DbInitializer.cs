using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            InitWorkLocations();
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
            if (_appDbContext.Roles.Any())
                return;

            foreach (var field in typeof(Roles).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                _appDbContext.Roles.Add(new Role
                {
                    Name = field.GetValue(null) as string
                });
            }

            _appDbContext.SaveChanges();
        }

        private void InitUsers()
        {
            if (_appDbContext.Users.Any())
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
                    Role = _appDbContext.Roles.First(o => o.Name == Roles.Administrator)
                },
            };

            _appDbContext.Add(user);
            _appDbContext.SaveChanges();
        }

        private void InitWorkLocations()
        {
            if (_appDbContext.WorkLocations.Any())
                return;

            _appDbContext.Add(new WorkLocation() {Name = "Analizator 1"});
            _appDbContext.Add(new WorkLocation() {Name = "Analizator 2"});
            _appDbContext.Add(new WorkLocation() {Name = "Analizator 3"});
            _appDbContext.SaveChanges();
        }

        private void InitSuppliers()
        {
            if (_appDbContext.Suppliers.Any())
                return;

            var supplier = new Supplier
            {
                Name = "Dobavitelj 1",
                Address = "Ljubljana, Slovenija",
                Contact = "dobavitelj@dobavitelj.com",
            };

            _appDbContext.Add(supplier);
            _appDbContext.SaveChanges();
        }
    }
}
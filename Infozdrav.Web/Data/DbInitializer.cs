using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Infozdrav.Web.Abstractions;
using Infozdrav.Web.Data.Manage;
using Infozdrav.Web.Data.Trbovlje;
using Infozdrav.Web.Helpers;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Buffer = Infozdrav.Web.Data.Trbovlje.Buffer;

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

            if (!UpdateDatabaseOnModelChange()) // todo: go back to if
                return;

            InitRoles().Wait();
            InitUsers().Wait();
            InitSuppliers();
            InitManufacturers();
            InitCatalogArticles();
            InitStorageTypes();
            InitStorageLocations();
            InitAnalysers();
            InitWorkLocations();
            // InitArticle();
            InitLaboratories();
            InitBuffer();
            InitOrderCatalogArticle();
        }

        private bool UpdateDatabaseOnModelChange()
        {
            var dbModels = GetDbModels();
            var currModels = Assembly.GetEntryAssembly()
                                .GetAllTypesWithBase<IEntity>()
                                .Select(e => new Table
                                {
                                    Name = e.FullName,
                                    Hash = GetSimpleHash(e)
                                })
                                .ToList();

            if (currModels.Any(m => dbModels.Count(o => o.Name == m.Name && o.Hash == m.Hash) != 1))
                _appDbContext.Database.EnsureDeleted();
            else
                return false;

            _appDbContext.Database.EnsureCreated();
            var hashTable = _appDbContext.Set<Table>();
            hashTable.AddRange(currModels);
            _appDbContext.SaveChanges();

            return true;
        }

        private List<Table> GetDbModels()
        {
            try
            {
                var hashTable = _appDbContext.Set<Table>();
                return hashTable.ToList();
            }
            catch (Exception e)
            {
                return new List<Table>();
            }
        }


        private static string GetSimpleHash(Type t)
        {
            return JsonConvert.SerializeObject(t.GetProperties().Select(prop => (type: prop.PropertyType.Name, name: prop.Name))).ToSHA1();
        }

        private async Task InitRoles()
        {
            if (_roleManager.Roles.Any())
                return;

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
                FirstName = "Admin",
                EmailConfirmed = true,
                Enabled = true
            };

            await _userManager.CreateAsync(user, "$Admin123");
            await _userManager.AddToRoleAsync(user, Roles.Administrator);

            _appDbContext.SaveChanges();
        }

        private void InitWorkLocations()
        {
            if (_appDbContext.WorkLocations.Any())
                return;

            _appDbContext.Add(new WorkLocation() {Name = "Workplace 1"});
            _appDbContext.Add(new WorkLocation() {Name = "Workplace 2" });
            _appDbContext.Add(new WorkLocation() {Name = "Workplace 3" });
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
                Email = "dobavitelj@dobavitelj.com",
                Phone = "041234567",
            };

            _appDbContext.Add(supplier);
            _appDbContext.SaveChanges();
        }

        private void InitManufacturers()
        {
            if (_appDbContext.Manufacturers.Any())
                return;

            var manufacturer = new Manufacturer
            {
                Name = "Proizvajalec 1",
                Address = "Ljubljana, Slovenija",
                Email = "proizvajalec@proizvajalec.si",
                Phone = "049876543",
            };

            _appDbContext.Add(manufacturer);
            _appDbContext.SaveChanges();
        }

        private void InitCatalogArticles()
        {
            if (_appDbContext.CatalogArticles.Any())
                return;

            var catalogArticle = new CatalogArticle
            {
                Name = "Artikel 1",
                CatalogNumber = 23942,
                Price = "14€",
                ArticleType = ArticleType.Kemikalija,
                Manufacturer = _appDbContext.Manufacturers.FirstOrDefault(),
                Supplier = _appDbContext.Suppliers.FirstOrDefault(),
                UseByDaysLimit = 2
            };

            _appDbContext.Add(catalogArticle);
            _appDbContext.SaveChanges();
        }

        private void InitStorageTypes()
        {
            if (_appDbContext.StorageTypes.Any())
                return;

            _appDbContext.Add(new StorageType() { Name = "-20 °c" });
            _appDbContext.Add(new StorageType() { Name = "2-8 °c" });
            _appDbContext.Add(new StorageType() { Name = "18-25 °c" });
            _appDbContext.SaveChanges();
        }

        private void InitStorageLocations()
        {
            if (_appDbContext.StorageLocations.Any())
                return;

            _appDbContext.Add(new StorageLocation() { Name = "Storage 1" });
            _appDbContext.Add(new StorageLocation() { Name = "Storage 2" });
            _appDbContext.Add(new StorageLocation() { Name = "Storage 3" });
            _appDbContext.SaveChanges();
        }

        private void InitAnalysers()
        {
            if (_appDbContext.Analysers.Any())
                return;

            _appDbContext.Add(new Analyser() { Name = "Analyser 1" });
            _appDbContext.Add(new Analyser() { Name = "Analyser 2" });
            _appDbContext.Add(new Analyser() { Name = "Analyser 3" });
            _appDbContext.SaveChanges();
        }

        private void InitArticle()
        {
            if (_appDbContext.Articles.Any())
                return;

            for (int i = 0; i < 5; i++)
            {
                _appDbContext.Add(new Article()
                {
                    CatalogArticle = _appDbContext.CatalogArticles.FirstOrDefault(),
                    Lot = "lot:000 " + i,
                    UseByDate = DateTime.Today,
                    NumberOfUnits = 1 + i * 10,
                    DeliveryCost = 4.9m,
                    Rejected = false,
                    Note = "Article note " + i,
                    StorageType = _appDbContext.StorageTypes.FirstOrDefault(),
                    StorageLocation = _appDbContext.StorageLocations.FirstOrDefault(),
                    WorkLocation = _appDbContext.WorkLocations.FirstOrDefault(),
                    Analyser = _appDbContext.Analysers.FirstOrDefault(),
                    ReceptionTime = DateTime.Today.AddDays(-1),
                    ReceptionUser = _appDbContext.Users.FirstOrDefault(),
                });
            }
            _appDbContext.Add(new Article()
            {
                CatalogArticle = _appDbContext.CatalogArticles.FirstOrDefault(),
                Lot = "lot:125864862",
                UseByDate = DateTime.Today,
                NumberOfUnits = 100,
                DeliveryCost = 4.9m,
                Rejected = false,
                Note = "Article note",
                StorageType = _appDbContext.StorageTypes.FirstOrDefault(),
                StorageLocation = _appDbContext.StorageLocations.FirstOrDefault(),
                WorkLocation = _appDbContext.WorkLocations.FirstOrDefault(),
                Analyser = _appDbContext.Analysers.FirstOrDefault(),
                ReceptionTime = DateTime.Today.AddDays(-1),
                ReceptionUser = _appDbContext.Users.FirstOrDefault(),
                WriteOffTime = DateTime.Today,
                WriteOfUser = _appDbContext.Users.FirstOrDefault(),
                WriteOffReason = WriteOffReason.Expired,
                WriteOffNote = "Note for writeoff"
            });

            _appDbContext.Add(new Article()
            {
                CatalogArticle = _appDbContext.CatalogArticles.FirstOrDefault(),
                Lot = "badLot",
                NumberOfUnits = 200,
                DeliveryCost = 120.9m,
                Rejected = true,
                Note = "Damaged package",
                WriteOffReason = WriteOffReason.Other,
            });
            _appDbContext.SaveChanges();
        }
        private void InitLaboratories()
        {
            if (_appDbContext.Laboratories.Any())
                return;

            _appDbContext.Add(new Laboratory() { Name = "Urgenca" });
            _appDbContext.Add(new Laboratory() { Name = "Bolnišnica Trbovlje" });
            _appDbContext.SaveChanges();
        }

        private void InitBuffer()
        {
            if (_appDbContext.Buffers.Any())
                return;

            _appDbContext.Add(new Buffer()
            {
                Article = _appDbContext.Articles.FirstOrDefault(),
                UseByDate = DateTime.Today,
                Note = "Buffer note",
                StorageType = _appDbContext.StorageTypes.FirstOrDefault(),
                StorageLocation = _appDbContext.StorageLocations.FirstOrDefault(),
                WorkLocation = _appDbContext.WorkLocations.FirstOrDefault(),
                Analyser = _appDbContext.Analysers.FirstOrDefault(),
                PreparationTime = DateTime.Today.AddDays(-1),
                PreparationUser = _appDbContext.Users.FirstOrDefault(),
            });
            _appDbContext.SaveChanges();
        }

        private void InitOrderCatalogArticle()
        {
            if (_appDbContext.OrderCatalogArticles.Any())
                return;

            var orderCatalogArticle = new OrderCatalogArticle
            {
                CatalogArticle = _appDbContext.CatalogArticles.FirstOrDefault(),
                Quantity = 4,
                UrgencyDegree = UrgencyDegree.Three,
                Note = "Order note",
                ReceptionTime = DateTime.Today.AddDays(-1),
                ReceptionUser = _appDbContext.Users.FirstOrDefault(),
            };

            _appDbContext.Add(orderCatalogArticle);
            _appDbContext.SaveChanges();
        }
    }
}
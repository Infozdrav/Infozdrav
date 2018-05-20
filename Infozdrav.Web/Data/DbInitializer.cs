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
            InitSampleType();
            InitRooms();
            InitFridges();
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
            return JsonConvert
                .SerializeObject(t.GetProperties().Select(prop => (type: prop.PropertyType.Name, name: prop.Name)))
                .ToSHA1();
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

            user.Roles = new List<UserRole>
            {
                new UserRole
                {
                    User = user,
                    Role = _appDbContext.Roles.First(o => o.Name == Roles.Administrator)
                },
            };

            _appDbContext.Add(user);
            _appDbContext.SaveChanges();
        }

        private void InitSampleType()
        {
            if (_appDbContext.SampleTypes.Any())
                return;

            _appDbContext.SampleTypes.Add(new SampleType{ShortName = "WBL",SampleTypeName = "cela kri"});
            _appDbContext.SampleTypes.Add(new SampleType{ShortName = "PBL",SampleTypeName = "plazma"});
            _appDbContext.SampleTypes.Add(new SampleType{ShortName = "SBL",SampleTypeName = "serum"});
            _appDbContext.SampleTypes.Add(new SampleType {ShortName = "DNA", SampleTypeName = "DNA"});
            _appDbContext.SampleTypes.Add(new SampleType {ShortName = "RNA", SampleTypeName = "RNA"});
            _appDbContext.SampleTypes.Add(new SampleType {ShortName = "miR", SampleTypeName = "miRNA"});
            _appDbContext.SampleTypes.Add(new SampleType {ShortName = "cfD", SampleTypeName = "cfDNA"});
            _appDbContext.SampleTypes.Add(new SampleType {ShortName = "PRT", SampleTypeName = "protein"});
            _appDbContext.SampleTypes.Add(new SampleType {ShortName = "CLC", SampleTypeName = "celice (kultura)"});
            _appDbContext.SampleTypes.Add(new SampleType {ShortName = "CLT", SampleTypeName = "celice (tkivo)"});
            _appDbContext.SampleTypes.Add(new SampleType {ShortName = "BMW", SampleTypeName = "kostni mozeg"});
            _appDbContext.SampleTypes.Add(new SampleType {ShortName = "ASB", SampleTypeName = "aspiracijska biopsija"});
            _appDbContext.SampleTypes.Add(new SampleType {ShortName = "LKV", SampleTypeName = "likvor"});
            _appDbContext.SampleTypes.Add(new SampleType {ShortName = "URN", SampleTypeName = "urin"});
            _appDbContext.SampleTypes.Add(new SampleType {ShortName = "PLT", SampleTypeName = "rastlinski material"});
            _appDbContext.SampleTypes.Add(new SampleType {ShortName = "WTR", SampleTypeName = "voda"});
            _appDbContext.SampleTypes.Add(new SampleType {ShortName = "BCT", SampleTypeName = "bakterije"});
            _appDbContext.SampleTypes.Add(new SampleType {ShortName = "YST", SampleTypeName = "kvasovke"});
            _appDbContext.SampleTypes.Add(new SampleType {ShortName = "SWP", SampleTypeName = "bris"});
            _appDbContext.SampleTypes.Add(new SampleType {ShortName = "SLV", SampleTypeName = "slina"});
            _appDbContext.SampleTypes.Add(new SampleType {ShortName = "OTH", SampleTypeName = "drugo"});
            _appDbContext.SaveChanges();
        }

        private void InitRooms()
        {
            if (_appDbContext.Rooms.Any())
                return;

            _appDbContext.Rooms.Add(new Room {RoomName = "prePCR"});
            _appDbContext.Rooms.Add(new Room {RoomName = "postPCR"});
            _appDbContext.SaveChanges();
        }

        private void InitFridges()
        {
            if (_appDbContext.Fridges.Any())
                return;

            var preFridge = new Fridge
            {
                Name = "prePCR 1",
                Place = _appDbContext.Rooms.First(o => o.RoomName == "prePCR")
            };
            _appDbContext.Fridges.Add(preFridge);

            var poFridge = new Fridge
            {
                Name = "postPCR 1",
                Place = _appDbContext.Rooms.First(o => o.RoomName == "postPCR")
            };
            _appDbContext.Fridges.Add(poFridge);
            _appDbContext.SaveChanges();
        }
    }
}
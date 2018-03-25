using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Infozdrav.Web.Abstractions;
using Infozdrav.Web.Helpers;

namespace Infozdrav.Web.Data
{
    public class DbInitializer : ISingletonDependency
    {
        private readonly AppDbContext _appDbContext;

        public DbInitializer(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _appDbContext.Database.EnsureCreated();

            InitRoles();
            InitUsers();
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
                Password = "infozdrav".SHA512(),
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
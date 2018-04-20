using System.Reflection;
using Infozdrav.Web.Abstractions;
using Infozdrav.Web.Data.Manage;
using Infozdrav.Web.Helpers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infozdrav.Web.Data
{
    public class AppDbContext : IdentityDbContext<User, Role, int>
    {
        public AppDbContext(DbContextOptions options)
            : base(options)
        {
            
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var dbEntities = Assembly.GetEntryAssembly().GetAllTypesWithBase<IEntity>();
            foreach (var dbEntity in dbEntities)
                modelBuilder.Entity(dbEntity);
        }
    }
}
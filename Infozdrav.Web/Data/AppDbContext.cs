using System.Reflection;
using Infozdrav.Web.Abstractions;
using Infozdrav.Web.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Infozdrav.Web.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<WorkLocation> WorkLocations { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<CatalogArticle> CatalogArticles { get; set; }
        public DbSet<StorageType> StorageTypes { get; set; }
        public DbSet<StorageLocation> StorageLocations { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Analyser> Analysers { get; set; }
        public DbSet<Laboratory> Laboratories { get; set; }
        public DbSet<Buffer> Buffers { get; set; }


        public AppDbContext(DbContextOptions options) : base(options)
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
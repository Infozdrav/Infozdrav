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
        public DbSet<SampleType> SampleTypes { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Fridge> Fridges { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<Acceptance> Acceptances { get; set; }
        public DbSet<Sample> Samples { get; set; }
        public DbSet<Box> Boxes { get; set; }
        public DbSet<Results> Results { get; set; }
        public DbSet<Processing> Processings { get; set; }
        public DbSet<ContactPerson> ContactPeople { get; set; }
        public DbSet<Project> Projects { get; set; }
        
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
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Infozdrav.Web.Abstractions;
using Infozdrav.Web.Attributes;
using Infozdrav.Web.Data.Manage;
using Infozdrav.Web.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infozdrav.Web.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }


        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var dbEntities = Assembly.GetEntryAssembly().GetAllTypesWithBase<IEntity>();
            foreach (var dbEntity in dbEntities)
            {
                var dbTable = modelBuilder.Entity(dbEntity);
                if (dbEntity.GetInterfaces().Contains(typeof(IAuditEntity)))
                {
                    Expression<Func<IAuditEntity, bool>> f = o => o.DeletedAt != null;
                    dbTable.HasQueryFilter(f);
                }
            }

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            AuditData();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            AuditData();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AuditData();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            AuditData();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void AuditData()
        {
            var audits = Set<Audit>();
            var tables = Set<Table>();

            var entities = ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged).ToList();
            foreach (var entity in entities)
            {
                var baseEntity = entity.Entity as IAuditEntity;

                if (baseEntity == null)
                    continue;

                var table = tables.FirstOrDefault(t => t.Name == baseEntity.GetType().FullName);
                if (table == null)
                    continue;


                baseEntity.LastModified = DateTime.UtcNow;

                switch (entity.State)
                {
                    case EntityState.Deleted:
                        entity.State = EntityState.Modified;
                        baseEntity.DeletedAt = DateTime.Now;
                        break;
                    case EntityState.Added:
                        baseEntity.CreatedAt = DateTime.Now;
                        break;
                    default:
                        var propertyNames = entity.Metadata.GetProperties().ToList();
                        var auditType = StateToAuditType(entity);
                        foreach (var prop in propertyNames)
                        {
                            var p = entity.Property(prop.Name);
                            var type = baseEntity.GetType().GetProperty(prop.Name);

                            if (p.IsTemporary
                                || type == null
                                || !type.PropertyType.IsPrimitive())
                                continue;

                            var attrs = type.GetCustomAttributes(typeof(AuditAttribute), false);
                            if (attrs.Any(t => t.GetType() == typeof(IgnoreAudit)))
                                continue;

                            var maskAttr = attrs.FirstOrDefault(t => t.GetType() == typeof(MaskedAudit)) as MaskedAudit;

                            var oldVal = maskAttr?.DisplayValue ?? p.OriginalValue.ToString();
                            var newVal = maskAttr?.DisplayValue ?? p.CurrentValue.ToString();

                            var audit = new Audit
                            {
                                User = null, // TODO
                                EntityId = baseEntity.Id,
                                Table = table,
                                Timestamp = baseEntity.LastModified.Value,
                                Type = auditType,
                                PropertyName = prop.Name,
                                PropertyOldValue = oldVal,
                                PropertyNewValue = newVal,
                            };

                            audits.Add(audit);
                        }
                        break;
                }
            }
        }

        private static AuditType StateToAuditType(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entity)
        {
            switch (entity.State)
            {
                case EntityState.Deleted:
                    return AuditType.Deleted;
                case EntityState.Modified:
                    return AuditType.Modified;
                case EntityState.Added:
                    return AuditType.Added;
            }

            throw new NotImplementedException();
        }
    }
}
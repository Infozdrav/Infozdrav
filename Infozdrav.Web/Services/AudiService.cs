using System.Collections.Generic;
using System.Linq;
using Infozdrav.Web.Abstractions;
using Infozdrav.Web.Data;

namespace Infozdrav.Web.Services
{
    public class AudiService : IDependency
    {
        private readonly AppDbContext _appDbContext;

        public AudiService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Audit> GetEntityHistory(IAuditEntity e)
        {
            var name = e.GetType().FullName;

            var tables = _appDbContext.Set<Table>();
            var table = tables.FirstOrDefault(o => o.Name == name);

            if (table == null)
                return null;

            var auditTable = _appDbContext.Set<Audit>();

            return auditTable.Where(o => o.EntityId == e.Id && o.TableId == table.Id).ToList();
        }
    }
}
using System;
using Infozdrav.Web.Abstractions;
using Infozdrav.Web.Data.Manage;

namespace Infozdrav.Web.Data
{
    public enum AuditType
    {
        Added,
        Deleted,
        Modified
    }

    public class Audit : IEntity
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public Table Table { get; set; }

        public User User { get; set; }
        public DateTime Timestamp { get; set; }
        public int EntityId { get; set; }
        public AuditType Type { get; set; }
        public string PropertyNewValue { get; set; }
        public string PropertyOldValue { get; set; }
        public string PropertyName { get; set; }
    }
}
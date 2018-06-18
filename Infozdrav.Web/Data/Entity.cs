using System;
using Infozdrav.Web.Abstractions;
using Infozdrav.Web.Attributes;

namespace Infozdrav.Web.Data
{
    public abstract class Entity : IAuditEntity
    {
        public int Id { get; set; }

        [IgnoreAudit]
        public DateTime? LastModified { get; set; }

        [IgnoreAudit]
        public DateTime? DeletedAt { get; set; }

        [IgnoreAudit]
        public DateTime CreatedAt { get; set; }
    }
}
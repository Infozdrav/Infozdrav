using System;

namespace Infozdrav.Web.Abstractions
{
    public interface IAuditEntity : IEntity
    {
        int Id { get; set; }
        DateTime? LastModified { get; set; }
        DateTime? DeletedAt { get; set; }
        DateTime CreatedAt { get; set; }
    }
}
using System;

namespace Infozdrav.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreAudit : AuditAttribute
    {
    }
}
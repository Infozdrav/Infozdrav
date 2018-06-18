using System;

namespace Infozdrav.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MaskedAudit : AuditAttribute
    {
        public string DisplayValue { get; }

        public MaskedAudit(string displayValue = "")
        {
            DisplayValue = displayValue;
        }
    }
}
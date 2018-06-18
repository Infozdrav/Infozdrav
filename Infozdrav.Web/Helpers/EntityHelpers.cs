using Infozdrav.Web.Abstractions;

namespace Infozdrav.Web.Helpers
{
    public static class EntityHelpers
    {
        public static bool IsDeleted(this IAuditEntity e)
        {
            return e.DeletedAt != null;
        }
    }
}
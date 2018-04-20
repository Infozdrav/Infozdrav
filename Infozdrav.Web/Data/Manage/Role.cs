using Microsoft.AspNetCore.Identity;

namespace Infozdrav.Web.Data.Manage
{
    public class Role : IdentityRole<int>
    {
    }

    public sealed class Roles
    {
        public static readonly string Administrator = "Administrator";
    }
}
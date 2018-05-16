using Infozdrav.Web.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Infozdrav.Web.Data.Manage
{
    public class Role : IdentityRole<int>, IEntity
    {
    }

    public sealed class Roles
    {
        public const string Administrator = "Administrator";
    }
}
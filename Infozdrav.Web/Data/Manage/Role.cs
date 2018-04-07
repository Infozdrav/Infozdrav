namespace Infozdrav.Web.Data.Manage
{
    public class Role : Entity
    {
        public string Name { get; set; }
    }

    public sealed class Roles
    {
        public static readonly string Administrator = "Administrator";
    }
}
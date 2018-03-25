namespace Infozdrav.Web.Data
{
    public class UserRole : Entity
    {
        public User User { get; set; }
        public Role Role { get; set; }
    }
}
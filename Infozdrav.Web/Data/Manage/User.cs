using System.Collections.Generic;

namespace Infozdrav.Web.Data
{
    public class User : Entity
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }

        public List<UserRole> Roles { get; set; }
    }
}
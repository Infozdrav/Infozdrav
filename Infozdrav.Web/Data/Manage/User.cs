using System.Collections.Generic;
using Infozdrav.Web.Attributes;

namespace Infozdrav.Web.Data.Manage
{
    public class User : Entity
    {
        public string Name { get; set; }
        public string Email { get; set; }

        [MaskedAudit("MASKED PASSWORD")]
        public string Password { get; set; }

        public List<UserRole> Roles { get; set; }
    }
}
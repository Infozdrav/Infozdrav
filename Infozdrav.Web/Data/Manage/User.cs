using System.Collections.Generic;
using Infozdrav.Web.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Infozdrav.Web.Data.Manage
{
    public class User : IdentityUser<int>, IEntity
    {
        public User()
        {
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool Enabled { get; set; }
    }
}
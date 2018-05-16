using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Infozdrav.Web.Models.Manage
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [DisplayName("Email address")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DisplayName("User Name")]
        [Required]
        public string UserName { get; set; }

        [DisplayName("First name")]
        [Required]
        public string FirstName { get; set; }

        [DisplayName("Last name")]
        [Required]
        public string LastName { get; set; }

        [DisplayName("Password")]
        public string Password { get; set; }

        [DisplayName("Enabled")]
        public bool Enabled { get; set; }
    }
}
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Infozdrav.Web.Models.Manage
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [DisplayName("Email address")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
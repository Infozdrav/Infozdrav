using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Infozdrav.Web.Models.Manage
{
    public class PasswordChangeViewModel
    {
        [DisplayName("Password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Repeat Password")]
        [Required]
        [HiddenInput]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string RepeatPassword { get; set; }
    }
}
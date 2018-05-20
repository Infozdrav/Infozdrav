using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Infozdrav.Web.Models.Trbovlje
{
    public class ManufacturerViewModel
    {
        public int Id { get; set; }
    
        [DisplayName("Manufacturer name")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Manufacturer address")]
        [Required]
        public string Address { get; set; }

        [DisplayName("Manufacturer email")]
        [Required]
        public string Email { get; set; }

        [DisplayName("Manufacturer phone number")]
        [Required]
        public string Phone { get; set; }
    }
}
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Infozdrav.Web.Models.Trbovlje
{
    public class SupplierViewModel
    {
        public int Id { get; set; }
    
        [DisplayName("Supplier name")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Supplier address")]
        [Required]
        public string Address { get; set; }

        [DisplayName("Supplier contact")]
        [Required]
        public string Contact { get; set; }
    }
}
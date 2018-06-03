using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Infozdrav.Web.Models.Trbovlje
{
    public class LaboratoryViewModel
    {
        public int Id { get; set; }

        [DisplayName("Laboratory name")]
        [Required]
        public string Name { get; set; }
    }
}
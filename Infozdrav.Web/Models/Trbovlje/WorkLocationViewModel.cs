using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Infozdrav.Web.Models.Trbovlje
{
    public class WorkLocationViewModel
    {
        public int Id { get; set; }

        [DisplayName("Work location name")]
        [Required]
        public string Name { get; set; }
    }
}
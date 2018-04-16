using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Infozdrav.Web.Models.Trbovlje
{
    public class StorageLocationViewModel
    {
        public int Id { get; set; }

        [DisplayName("Storage location name")]
        [Required]
        public string Name { get; set; }
    }
}
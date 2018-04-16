using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Infozdrav.Web.Models.Trbovlje
{
    public class StorageTypeViewModel
    {
        public int Id { get; set; }

        [DisplayName("Storage type name")]
        [Required]
        public string Name { get; set; }
    }
}
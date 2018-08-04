using System.ComponentModel.DataAnnotations;

namespace Infozdrav.Web.Models.Labena
{
    public class DnaResultsViewModel : ProteinResultsViewModel
    {
        [Required]
        public double Mass { get; set; }
        [Required]
        public double A260A280 { get; set; }
        [Required]
        public double A260A230 { get; set; }
    }
}

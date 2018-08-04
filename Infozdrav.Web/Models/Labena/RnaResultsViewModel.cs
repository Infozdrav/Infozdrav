using System.ComponentModel.DataAnnotations;

namespace Infozdrav.Web.Models.Labena
{
    public class RnaResultsViewModel : DnaResultsViewModel
    {
        [Required]
        public double Rin { get; set; }
    }
}
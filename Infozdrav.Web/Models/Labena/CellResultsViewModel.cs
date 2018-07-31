using System.ComponentModel.DataAnnotations;

namespace Infozdrav.Web.Models.Labena
{
    public class CellResultsViewModel : ResultsViewModel
    {
        [Required]
        public double CellNumber { get; set; }
    }
}

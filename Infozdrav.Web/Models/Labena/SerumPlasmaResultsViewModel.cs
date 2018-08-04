using System.ComponentModel.DataAnnotations;

namespace Infozdrav.Web.Models.Labena
{
    public class SerumPlasmaResultsViewModel : ResultsViewModel
    {
        [Required]
        public double Volume { get; set; }
    }
}
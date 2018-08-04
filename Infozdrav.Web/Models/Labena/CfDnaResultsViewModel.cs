using System.ComponentModel.DataAnnotations;

namespace Infozdrav.Web.Models.Labena
{
    public class CfDnaResultsViewModel : ProteinResultsViewModel
    {
        [Required]
        public double NumberOfCopies { get; set; }
    }
}
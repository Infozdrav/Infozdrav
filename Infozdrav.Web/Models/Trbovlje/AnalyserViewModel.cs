using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Infozdrav.Web.Models.Trbovlje
{
    public class AnalyserViewModel
    {
        public int Id { get; set; }

        [DisplayName("Analyser name")]
        [Required]
        public string Name { get; set; }
    }
}
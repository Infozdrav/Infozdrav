using System.ComponentModel.DataAnnotations;

namespace Infozdrav.Web.Data.Trbovlje
{
    public enum UrgencyDegree
    {
        [Display(Name = "Zelo nujno")]
        One,
        [Display(Name = "Nujno")]
        Two,
        [Display(Name = "Nenujno")]
        Three
    }
}
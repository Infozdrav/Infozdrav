using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Infozdrav.Web.Models.Labena
{
    public class ObdelavaVzorcevViewModel
    {
        [Required]
        public DateTime? DatumObdelave { get; set; }

        [Required]
        public string Oseba { get; set; }

        [Required]
        public string Izolat { get; set; }

        [Required]
        public string Protokol { get; set; }

        [Required]
        public string Aparatura { get; set; }

        [Required]
        public string Kemikalije { get; set; }

    }
}

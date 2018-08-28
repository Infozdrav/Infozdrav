using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Infozdrav.Web.Models.Labena
{
    public class ProjektViewModel
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
    }
}

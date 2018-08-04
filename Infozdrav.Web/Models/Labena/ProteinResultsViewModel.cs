using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Infozdrav.Web.Models.Labena
{
    public class ProteinResultsViewModel : ResultsViewModel
    {   
        [Required]
        public double Concentration { get; set; }
        
        [Required]
        public double Volume { get; set; }
        
    }
}
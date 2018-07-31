using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace Infozdrav.Web.Models.Labena
{
    public class ResultsViewModel
    {
        [Required]
        public string Sample { get; set; }
    }
}
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Infozdrav.Web.Data;
using Infozdrav.Web.Data.Trbovlje;

namespace Infozdrav.Web.Models.Trbovlje
{
    public class ArticleUseViewModel
    {
        [Required]
        public int ArticleId { get; set; }
        public Article Article { get; set; }

        [Required]
        public int UnitNumber { get; set; }
    }
}
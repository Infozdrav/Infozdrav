using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Infozdrav.Web.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Infozdrav.Web.Models.Trbovlje
{
    public class CatalogArticleViewModel
    {
        public int Id { get; set; }

        [DisplayName("Article name")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Article catalog number")]
        [Required]
        public string CatalogNumber { get; set; }

        [DisplayName("Article price")]
        [Required]
        public string Price { get; set; }

        [DisplayName("Article type")]
        [Required]
        public string Type { get; set; }

        [Required]
        public int ManufacturerId { get; set; }
        public IEnumerable<SelectListItem> Manufacturers { get; set; }

        [Required]
        public int SupplierId { get; set; }
        public IEnumerable<SelectListItem> Suppliers { get; set; }
    }
}
﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Infozdrav.Web.Data;

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

        [DisplayName("Supplier")]
        [Required]
        public Supplier Supplier { get; set; }

        [DisplayName("Manufacturer")]
        [Required]
        public Manufacturer Manufacturer { get; set; }
    }
}
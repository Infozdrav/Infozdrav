using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Infozdrav.Web.Data;
using Infozdrav.Web.Data.Trbovlje;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Infozdrav.Web.Models.Trbovlje
{
    public class CatalogArticleFullViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CatalogNumber { get; set; }
        public decimal Price { get; set; }

        public ArticleType ArticleType { get; set; }

        public int? ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }

        public int? SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        public int UseByDaysLimit { get; set; }
    }

    public class CatalogArticleAddViewModel
    {
        public int Id { get; set; }

        [DisplayName("Ime artikla")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Kataloška številka")]
        [Required]
        public int CatalogNumber { get; set; }

        [DisplayName("Cena/enoto (€)")]
        [Required]
        public decimal Price { get; set; }

        [DisplayName("Tip artikla")]
        [Required]
        public ArticleType ArticleType { get; set; }

        [Required]
        public int ManufacturerId { get; set; }
        public IEnumerable<SelectListItem> Manufacturers { get; set; }

        [Required]
        public int SupplierId { get; set; }
        public IEnumerable<SelectListItem> Suppliers { get; set; }

        [Required]
        public int UseByDaysLimit { get; set; }
    }

    public class CatalogArticleEditViewModel
    {
        public int Id { get; set; }

        [DisplayName("Ime artikla")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Kataloška številka")]
        [Required]
        public int CatalogNumber { get; set; }

        [DisplayName("Cena/enoto (€)")]
        [Required]
        public decimal Price { get; set; }

        [DisplayName("Tip artikla")]
        [Required]
        public ArticleType ArticleType { get; set; }

        [Required]
        public int ManufacturerId { get; set; }
        public IEnumerable<SelectListItem> Manufacturers { get; set; }

        [Required]
        public int SupplierId { get; set; }
        public IEnumerable<SelectListItem> Suppliers { get; set; }

        [Required]
        public int UseByDaysLimit { get; set; }
    }

    public class CatalogArticleTableViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CatalogNumber { get; set; }
        public decimal Price { get; set; }

        public ArticleType ArticleType { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public Supplier Supplier { get; set; }

        public int UseByDaysLimit { get; set; }
    }
}
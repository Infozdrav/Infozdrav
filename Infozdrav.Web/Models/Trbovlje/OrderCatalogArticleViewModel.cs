using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Infozdrav.Web.Data;
using Infozdrav.Web.Data.Manage;
using Infozdrav.Web.Data.Trbovlje;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Infozdrav.Web.Models.Trbovlje
{
    public class OrderCatalogArticleViewModel
    {
        public int Id { get; set; }

        [DisplayName("Article from catalog")]
        [Required]
        public int CatalogArticleId { get; set; }
        public IEnumerable<SelectListItem> CatalogArticles { get; set; }

        [DisplayName("Article price")]
        public string Price { get; set; }

        [DisplayName("Quantity")]
        [Required]
        public int Quantity { get; set; }

        public string Note { get; set; }

        [Required]
        public UrgencyDegree UrgencyDegree { get; set; }

        public DateTime ReceptionTime { get; set; }
        public User ReceptionUser { get; set; }
    }
}
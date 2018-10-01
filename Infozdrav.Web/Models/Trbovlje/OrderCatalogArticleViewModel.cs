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
    public class OrderCatalogArticleFullViewModel
    {
        public int Id { get; set; }
        public int CatalogArticleId { get; set; }
        public CatalogArticle CatalogArticle { get; set; }
        public IEnumerable<SelectListItem> CatalogArticles { get; set; }

        public int Price { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }

        public UrgencyDegree? UrgencyDegree { get; set; }

        public DateTime ReceptionTime { get; set; }
        public User ReceptionUser { get; set; }

        //public DateTime ConfirmedTime { get; set; }
        //public User ConfirmedUser { get; set; }
    }

    public class NewOrderCatalogArticleViewModel
    {
        public int Id { get; set; }

        [Required]
        public int CatalogArticleId { get; set; }
        public IEnumerable<SelectListItem> CatalogArticles { get; set; }

        public UrgencyDegree? UrgencyDegree { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int Price { get; set; }

        public string Note { get; set; }

    }

    public class OrderCatalogArticleTableViewModel
    {
        public int Id { get; set; }

        public CatalogArticle CatalogArticle { get; set; }

        public int Price { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }

        public UrgencyDegree? UrgencyDegree { get; set; }
    }

    public class OrderCatalogArticleEditViewModel
    {
        public int Id { get; set; }

        [Required]
        public int CatalogArticleId { get; set; }
        public IEnumerable<SelectListItem> CatalogArticles { get; set; }

        public UrgencyDegree? UrgencyDegree { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int Price { get; set; }

        public string Note { get; set; }

    }
}
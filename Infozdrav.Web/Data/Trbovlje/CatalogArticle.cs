using System;
using System.Collections.Generic;
using Infozdrav.Web.Data.Manage;

namespace Infozdrav.Web.Data
{
    public class CatalogArticle : Entity
    {
        public string Name { get; set; }
        public int CatalogNumber { get; set; }
        public string Price { get; set; }

        public int? ArticleTypeId { get; set; }
        public ArticleType ArticleType { get; set; }

        public int? ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }

        public int? SupplierId { get; set; }
        public Supplier Supplier { get; set; }

         public DateTime? ReceptionTime { get; set; }
         public User ReceptionUser { get; set; }
    }
}
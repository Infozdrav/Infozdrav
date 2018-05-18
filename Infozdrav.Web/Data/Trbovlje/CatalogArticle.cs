using System;
using System.Collections.Generic;

namespace Infozdrav.Web.Data
{
    public class CatalogArticle : Entity
    {
        public string Name { get; set; }
        public string CatalogNumber { get; set; }
        public string Price { get; set; }
        public string Type { get; set; }

        public int? ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }

        public int? SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        /* public int? UrgencyDegreeId { get; set; }
         public UrgencyDegree UrgencyDegree { get; set; }

         public DateTime? ReceptionTime { get; set; }
         public User ReceptionUser { get; set; }

         public int Quantity { get; set; }*/
    }
}
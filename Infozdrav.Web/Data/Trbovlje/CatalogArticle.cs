using System.Collections.Generic;

namespace Infozdrav.Web.Data
{
    public class CatalogArticle : Entity
    {
        public string Name { get; set; }
        public string CatalogNumber { get; set; }
        public string Price { get; set; }
        public string Type { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public Supplier Supplier { get; set; }

    }
}
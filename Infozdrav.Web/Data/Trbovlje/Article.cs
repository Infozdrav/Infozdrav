using System;
using System.Collections.Generic;
using System.IO;

namespace Infozdrav.Web.Data
{
    public class Article : Entity
    {
        // Article reception
        public CatalogArticle CatalogArticle { get; set; } // TODO: Link to actuall catalog number

        public string Lot { get; set; }
        public DateTime UseByDate { get; set; }
        public int NumberOfUnits { get; set; }
        public decimal DeliveryCost { get; set; }

        public bool Rejected { get; set; }
        public string Note { get; set; }

        public StorageType StorageType { get; set; }
        public StorageLocation StorageLocation { get; set; }
        public WorkLocation WorkLocation { get; set; }
        public Analyser Analyser { get; set; }

        //public FileStream Certificate { get; set; }
        //public FileStream SafteyList { get; set; } // TODO: Implement files

        public string Certificate { get; set; }
        public string SafteyList { get; set; }

        public DateTime ReceptionTime { get; set; }
        public User ReceptionUser { get; set; }

        public DateTime WriteOffTime { get; set; }
        public User WriteOfUser { get; set; }
        public WriteOffReason WriteOffReason { get; set; }
        public string WriteOffNote { get; set; }
    }
}
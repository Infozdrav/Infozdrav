using System;
using System.Collections.Generic;
using Infozdrav.Web.Data.Manage;

namespace Infozdrav.Web.Data
{
    public class Buffer : Entity
    {
        public string Name { get; set; }

        public int? ArticleId { get; set; }
        public Article Article { get; set; }

        public DateTime UseByDate { get; set; }
        public string Note { get; set; }

        public int? StorageTypeId { get; set; }
        public StorageType StorageType { get; set; }
        public int? StorageLocationId { get; set; }
        public StorageLocation StorageLocation { get; set; }
        public int? WorkLocationId { get; set; }
        public WorkLocation WorkLocation { get; set; }
        public int? AnalyserId { get; set; }
        public Analyser Analyser { get; set; }

        public DateTime? PreparationTime { get; set; }
        public User PreparationUser { get; set; }

    }
}
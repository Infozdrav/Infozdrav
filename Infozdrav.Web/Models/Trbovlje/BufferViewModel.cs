using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Infozdrav.Web.Data;
using Infozdrav.Web.Data.Manage;
using Infozdrav.Web.Data.Trbovlje;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Infozdrav.Web.Models.Trbovlje
{
    public class BufferViewModel
    {
        public int Id { get; set; }

        [DisplayName("Buffer name")]
        [Required]
        public string Name { get; set; }

        [Required]
        public int ArticleId { get; set; }
        public IEnumerable<SelectListItem> Articles { get; set; }

        [Required]
        public DateTime UseByDate { get; set; }

        [Required]
        public DateTime PreparationDate { get; set; }

        [Required]
        public int UserId { get; set; }
        public IEnumerable<SelectListItem> Users { get; set; }

        public string Note { get; set; }

        [Required]
        public int StorageTypeId { get; set; }
        public IEnumerable<SelectListItem> StorageTypes { get; set; }

        [Required]
        public int StorageLocationId { get; set; }
        public IEnumerable<SelectListItem> StorageLocations { get; set; }

        [Required]
        public int WorkLocationId { get; set; }
        public IEnumerable<SelectListItem> WorkLocations { get; set; }

        [Required]
        public int AnalyserId { get; set; }
        public IEnumerable<SelectListItem> Analysers { get; set; }

    }

    public class BufferTableViewModel
    {
        public string Name { get; set; }

        public Article Article { get; set; }

        public DateTime UseByDate { get; set; }
        public string Note { get; set; }

        public StorageType StorageType { get; set; }
        public StorageLocation StorageLocation { get; set; }
        public WorkLocation WorkLocation { get; set; }
        public Analyser Analyser { get; set; }

        public DateTime? PreparationTime { get; set; }
        public User PreparationUser { get; set; }
    }
}
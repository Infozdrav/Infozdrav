using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Infozdrav.Web.Data;
using Infozdrav.Web.Data.Manage;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Infozdrav.Web.Models.Trbovlje
{
    public class ArticleFullViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

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

    public class ArticleReceptionViewModel
    {
        [Required]
        public int CatalogArticleId { get; set; }
        public IEnumerable<SelectListItem> CatalogArticles { get; set; }

        [Required]
        public string Lot { get; set; }
        [Required]
        public DateTime? UseByDate { get; set; }
        [Required]
        public int NumberOfUnits { get; set; }
        public decimal DeliveryCost { get; set; }

        [Required]
        public bool Rejected { get; set; }

        public bool ShowIgnoreBadLot { get; set; }
        public bool IgnoreBadLot { get; set; }
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

        public string Certificate { get; set; } // TODO: File
        public string SafteyList { get; set; }
    }

    public class ArticleWriteOffViewModel
    {
        public int Id { get; set; }

        public CatalogArticle CatalogArticle { get; set; } // TODO: Link to actuall catalog number

        public string Lot { get; set; }
        public DateTime? UseByDate { get; set; }
        public int? NumberOfUnits { get; set; }
        
        [Required]
        public WriteOffReason? WriteOffReason { get; set; }
        public string WriteOffNote { get; set; }
    }

    public class ArticleTableViewModel
    {
        public int Id { get; set; }

        public CatalogArticle CatalogArticle { get; set; } // TODO: Link to actuall catalog number

        public string Lot { get; set; }
        public DateTime UseByDate { get; set; }

        [Display(Name = "Neuporabljenih enot")]
        public int NumberOfAvailableUnits { get; set; }

       // public string Note { get; set; }

        public StorageType StorageType { get; set; }
        public StorageLocation StorageLocation { get; set; }
        public WorkLocation WorkLocation { get; set; }
        public Analyser Analyser { get; set; }

        //public FileStream Certificate { get; set; }
        //public FileStream SafteyList { get; set; } // TODO: Implement files

        public string Certificate { get; set; }
        public string SafteyList { get; set; }

      //  public DateTime ReceptionTime { get; set; }
    }

    public class ArticleEditViewModel
    {
        public int Id { get; set; }
    
        [Required]
        public CatalogArticle CatalogArticle { get; set; } // TODO: Link to actuall catalog number
        [Required]
        public string Lot { get; set; }
        [Required]
        public DateTime UseByDate { get; set; }
        [Required]
        public int NumberOfUnits { get; set; }
        public decimal DeliveryCost { get; set; }

        [Required]
        public bool Rejected { get; set; }
        public string Note { get; set; }

        [Required]
        public StorageType StorageType { get; set; }
        [Required]
        public StorageLocation StorageLocation { get; set; }
        [Required]
        public WorkLocation WorkLocation { get; set; }
        [Required]
        public Analyser Analyser { get; set; }

        //public FileStream Certificate { get; set; }
        //public FileStream SafteyList { get; set; } // TODO: Implement files

        public string Certificate { get; set; }
        public string SafteyList { get; set; }
    }
}
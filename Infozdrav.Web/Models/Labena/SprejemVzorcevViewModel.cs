using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Infozdrav.Web.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Infozdrav.Web.Models.Labena
{
    public class SprejemVzorcevViewModel
    {
//        [Required]
//        public int SupplierId { get; set; }

        [Required(ErrorMessage = "Polje je potrebno izpolniti")]
        public DateTime? DateOfReception { get; set; }

        [Required(ErrorMessage = "Polje je potrebno izpolniti")]
        public string SupplierName { get; set; }

        [Required(ErrorMessage = "Polje je potrebno izpolniti")]
        public string TaxNumber { get; set; }

        [Required(ErrorMessage = "Polje je potrebno izpolniti")]
        public string StreetNum { get; set; }

        [Required(ErrorMessage = "Polje je potrebno izpolniti")]
        public int? ZipNumber { get; set; }

        [Required(ErrorMessage = "Polje je potrebno izpolniti")]
        public string City { get; set; }

        [Required(ErrorMessage = "Polje je potrebno izpolniti")]
        public string Country { get; set; }

        public string IdDDV { get; set; }


        [Required(ErrorMessage = "Polje je potrebno izpolniti")]
        public string ContactName { get; set; }

        public string Mail { get; set; }

        public string TelNumber { get; set; }

    }

    public class DodajTipViewModel
    {
        [Required(ErrorMessage = "Polje je potrebno izpolniti")]
        public string TipVzorca { get; set; }
    }

    public class DodajanjeVzorcaViewModel
    {
        [Required(ErrorMessage = "Polje je potrebno izpolniti")]
        public string Provider { get; set; }
        
        [Required(ErrorMessage = "Polje je potrebno izpolniti")]
        public DateTime? DateReception { get; set; }
        
        [Required(ErrorMessage = "Polje je potrebno izpolniti")]
        public string IdProvider { get; set; }

        [Required(ErrorMessage = "Polje je potrebno izpolniti")]
        public string IdSample { get; set; }

        public DateTime? Date { get; set; }

//        public string Tip { get; set; }
        // TODO: Add radio buttons for sample and volume type

        public double? Volume { get; set; }

        public string Notes { get; set; }

        public string Project { get; set; }

        // [Required]
        // public string Room { get; set; }

        [Required(ErrorMessage = "Polje je potrebno izpolniti")]
        public string Fridge { get; set; }

        [Required(ErrorMessage = "Polje je potrebno izpolniti")]
        public int? Razdelek { get; set; }

        public string Box { get; set; }

        public string Location { get; set; }

        public string Type { get; set; }
        
        public string VolType { get; set; }
        
        [Required(ErrorMessage = "Polje je potrebno izpolniti")]
        public string Temp { get; set; }

    }

    public class DodajanjeAlikvotovViewModel
    {
        [Required(ErrorMessage = "Polje je potrebno izpolniti")]
        public string IdVzorca { get; set; }

        [Required(ErrorMessage = "Polje je potrebno izpolniti")]
        public int? StAlikvotov { get; set; }

        [Required(ErrorMessage = "Polje je potrebno izpolniti")]
        public DateTime? Date { get; set; }

        public double? Volume { get; set; }

        public string Notes { get; set; }

        public string Project { get; set; }

        [Required(ErrorMessage = "Polje je potrebno izpolniti")]
        public string Room { get; set; }

        [Required(ErrorMessage = "Polje je potrebno izpolniti")]
        public string Fridge { get; set; }

        public int? Razdelek { get; set; }

        public string Box { get; set; }

        public string Location { get; set; }
        public string VolType { get; set; }
        public string Temp { get; set; }
    }

    public class SkatlaViewModel
    {
        [Required(ErrorMessage = "Polje je potrebno izpolniti")]
        public string ImeSkatle { get; set; }

        [Required(ErrorMessage = "Polje je potrebno izpolniti")]
        public int? Velikost { get; set; }

        [Required(ErrorMessage = "Polje je potrebno izpolniti")]
        public string Tip { get; set; }
    }

    public class PoveziVzorecViewModel
    {

        [Required(ErrorMessage = "Polje je potrebno izpolniti")]
        public string Vzorec { get; set; }

        [DisplayName("Oznaka")]
        [Required(ErrorMessage = "Polje je potrebno izpolniti")]
        public string Oznaka { get; set; }

        [DisplayName("Id")]
        [Required(ErrorMessage = "Polje je potrebno izpolniti")]
        public string Id { get; set; }

        [DisplayName("Datum")]
        [Required(ErrorMessage = "Polje je potrebno izpolniti")]
        public string Datum { get; set; }

        [DisplayName("Projekt")]
        [Required(ErrorMessage = "Polje je potrebno izpolniti")]
        public string Projekt { get; set; }

    }

    public class VnosHladilnikaViewModel
    {
        [Required(ErrorMessage = "Polje je potrebno izpolniti")]
        public string Fridge { get; set; }
        
        [Required(ErrorMessage = "Polje je potrebno izpolniti")]
        public string Room { get; set; }
    }

}

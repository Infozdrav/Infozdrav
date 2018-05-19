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
        [Required]
        public int SupplyerId { get; set; }

        [Required]
        public DateTime? DateOfReception { get; set; }


        [Required]
        public string SupplyerName { get; set; }

        [Required]
        public string TaxNumber { get; set; }

        [Required]
        public string StreetNum { get; set; }

        [Required]
        public string ZipNumber { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        public string IdDDV { get; set; }


        [Required]
        public string ContactName { get; set; }

        public string Mail { get; set; }

        public string TelNumber { get; set; }

    }

    public class DodajanjeVzorcaViewModel
    {
        [Required]
        public string IdProvider { get; set; }

        [Required]
        public string IdSample { get; set; }

        public DateTime? Date { get; set; }

        public string Tip { get; set; }
        // TODO: Add radio buttons for sample and volume type

        public string Volume { get; set; }

        public string Notes { get; set; }

        public string Project { get; set; }

        [Required]
        public string Room { get; set; }

        [Required]
        public string Fridge { get; set; }

        public string Razdelek { get; set; }

        public string Box { get; set; }

        public string Location { get; set; }

        public string Type { get; set; }
        public string VolType { get; set; }
        public string Temp { get; set; }

    }

    public class DodajanjeAlikvotovViewModel
    {
        [Required]
        public string IdVzorca { get; set; }

        [Required]
        public string IdAlikvota { get; set; }

        [Required]
        public DateTime? Date { get; set; }

        public string Volume { get; set; }

        public string Notes { get; set; }

        public string Project { get; set; }

        [Required]
        public string Room { get; set; }

        [Required]
        public string Fridge { get; set; }

        public string Razdelek { get; set; }

        public string Box { get; set; }

        public string Location { get; set; }
        public string VolType { get; set; }
        public string Temp { get; set; }
    }

    public class SkatlaViewModel
    {
        [Required]
        public string Skatla { get; set; }

        [Required]
        public string Velikost { get; set; }

        [Required]
        public string Tip { get; set; }
    }

    public class PoveziVzorecViewModel
    {

        [Required]
        public string Vzorec { get; set; }

        [DisplayName("Oznaka")]
        [Required]
        public string Oznaka { get; set; }

        [DisplayName("Id")]
        [Required]
        public string Id { get; set; }

        [DisplayName("Datum")]
        [Required]
        public string Datum { get; set; }

        [DisplayName("Projekt")]
        [Required]
        public string Projekt { get; set; }

    }

    public class VnosHladilnikaViewModel
    {
        [Required]
        public string Fridge { get; set; }
    }

}

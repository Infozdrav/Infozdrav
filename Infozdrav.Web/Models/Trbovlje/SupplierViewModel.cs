using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Infozdrav.Web.Data;

namespace Infozdrav.Web.Models.Trbovlje
{
    public class SupplierViewModel
    {
        public int Id { get; set; }

        [DisplayName("Šifra dobavitelja")]
        public string IdCode { get; set; }

        [DisplayName("Naziv dobavitelja")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Naslov dobavitelja")]
        [Required]
        public string Address { get; set; }

        [DisplayName("E-pošta dobavitelja")]
        [Required]
        public string Email { get; set; }

        [DisplayName("Telefonska številka dobavitelja")]
        [Required]
        public string Phone { get; set; }
    }
}
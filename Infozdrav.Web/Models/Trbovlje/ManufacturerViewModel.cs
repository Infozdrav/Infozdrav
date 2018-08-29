using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Infozdrav.Web.Data;

namespace Infozdrav.Web.Models.Trbovlje
{
    public class ManufacturerViewModel
    {
        public int Id { get; set; }

        [DisplayName("Šifra proizvajalca")]
        public string IdCode { get; set; }

        [DisplayName("Naziv proizvajalca")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Naslov proizvajalca")]
        [Required]
        public string Address { get; set; }

        [DisplayName("E-pošta proizvajalca")]
        [Required]
        public string Email { get; set; }

        [DisplayName("Telefonska številka proizvajalca")]
        [Required]
        public string Phone { get; set; }
    }
}
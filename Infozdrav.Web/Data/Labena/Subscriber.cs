namespace Infozdrav.Web.Data
{
    public class Subscriber : Entity
    {
        public string Name { get; set; }
        public string TaxNumber { get; set; }
        public string Address { get; set; }
        public int PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string IdNumber { get; set; }
    }
}
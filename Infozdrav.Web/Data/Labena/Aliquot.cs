using System;

namespace Infozdrav.Web.Data
{
    public class Aliquot : Sample
    {
        public Sample OriginalSample { set; get; }
        public DateTime AliquotationDate { set; get; }
    }
}
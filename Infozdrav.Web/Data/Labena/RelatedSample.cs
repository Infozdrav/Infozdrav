using System.Collections.Generic;

namespace Infozdrav.Web.Data
{
    public class RelatedSample : Entity
    {
        public Sample OldSample { get; set;}
        public List<Sample> NewSample { get; set; }
    }
}
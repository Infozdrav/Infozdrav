using System.Collections.Generic;

namespace Infozdrav.Web.Data
{
    public class RelatedSample : Entity
    {
        public Sample OldSample { get; set;}
        public LinkedList<Sample> NewSample { get; set; }
    }
}
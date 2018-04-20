using System;
using System.Collections.Generic;

namespace Infozdrav.Web.Data
{
    public class Processing
    {
        public HashSet<Sample> Samples { get; set; }
        public DateTime Date { get; set; }
        public User UserName { get; set; }
        public Isolate IsolateName { get; set; }
        // add protocole, aparature, chemicals
    }
}
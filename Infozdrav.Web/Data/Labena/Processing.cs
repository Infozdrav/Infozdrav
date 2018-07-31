using System;
using System.Collections.Generic;

namespace Infozdrav.Web.Data
{
    public class Processing : Entity
    {
        public List<Sample> Samples { get; set; }
        public DateTime Date { get; set; }
        public User UserName { get; set; }
//        public Isolate IsolateName { get; set; }
        public SampleType IsolateName { get; set; }
        public string Protocole { get; set; }
        public string Aparature { get; set; }
        public string Chemicals { get; set; } // popravi, da bodo objekti iz ZD Trbovlje
    }
}
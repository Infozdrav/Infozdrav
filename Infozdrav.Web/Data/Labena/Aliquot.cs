using System;

namespace Infozdrav.Web.Data
{
    public class Aliquot : Entity
    {
        public Sample OriginalSample { set; get; }
        public SampleId AliquotId { set; get; }
        public DateTime Date { set; get; }
        public double Volume { set; get; }
        public string Comment { set; get; }
        public Project ProjectName { set; get; }
        public Storage StoredAt { set; get; }
    }
}
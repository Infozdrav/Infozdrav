namespace Infozdrav.Web.Data
{
    public class SampleId : Entity
    {
        public int SequenceNumber { get; set; }
        public int Year { get; set; }
        public string IsolateType { set; get; }
        public int AliquotSequenceNumber { get; set; }
        public string ProcessedIsolateType { get; set; }
        public int ProcessedAliquotSequenceNumber { get; set; }
        public int Rest { get; set; }
    }
}
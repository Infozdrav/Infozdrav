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

        public override string ToString()
        {
            if (ProcessedIsolateType == null)
                return Year + string.Format("{0,3:D3}", SequenceNumber) + IsolateType +
                       string.Format("{0,3:D3}", AliquotSequenceNumber);

            return Year + string.Format("{0,3:D3}", SequenceNumber) + IsolateType +
                   string.Format("{0,3:D3}", AliquotSequenceNumber) + ProcessedIsolateType +
                   string.Format("{0,3:D3}", ProcessedAliquotSequenceNumber);
        }
    }
}
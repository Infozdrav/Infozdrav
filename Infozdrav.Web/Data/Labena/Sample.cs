using System;
using Infozdrav.Web.Abstractions;

namespace Infozdrav.Web.Data
{
    public class Sample : IEntity
    {
        public int Id { get; set; }
        public SampleId NewId { get; set; }
        public Acceptance Accepted { get; set; }
        public string SubscriberName { get; set; }
        public DateTime Time { get; set; }
        public SampleType Type { get; set; }
        public double Volume { get; set; }
        public string Comments { get; set; }
        public Project ProjectName { get; set; }
        public Storage Stored { get; set; }
    }
}
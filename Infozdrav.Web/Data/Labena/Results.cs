using Infozdrav.Web.Abstractions;

namespace Infozdrav.Web.Data
{
    public abstract class Results : IEntity
    {
        public int Id { get; set; }
        public Sample SampleUsed { get; set; }
    }
}
using Infozdrav.Web.Abstractions;

namespace Infozdrav.Web.Data
{
    public abstract class Entity : IEntity
    {
        public int Id { get; set; }
    }
}
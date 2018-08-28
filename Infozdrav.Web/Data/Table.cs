using Infozdrav.Web.Abstractions;

namespace Infozdrav.Web.Data
{
    public class Table : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Hash { get; set; }
    }
}
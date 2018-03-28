using Infozdrav.Web.Abstractions;

namespace Infozdrav.Web.Data
{
    public class ModelHash : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Hash { get; set; }
        public string b { get; set; }
    }
}
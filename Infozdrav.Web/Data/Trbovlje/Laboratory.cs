using System.Collections.Generic;

namespace Infozdrav.Web.Data.Trbovlje
{
    public class Laboratory : Entity
    {
        public string Name { get; set; }

        public List<Article> Articles { get; set; }
    }
}
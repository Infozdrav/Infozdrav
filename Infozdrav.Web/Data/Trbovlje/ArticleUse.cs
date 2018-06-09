using System;
using Infozdrav.Web.Data.Manage;
using Newtonsoft.Json;

namespace Infozdrav.Web.Data.Trbovlje
{
    public class ArticleUse : Entity
    {
        public int ArticleId { get; set; }
        [JsonIgnore]
        public Article Article { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime UseTime { get; set; }
        public int UnitNumber { get; set; }
    }
}
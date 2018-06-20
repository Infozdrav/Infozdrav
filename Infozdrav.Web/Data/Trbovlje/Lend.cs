using System;
using Infozdrav.Web.Data.Manage;

namespace Infozdrav.Web.Data.Trbovlje
{
    public class Lend : Entity
    {
        public User LendGiveUser { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public DateTime LendGiveTime { get; set; }
        public int LaboratoryId { get; set; }
        public Laboratory Laboratory { get; set; }
        public int UnitsUsed { get; set; }
        public DateTime? LendReciveTime { get; set; }
        public User LendReciveUser { get; set; }
    }
}
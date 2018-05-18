using System;
using System.Collections.Generic;
using System.IO;
using Infozdrav.Web.Data.Manage;

namespace Infozdrav.Web.Data
{
    public class ArticleUse : Entity
    {
        public int ArticleId { get; set; }
        public Article Article { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime UseTime { get; set; }
        public int UnitNumber { get; set; }
    }
}
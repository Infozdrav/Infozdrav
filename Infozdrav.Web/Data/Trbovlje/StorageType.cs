using System;
using System.Collections.Generic;

namespace Infozdrav.Web.Data
{
    public class StorageType : Entity
    {
        public string Name { get; set; }
    }

    public sealed class StorageTypes
    {
        public static readonly string Frezzer = "-20 °c";
        public static readonly string Refrigerator = "2-8 °c";
        public static readonly string Room = "18-25 °c";
    }
}
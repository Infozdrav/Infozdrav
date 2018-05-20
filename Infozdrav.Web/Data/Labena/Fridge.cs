using System;

namespace Infozdrav.Web.Data
{
    public class Fridge : Entity
    {
        public Room Place { get; set; }
        public string Name { get; set; }
    }
}
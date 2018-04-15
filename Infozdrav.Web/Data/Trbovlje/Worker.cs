using System.Collections.Generic;

namespace Infozdrav.Web.Data
{
    public class Worker : Entity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string DateOfBirth { get; set; }
        public string Workplace { get; set; }

    }
}
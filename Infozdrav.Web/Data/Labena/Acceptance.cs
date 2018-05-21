using System;

namespace Infozdrav.Web.Data
{
    public class Acceptance : Entity
    {
        public DateTime Date { get; set; }
        public Subscriber SubscriberName { get; set; }
        public ContactPerson Contact { get; set; }
    }
}
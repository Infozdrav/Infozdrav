using System;

namespace Infozdrav.Web.Data
{
    public class Acceptance : Entity
    {
        public int AcceptanceId { get; set; }
        public DateTime Date { get; set; }
        public Subscriber SubscriberName { get; set; }
        public ContactPerson Contact { get; set; }
    }
}
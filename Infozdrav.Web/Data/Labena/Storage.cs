namespace Infozdrav.Web.Data
{
    public class Storage : Entity
    {
        public Fridge FridgeName { get; set; }
        public string Temperature { get; set; }
        public int Section { get; set; }
        public Box BoxName { get; set; }
        public int Position { get; set; }
    }
}
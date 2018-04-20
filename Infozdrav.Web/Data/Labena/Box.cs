namespace Infozdrav.Web.Data
{
    public class Box : Entity
    {
        public string BoxName { set; get; }
        public int Volume { set; get; }
        public bool Cryovial { set; get; }
        public int Size { set; get; }
    }
}
namespace Nantou_bus.Model
{
    public class Route
    {
        public string UID { get; set; }

        public bool Direction { get; set; }

        public string HeadSign { get; set; }

        public Name Name { get; set; }

        public string Number { get; set; }

        public string ParentUID { get; set; }
    }
}

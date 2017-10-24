namespace Nantou_bus.Model
{
    public class Operator
    {
        public string Authority { get; set; }

        public string ID { get; private set; }

        public string Name { get; private set; }

        public string Phone { get; set; }

        public string Tel { get; set; }

        public Operator(string id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}

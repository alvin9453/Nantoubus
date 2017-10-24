using SQLite;

namespace Nantou_bus.Model
{
    public abstract class AbstractRecord
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Name { get; set; }
    }
}

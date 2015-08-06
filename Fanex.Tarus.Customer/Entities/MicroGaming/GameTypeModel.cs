namespace Fanex.Tarus.Customer.Entities.MicroGaming
{
    public class GameTypeModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal PositionTaking { get; set; }

        public decimal MaxPositionTaking { get; set; }

        public decimal MinPositionTaking { get; set; }

        public bool HasMinPositionTaking { get; set; }
    }
}

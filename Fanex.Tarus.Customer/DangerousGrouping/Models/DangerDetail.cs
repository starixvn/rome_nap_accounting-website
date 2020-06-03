namespace Fanex.Tarus.Customer.DangerousGrouping.Models
{
    public class DangerDetail
    {
        public DangerDetail()
        {
        }

        public DangerDetail(int id, string name, string color, string backgroundColor = "#ffffff")
        {
            Id = id;
            Name = name;
            Color = color;
            BackgroundColor = backgroundColor;
        }

        public string Color { get; set; }

        public string BackgroundColor { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }
    }
}
namespace Sunplus.Accounting.Business.Customer
{
    public class DangerousDetail
    {
        public DangerousDetail()
        {
        }

        public DangerousDetail(int id, string name, string color, string backgroundColor = "#ffffff")
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

    public class DangerousType
    {
        public DangerousType()
        {
        }

        public DangerousType(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }

        public string Name { get; set; }
    }
}
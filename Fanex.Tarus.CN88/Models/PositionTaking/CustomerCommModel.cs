namespace Fanex.Tarus.CN88.Models.PositionTaking
{
    public class CustomerCommModel
    {
        public string CommGroup { get; set; }

        public SportCommModel Soccer { get; set; }

        public SportCommModel Basketball { get; set; }

        public SportCommModel OtherSports { get; set; }

        public SportCommModel AllSports { get; set; }
    }
}

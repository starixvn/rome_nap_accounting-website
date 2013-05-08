namespace Sunplus.Accounting.Business.PositionTaking.SportBook
{
    /// <summary>
    /// Sport Position Taking
    /// </summary>
    public class SportDetail
    {
        public SportDetail()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        public SportDetail(string name, int id, string bettype)
        {
            Name = name;
            Id = id;
            BetTypes = bettype;
        }

        public string Name { get; set; }

        public int Id { get; set; }

        public string BetTypes { get; set; }
    }
}
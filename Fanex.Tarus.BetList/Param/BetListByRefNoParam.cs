namespace Fanex.Tarus.BetList.Param
{
    public class BetListByRefNoParam
    {
        public BetListByRefNoParam()
        {
            Language = Constants.Language.Default;
        }

        public int UserId { get; set; }

        public string RefNo { get; set; }

        public string Language { get; set; }

        public string Permission { get; set; }       

    }
}

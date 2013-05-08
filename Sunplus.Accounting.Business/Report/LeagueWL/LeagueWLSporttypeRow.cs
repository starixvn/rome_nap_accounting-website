 /// <summary>
/// Summary description for LeagueWLSporttypeRow
/// </summary>

namespace BLL.Report.LeagueWL
{

    #region LeagueWLRow

    public class LeagueWLRow
    {
        private string _Header = string.Empty;
        private string _matchcode = string.Empty;
        public int sporttype { get; set; }

        public int leagueid { get; set; }

        public decimal WinLost { get; set; }

        public decimal Position { get; set; }

        public decimal Commission { get; set; }

        public decimal LineTotal { get; set; }

        public decimal Turnover { get; set; }

        public decimal Margin
        {
            get
            {
                decimal rs = 0;
                if (Turnover == 0)
                    rs = 0;
                else
                    rs = (LineTotal/Turnover)*100;

                return rs;
            }
        }

        public string Header
        {
            get { return _Header; }
            set { _Header = value; }
        }

        public bool liveindicator { get; set; }

        public int Bettype { get; set; }

        public string isHedge { get; set; }

        public string matchcode
        {
            get { return _matchcode; }
            set { _matchcode = value; }
        }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public bool IsNeutral { get; set; }
    }

    #endregion
}
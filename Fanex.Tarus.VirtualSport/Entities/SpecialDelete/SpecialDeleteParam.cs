

using System;

namespace Fanex.Tarus.VirtualSport.Entities.SpecialDelete
{
    [Serializable]
    public class SpecialDeleteParam
    {
        #region IFields

        public string MatchId { get; set; }
        public string FromRefNo { get; set; }
        public string ToRefNo { get; set; }
        public string Hdp { get; set; }
        public string LiveIndicator { get; set; }
        public string Bettypes { get; set; }
        public string Betteam1378 { get; set; }
        public string Betteam515 { get; set; }
        public string Betteam4 { get; set; }
        public string Betteam16 { get; set; }
        public string Betteam6 { get; set; }
        public string Betteam14 { get; set; }
        public string Type { get; set; }
        public string HomeScore { get; set; }
        public string AwayScore { get; set; }
        public string FromTransDate { get; set; }
        public string ToTransDate { get; set; }
        public string Betteam430 { get; set; } // update for 1 first correct score
        public int SelectBettype { get; set; }
        public int SAFilter { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string UserIp { get; set; }

        public string DelOptions { get; set; }
        public string TransIds { get; set; }
        public string DelOption_MHC { get; set; }
        public string TransIds_MHC { get; set; }
        public string DelOption_BD { get; set; }
        public string TransIds_BD { get; set; }
        public string DelOption_12EU { get; set; }
        public string TransIds_12EU { get; set; }
        public Message SpecialDeleteMessage { get; set; }

        #endregion IFields
    }

}

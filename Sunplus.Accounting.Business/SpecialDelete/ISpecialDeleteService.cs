using System.Collections.Generic;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.SpecialDelete;
using Sunplus.Accounting.Entities.SpecialDelete.EarlyKO;
using Sunplus.Accounting.Entities.SpecialDelete.Mixparlay;

namespace Sunplus.Accounting.Business.SpecialDelete
{
    public interface ISpecialDeleteService
    {
        Result DeleteSpecialTransaction(string matchId, string frefno, string trefno, string bettype,
                                          string betteam1378, string betteam515, string hdp, string uname,
                                          string userId, string pass, string DelOption, string transids,
                                          string liveindicator, string DelOption_BD, string transids_BD,
                                          string DelOption_12EU, string transids_12EU,
                                          string DelOption_MHC, string transids_MHC, string betteam430,
                                          string betteam6, string betteam14, string betteam16, Message message, string homescore, string awayscore, string fromtransdate, string totransdate, int SAFilter);

        SpecialDeleteDTO SelRecoverBets(string transid);

        Result RecoverBets(string transid, int UserID);

        #region EarlyKOBet

        SpecialDeleteDTO SelectEarlyKOBet(SpecialDeleteParamEarlyKO param);

        MixparlayChoice SelectEarlyKOMixparlay(string refno, string transid, string currency, string subtransid);

        Result DeleteSpecialEarlyKO(string matchId, string bettype,
                                          string betteam1378, string betteam515, string hdp, string uname,
                                          string userId, string pass, string DelOption, string transids,
                                          string kickoffdate, string DelOption_BD, string transids_BD,
                                          string DelOption_12EU, string transids_12EU,
                                          string DelOption_MHC, string transids_MHC, string betteam430,
                                          string betteam6, string betteam14, string betteam16, Message message, int SAFilter);

        Dictionary<int, string> GetAllBettype(int type);

        List<Bettype> SelAllBettype(int type);

        #endregion EarlyKOBet

        #region Mixparlay

        MixParlayBO GetMPList(SpecialDeleteParam param);

        Dictionary<string, string> GetMPTicket(string transids, string matchid);

        Result DeleteMPTicket(string matchId, int Type, string HomeScore, string AwayScore, string bettypes,
                                          string betteam1378, string betteam515, string hdp, string uname,
                                          string userId, string pass,
                                          string DelOption, string transids,
                                          string DelOption_D, string transids_D,
                                          string DelOption_BD, string transids_BD, string DelOption_12EU,
                                          string transids_12EU, string DelOption_MHC, string transids_MHC, string betteam430,
                                          string betteam6, string betteam14, string betteam16, MessageMP message, string Fromtransdate, string Totransdate, string ParlayType);

        MixParlayBOPager GetMPListPager(SpecialDeleteParamPager param);

        #endregion Mixparlay

        Dictionary<int, string> CheckRefno(string matchid, string fromRefno, string toRefno, string type);
    }
}
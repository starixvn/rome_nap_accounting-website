using System.Collections.Generic;
using System.ServiceModel.Activation;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.SpecialDelete;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.SpecialDelete;
using Sunplus.Accounting.Entities.SpecialDelete.EarlyKO;
using Sunplus.Accounting.Entities.SpecialDelete.Mixparlay;

namespace Sunplus.Accounting.WcfServiceLibrary.SpecialDelete
{
    

    /// <summary>
    /// Summary description for MenuManager
    /// </summary>
    ///
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SpecialDeleteService : ISpecialDeleteService
    {
        public SpecialDeleteDTO GetSpecialDeleteNG(int matchId, long fromRefNo, long toRefNo, string live, string dead, string userName)
        {
            var specialDelete = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.SpecialDelete.NumberGame.ISpecialDeleteService>();
            return specialDelete.SelSpecialDeleteNumberGame(matchId, fromRefNo, toRefNo, live, dead, userName);
        }

        public Result DeleteSpecialTransactionNumberGame(int matchId, long frefno, long trefno, string live, string dead, int DelOption, string transids, int DelOption_BD, string transids_BD, int DelOption_12EU, string transids_12EU, int DelOption_MHC, string transids_MHC, Message message, string reason)
        {
            var specialDelete = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.SpecialDelete.NumberGame.ISpecialDeleteService>();
            return specialDelete.DeleteSpecialTransactionNumberGame(matchId, frefno, trefno, live, dead,
                                                                    DelOption, transids,
                                                                    DelOption_BD, transids_BD,
                                                                    DelOption_12EU, transids_12EU,
                                                                    DelOption_MHC, transids_MHC, message,
                                                                    reason);
        }

        public Result DeleteSpecialTransaction(string matchId, string frefno, string trefno, string bettype,
            string betteam1378, string betteam515, string hdp, string uname, string userId, string pass,
            string DelOption, string transids, string Liveindicator, string DelOption_BD, string transids_BD,
            string DelOption_12EU, string transids_12EU, string DelOption_MHC, string transids_MHC,
            string betteam430, string betteam6, string betteam14, string betteam16, Message message, string homescore, string awayscore, string Fromtransdate, string Totransdate, int SAFilter)
        {
            var specialDelete = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.SpecialDelete.ISpecialDeleteService>();
            return specialDelete.DeleteSpecialTransaction(matchId, frefno, trefno, bettype,
                                          betteam1378, betteam515, hdp, uname,
                                          userId, pass, DelOption, transids,
                                          Liveindicator, DelOption_BD, transids_BD,
                                          DelOption_12EU, transids_12EU,
                                          DelOption_MHC, transids_MHC, betteam430, betteam6, betteam14, betteam16, message, homescore, awayscore, Fromtransdate, Totransdate, SAFilter);
            //Result result = new Result();
            //return result;
        }

        public SpecialDeleteDTO SelRecoverBets(string transid)
        {
            var specialDelete = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.SpecialDelete.ISpecialDeleteService>();
            return specialDelete.SelRecoverBets(transid);
        }

        public Result RecoverBet(string transid)
        {
            var specialDelete = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.SpecialDelete.ISpecialDeleteService>();
            return specialDelete.RecoverBets(transid, AppSession.AccId);
        }

        public SpecialDeleteDTO BettransSelSpecialDel(SpecialDeleteParam param)
        {
            var specialDelete = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.SpecialDelete.NumberGame.ISpecialDeleteService>();
            return specialDelete.BettransSelSpecialDel(param);
        }

        public SpecialDeleteDTO GetEarlyKOBetList(SpecialDeleteParamEarlyKO param)
        {
            var specialDelete = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.SpecialDelete.ISpecialDeleteService>();
            return specialDelete.SelectEarlyKOBet(param);
        }

        /// <summary>
        /// GetEarlyKOMixparlay
        /// </summary>
        /// <param name="refno"></param>
        /// <param name="transid"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public MixparlayChoice GetEarlyKOMixparlay(string refno, string transid, string currency, string subtransid)
        {
            var specialDelete = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.SpecialDelete.ISpecialDeleteService>();
            return specialDelete.SelectEarlyKOMixparlay(refno, transid, currency, subtransid);
        }

        /// <summary>
        /// DeleteSpecialEarlyKO
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="frefno"></param>
        /// <param name="trefno"></param>
        /// <param name="bettype"></param>
        /// <param name="betteam1378"></param>
        /// <param name="betteam515"></param>
        /// <param name="hdp"></param>
        /// <param name="uname"></param>
        /// <param name="userId"></param>
        /// <param name="pass"></param>
        /// <param name="DelOption"></param>
        /// <param name="transids"></param>
        /// <param name="liveindicator"></param>
        /// <param name="DelOption_BD"></param>
        /// <param name="transids_BD"></param>
        /// <param name="DelOption_12EU"></param>
        /// <param name="transids_12EU"></param>
        /// <param name="DelOption_MHC"></param>
        /// <param name="transids_MHC"></param>
        /// <param name="betteam430"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Result DeleteSpecialEarlyKO(string matchId, string bettype, string betteam1378, string betteam515, string hdp,
            string uname, string userId, string pass, string DelOption, string transids, string kickoffdate, string DelOption_BD,
            string transids_BD, string DelOption_12EU, string transids_12EU, string DelOption_MHC, string transids_MHC,
            string betteam430, string betteam6, string betteam14, string betteam16, Message message, int SAFilter)
        {
            var specialDelete = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.SpecialDelete.ISpecialDeleteService>();
            return specialDelete.DeleteSpecialEarlyKO(matchId, bettype,
                                          betteam1378, betteam515, hdp, uname,
                                          userId, pass, DelOption, transids,
                                          kickoffdate, DelOption_BD, transids_BD,
                                          DelOption_12EU, transids_12EU,
                                          DelOption_MHC, transids_MHC, betteam430, betteam6, betteam14, betteam16, message, SAFilter);
            //Result result = new Result();
            //return result;
        }

        public Dictionary<int, string> GetBettype(int type)
        {
            var specialDelete = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.SpecialDelete.ISpecialDeleteService>();
            Dictionary<int, string> bettypes = specialDelete.GetAllBettype(type);
            return bettypes;
        }

        public MixParlayBO GetMPList(SpecialDeleteParam param)
        {
            var specialDelete = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.SpecialDelete.ISpecialDeleteService>();
            MixParlayBO mixParlayBO = specialDelete.GetMPList(param);
            return mixParlayBO;
        }

        public Dictionary<string, string> GetMPTicket(string transids, string matchid)
        {
            var specialDelete = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.SpecialDelete.ISpecialDeleteService>();
            return specialDelete.GetMPTicket(transids, matchid);
        }

        public Result DelMPList(string matchId, int Type, string HomeScore, string AwayScore, string bettypes,
            string betteam1378, string betteam515, string hdp,
            string uname, string userId, string pass,
            string DelOption, string transids,
            string DelOption_D, string transids_D,
            string DelOption_BD, string transids_BD,
            string DelOption_12EU, string transids_12EU,
            string DelOption_MHC, string transids_MHC,
            string betteam430, string betteam6, string betteam14, string betteam16, string Fromtransdate, string Totransdate, string ParlayType,
            MessageMP message)
        {
            var specialDelete = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.SpecialDelete.ISpecialDeleteService>();
            return specialDelete.DeleteMPTicket(matchId, Type, HomeScore, AwayScore, bettypes,
                                          betteam1378, betteam515, hdp,
                                          uname, userId, pass,
                                          DelOption, transids,
                                          DelOption_D, transids_D,
                                          DelOption_BD, transids_BD,
                                          DelOption_12EU, transids_12EU,
                                          DelOption_MHC, transids_MHC,
                                          betteam430, betteam6, betteam14, betteam16, message, Fromtransdate, Totransdate, ParlayType);
        }

        public MixParlayBOPager GetMPListPager(SpecialDeleteParamPager param)
        {
            var specialDelete = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.SpecialDelete.ISpecialDeleteService>();
            return specialDelete.GetMPListPager(param);
        }

        public Dictionary<int, string> CheckRefno(string matchid, string fromRefno, string toRefno, string type)
        {
            var specialDelete = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.SpecialDelete.ISpecialDeleteService>();
            return specialDelete.CheckRefno(matchid, fromRefno, toRefno, type);
        }

        public Dictionary<string, string> UploadMsg(Message pmsg, string TransIds)
        {
            var msg = new UpdateBatchMsg();
            return msg.UploadMsg(pmsg, TransIds);
        }
    }
}
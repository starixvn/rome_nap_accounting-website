using System;
using System.Web;

//using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.Business.PositionTaking.SportBook
{
    /// <summary>
    /// Summary description for PtManager
    /// </summary>
    public class PtManager : IPtService
    {
        #region Level enum

        public enum Level
        {
            Super = 4,
            Master = 3,
            Agent = 2,
            Member = 1
        }

        #endregion Level enum

        #region PtType enum

        public enum PtType
        {
            EditSuper = 0,
            EditMaster = 1,
            EditAgent = 2,
            EditSuperOnMember = 3,
            EditMasterOnMember = 4,
            EditAgentOnMember = 5,
            AddNewSuper = 6,
            AddNewMaster = 7,
            AddNewAgent = 8,
            AddNewMember = 9
        }

        #endregion PtType enum

        #region IPtService Members

        /// <summary>
        /// Insert New Super, Master, Agent or Member
        /// </summary>
        /// <param name="level">Super=4, Master=3, Agent=2 or Member=1</param>
        /// <param name="ptType"> EditSuper = 0, EditMaster = 1, EditAgent = 2, EditSuperOnMember = 3, EditMasterOnMember =4, EditAgentOnMember = 5,
        ///AddNewSuper = 6, AddNewMaster = 7, AddNewAgent = 8, AddNewMember = 9</param>
        /// <param name="httpRequest"></param>
        /// <returns></returns>
        public string Insert(HttpRequest httpRequest)
        {
            try
            {
                var basePt = new BasePT();
                return basePt.Insert(httpRequest);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return string.Empty;
            }
        }

        public string Update(HttpRequest httpRequest)
        {
            try
            {
                var basePt = new BasePT();
                return basePt.Update(httpRequest);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return string.Empty;
            }
        }

        public string Select(Level level, PtType ptType)
        {
            try
            {
                BasePT basePt = null;
                switch (level)
                {
                    case Level.Super:
                        basePt = new SuperPT();
                        break;

                    case Level.Master:
                        basePt = new MasterPT();
                        break;

                    case Level.Agent:
                        basePt = new AgentPT();
                        break;

                    case Level.Member:
                        basePt = new MemberPT();
                        break;
                }
                return basePt.Select(ptType);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return string.Empty;
            }
        }

        #endregion IPtService Members
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DNAide.Core;

using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess.PositionTaking.Racing;
using Sunplus.Accounting.Entities.PositionTaking.Racing;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.Business.PT.Racing
{
    public class MeetingCommManager : BaseManager, IMeetingCommManager
    {
        private static IMeetingCommDAO _mettingCommDAO;
        public MeetingCommManager(IMeetingCommDAO mettingCommDao)
        {
            _mettingCommDAO = mettingCommDao;
        }

        public List<Partner> GetPartnerList()
        {
            var listPartner = new List<Partner>();
            try
            {
                var dbResult = _mettingCommDAO.GetPartnerList();
                if (dbResult != null && dbResult.ResultSet != null && dbResult.ResultSet.Tables.Count > 0)
                {
                    var rows = dbResult.ResultSet.Tables[0].Rows;
                    listPartner.AddRange(from DataRow row in rows
                                         select
                                             new Partner() { 
                                                 PartnerId = CastAide.AsInt32(row["partnerid"]), 
                                                 PartnerName = row["partnername"].ToString()});
                }
            } 
            catch(Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return listPartner;
        }

        public List<PartnerItem> GetPartnerDetail(int partnerId)
        {
            var partnerDetail = new List<PartnerItem>();
            try {
                var dbResult = _mettingCommDAO.GetPartnerDetail(partnerId);
                if (dbResult != null && dbResult.ResultSet != null && dbResult.ResultSet.Tables.Count > 0)
                {
                    var rows = dbResult.ResultSet.Tables[0].Rows;
                    var lsExchange = CacheUtils.Exchange;
                    partnerDetail.AddRange(from DataRow row in rows
                                           select new PartnerItem()
                                           {
                                                MeetingName = CastAide.AsString(RemotingRef.RefData.GetLeagueName(row["meetingid"].ToString(), "EN")),
                                                PoolCurrency = CacheUtils.GetExchangeName(lsExchange, CastAide.AsInt32(row["PoolCurrency"])),
                                                CommWin = CastAide.AsDecimal(row["totecommwin"]),
                                                CommPlace = CastAide.AsDecimal(row["totecommplace"]),
                                                Fee = CastAide.AsDecimal(row["totefee"])
                                         });
                }
            } 
            catch(Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return partnerDetail;
        }   
    }
}

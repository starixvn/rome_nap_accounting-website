using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess.IReportDAO;
using Sunplus.Accounting.Entities.Report.Others;

namespace Sunplus.Accounting.Business.Report.Other.DuplicateTickets
{
    public class DuplicateTickets:IDuplicateTickets
    {
        private IOthersReportDAO _othersReportDao;

        public DuplicateTickets(IOthersReportDAO iOthersReportDao)
        {
            _othersReportDao = iOthersReportDao;
        }

        public DuplicateTicketsDTO SelDuplicateTickets(string wldate)
        {
            DuplicateTicketsDTO duplicate = new DuplicateTicketsDTO();
            duplicate.Items = new List<Entities.Report.Others.DuplicateTickets>();
            try
            {
                DbResult dbResult = _othersReportDao.SelDuplicateTickets(wldate);
                DataTable dataTable = new DataTable();
                if (dbResult != null && dbResult.ResultSet.Tables.Count > 0)
                    dataTable = dbResult.ResultSet.Tables[0];

                DataRow[] rows = dataTable.Select();

                foreach (DataRow row in rows)
                {
                    duplicate.Items.Add(new Entities.Report.Others.DuplicateTickets()
                                            {
                                                Username = CastAide.AsString(row["username"]),
                                                CustId = CastAide.AsString(row["custid"]),
                                                Bettype = CastAide.AsString(row["typenamee"]),
                                                WinLostDate = CastAide.AsDateTime(row["winlostdate"]).ToShortDateString(),
                                                BalanceBeforePlaceBet = NumberFormat.DecFormat(row["balance"],2),
                                                Stake = NumberFormat.DecFormat(row["stake"],2),
                                                Odds = NumberFormat.DecFormat(row["odds"], 3),
                                                Status = CastAide.AsString(row["status"]),
                                                RefNo1stTicket = CastAide.AsString(row["refno1"]),
                                                RefNo2ndTicket = CastAide.AsString(row["refno2"]),
                                                BetTime1stTicket = CastAide.AsDateTime(row["bettime1"]).ToString(),
                                                BetTime2ndTicket = CastAide.AsDateTime(row["bettime2"]).ToString(),
                                                TimeDiff = NumberFormat.DecFormat(row["timediff"], 0),
                                            });
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return duplicate;
        }
    }
}

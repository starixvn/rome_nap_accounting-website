using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using DNAide.Core;
using DNAide.Utils;
using ExcelUtil.Spreadsheet;

using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess.IReportDAO;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Report.Racebook;
using Sunplus.Core.LoggingLib;

/// <summary>
/// Summary description for Winloss Racing
/// </summary>
namespace Sunplus.Accounting.Business.Report.Racing
{
    public partial class ReportRacing : BaseManager, IRequiresSessionState, IReportRacing
    {
        private static IReportRacingDAO _reportRacingDao;
        private string _winLossRacingReportSessionKey = "winlossracingreportsessionkey{0}";
        private string _winLossRacingReportDetailSessionKey = "winlossracingreportdetailsessionkey{0}_{1}";
        private string _winLossRacingGroupReportSessionKey = "winlossracinggroupreportsessionkey";
        private string _winLossRacingGroupForToteReportSessionKey = "winlossracinggroupfortotereportsessionkey";
        public ReportRacing(IReportRacingDAO reportRacingDao)
		{
            _reportRacingDao = reportRacingDao;
		}

        #region GetWinlossRacingReport
        public RacingWinlossBO GetWinlossRacingReport(Enumerations.ReportType reportType, string winlossDate)
        {
            var racingBO = new RacingWinlossBO();
            racingBO.Items=new List<RacingWinlossMeetingBO>();
            racingBO.Totals=new List<RacingWinlossRowBO>();

            DataTable dt = _reportRacingDao.WinlostRacingTote(reportType, winlossDate);
            
            #region variable
            
            decimal AgencyWinTO;
            decimal AgencyPlaceTO;
            decimal AgencyWPTO;
            decimal AgencyTotalTO;
            decimal AgencyCommIncome;
            decimal AgencyPTWL;

            decimal IBCWinTO;
            decimal IBCPlaceTO;
            decimal IBCWPTO;
            decimal IBCTotalTO;
            decimal IBCCommIncome;
            decimal IBCToteFee;
            decimal IBCCommExpense;
            decimal IBCPTWL;
            decimal IBCNetIncome;

            decimal PartnerWinTO;
            decimal PartnerPlaceTO;
            decimal PartnerWPTO;
            decimal PartnerTotalTO;
            decimal PartnerDividendWon;
            decimal PartnerCommExpense;
            decimal PartnerToteFee;
            decimal PartnerAmountDue;

            //Pool total
            decimal AgencyWinTO_pool;
            decimal AgencyPlaceTO_pool;
            decimal AgencyWPTO_pool;
            decimal AgencyTotalTO_pool;
            decimal AgencyCommIncome_pool;
            decimal AgencyPTWL_pool;

            decimal IBCWinTO_pool;
            decimal IBCPlaceTO_pool;
            decimal IBCWPTO_pool;
            decimal IBCTotalTO_pool;
            decimal IBCCommIncome_pool;
            decimal IBCToteFee_pool;
            decimal IBCCommExpense_pool;
            decimal IBCPTWL_pool;
            decimal IBCNetIncome_pool;

            decimal PartnerWinTO_pool;
            decimal PartnerPlaceTO_pool;
            decimal PartnerWPTO_pool;
            decimal PartnerTotalTO_pool;
            decimal PartnerDividendWon_pool;
            decimal PartnerCommExpense_pool;
            decimal PartnerToteFee_pool;
            decimal PartnerAmountDue_pool;

            //Base total
            decimal AgencyWinTO_base;
            decimal AgencyPlaceTO_base;
            decimal AgencyWPTO_base;
            decimal AgencyTotalTO_base;
            decimal AgencyCommIncome_base;
            decimal AgencyPTWL_base;

            decimal IBCWinTO_base;
            decimal IBCPlaceTO_base;
            decimal IBCWPTO_base;
            decimal IBCTotalTO_base;
            decimal IBCCommIncome_base;
            decimal IBCToteFee_base;
            decimal IBCCommExpense_base;
            decimal IBCPTWL_base;
            decimal IBCNetIncome_base;

            decimal PartnerWinTO_base;
            decimal PartnerPlaceTO_base;
            decimal PartnerWPTO_base;
            decimal PartnerTotalTO_base;
            decimal PartnerDividendWon_base;
            decimal PartnerCommExpense_base;
            decimal PartnerToteFee_base;
            decimal PartnerAmountDue_base;
            #endregion
            
            if (dt != null)
            {
                var listPartnerIds = GetDistinctPartner(dt);
                var racingwinloss=new RacingWinlossBO();
                racingwinloss.Items=new List<RacingWinlossMeetingBO>();
                racingwinloss.Totals=new List<RacingWinlossRowBO>();
                foreach (PartnerPoolCurr partner in listPartnerIds)
                {
                    
                    #region assign default

                    AgencyWinTO_pool = 0;
                    AgencyPlaceTO_pool = 0;
                    AgencyWPTO_pool = 0;
                    AgencyTotalTO_pool = 0;
                    AgencyCommIncome_pool = 0;
                    AgencyPTWL_pool = 0;

                    IBCWinTO_pool = 0;
                    IBCPlaceTO_pool = 0;
                    IBCWPTO_pool = 0;
                    IBCTotalTO_pool = 0;
                    IBCCommIncome_pool = 0;
                    IBCToteFee_pool = 0;
                    IBCCommExpense_pool = 0;
                    IBCPTWL_pool = 0;
                    IBCNetIncome_pool = 0;

                    PartnerWinTO_pool = 0;
                    PartnerPlaceTO_pool = 0;
                    PartnerWPTO_pool = 0;
                    PartnerTotalTO_pool = 0;
                    PartnerDividendWon_pool = 0;
                    PartnerCommExpense_pool = 0;
                    PartnerToteFee_pool = 0;
                    PartnerAmountDue_pool = 0;

                    AgencyWinTO_base = 0;
                    AgencyPlaceTO_base = 0;
                    AgencyWPTO_base = 0;
                    AgencyTotalTO_base = 0;
                    AgencyCommIncome_base = 0;
                    AgencyPTWL_base = 0;

                    IBCWinTO_base = 0;
                    IBCPlaceTO_base = 0;
                    IBCWPTO_base = 0;
                    IBCTotalTO_base = 0;
                    IBCCommIncome_base = 0;
                    IBCToteFee_base = 0;
                    IBCCommExpense_base = 0;
                    IBCPTWL_base = 0;
                    IBCNetIncome_base = 0;

                    PartnerWinTO_base = 0;
                    PartnerPlaceTO_base = 0;
                    PartnerWPTO_base = 0;
                    PartnerTotalTO_base = 0;
                    PartnerDividendWon_base = 0;
                    PartnerCommExpense_base = 0;
                    PartnerToteFee_base = 0;
                    PartnerAmountDue_base = 0;
                    #endregion

                    var partners = new RacingWinlossMeetingBO();
                    partners.Items = new List<RacingWinlossRowBO>();
                    partners.Totals = new List<RacingWinlossRowBO>();
                    DataRow[] rows = dt.Select("partnerID=" + partner.PartnerId);
                    
                    foreach (DataRow row in rows)
                    {
                        var item = new RacingWinlossRowBO();
                        
                        #region origin
                        AgencyWinTO =CastAide.AsDecimal(row["winTO_Agency_Origin"].ToString());
                        AgencyPlaceTO = CastAide.AsDecimal(row["placeTO_Agency_Origin"].ToString());
                        AgencyWPTO = CastAide.AsDecimal(row["winplaceTO_Agency_Origin"].ToString());
                        AgencyTotalTO = CastAide.AsDecimal(row["totalTO_Agency_Origin"].ToString());
                        AgencyCommIncome = CastAide.AsDecimal(row["commIncome_Agency_Origin"].ToString());
                        AgencyPTWL = CastAide.AsDecimal(row["PTWL_Agency_Origin"].ToString());

                        IBCWinTO = CastAide.AsDecimal(row["winTO_IBC_Origin"].ToString());
                        IBCPlaceTO = CastAide.AsDecimal(row["placeTO_IBC_Origin"].ToString());
                        IBCWPTO = CastAide.AsDecimal(row["winplaceTO_IBC_Origin"].ToString());
                        IBCTotalTO = CastAide.AsDecimal(row["totalTO_IBC_Origin"].ToString());
                        IBCCommIncome = CastAide.AsDecimal(row["commIncome_IBC_Origin"].ToString());
                        IBCToteFee = CastAide.AsDecimal(row["FeeDue2_Origin"].ToString());
                        IBCCommExpense = CastAide.AsDecimal(row["commIncome_Agency_Origin"].ToString());
                        IBCPTWL = CastAide.AsDecimal(row["PTWL_IBC_Origin"].ToString());
                        IBCNetIncome = CastAide.AsDecimal(row["netIncome_IBC_Origin"].ToString());

                        PartnerWinTO = CastAide.AsDecimal(row["winTO_Partner_Origin"].ToString());
                        PartnerPlaceTO = CastAide.AsDecimal(row["placeTO_Partner_Origin"].ToString());
                        PartnerWPTO = CastAide.AsDecimal(row["winPlaceTO_Partner_Origin"].ToString());
                        PartnerTotalTO = CastAide.AsDecimal(row["totalTO_Partner_Origin"].ToString());
                        PartnerDividendWon = CastAide.AsDecimal(row["dividendWon_Origin"].ToString());
                        PartnerCommExpense = CastAide.AsDecimal(row["commIncome_IBC_Origin"].ToString());
                        PartnerToteFee = CastAide.AsDecimal(row["FeeDue2_Origin"].ToString());
                        PartnerAmountDue = CastAide.AsDecimal(row["AmountDue_Origin"].ToString());

                        item.Id = row["partnerID"].ToString() + "_" + row["PoolCurrency"].ToString() + "_" + row["currency"].ToString();
                        item.Name = row["PartnerName"].ToString();
                        item.Currency = row["currencyName"].ToString();

                        item.AgencyWinTO = NumberFormat.DecFormat(AgencyWinTO, 2);
                        item.AgencyPlaceTO = NumberFormat.DecFormat(AgencyPlaceTO, 2);
                        item.AgencyWPTO = NumberFormat.DecFormat(AgencyWPTO, 2);
                        item.AgencyTotalTO = NumberFormat.DecFormat(AgencyTotalTO, 2);
                        item.AgencyCommIncome = NumberFormat.DecFormat(AgencyCommIncome, 2);
                        item.AgencyPTWL = NumberFormat.DecFormat(AgencyPTWL, 2);

                        item.IBCWinTO = NumberFormat.DecFormat(IBCWinTO, 2);
                        item.IBCPlaceTO = NumberFormat.DecFormat(IBCPlaceTO, 2);
                        item.IBCWPTO = NumberFormat.DecFormat(IBCWPTO, 2);
                        item.IBCTotalTO = NumberFormat.DecFormat(IBCTotalTO, 2);
                        item.IBCCommIncome = NumberFormat.DecFormat(IBCCommIncome, 2);
                        item.IBCToteFee = NumberFormat.DecFormat(IBCToteFee, 2);
                        item.IBCCommExpense = NumberFormat.DecFormat(IBCCommExpense, 2);
                        item.IBCPTWL = NumberFormat.DecFormat(IBCPTWL, 2);
                        item.IBCNetIncome = NumberFormat.DecFormat(IBCNetIncome, 2);

                        item.PartnerWinTO = NumberFormat.DecFormat(PartnerWinTO, 2);
                        item.PartnerPlaceTO = NumberFormat.DecFormat(PartnerPlaceTO, 2);
                        item.PartnerWPTO = NumberFormat.DecFormat(PartnerWPTO, 2);
                        item.PartnerTotalTO = NumberFormat.DecFormat(PartnerTotalTO, 2);
                        item.PartnerDividendWon = NumberFormat.DecFormat(PartnerDividendWon, 2);
                        item.PartnerCommExpense = NumberFormat.DecFormat(PartnerCommExpense, 2);
                        item.PartnerToteFee = NumberFormat.DecFormat(PartnerToteFee, 2);
                        item.PartnerAmountDue = NumberFormat.DecFormat(PartnerAmountDue, 2);

                        partners.Items.Add(item);

                        #endregion

                        #region PoolCurr
                        AgencyWinTO_pool +=CastAide.AsDecimal(row["winTO_Agency_PoolCurr"].ToString());
                        AgencyPlaceTO_pool += CastAide.AsDecimal(row["placeTO_Agency_PoolCurr"].ToString());
                        AgencyWPTO_pool += CastAide.AsDecimal(row["winplaceTO_Agency_PoolCurr"].ToString());
                        AgencyTotalTO_pool += CastAide.AsDecimal(row["totalTO_Agency_PoolCurr"].ToString());
                        AgencyCommIncome_pool += CastAide.AsDecimal(row["commIncome_Agency_PoolCurr"].ToString());
                        AgencyPTWL_pool += CastAide.AsDecimal(row["PTWL_Agency_PoolCurr"].ToString());

                        IBCWinTO_pool += CastAide.AsDecimal(row["winTO_IBC_PoolCurr"].ToString());
                        IBCPlaceTO_pool += CastAide.AsDecimal(row["placeTO_IBC_PoolCurr"].ToString());
                        IBCWPTO_pool += CastAide.AsDecimal(row["winplaceTO_IBC_PoolCurr"].ToString());
                        IBCTotalTO_pool += CastAide.AsDecimal(row["totalTO_IBC_PoolCurr"].ToString());
                        IBCCommIncome_pool += CastAide.AsDecimal(row["commIncome_IBC_PoolCurr"].ToString());
                        IBCToteFee_pool += CastAide.AsDecimal(row["FeeDue2_PoolCurr"].ToString());
                        IBCCommExpense_pool += CastAide.AsDecimal(row["commIncome_Agency_PoolCurr"].ToString());
                        IBCPTWL_pool += CastAide.AsDecimal(row["PTWL_IBC_PoolCurr"].ToString());
                        IBCNetIncome_pool += CastAide.AsDecimal(row["netIncome_IBC_PoolCurr"].ToString());

                        PartnerWinTO_pool += CastAide.AsDecimal(row["winTO_Partner_PoolCurr"].ToString());
                        PartnerPlaceTO_pool += CastAide.AsDecimal(row["placeTO_Partner_PoolCurr"].ToString());
                        PartnerWPTO_pool += CastAide.AsDecimal(row["winPlaceTO_Partner_PoolCurr"].ToString());
                        PartnerTotalTO_pool += CastAide.AsDecimal(row["totalTO_Partner_PoolCurr"].ToString());
                        PartnerDividendWon_pool += CastAide.AsDecimal(row["dividendWon_PoolCurr"].ToString());
                        PartnerCommExpense_pool += CastAide.AsDecimal(row["commIncome_IBC_PoolCurr"].ToString());
                        PartnerToteFee_pool += CastAide.AsDecimal(row["FeeDue2_PoolCurr"].ToString());
                        PartnerAmountDue_pool += CastAide.AsDecimal(row["AmountDue_PoolCurr"].ToString());
                        #endregion

                        #region base
                        AgencyWinTO_base += CastAide.AsDecimal(row["winTO_Agency"].ToString()); ;
                        AgencyPlaceTO_base += CastAide.AsDecimal(row["placeTO_Agency"].ToString()); ;
                        AgencyWPTO_base += CastAide.AsDecimal(row["winplaceTO_Agency"].ToString());
                        AgencyTotalTO_base += CastAide.AsDecimal(row["totalTO_Agency"].ToString());
                        AgencyCommIncome_base += CastAide.AsDecimal(row["commIncome_Agency"].ToString());
                        AgencyPTWL_base += CastAide.AsDecimal(row["PTWL_Agency"].ToString());

                        IBCWinTO_base += CastAide.AsDecimal(row["winTO_IBC"].ToString());
                        IBCPlaceTO_base += CastAide.AsDecimal(row["placeTO_IBC"].ToString());
                        IBCWPTO_base += CastAide.AsDecimal(row["winplaceTO_IBC"].ToString());
                        IBCTotalTO_base += CastAide.AsDecimal(row["totalTO_IBC"].ToString());
                        IBCCommIncome_base += CastAide.AsDecimal(row["commIncome_IBC"].ToString());
                        IBCToteFee_base += CastAide.AsDecimal(row["FeeDue2"].ToString());
                        IBCCommExpense_base += CastAide.AsDecimal(row["commIncome_Agency"].ToString());
                        IBCPTWL_base += CastAide.AsDecimal(row["PTWL_IBC"].ToString());
                        IBCNetIncome_base += CastAide.AsDecimal(row["netIncome_IBC"].ToString());

                        PartnerWinTO_base += CastAide.AsDecimal(row["winTO_Partner"].ToString());
                        PartnerPlaceTO_base += CastAide.AsDecimal(row["placeTO_Partner"].ToString());
                        PartnerWPTO_base += CastAide.AsDecimal(row["winPlaceTO_Partner"].ToString());
                        PartnerTotalTO_base += CastAide.AsDecimal(row["totalTO_Partner"].ToString());
                        PartnerDividendWon_base += CastAide.AsDecimal(row["dividendWon"].ToString());
                        PartnerCommExpense_base += CastAide.AsDecimal(row["commIncome_IBC"].ToString());
                        PartnerToteFee_base += CastAide.AsDecimal(row["FeeDue2"].ToString());
                        PartnerAmountDue_base += CastAide.AsDecimal(row["AmountDue"].ToString()); 
                        #endregion
                    }

                    #region add poolcurr item
                    var poolcurritem = new RacingWinlossRowBO();
                    poolcurritem.Id = partner.PartnerId.ToString();  
                    poolcurritem.Name = partners.Items[0].Name + " Total";
                    poolcurritem.Currency = CacheUtils.GetExchangeName(partner.PoolCurr);
                    poolcurritem.CurrencyId = partner.PoolCurr.ToString();

                    poolcurritem.AgencyWinTO = NumberFormat.DecFormat(AgencyWinTO_pool, 2);
                    poolcurritem.AgencyPlaceTO = NumberFormat.DecFormat(AgencyPlaceTO_pool, 2);
                    poolcurritem.AgencyWPTO = NumberFormat.DecFormat(AgencyWPTO_pool, 2);
                    poolcurritem.AgencyTotalTO = NumberFormat.DecFormat(AgencyTotalTO_pool, 2);
                    poolcurritem.AgencyCommIncome = NumberFormat.DecFormat(AgencyCommIncome_pool, 2);
                    poolcurritem.AgencyPTWL = NumberFormat.DecFormat(AgencyPTWL_pool, 2);

                    poolcurritem.IBCWinTO = NumberFormat.DecFormat(IBCWinTO_pool, 2);
                    poolcurritem.IBCPlaceTO = NumberFormat.DecFormat(IBCPlaceTO_pool, 2);
                    poolcurritem.IBCWPTO = NumberFormat.DecFormat(IBCWPTO_pool, 2);
                    poolcurritem.IBCTotalTO = NumberFormat.DecFormat(IBCTotalTO_pool, 2);
                    poolcurritem.IBCCommIncome = NumberFormat.DecFormat(IBCCommIncome_pool, 2);
                    poolcurritem.IBCToteFee = NumberFormat.DecFormat(IBCToteFee_pool, 2);
                    poolcurritem.IBCCommExpense = NumberFormat.DecFormat(IBCCommExpense_pool, 2);
                    poolcurritem.IBCPTWL = NumberFormat.DecFormat(IBCPTWL_pool, 2);
                    poolcurritem.IBCNetIncome = NumberFormat.DecFormat(IBCNetIncome_pool, 2);

                    poolcurritem.PartnerWinTO = NumberFormat.DecFormat(PartnerWinTO_pool, 2);
                    poolcurritem.PartnerPlaceTO = NumberFormat.DecFormat(PartnerPlaceTO_pool, 2);
                    poolcurritem.PartnerWPTO = NumberFormat.DecFormat(PartnerWPTO_pool, 2);
                    poolcurritem.PartnerTotalTO = NumberFormat.DecFormat(PartnerTotalTO_pool, 2);
                    poolcurritem.PartnerDividendWon = NumberFormat.DecFormat(PartnerDividendWon_pool, 2);
                    poolcurritem.PartnerCommExpense = NumberFormat.DecFormat(PartnerCommExpense_pool, 2);
                    poolcurritem.PartnerToteFee = NumberFormat.DecFormat(PartnerToteFee_pool, 2);
                    poolcurritem.PartnerAmountDue = NumberFormat.DecFormat(PartnerAmountDue_pool, 2);

                    partners.Totals.Add(poolcurritem);
                    #endregion

                    #region add base item
                    var baseitem = new RacingWinlossRowBO();
                    baseitem.Id = partner.PartnerId.ToString();
                    baseitem.Name = partners.Items[0].Name + " Total";
                    baseitem.Currency = "Base (RM)";

                    baseitem.AgencyWinTO = NumberFormat.DecFormat(AgencyWinTO_base, 2);
                    baseitem.AgencyPlaceTO = NumberFormat.DecFormat(AgencyPlaceTO_base, 2);
                    baseitem.AgencyWPTO = NumberFormat.DecFormat(AgencyWPTO_base, 2);
                    baseitem.AgencyTotalTO = NumberFormat.DecFormat(AgencyTotalTO_base, 2);
                    baseitem.AgencyCommIncome = NumberFormat.DecFormat(AgencyCommIncome_base, 2);
                    baseitem.AgencyPTWL = NumberFormat.DecFormat(AgencyPTWL_base, 2);

                    baseitem.IBCWinTO = NumberFormat.DecFormat(IBCWinTO_base, 2);
                    baseitem.IBCPlaceTO = NumberFormat.DecFormat(IBCPlaceTO_base, 2);
                    baseitem.IBCWPTO = NumberFormat.DecFormat(IBCWPTO_base, 2);
                    baseitem.IBCTotalTO = NumberFormat.DecFormat(IBCTotalTO_base, 2);
                    baseitem.IBCCommIncome = NumberFormat.DecFormat(IBCCommIncome_base, 2);
                    baseitem.IBCToteFee = NumberFormat.DecFormat(IBCToteFee_base, 2);
                    baseitem.IBCCommExpense = NumberFormat.DecFormat(IBCCommExpense_base, 2);
                    baseitem.IBCPTWL = NumberFormat.DecFormat(IBCPTWL_base, 2);
                    baseitem.IBCNetIncome = NumberFormat.DecFormat(IBCNetIncome_base, 2);

                    baseitem.PartnerWinTO = NumberFormat.DecFormat(PartnerWinTO_base, 2);
                    baseitem.PartnerPlaceTO = NumberFormat.DecFormat(PartnerPlaceTO_base, 2);
                    baseitem.PartnerWPTO = NumberFormat.DecFormat(PartnerWPTO_base, 2);
                    baseitem.PartnerTotalTO = NumberFormat.DecFormat(PartnerTotalTO_base, 2);
                    baseitem.PartnerDividendWon = NumberFormat.DecFormat(PartnerDividendWon_base, 2);
                    baseitem.PartnerCommExpense = NumberFormat.DecFormat(PartnerCommExpense_base, 2);
                    baseitem.PartnerToteFee = NumberFormat.DecFormat(PartnerToteFee_base, 2);
                    baseitem.PartnerAmountDue = NumberFormat.DecFormat(PartnerAmountDue_base, 2);

                    partners.Totals.Add(baseitem);
                    #endregion

                   
                    racingBO.Items.Add(partners);

                }

                #region Grand Total Pool and Base currency

                List<RacingWinlossRowBO> lstPoolCurrency = new List<RacingWinlossRowBO>();
                foreach (var racingWinlossBo in racingBO.Items)
                {
                    foreach (RacingWinlossRowBO item in racingWinlossBo.Totals)
                    {
                        if (string.IsNullOrEmpty(item.CurrencyId))
                            item.CurrencyId = string.Empty;
                        lstPoolCurrency.Add(item);
                    }
                }
                var listDistinctPoolCurrency =
                    lstPoolCurrency.OrderByDescending(c =>c.CurrencyId).Select(s => s.Currency).Distinct().ToArray();
                foreach (var currencyName in listDistinctPoolCurrency)
                {
                    racingBO.Totals.Add(new RacingWinlossRowBO()
                                                   {
                                                       Currency = currencyName,
                                                       AgencyWinTO = NumberFormat.DecFormat( lstPoolCurrency.Where(s=>s.Currency == currencyName).Sum(s=> CastAide.AsDecimal(s.AgencyWinTO)),2),
                                                       AgencyWPTO = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.AgencyWPTO)), 2),
                                                       AgencyTotalTO = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.AgencyTotalTO)), 2),
                                                       AgencyPlaceTO = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.AgencyPlaceTO)), 2),
                                                       AgencyPTWL = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.AgencyPTWL)), 2),
                                                       AgencyCommIncome = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.AgencyCommIncome)), 2),

                                                       IBCCommExpense = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.IBCCommExpense)), 2),
                                                       IBCCommIncome = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.IBCCommIncome)), 2),
                                                       IBCNetIncome = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.IBCNetIncome)), 2),
                                                       IBCPTWL = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.IBCPTWL )), 2),
                                                       IBCPlaceTO = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.IBCPlaceTO )), 2),
                                                       IBCTotalTO = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.IBCTotalTO)), 2),
                                                       IBCToteFee = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.IBCToteFee )), 2),
                                                       IBCWPTO = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.IBCWPTO)), 2),
                                                       IBCWinTO = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.IBCWinTO )), 2),

                                                       PartnerAmountDue = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.PartnerAmountDue )), 2),
                                                       PartnerCommExpense = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.PartnerCommExpense )), 2),
                                                       PartnerDividendWon = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.PartnerDividendWon )), 2),
                                                       Name = "Grand Total",
                                                       PartnerTotalTO = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.PartnerTotalTO )), 2),
                                                       PartnerPlaceTO = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.PartnerPlaceTO )), 2),
                                                       PartnerToteFee = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.PartnerToteFee)), 2),
                                                       PartnerWPTO = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.PartnerWPTO )), 2),
                                                       PartnerWinTO = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.PartnerWinTO)), 2),
                                                   });
                }
                #endregion

                #region Comparision
                //@reportType:1-UK Horse Racing ; 3-UK Greyhound
                if (reportType == Enumerations.ReportType.Horse_Racing_Daily_Report_For_AU ||
                    reportType == Enumerations.ReportType.Horse_Racing_Daily_Report_For_UK ||
                    reportType == Enumerations.ReportType.Greyhound_Daily_Report_For_UK ||
                    reportType == Enumerations.ReportType.Greyhound_Daily_Report_For_AU ||
                    reportType == Enumerations.ReportType.Horse_Racing_Report_for_SA_Tote_NEW)
                {
                    DataTable dtDumpFile = _reportRacingDao.WinlostRacingToteDumpFiles(winlossDate,(int) reportType);
                    var lstComparision = new List<RacingWinlossDumpFileBO>();
                    if (dtDumpFile != null)
                    {
                        if (dtDumpFile.Rows.Count > 0)
                        {
                            racingBO.IsGotDumpFile = true;

                            foreach (DataRow row in dtDumpFile.Rows)
                            {
                                var item = new RacingWinlossDumpFileBO();

                                item.currency = CastAide.AsString(row["currencyName"]);
                                item.partnerTO = NumberFormat.DecFormat(row["TO3rd"], 2);
                                item.partnerDividendWon = NumberFormat.DecFormat(row["DividendWon3rd"], 2);

                                item.ibcTO =
                                    NumberFormat.DecFormat(
                                        lstPoolCurrency.Where(
                                            s =>
                                            s.Currency.Substring(0, 3).ToLower() ==
                                            CastAide.AsString(row["currencyName"]).ToString().ToLower()).Sum(
                                                s => CastAide.AsDecimal(s.PartnerTotalTO)), 2);
                                item.ibcDividendWon =
                                    NumberFormat.DecFormat(
                                        lstPoolCurrency.Where(
                                            s =>
                                            s.Currency.Substring(0, 3).ToLower() ==
                                            CastAide.AsString(row["currencyName"]).ToString().ToLower()).Sum(
                                                s => CastAide.AsDecimal(s.PartnerDividendWon)), 2);


                                if (System.Math.Abs(CastAide.AsDecimal(item.ibcDividendWon) - CastAide.AsDecimal(item.partnerDividendWon)) < 1)
                                    item.IsMatchDividend = true;
                                if (System.Math.Abs(CastAide.AsDecimal(item.ibcTO) - CastAide.AsDecimal(item.partnerTO)) < 1)
                                    item.IsMatchTO = true;

                                lstComparision.Add(item);

                            }
                        }
                    }


                    racingBO.Comparision = lstComparision;
                }
               

                #endregion
            }

            SessionExtension.AddSession(HttpContext.Current.Session, string.Format(_winLossRacingReportSessionKey, reportType), racingBO);
            return racingBO;
        }

        public RacingWinlossBO GetWinlossRacingReportNew(Enumerations.ReportType reportType, string winlossDate)
        {
            var racingBO = new RacingWinlossBO();
            racingBO.Items = new List<RacingWinlossMeetingBO>();
            racingBO.Totals = new List<RacingWinlossRowBO>();

            DataTable dt = _reportRacingDao.WinlostRacingToteNew(reportType, winlossDate);

            #region variable

            decimal AgencyWinTO;
            decimal AgencyPlaceTO;
            decimal AgencyWPTO;
            decimal AgencyTotalTO;
            decimal AgencyCommIncome;
            decimal AgencyPTWL;

            decimal IBCWinTO;
            decimal IBCPlaceTO;
            decimal IBCWPTO;
            decimal IBCTotalTO;
            decimal IBCCommIncome;
            decimal IBCToteFee;
            decimal IBCCommExpense;
            decimal IBCPTWL;
            decimal IBCNetIncome;

            decimal PartnerWinTO;
            decimal PartnerPlaceTO;
            decimal PartnerWPTO;
            decimal PartnerTotalTO;
            decimal PartnerDividendWon;
            decimal PartnerCommExpense;
            decimal PartnerToteFee;
            decimal PartnerAmountDue;

            //Pool total
            decimal AgencyWinTO_pool;
            decimal AgencyPlaceTO_pool;
            decimal AgencyWPTO_pool;
            decimal AgencyTotalTO_pool;
            decimal AgencyCommIncome_pool;
            decimal AgencyPTWL_pool;

            decimal IBCWinTO_pool;
            decimal IBCPlaceTO_pool;
            decimal IBCWPTO_pool;
            decimal IBCTotalTO_pool;
            decimal IBCCommIncome_pool;
            decimal IBCToteFee_pool;
            decimal IBCCommExpense_pool;
            decimal IBCPTWL_pool;
            decimal IBCNetIncome_pool;

            decimal PartnerWinTO_pool;
            decimal PartnerPlaceTO_pool;
            decimal PartnerWPTO_pool;
            decimal PartnerTotalTO_pool;
            decimal PartnerDividendWon_pool;
            decimal PartnerCommExpense_pool;
            decimal PartnerToteFee_pool;
            decimal PartnerAmountDue_pool;

            //Base total
            decimal AgencyWinTO_base;
            decimal AgencyPlaceTO_base;
            decimal AgencyWPTO_base;
            decimal AgencyTotalTO_base;
            decimal AgencyCommIncome_base;
            decimal AgencyPTWL_base;

            decimal IBCWinTO_base;
            decimal IBCPlaceTO_base;
            decimal IBCWPTO_base;
            decimal IBCTotalTO_base;
            decimal IBCCommIncome_base;
            decimal IBCToteFee_base;
            decimal IBCCommExpense_base;
            decimal IBCPTWL_base;
            decimal IBCNetIncome_base;

            decimal PartnerWinTO_base;
            decimal PartnerPlaceTO_base;
            decimal PartnerWPTO_base;
            decimal PartnerTotalTO_base;
            decimal PartnerDividendWon_base;
            decimal PartnerCommExpense_base;
            decimal PartnerToteFee_base;
            decimal PartnerAmountDue_base;
            #endregion

            if (dt != null)
            {
                var listPartnerIds = GetDistinctPartner(dt);
                var racingwinloss = new RacingWinlossBO();
                racingwinloss.Items = new List<RacingWinlossMeetingBO>();
                racingwinloss.Totals = new List<RacingWinlossRowBO>();
                foreach (PartnerPoolCurr partner in listPartnerIds)
                {

                    #region assign default

                    AgencyWinTO_pool = 0;
                    AgencyPlaceTO_pool = 0;
                    AgencyWPTO_pool = 0;
                    AgencyTotalTO_pool = 0;
                    AgencyCommIncome_pool = 0;
                    AgencyPTWL_pool = 0;

                    IBCWinTO_pool = 0;
                    IBCPlaceTO_pool = 0;
                    IBCWPTO_pool = 0;
                    IBCTotalTO_pool = 0;
                    IBCCommIncome_pool = 0;
                    IBCToteFee_pool = 0;
                    IBCCommExpense_pool = 0;
                    IBCPTWL_pool = 0;
                    IBCNetIncome_pool = 0;

                    PartnerWinTO_pool = 0;
                    PartnerPlaceTO_pool = 0;
                    PartnerWPTO_pool = 0;
                    PartnerTotalTO_pool = 0;
                    PartnerDividendWon_pool = 0;
                    PartnerCommExpense_pool = 0;
                    PartnerToteFee_pool = 0;
                    PartnerAmountDue_pool = 0;

                    AgencyWinTO_base = 0;
                    AgencyPlaceTO_base = 0;
                    AgencyWPTO_base = 0;
                    AgencyTotalTO_base = 0;
                    AgencyCommIncome_base = 0;
                    AgencyPTWL_base = 0;

                    IBCWinTO_base = 0;
                    IBCPlaceTO_base = 0;
                    IBCWPTO_base = 0;
                    IBCTotalTO_base = 0;
                    IBCCommIncome_base = 0;
                    IBCToteFee_base = 0;
                    IBCCommExpense_base = 0;
                    IBCPTWL_base = 0;
                    IBCNetIncome_base = 0;

                    PartnerWinTO_base = 0;
                    PartnerPlaceTO_base = 0;
                    PartnerWPTO_base = 0;
                    PartnerTotalTO_base = 0;
                    PartnerDividendWon_base = 0;
                    PartnerCommExpense_base = 0;
                    PartnerToteFee_base = 0;
                    PartnerAmountDue_base = 0;
                    #endregion

                    var partners = new RacingWinlossMeetingBO();
                    partners.Items = new List<RacingWinlossRowBO>();
                    partners.Totals = new List<RacingWinlossRowBO>();
                    DataRow[] rows = dt.Select("partnerID=" + partner.PartnerId + " and PoolCurrency=" + partner.PoolCurr);

                    foreach (DataRow row in rows)
                    {
                        var item = new RacingWinlossRowBO();

                        #region origin
                        AgencyWinTO = CastAide.AsDecimal(row["winTO_Agency_Origin"].ToString());
                        AgencyPlaceTO = CastAide.AsDecimal(row["placeTO_Agency_Origin"].ToString());
                        AgencyWPTO = CastAide.AsDecimal(row["winplaceTO_Agency_Origin"].ToString());
                        AgencyTotalTO = CastAide.AsDecimal(row["totalTO_Agency_Origin"].ToString());
                        AgencyCommIncome = CastAide.AsDecimal(row["commIncome_Agency_Origin"].ToString());
                        AgencyPTWL = CastAide.AsDecimal(row["PTWL_Agency_Origin"].ToString());

                        IBCWinTO = CastAide.AsDecimal(row["winTO_IBC_Origin"].ToString());
                        IBCPlaceTO = CastAide.AsDecimal(row["placeTO_IBC_Origin"].ToString());
                        IBCWPTO = CastAide.AsDecimal(row["winplaceTO_IBC_Origin"].ToString());
                        IBCTotalTO = CastAide.AsDecimal(row["totalTO_IBC_Origin"].ToString());
                        IBCCommIncome = CastAide.AsDecimal(row["commIncome_IBC_Origin"].ToString());
                        IBCToteFee = CastAide.AsDecimal(row["FeeDue2_Origin"].ToString());
                        IBCCommExpense = CastAide.AsDecimal(row["commIncome_Agency_Origin"].ToString());
                        IBCPTWL = CastAide.AsDecimal(row["PTWL_IBC_Origin"].ToString());
                        IBCNetIncome = CastAide.AsDecimal(row["netIncome_IBC_Origin"].ToString());

                        PartnerWinTO = CastAide.AsDecimal(row["winTO_Partner_Origin"].ToString());
                        PartnerPlaceTO = CastAide.AsDecimal(row["placeTO_Partner_Origin"].ToString());
                        PartnerWPTO = CastAide.AsDecimal(row["winPlaceTO_Partner_Origin"].ToString());
                        PartnerTotalTO = CastAide.AsDecimal(row["totalTO_Partner_Origin"].ToString());
                        PartnerDividendWon = CastAide.AsDecimal(row["dividendWon_Origin"].ToString());
                        PartnerCommExpense = CastAide.AsDecimal(row["commIncome_IBC_Origin"].ToString());
                        PartnerToteFee = CastAide.AsDecimal(row["FeeDue2_Origin"].ToString());
                        PartnerAmountDue = CastAide.AsDecimal(row["AmountDue_Origin"].ToString());

                        item.Id = row["partnerID"].ToString() + "_" + row["PoolCurrency"].ToString() + "_" + row["currency"].ToString();
                        item.Name = row["PartnerName"].ToString();
                        item.Currency = row["currencyName"].ToString();

                        item.AgencyWinTO = NumberFormat.DecFormat(AgencyWinTO, 2);
                        item.AgencyPlaceTO = NumberFormat.DecFormat(AgencyPlaceTO, 2);
                        item.AgencyWPTO = NumberFormat.DecFormat(AgencyWPTO, 2);
                        item.AgencyTotalTO = NumberFormat.DecFormat(AgencyTotalTO, 2);
                        item.AgencyCommIncome = NumberFormat.DecFormat(AgencyCommIncome, 2);
                        item.AgencyPTWL = NumberFormat.DecFormat(AgencyPTWL, 2);

                        item.IBCWinTO = NumberFormat.DecFormat(IBCWinTO, 2);
                        item.IBCPlaceTO = NumberFormat.DecFormat(IBCPlaceTO, 2);
                        item.IBCWPTO = NumberFormat.DecFormat(IBCWPTO, 2);
                        item.IBCTotalTO = NumberFormat.DecFormat(IBCTotalTO, 2);
                        item.IBCCommIncome = NumberFormat.DecFormat(IBCCommIncome, 2);
                        item.IBCToteFee = NumberFormat.DecFormat(IBCToteFee, 2);
                        item.IBCCommExpense = NumberFormat.DecFormat(IBCCommExpense, 2);
                        item.IBCPTWL = NumberFormat.DecFormat(IBCPTWL, 2);
                        item.IBCNetIncome = NumberFormat.DecFormat(IBCNetIncome, 2);

                        item.PartnerWinTO = NumberFormat.DecFormat(PartnerWinTO, 2);
                        item.PartnerPlaceTO = NumberFormat.DecFormat(PartnerPlaceTO, 2);
                        item.PartnerWPTO = NumberFormat.DecFormat(PartnerWPTO, 2);
                        item.PartnerTotalTO = NumberFormat.DecFormat(PartnerTotalTO, 2);
                        item.PartnerDividendWon = NumberFormat.DecFormat(PartnerDividendWon, 2);
                        item.PartnerCommExpense = NumberFormat.DecFormat(PartnerCommExpense, 2);
                        item.PartnerToteFee = NumberFormat.DecFormat(PartnerToteFee, 2);
                        item.PartnerAmountDue = NumberFormat.DecFormat(PartnerAmountDue, 2);

                        partners.Items.Add(item);

                        #endregion

                        #region PoolCurr
                        AgencyWinTO_pool += CastAide.AsDecimal(row["winTO_Agency_PoolCurr"].ToString());
                        AgencyPlaceTO_pool += CastAide.AsDecimal(row["placeTO_Agency_PoolCurr"].ToString());
                        AgencyWPTO_pool += CastAide.AsDecimal(row["winplaceTO_Agency_PoolCurr"].ToString());
                        AgencyTotalTO_pool += CastAide.AsDecimal(row["totalTO_Agency_PoolCurr"].ToString());
                        AgencyCommIncome_pool += CastAide.AsDecimal(row["commIncome_Agency_PoolCurr"].ToString());
                        AgencyPTWL_pool += CastAide.AsDecimal(row["PTWL_Agency_PoolCurr"].ToString());

                        IBCWinTO_pool += CastAide.AsDecimal(row["winTO_IBC_PoolCurr"].ToString());
                        IBCPlaceTO_pool += CastAide.AsDecimal(row["placeTO_IBC_PoolCurr"].ToString());
                        IBCWPTO_pool += CastAide.AsDecimal(row["winplaceTO_IBC_PoolCurr"].ToString());
                        IBCTotalTO_pool += CastAide.AsDecimal(row["totalTO_IBC_PoolCurr"].ToString());
                        IBCCommIncome_pool += CastAide.AsDecimal(row["commIncome_IBC_PoolCurr"].ToString());
                        IBCToteFee_pool += CastAide.AsDecimal(row["FeeDue2_PoolCurr"].ToString());
                        IBCCommExpense_pool += CastAide.AsDecimal(row["commIncome_Agency_PoolCurr"].ToString());
                        IBCPTWL_pool += CastAide.AsDecimal(row["PTWL_IBC_PoolCurr"].ToString());
                        IBCNetIncome_pool += CastAide.AsDecimal(row["netIncome_IBC_PoolCurr"].ToString());

                        PartnerWinTO_pool += CastAide.AsDecimal(row["winTO_Partner_PoolCurr"].ToString());
                        PartnerPlaceTO_pool += CastAide.AsDecimal(row["placeTO_Partner_PoolCurr"].ToString());
                        PartnerWPTO_pool += CastAide.AsDecimal(row["winPlaceTO_Partner_PoolCurr"].ToString());
                        PartnerTotalTO_pool += CastAide.AsDecimal(row["totalTO_Partner_PoolCurr"].ToString());
                        PartnerDividendWon_pool += CastAide.AsDecimal(row["dividendWon_PoolCurr"].ToString());
                        PartnerCommExpense_pool += CastAide.AsDecimal(row["commIncome_IBC_PoolCurr"].ToString());
                        PartnerToteFee_pool += CastAide.AsDecimal(row["FeeDue2_PoolCurr"].ToString());
                        PartnerAmountDue_pool += CastAide.AsDecimal(row["AmountDue_PoolCurr"].ToString());
                        #endregion

                        #region base
                        AgencyWinTO_base += CastAide.AsDecimal(row["winTO_Agency"].ToString()); ;
                        AgencyPlaceTO_base += CastAide.AsDecimal(row["placeTO_Agency"].ToString()); ;
                        AgencyWPTO_base += CastAide.AsDecimal(row["winplaceTO_Agency"].ToString());
                        AgencyTotalTO_base += CastAide.AsDecimal(row["totalTO_Agency"].ToString());
                        AgencyCommIncome_base += CastAide.AsDecimal(row["commIncome_Agency"].ToString());
                        AgencyPTWL_base += CastAide.AsDecimal(row["PTWL_Agency"].ToString());

                        IBCWinTO_base += CastAide.AsDecimal(row["winTO_IBC"].ToString());
                        IBCPlaceTO_base += CastAide.AsDecimal(row["placeTO_IBC"].ToString());
                        IBCWPTO_base += CastAide.AsDecimal(row["winplaceTO_IBC"].ToString());
                        IBCTotalTO_base += CastAide.AsDecimal(row["totalTO_IBC"].ToString());
                        IBCCommIncome_base += CastAide.AsDecimal(row["commIncome_IBC"].ToString());
                        IBCToteFee_base += CastAide.AsDecimal(row["FeeDue2"].ToString());
                        IBCCommExpense_base += CastAide.AsDecimal(row["commIncome_Agency"].ToString());
                        IBCPTWL_base += CastAide.AsDecimal(row["PTWL_IBC"].ToString());
                        IBCNetIncome_base += CastAide.AsDecimal(row["netIncome_IBC"].ToString());

                        PartnerWinTO_base += CastAide.AsDecimal(row["winTO_Partner"].ToString());
                        PartnerPlaceTO_base += CastAide.AsDecimal(row["placeTO_Partner"].ToString());
                        PartnerWPTO_base += CastAide.AsDecimal(row["winPlaceTO_Partner"].ToString());
                        PartnerTotalTO_base += CastAide.AsDecimal(row["totalTO_Partner"].ToString());
                        PartnerDividendWon_base += CastAide.AsDecimal(row["dividendWon"].ToString());
                        PartnerCommExpense_base += CastAide.AsDecimal(row["commIncome_IBC"].ToString());
                        PartnerToteFee_base += CastAide.AsDecimal(row["FeeDue2"].ToString());
                        PartnerAmountDue_base += CastAide.AsDecimal(row["AmountDue"].ToString());
                        #endregion
                    }

                    #region add poolcurr item
                    var poolcurritem = new RacingWinlossRowBO();
                    poolcurritem.Id = partner.PartnerId.ToString();
                    poolcurritem.Name = partners.Items[0].Name + " Total";
                    poolcurritem.Currency = CacheUtils.GetExchangeName(partner.PoolCurr);
                    poolcurritem.CurrencyId = partner.PoolCurr.ToString();

                    poolcurritem.AgencyWinTO = NumberFormat.DecFormat(AgencyWinTO_pool, 2);
                    poolcurritem.AgencyPlaceTO = NumberFormat.DecFormat(AgencyPlaceTO_pool, 2);
                    poolcurritem.AgencyWPTO = NumberFormat.DecFormat(AgencyWPTO_pool, 2);
                    poolcurritem.AgencyTotalTO = NumberFormat.DecFormat(AgencyTotalTO_pool, 2);
                    poolcurritem.AgencyCommIncome = NumberFormat.DecFormat(AgencyCommIncome_pool, 2);
                    poolcurritem.AgencyPTWL = NumberFormat.DecFormat(AgencyPTWL_pool, 2);

                    poolcurritem.IBCWinTO = NumberFormat.DecFormat(IBCWinTO_pool, 2);
                    poolcurritem.IBCPlaceTO = NumberFormat.DecFormat(IBCPlaceTO_pool, 2);
                    poolcurritem.IBCWPTO = NumberFormat.DecFormat(IBCWPTO_pool, 2);
                    poolcurritem.IBCTotalTO = NumberFormat.DecFormat(IBCTotalTO_pool, 2);
                    poolcurritem.IBCCommIncome = NumberFormat.DecFormat(IBCCommIncome_pool, 2);
                    poolcurritem.IBCToteFee = NumberFormat.DecFormat(IBCToteFee_pool, 2);
                    poolcurritem.IBCCommExpense = NumberFormat.DecFormat(IBCCommExpense_pool, 2);
                    poolcurritem.IBCPTWL = NumberFormat.DecFormat(IBCPTWL_pool, 2);
                    poolcurritem.IBCNetIncome = NumberFormat.DecFormat(IBCNetIncome_pool, 2);

                    poolcurritem.PartnerWinTO = NumberFormat.DecFormat(PartnerWinTO_pool, 2);
                    poolcurritem.PartnerPlaceTO = NumberFormat.DecFormat(PartnerPlaceTO_pool, 2);
                    poolcurritem.PartnerWPTO = NumberFormat.DecFormat(PartnerWPTO_pool, 2);
                    poolcurritem.PartnerTotalTO = NumberFormat.DecFormat(PartnerTotalTO_pool, 2);
                    poolcurritem.PartnerDividendWon = NumberFormat.DecFormat(PartnerDividendWon_pool, 2);
                    poolcurritem.PartnerCommExpense = NumberFormat.DecFormat(PartnerCommExpense_pool, 2);
                    poolcurritem.PartnerToteFee = NumberFormat.DecFormat(PartnerToteFee_pool, 2);
                    poolcurritem.PartnerAmountDue = NumberFormat.DecFormat(PartnerAmountDue_pool, 2);

                    partners.Totals.Add(poolcurritem);
                    #endregion

                    #region add base item
                    var baseitem = new RacingWinlossRowBO();
                    baseitem.Id = partner.PartnerId.ToString();
                    baseitem.Name = partners.Items[0].Name + " Total";
                    baseitem.Currency = "Base (RM)";

                    baseitem.AgencyWinTO = NumberFormat.DecFormat(AgencyWinTO_base, 2);
                    baseitem.AgencyPlaceTO = NumberFormat.DecFormat(AgencyPlaceTO_base, 2);
                    baseitem.AgencyWPTO = NumberFormat.DecFormat(AgencyWPTO_base, 2);
                    baseitem.AgencyTotalTO = NumberFormat.DecFormat(AgencyTotalTO_base, 2);
                    baseitem.AgencyCommIncome = NumberFormat.DecFormat(AgencyCommIncome_base, 2);
                    baseitem.AgencyPTWL = NumberFormat.DecFormat(AgencyPTWL_base, 2);

                    baseitem.IBCWinTO = NumberFormat.DecFormat(IBCWinTO_base, 2);
                    baseitem.IBCPlaceTO = NumberFormat.DecFormat(IBCPlaceTO_base, 2);
                    baseitem.IBCWPTO = NumberFormat.DecFormat(IBCWPTO_base, 2);
                    baseitem.IBCTotalTO = NumberFormat.DecFormat(IBCTotalTO_base, 2);
                    baseitem.IBCCommIncome = NumberFormat.DecFormat(IBCCommIncome_base, 2);
                    baseitem.IBCToteFee = NumberFormat.DecFormat(IBCToteFee_base, 2);
                    baseitem.IBCCommExpense = NumberFormat.DecFormat(IBCCommExpense_base, 2);
                    baseitem.IBCPTWL = NumberFormat.DecFormat(IBCPTWL_base, 2);
                    baseitem.IBCNetIncome = NumberFormat.DecFormat(IBCNetIncome_base, 2);

                    baseitem.PartnerWinTO = NumberFormat.DecFormat(PartnerWinTO_base, 2);
                    baseitem.PartnerPlaceTO = NumberFormat.DecFormat(PartnerPlaceTO_base, 2);
                    baseitem.PartnerWPTO = NumberFormat.DecFormat(PartnerWPTO_base, 2);
                    baseitem.PartnerTotalTO = NumberFormat.DecFormat(PartnerTotalTO_base, 2);
                    baseitem.PartnerDividendWon = NumberFormat.DecFormat(PartnerDividendWon_base, 2);
                    baseitem.PartnerCommExpense = NumberFormat.DecFormat(PartnerCommExpense_base, 2);
                    baseitem.PartnerToteFee = NumberFormat.DecFormat(PartnerToteFee_base, 2);
                    baseitem.PartnerAmountDue = NumberFormat.DecFormat(PartnerAmountDue_base, 2);

                    partners.Totals.Add(baseitem);
                    #endregion


                    racingBO.Items.Add(partners);

                }

                #region Grand Total Pool and Base currency

                List<RacingWinlossRowBO> lstPoolCurrency = new List<RacingWinlossRowBO>();
                foreach (var racingWinlossBo in racingBO.Items)
                {
                    foreach (RacingWinlossRowBO item in racingWinlossBo.Totals)
                    {
                        if (string.IsNullOrEmpty(item.CurrencyId))
                            item.CurrencyId = string.Empty;
                        lstPoolCurrency.Add(item);
                    }
                }
                var listDistinctPoolCurrency =
                    lstPoolCurrency.OrderByDescending(c => c.CurrencyId).Select(s => s.Currency).Distinct().ToArray();
                foreach (var currencyName in listDistinctPoolCurrency)
                {
                    racingBO.Totals.Add(new RacingWinlossRowBO()
                    {
                        Currency = currencyName,
                        AgencyWinTO = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.AgencyWinTO)), 2),
                        AgencyWPTO = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.AgencyWPTO)), 2),
                        AgencyTotalTO = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.AgencyTotalTO)), 2),
                        AgencyPlaceTO = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.AgencyPlaceTO)), 2),
                        AgencyPTWL = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.AgencyPTWL)), 2),
                        AgencyCommIncome = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.AgencyCommIncome)), 2),

                        IBCCommExpense = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.IBCCommExpense)), 2),
                        IBCCommIncome = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.IBCCommIncome)), 2),
                        IBCNetIncome = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.IBCNetIncome)), 2),
                        IBCPTWL = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.IBCPTWL)), 2),
                        IBCPlaceTO = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.IBCPlaceTO)), 2),
                        IBCTotalTO = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.IBCTotalTO)), 2),
                        IBCToteFee = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.IBCToteFee)), 2),
                        IBCWPTO = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.IBCWPTO)), 2),
                        IBCWinTO = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.IBCWinTO)), 2),

                        PartnerAmountDue = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.PartnerAmountDue)), 2),
                        PartnerCommExpense = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.PartnerCommExpense)), 2),
                        PartnerDividendWon = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.PartnerDividendWon)), 2),
                        Name = "Grand Total",
                        PartnerTotalTO = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.PartnerTotalTO)), 2),
                        PartnerPlaceTO = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.PartnerPlaceTO)), 2),
                        PartnerToteFee = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.PartnerToteFee)), 2),
                        PartnerWPTO = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.PartnerWPTO)), 2),
                        PartnerWinTO = NumberFormat.DecFormat(lstPoolCurrency.Where(s => s.Currency == currencyName).Sum(s => CastAide.AsDecimal(s.PartnerWinTO)), 2),
                    });
                }
                #endregion

                #region Comparision
                //@reportType:1-UK Horse Racing ; 3-UK Greyhound
                if (reportType == Enumerations.ReportType.Horse_Racing_Daily_Report_For_AU ||
                    reportType == Enumerations.ReportType.Horse_Racing_Daily_Report_For_UK ||
                    reportType == Enumerations.ReportType.Greyhound_Daily_Report_For_UK ||
                    reportType == Enumerations.ReportType.Greyhound_Daily_Report_For_AU ||
                    reportType == Enumerations.ReportType.Horse_Racing_Report_for_SA_Tote_NEW)
                {
                    DataTable dtDumpFile = _reportRacingDao.WinlostRacingToteDumpFiles(winlossDate, (int)reportType);
                    var lstComparision = new List<RacingWinlossDumpFileBO>();
                    if (dtDumpFile != null)
                    {
                        if (dtDumpFile.Rows.Count > 0)
                        {
                            racingBO.IsGotDumpFile = true;

                            foreach (DataRow row in dtDumpFile.Rows)
                            {
                                var item = new RacingWinlossDumpFileBO();

                                item.currency = CastAide.AsString(row["currencyName"]);
                                item.partnerTO = NumberFormat.DecFormat(row["TO3rd"], 2);
                                item.partnerDividendWon = NumberFormat.DecFormat(row["DividendWon3rd"], 2);

                                item.ibcTO =
                                    NumberFormat.DecFormat(
                                        lstPoolCurrency.Where(
                                            s =>
                                            s.Currency.Substring(0, 3).ToLower() ==
                                            CastAide.AsString(row["currencyName"]).ToString().ToLower()).Sum(
                                                s => CastAide.AsDecimal(s.PartnerTotalTO)), 2);
                                item.ibcDividendWon =
                                    NumberFormat.DecFormat(
                                        lstPoolCurrency.Where(
                                            s =>
                                            s.Currency.Substring(0, 3).ToLower() ==
                                            CastAide.AsString(row["currencyName"]).ToString().ToLower()).Sum(
                                                s => CastAide.AsDecimal(s.PartnerDividendWon)), 2);


                                if (System.Math.Abs(CastAide.AsDecimal(item.ibcDividendWon) - CastAide.AsDecimal(item.partnerDividendWon)) < 1)
                                    item.IsMatchDividend = true;
                                if (System.Math.Abs(CastAide.AsDecimal(item.ibcTO) - CastAide.AsDecimal(item.partnerTO)) < 1)
                                    item.IsMatchTO = true;

                                lstComparision.Add(item);

                            }
                        }
                    }


                    racingBO.Comparision = lstComparision;
                }


                #endregion
            }

            SessionExtension.AddSession(HttpContext.Current.Session, string.Format(_winLossRacingReportSessionKey, reportType), racingBO);
            return racingBO;
        }

        #region GetDistinctPartner
        private static List<PartnerPoolCurr> GetDistinctPartner(DataTable dt)
        {
            return
            (from rows in dt.AsEnumerable()
             select new
             {
                 //PartnerId = rows.Field<int>("partnerID"),
                 PartnerId = CastAide.AsInt32(rows["partnerID"]),
                 PoolCurr = CastAide.AsInt32(rows["PoolCurrency"])
             }).Distinct().Select(x =>
                 new PartnerPoolCurr() { PartnerId = x.PartnerId, PoolCurr = x.PoolCurr })
                 .ToList();
        }
        #endregion
        #endregion

        #region GetWinlossRacingReport_Details

        #region GetDistinctMeetingCurrency
        private List<MeetingCurr> GetDistinctMeetingCurrency(DataTable dataTable, int currency)
        {
            return
           (from rows in dataTable.AsEnumerable().Where(s => CastAide.AsInt32( s["currency"]) == currency)
            select new
            {
                CurrId = CastAide.AsInt32( rows["currency"]),
                MeetingId =  CastAide.AsInt32(rows["MeetingId"])
            }).Distinct().Select(x =>
                new MeetingCurr() { CurrencyId = x.CurrId, MeetingId = x.MeetingId })
                .ToList();
        }

        private List<MeetingCurr> GetDistinctMeeting(DataTable dataTable)
        {
            return
           (from rows in dataTable.AsEnumerable()
            select new
            {
                MeetingId = CastAide.AsInt32(rows["MeetingId"])
            }).Distinct().Select(x =>
                new MeetingCurr() { MeetingId = x.MeetingId})
                .ToList();
        }

        private List<MeetingCurr> GetDistinctCurrency(DataTable dataTable)
        {
            return
           (from rows in dataTable.AsEnumerable()
            select new
            {
                CurrId = CastAide.AsInt32(rows["currency"])
            }).Distinct().Select(x =>
                new MeetingCurr() { CurrencyId = x.CurrId})
                .ToList();
            
        }
        #endregion

        public RacingWinlossDetailsBO GetWinlossRacingReportDetails(string winlossDate, string poolCurrency, string partnerId)
        {
            
            DataTable dataTable  = _reportRacingDao.WinlostRacingTote_Details(winlossDate, poolCurrency, partnerId);

            if(dataTable == null)
                return new RacingWinlossDetailsBO();

            #region Variable List Object

           
            var arrCurrencies = GetDistinctCurrency(dataTable);

            var racingDetailsBo = new RacingWinlossDetailsBO();
            racingDetailsBo.Items = new List<RacingWinlossCurrencyBO>();
            racingDetailsBo.Totals = new List<RacingWinlossRowBO>();
            racingDetailsBo.GrandTotals = new List<RacingWinlossRowBO>();
            #endregion
            
            #region Currency
            
            #region variable

            decimal AgencyWinTO;
            decimal AgencyPlaceTO;
            decimal AgencyWPTO;
            decimal AgencyTotalTO;
            decimal AgencyCommIncome;
            decimal AgencyPTWL;

            decimal IBCWinTO;
            decimal IBCPlaceTO;
            decimal IBCWPTO;
            decimal IBCTotalTO;
            decimal IBCCommIncome;
            decimal IBCToteFee;
            decimal IBCCommExpense;
            decimal IBCPTWL;
            decimal IBCNetIncome;

            decimal PartnerWinTO;
            decimal PartnerPlaceTO;
            decimal PartnerWPTO;
            decimal PartnerTotalTO;
            decimal PartnerDividendWon;
            decimal PartnerCommExpense;
            decimal PartnerToteFee;
            decimal PartnerAmountDue;

            //currency origin
            decimal AgencyWinTO_origin = 0;
            decimal AgencyPlaceTO_origin = 0;
            decimal AgencyWPTO_origin = 0;
            decimal AgencyTotalTO_origin = 0;
            decimal AgencyCommIncome_origin = 0;
            decimal AgencyPTWL_origin = 0;

            decimal IBCWinTO_origin = 0;
            decimal IBCPlaceTO_origin = 0;
            decimal IBCWPTO_origin = 0;
            decimal IBCTotalTO_origin = 0;
            decimal IBCCommIncome_origin = 0;
            decimal IBCToteFee_origin = 0;
            decimal IBCCommExpense_origin = 0;
            decimal IBCPTWL_origin = 0;
            decimal IBCNetIncome_origin = 0;

            decimal PartnerWinTO_origin = 0;
            decimal PartnerPlaceTO_origin = 0;
            decimal PartnerWPTO_origin = 0;
            decimal PartnerTotalTO_origin = 0;
            decimal PartnerDividendWon_origin = 0;
            decimal PartnerCommExpense_origin = 0;
            decimal PartnerToteFee_origin = 0;
            decimal PartnerAmountDue_origin = 0;

            //Pool total
            decimal AgencyWinTO_pool = 0;
            decimal AgencyPlaceTO_pool = 0;
            decimal AgencyWPTO_pool = 0;
            decimal AgencyTotalTO_pool = 0;
            decimal AgencyCommIncome_pool = 0;
            decimal AgencyPTWL_pool = 0;

            decimal IBCWinTO_pool = 0;
            decimal IBCPlaceTO_pool = 0;
            decimal IBCWPTO_pool = 0;
            decimal IBCTotalTO_pool = 0;
            decimal IBCCommIncome_pool = 0;
            decimal IBCToteFee_pool = 0;
            decimal IBCCommExpense_pool = 0;
            decimal IBCPTWL_pool = 0;
            decimal IBCNetIncome_pool = 0;

            decimal PartnerWinTO_pool = 0;
            decimal PartnerPlaceTO_pool = 0;
            decimal PartnerWPTO_pool = 0;
            decimal PartnerTotalTO_pool = 0;
            decimal PartnerDividendWon_pool = 0;
            decimal PartnerCommExpense_pool = 0;
            decimal PartnerToteFee_pool = 0;
            decimal PartnerAmountDue_pool = 0;

            //Base total
            decimal AgencyWinTO_base = 0;
            decimal AgencyPlaceTO_base = 0;
            decimal AgencyWPTO_base = 0;
            decimal AgencyTotalTO_base = 0;
            decimal AgencyCommIncome_base = 0;
            decimal AgencyPTWL_base = 0;

            decimal IBCWinTO_base = 0;
            decimal IBCPlaceTO_base = 0;
            decimal IBCWPTO_base = 0;
            decimal IBCTotalTO_base = 0;
            decimal IBCCommIncome_base = 0;
            decimal IBCToteFee_base = 0;
            decimal IBCCommExpense_base = 0;
            decimal IBCPTWL_base = 0;
            decimal IBCNetIncome_base = 0;

            decimal PartnerWinTO_base = 0;
            decimal PartnerPlaceTO_base = 0;
            decimal PartnerWPTO_base = 0;
            decimal PartnerTotalTO_base = 0;
            decimal PartnerDividendWon_base = 0;
            decimal PartnerCommExpense_base = 0;
            decimal PartnerToteFee_base = 0;
            decimal PartnerAmountDue_base = 0;
            #endregion
            #endregion

            List<RacingWinlossRowBO> grandTotal = new List<RacingWinlossRowBO>();
            foreach (MeetingCurr curr in arrCurrencies)
            {
                #region GrandTotal
                #region PoolCurr
                grandTotal.Add(new RacingWinlossRowBO()
                                   {
                                       CurrencyId = poolCurrency,
                                       Currency = CacheUtils.GetExchangeName(CastAide.AsInt32( poolCurrency)),

                                       AgencyWinTO =   AgencyWinTO_pool.ToString() ,
                                       AgencyPlaceTO = AgencyPlaceTO_pool.ToString(),
                                       AgencyWPTO  = AgencyWPTO_pool.ToString() ,
                                       AgencyTotalTO = AgencyTotalTO_pool.ToString() ,
                                       AgencyCommIncome = AgencyCommIncome_pool.ToString() ,
                                       AgencyPTWL = AgencyPTWL_pool .ToString(),
                
                                       IBCWinTO = IBCWinTO_pool.ToString(),
                                       IBCPlaceTO  = IBCPlaceTO_pool .ToString(),
                                       IBCWPTO  = IBCWPTO_pool .ToString(),
                                       IBCTotalTO = IBCTotalTO_pool .ToString(),
                                       IBCCommIncome = IBCCommIncome_pool .ToString(),
                                       IBCToteFee  = IBCToteFee_pool .ToString(),
                                       IBCCommExpense = IBCCommExpense_pool .ToString(),
                                       IBCPTWL = IBCPTWL_pool .ToString(),
                                       IBCNetIncome = IBCNetIncome_pool .ToString(),
                
                                       PartnerWinTO = PartnerWinTO_pool .ToString(),
                                       PartnerPlaceTO = PartnerPlaceTO_pool .ToString(),
                                       PartnerWPTO = PartnerWPTO_pool .ToString(),
                                       PartnerTotalTO = PartnerTotalTO_pool .ToString(),
                                       PartnerDividendWon = PartnerDividendWon_pool .ToString(),
                                       PartnerCommExpense = PartnerCommExpense_pool .ToString(),
                                       PartnerToteFee = PartnerToteFee_pool .ToString(),
                                       PartnerAmountDue = PartnerAmountDue_pool .ToString()
                                   });
                
                #endregion

                #region Base
                grandTotal.Add(new RacingWinlossRowBO()
                {
                    CurrencyId = "2",
                    Currency = "Base (RM)",

                    AgencyWinTO = AgencyWinTO_base.ToString(),
                    AgencyPlaceTO = AgencyPlaceTO_base.ToString(),
                    AgencyWPTO = AgencyWPTO_base.ToString(),
                    AgencyTotalTO = AgencyTotalTO_base.ToString(),
                    AgencyCommIncome = AgencyCommIncome_base.ToString(),
                    AgencyPTWL = AgencyPTWL_base.ToString(),

                    IBCWinTO = IBCWinTO_base.ToString(),
                    IBCPlaceTO = IBCPlaceTO_base.ToString(),
                    IBCWPTO = IBCWPTO_base.ToString(),
                    IBCTotalTO = IBCTotalTO_base.ToString(),
                    IBCCommIncome = IBCCommIncome_base.ToString(),
                    IBCToteFee = IBCToteFee_base.ToString(),
                    IBCCommExpense = IBCCommExpense_base.ToString(),
                    IBCPTWL = IBCPTWL_base.ToString(),
                    IBCNetIncome = IBCNetIncome_base.ToString(),

                    PartnerWinTO = PartnerWinTO_base.ToString(),
                    PartnerPlaceTO = PartnerPlaceTO_base.ToString(),
                    PartnerWPTO = PartnerWPTO_base.ToString(),
                    PartnerTotalTO = PartnerTotalTO_base.ToString(),
                    PartnerDividendWon = PartnerDividendWon_base.ToString(),
                    PartnerCommExpense = PartnerCommExpense_base.ToString(),
                    PartnerToteFee = PartnerToteFee_base.ToString(),
                    PartnerAmountDue = PartnerAmountDue_base.ToString()
                });

                #endregion
                #endregion

                #region assign default
                #region origin
                AgencyWinTO_origin = 0;
                AgencyPlaceTO_origin = 0;
                AgencyWPTO_origin = 0;
                AgencyTotalTO_origin = 0;
                AgencyCommIncome_origin = 0;
                AgencyPTWL_origin = 0;

                IBCWinTO_origin = 0;
                IBCPlaceTO_origin = 0;
                IBCWPTO_origin = 0;
                IBCTotalTO_origin = 0;
                IBCCommIncome_origin = 0;
                IBCToteFee_origin = 0;
                IBCCommExpense_origin = 0;
                IBCPTWL_origin = 0;
                IBCNetIncome_origin = 0;

                PartnerWinTO_origin = 0;
                PartnerPlaceTO_origin = 0;
                PartnerWPTO_origin = 0;
                PartnerTotalTO_origin = 0;
                PartnerDividendWon_origin = 0;
                PartnerCommExpense_origin = 0;
                PartnerToteFee_origin = 0;
                PartnerAmountDue_origin = 0;
                #endregion
                #region Pool
                AgencyWinTO_pool = 0;
                AgencyPlaceTO_pool = 0;
                AgencyWPTO_pool = 0;
                AgencyTotalTO_pool = 0;
                AgencyCommIncome_pool = 0;
                AgencyPTWL_pool = 0;

                IBCWinTO_pool = 0;
                IBCPlaceTO_pool = 0;
                IBCWPTO_pool = 0;
                IBCTotalTO_pool = 0;
                IBCCommIncome_pool = 0;
                IBCToteFee_pool = 0;
                IBCCommExpense_pool = 0;
                IBCPTWL_pool = 0;
                IBCNetIncome_pool = 0;

                PartnerWinTO_pool = 0;
                PartnerPlaceTO_pool = 0;
                PartnerWPTO_pool = 0;
                PartnerTotalTO_pool = 0;
                PartnerDividendWon_pool = 0;
                PartnerCommExpense_pool = 0;
                PartnerToteFee_pool = 0;
                PartnerAmountDue_pool = 0;
                #endregion

                #region Base
                AgencyWinTO_base = 0;
                AgencyPlaceTO_base = 0;
                AgencyWPTO_base = 0;
                AgencyTotalTO_base = 0;
                AgencyCommIncome_base = 0;
                AgencyPTWL_base = 0;

                IBCWinTO_base = 0;
                IBCPlaceTO_base = 0;
                IBCWPTO_base = 0;
                IBCTotalTO_base = 0;
                IBCCommIncome_base = 0;
                IBCToteFee_base = 0;
                IBCCommExpense_base = 0;
                IBCPTWL_base = 0;
                IBCNetIncome_base = 0;

                PartnerWinTO_base = 0;
                PartnerPlaceTO_base = 0;
                PartnerWPTO_base = 0;
                PartnerTotalTO_base = 0;
                PartnerDividendWon_base = 0;
                PartnerCommExpense_base = 0;
                PartnerToteFee_base = 0;
                PartnerAmountDue_base = 0;
                #endregion
                #endregion

                var arrDistinctMeeting = GetDistinctMeetingCurrency(dataTable, curr.CurrencyId);

                var lstCurrency = new RacingWinlossCurrencyBO();
                lstCurrency.Items = new List<RacingWinlossMeetingBO>();
                lstCurrency.Totals = new List<RacingWinlossRowBO>();

                //Meeting by Currency
                int [] arrBaseCurr = {2};
                int currentCurr = 0;
                foreach (MeetingCurr meetingCurr in arrDistinctMeeting)
                {
                    currentCurr = meetingCurr.CurrencyId;
                    DataRow[] rows = dataTable.Select("currency = " + curr.CurrencyId + " and meetingid = " + meetingCurr.MeetingId);

                    var lstMeeting = new RacingWinlossMeetingBO();
                    lstMeeting.Items = new List<RacingWinlossRowBO>();
                    lstMeeting.Totals = new List<RacingWinlossRowBO>();

                    foreach (DataRow dataRow in rows)
                    {
                        #region row
                        var row = new RacingWinlossRowBO()
                        {
                            CurrencyId = CastAide.AsString(dataRow["currency"]),
                            Currency = CastAide.AsString(dataRow["currencyName"]),
                            Id = CastAide.AsString(dataRow["MeetingId"]),
                            Name = CacheRef.GetLeagueName(dataRow["MeetingId"]),
                            MeetingID = dataRow["MeetingId"].ToString(),
                            RaceNo = CastAide.AsString(dataRow["RaceNo"]),

                            AgencyWinTO = NumberFormat.DecFormat(dataRow["winTO_Agency_Origin"], 2),
                            AgencyPlaceTO = NumberFormat.DecFormat(dataRow["placeTO_Agency_Origin"], 2),
                            AgencyWPTO = NumberFormat.DecFormat(dataRow["winplaceTO_Agency_Origin"], 2),
                            AgencyTotalTO = NumberFormat.DecFormat(dataRow["totalTO_Agency_Origin"], 2),
                            AgencyCommIncome = NumberFormat.DecFormat(dataRow["commIncome_Agency_Origin"], 2),
                            AgencyPTWL = NumberFormat.DecFormat(dataRow["PTWL_Agency_Origin"], 2),

                            IBCWinTO = NumberFormat.DecFormat(dataRow["winTO_IBC_Origin"], 2),
                            IBCPlaceTO = NumberFormat.DecFormat(dataRow["placeTO_IBC_Origin"], 2),
                            IBCWPTO = NumberFormat.DecFormat(dataRow["winplaceTO_IBC_Origin"], 2),
                            IBCTotalTO = NumberFormat.DecFormat(dataRow["totalTO_IBC_Origin"], 2),
                            IBCCommIncome = NumberFormat.DecFormat(dataRow["commIncome_IBC_Origin"], 2),
                            IBCCommExpense = NumberFormat.DecFormat(dataRow["commIncome_Agency_Origin"], 2),
                            IBCNetIncome = NumberFormat.DecFormat(dataRow["netIncome_IBC_Origin"], 2),
                            IBCPTWL = NumberFormat.DecFormat(dataRow["PTWL_IBC_Origin"], 2),
                            IBCToteFee = NumberFormat.DecFormat(dataRow["FeeDue2_Origin"], 2),

                            PartnerWinTO = NumberFormat.DecFormat(dataRow["winTO_Partner_Origin"], 2),
                            PartnerPlaceTO = NumberFormat.DecFormat(dataRow["placeTO_Partner_Origin"], 2),
                            PartnerWPTO = NumberFormat.DecFormat(dataRow["winplaceTO_Partner_Origin"], 2),
                            PartnerTotalTO = NumberFormat.DecFormat(dataRow["totalTO_Partner_Origin"], 2),
                            PartnerDividendWon = NumberFormat.DecFormat(dataRow["dividendWon_Origin"], 2),
                            PartnerAmountDue = NumberFormat.DecFormat(dataRow["AmountDue_Origin"], 2),
                            PartnerCommExpense = NumberFormat.DecFormat(dataRow["commIncome_IBC_Origin"], 2),
                            PartnerToteFee = NumberFormat.DecFormat(dataRow["FeeDue2_Origin"], 2),
                        };
                        #endregion

                        lstMeeting.Items.Add(row);

                        #region Total Curr Origin
                        AgencyWinTO_origin += CastAide.AsDecimal(dataRow["winTO_Agency_Origin"].ToString());
                        AgencyPlaceTO_origin += CastAide.AsDecimal(dataRow["placeTO_Agency_Origin"].ToString());
                        AgencyWPTO_origin += CastAide.AsDecimal(dataRow["winplaceTO_Agency_Origin"].ToString());
                        AgencyTotalTO_origin += CastAide.AsDecimal(dataRow["totalTO_Agency_Origin"].ToString());
                        AgencyCommIncome_origin += CastAide.AsDecimal(dataRow["commIncome_Agency_Origin"].ToString());
                        AgencyPTWL_origin += CastAide.AsDecimal(dataRow["PTWL_Agency_Origin"].ToString());

                        IBCWinTO_origin += CastAide.AsDecimal(dataRow["winTO_IBC_Origin"].ToString());
                        IBCPlaceTO_origin += CastAide.AsDecimal(dataRow["placeTO_IBC_Origin"].ToString());
                        IBCWPTO_origin += CastAide.AsDecimal(dataRow["winplaceTO_IBC_Origin"].ToString());
                        IBCTotalTO_origin += CastAide.AsDecimal(dataRow["totalTO_IBC_Origin"].ToString());
                        IBCCommIncome_origin += CastAide.AsDecimal(dataRow["commIncome_IBC_Origin"].ToString());
                        IBCToteFee_origin += CastAide.AsDecimal(dataRow["FeeDue2_Origin"].ToString());
                        IBCCommExpense_origin += CastAide.AsDecimal(dataRow["commIncome_Agency_Origin"].ToString());
                        IBCPTWL_origin += CastAide.AsDecimal(dataRow["PTWL_IBC_Origin"].ToString());
                        IBCNetIncome_origin += CastAide.AsDecimal(dataRow["netIncome_IBC_Origin"].ToString());

                        PartnerWinTO_origin += CastAide.AsDecimal(dataRow["winTO_Partner_Origin"].ToString());
                        PartnerPlaceTO_origin += CastAide.AsDecimal(dataRow["placeTO_Partner_Origin"].ToString());
                        PartnerWPTO_origin += CastAide.AsDecimal(dataRow["winPlaceTO_Partner_Origin"].ToString());
                        PartnerTotalTO_origin += CastAide.AsDecimal(dataRow["totalTO_Partner_Origin"].ToString());
                        PartnerDividendWon_origin += CastAide.AsDecimal(dataRow["dividendWon_Origin"].ToString());
                        PartnerCommExpense_origin += CastAide.AsDecimal(dataRow["commIncome_IBC_Origin"].ToString());
                        PartnerToteFee_origin += CastAide.AsDecimal(dataRow["FeeDue2_Origin"].ToString());
                        PartnerAmountDue_origin += CastAide.AsDecimal(dataRow["AmountDue_Origin"].ToString());
                        #endregion

                        #region PoolCurr
                        AgencyWinTO_pool += CastAide.AsDecimal(dataRow["winTO_Agency_PoolCurr"].ToString());
                        AgencyPlaceTO_pool += CastAide.AsDecimal(dataRow["placeTO_Agency_PoolCurr"].ToString());
                        AgencyWPTO_pool += CastAide.AsDecimal(dataRow["winplaceTO_Agency_PoolCurr"].ToString());
                        AgencyTotalTO_pool += CastAide.AsDecimal(dataRow["totalTO_Agency_PoolCurr"].ToString());
                        AgencyCommIncome_pool += CastAide.AsDecimal(dataRow["commIncome_Agency_PoolCurr"].ToString());
                        AgencyPTWL_pool += CastAide.AsDecimal(dataRow["PTWL_Agency_PoolCurr"].ToString());

                        IBCWinTO_pool += CastAide.AsDecimal(dataRow["winTO_IBC_PoolCurr"].ToString());
                        IBCPlaceTO_pool += CastAide.AsDecimal(dataRow["placeTO_IBC_PoolCurr"].ToString());
                        IBCWPTO_pool += CastAide.AsDecimal(dataRow["winplaceTO_IBC_PoolCurr"].ToString());
                        IBCTotalTO_pool += CastAide.AsDecimal(dataRow["totalTO_IBC_PoolCurr"].ToString());
                        IBCCommIncome_pool += CastAide.AsDecimal(dataRow["commIncome_IBC_PoolCurr"].ToString());
                        IBCToteFee_pool += CastAide.AsDecimal(dataRow["FeeDue2_PoolCurr"].ToString());
                        IBCCommExpense_pool += CastAide.AsDecimal(dataRow["commIncome_Agency_PoolCurr"].ToString());
                        IBCPTWL_pool += CastAide.AsDecimal(dataRow["PTWL_IBC_PoolCurr"].ToString());
                        IBCNetIncome_pool += CastAide.AsDecimal(dataRow["netIncome_IBC_PoolCurr"].ToString());

                        PartnerWinTO_pool += CastAide.AsDecimal(dataRow["winTO_Partner_PoolCurr"].ToString());
                        PartnerPlaceTO_pool += CastAide.AsDecimal(dataRow["placeTO_Partner_PoolCurr"].ToString());
                        PartnerWPTO_pool += CastAide.AsDecimal(dataRow["winPlaceTO_Partner_PoolCurr"].ToString());
                        PartnerTotalTO_pool += CastAide.AsDecimal(dataRow["totalTO_Partner_PoolCurr"].ToString());
                        PartnerDividendWon_pool += CastAide.AsDecimal(dataRow["dividendWon_PoolCurr"].ToString());
                        PartnerCommExpense_pool += CastAide.AsDecimal(dataRow["commIncome_IBC_PoolCurr"].ToString());
                        PartnerToteFee_pool += CastAide.AsDecimal(dataRow["FeeDue2_PoolCurr"].ToString());
                        PartnerAmountDue_pool += CastAide.AsDecimal(dataRow["AmountDue_PoolCurr"].ToString());
                        #endregion

                        #region base
                        AgencyWinTO_base += CastAide.AsDecimal(dataRow["winTO_Agency"].ToString()); ;
                        AgencyPlaceTO_base += CastAide.AsDecimal(dataRow["placeTO_Agency"].ToString()); ;
                        AgencyWPTO_base += CastAide.AsDecimal(dataRow["winplaceTO_Agency"].ToString());
                        AgencyTotalTO_base += CastAide.AsDecimal(dataRow["totalTO_Agency"].ToString());
                        AgencyCommIncome_base += CastAide.AsDecimal(dataRow["commIncome_Agency"].ToString());
                        AgencyPTWL_base += CastAide.AsDecimal(dataRow["PTWL_Agency"].ToString());

                        IBCWinTO_base += CastAide.AsDecimal(dataRow["winTO_IBC"].ToString());
                        IBCPlaceTO_base += CastAide.AsDecimal(dataRow["placeTO_IBC"].ToString());
                        IBCWPTO_base += CastAide.AsDecimal(dataRow["winplaceTO_IBC"].ToString());
                        IBCTotalTO_base += CastAide.AsDecimal(dataRow["totalTO_IBC"].ToString());
                        IBCCommIncome_base += CastAide.AsDecimal(dataRow["commIncome_IBC"].ToString());
                        IBCToteFee_base += CastAide.AsDecimal(dataRow["FeeDue2"].ToString());
                        IBCCommExpense_base += CastAide.AsDecimal(dataRow["commIncome_Agency"].ToString());
                        IBCPTWL_base += CastAide.AsDecimal(dataRow["PTWL_IBC"].ToString());
                        IBCNetIncome_base += CastAide.AsDecimal(dataRow["netIncome_IBC"].ToString());

                        PartnerWinTO_base += CastAide.AsDecimal(dataRow["winTO_Partner"].ToString());
                        PartnerPlaceTO_base += CastAide.AsDecimal(dataRow["placeTO_Partner"].ToString());
                        PartnerWPTO_base += CastAide.AsDecimal(dataRow["winPlaceTO_Partner"].ToString());
                        PartnerTotalTO_base += CastAide.AsDecimal(dataRow["totalTO_Partner"].ToString());
                        PartnerDividendWon_base += CastAide.AsDecimal(dataRow["dividendWon"].ToString());
                        PartnerCommExpense_base += CastAide.AsDecimal(dataRow["commIncome_IBC"].ToString());
                        PartnerToteFee_base += CastAide.AsDecimal(dataRow["FeeDue2"].ToString());
                        PartnerAmountDue_base += CastAide.AsDecimal(dataRow["AmountDue"].ToString());
                        #endregion
                    }

                    lstCurrency.Items.Add(lstMeeting);

                    #region Totals Meeting Currency
                    lstMeeting.Totals.Add(new RacingWinlossRowBO()
                    {
                        Currency = CacheUtils.GetExchangeName( meetingCurr.CurrencyId),
                        Name = CacheRef.GetLeagueName(meetingCurr.MeetingId) + " - " + CacheUtils.GetExchangeName(meetingCurr.CurrencyId) + " Total",
                        MeetingID = meetingCurr.MeetingId.ToString(),

                        AgencyWinTO = NumberFormat.DecFormat(lstMeeting .Items.Sum(s => CastAide.AsDecimal(s.AgencyWinTO)), 2),
                        AgencyPlaceTO = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.AgencyPlaceTO)), 2),
                        AgencyWPTO = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.AgencyWPTO)), 2),
                        AgencyTotalTO = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.AgencyTotalTO)), 2),
                        AgencyCommIncome = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.AgencyCommIncome)), 2),
                        AgencyPTWL = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.AgencyPTWL)), 2),

                        IBCWinTO = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.IBCWinTO)), 2),
                        IBCPlaceTO = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.IBCPlaceTO)), 2),
                        IBCWPTO = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.IBCWPTO)), 2),
                        IBCTotalTO = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.IBCTotalTO)), 2),
                        IBCCommIncome = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.IBCCommIncome)), 2),
                        IBCCommExpense = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.IBCCommExpense)), 2),
                        IBCNetIncome = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.IBCNetIncome)), 2),
                        IBCPTWL = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.IBCPTWL)), 2),
                        IBCToteFee = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.IBCToteFee)), 2),

                        PartnerWinTO = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.PartnerWinTO)), 2),
                        PartnerPlaceTO = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.PartnerPlaceTO)), 2),
                        PartnerWPTO = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.PartnerWPTO)), 2),
                        PartnerTotalTO = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.PartnerTotalTO)), 2),
                        PartnerDividendWon = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.PartnerDividendWon)), 2),
                        PartnerCommExpense = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.PartnerCommExpense)), 2),
                        PartnerToteFee = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.PartnerToteFee)), 2),
                        PartnerAmountDue = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.PartnerAmountDue)), 2),
                    });

                      

                    #endregion

                }


                #region Total Currency

                #region origin
                var originCurr = new RacingWinlossRowBO()
                {
                    Currency = CacheUtils.GetExchangeName(curr.CurrencyId),
                    Name = CacheUtils.GetExchangeName(curr.CurrencyId) + " Total",

                    AgencyWinTO = NumberFormat.DecFormat(AgencyWinTO_origin, 2),
                    AgencyPlaceTO = NumberFormat.DecFormat(AgencyPlaceTO_origin, 2),
                    AgencyWPTO = NumberFormat.DecFormat(AgencyWPTO_origin, 2),
                    AgencyTotalTO = NumberFormat.DecFormat(AgencyTotalTO_origin, 2),
                    AgencyCommIncome = NumberFormat.DecFormat(AgencyCommIncome_origin, 2),
                    AgencyPTWL = NumberFormat.DecFormat(AgencyPTWL_origin, 2),

                    IBCWinTO = NumberFormat.DecFormat(IBCWinTO_origin, 2),
                    IBCPlaceTO = NumberFormat.DecFormat(IBCPlaceTO_origin, 2),
                    IBCWPTO = NumberFormat.DecFormat(IBCWPTO_origin, 2),
                    IBCTotalTO = NumberFormat.DecFormat(IBCTotalTO_origin, 2),
                    IBCCommIncome = NumberFormat.DecFormat(IBCCommIncome_origin, 2),
                    IBCCommExpense = NumberFormat.DecFormat(IBCCommExpense_origin, 2),
                    IBCNetIncome = NumberFormat.DecFormat(IBCNetIncome_origin, 2),
                    IBCPTWL = NumberFormat.DecFormat(IBCPTWL_origin, 2),
                    IBCToteFee = NumberFormat.DecFormat(IBCToteFee_origin, 2),

                    PartnerWinTO = NumberFormat.DecFormat(PartnerWinTO_origin, 2),
                    PartnerPlaceTO = NumberFormat.DecFormat(PartnerPlaceTO_origin, 2),
                    PartnerWPTO = NumberFormat.DecFormat(PartnerWPTO_origin, 2),
                    PartnerTotalTO = NumberFormat.DecFormat(PartnerTotalTO_origin, 2),
                    PartnerDividendWon = NumberFormat.DecFormat(PartnerDividendWon_origin, 2),
                    PartnerCommExpense = NumberFormat.DecFormat(PartnerCommExpense_origin, 2),
                    PartnerToteFee = NumberFormat.DecFormat(PartnerToteFee_origin, 2),
                    PartnerAmountDue = NumberFormat.DecFormat(PartnerAmountDue_origin, 2),

                };
                lstCurrency.Totals.Add(originCurr);
                #endregion

                #region base
                if (!arrBaseCurr.Contains(currentCurr))
                {
                    var baseCurr = new RacingWinlossRowBO()
                                       {
                                           Currency = "Base (RM)",
                                           Name = CacheUtils.GetExchangeName(curr.CurrencyId) + " Total",
                                           MeetingID = curr.MeetingId.ToString(),

                                           AgencyWinTO = NumberFormat.DecFormat(AgencyWinTO_base, 2),
                                           AgencyPlaceTO = NumberFormat.DecFormat(AgencyPlaceTO_base, 2),
                                           AgencyWPTO = NumberFormat.DecFormat(AgencyWPTO_base, 2),
                                           AgencyTotalTO = NumberFormat.DecFormat(AgencyTotalTO_base, 2),
                                           AgencyCommIncome = NumberFormat.DecFormat(AgencyCommIncome_base, 2),
                                           AgencyPTWL = NumberFormat.DecFormat(AgencyPTWL_base, 2),

                                           IBCWinTO = NumberFormat.DecFormat(IBCWinTO_base, 2),
                                           IBCPlaceTO = NumberFormat.DecFormat(IBCPlaceTO_base, 2),
                                           IBCWPTO = NumberFormat.DecFormat(IBCWPTO_base, 2),
                                           IBCTotalTO = NumberFormat.DecFormat(IBCTotalTO_base, 2),
                                           IBCCommIncome = NumberFormat.DecFormat(IBCCommIncome_base, 2),
                                           IBCCommExpense = NumberFormat.DecFormat(IBCCommExpense_base, 2),
                                           IBCNetIncome = NumberFormat.DecFormat(IBCNetIncome_base, 2),
                                           IBCPTWL = NumberFormat.DecFormat(IBCPTWL_base, 2),
                                           IBCToteFee = NumberFormat.DecFormat(IBCToteFee_base, 2),

                                           PartnerWinTO = NumberFormat.DecFormat(PartnerWinTO_base, 2),
                                           PartnerPlaceTO = NumberFormat.DecFormat(PartnerPlaceTO_base, 2),
                                           PartnerWPTO = NumberFormat.DecFormat(PartnerWPTO_base, 2),
                                           PartnerTotalTO = NumberFormat.DecFormat(PartnerTotalTO_base, 2),
                                           PartnerDividendWon = NumberFormat.DecFormat(PartnerDividendWon_base, 2),
                                           PartnerCommExpense = NumberFormat.DecFormat(PartnerCommExpense_base, 2),
                                           PartnerToteFee = NumberFormat.DecFormat(PartnerToteFee_base, 2),
                                           PartnerAmountDue = NumberFormat.DecFormat(PartnerAmountDue_base, 2),
                                       };
                    lstCurrency.Totals.Add(baseCurr);
                }

                #endregion
                #endregion

                racingDetailsBo.Items.Add(lstCurrency);

            }

            #region GrandTotal
            #region PoolCurr
            grandTotal.Add(new RacingWinlossRowBO()
            {
                CurrencyId = poolCurrency,
                Currency = CacheUtils.GetExchangeName(CastAide.AsInt32(poolCurrency)),

                AgencyWinTO = AgencyWinTO_pool.ToString(),
                AgencyPlaceTO = AgencyPlaceTO_pool.ToString(),
                AgencyWPTO = AgencyWPTO_pool.ToString(),
                AgencyTotalTO = AgencyTotalTO_pool.ToString(),
                AgencyCommIncome = AgencyCommIncome_pool.ToString(),
                AgencyPTWL = AgencyPTWL_pool.ToString(),

                IBCWinTO = IBCWinTO_pool.ToString(),
                IBCPlaceTO = IBCPlaceTO_pool.ToString(),
                IBCWPTO = IBCWPTO_pool.ToString(),
                IBCTotalTO = IBCTotalTO_pool.ToString(),
                IBCCommIncome = IBCCommIncome_pool.ToString(),
                IBCToteFee = IBCToteFee_pool.ToString(),
                IBCCommExpense = IBCCommExpense_pool.ToString(),
                IBCPTWL = IBCPTWL_pool.ToString(),
                IBCNetIncome = IBCNetIncome_pool.ToString(),

                PartnerWinTO = PartnerWinTO_pool.ToString(),
                PartnerPlaceTO = PartnerPlaceTO_pool.ToString(),
                PartnerWPTO = PartnerWPTO_pool.ToString(),
                PartnerTotalTO = PartnerTotalTO_pool.ToString(),
                PartnerDividendWon = PartnerDividendWon_pool.ToString(),
                PartnerCommExpense = PartnerCommExpense_pool.ToString(),
                PartnerToteFee = PartnerToteFee_pool.ToString(),
                PartnerAmountDue = PartnerAmountDue_pool.ToString()
            });

            #endregion

            #region Base
            grandTotal.Add(new RacingWinlossRowBO()
            {
                CurrencyId = "2",
                Currency = "Base (RM)",

                AgencyWinTO = AgencyWinTO_base.ToString(),
                AgencyPlaceTO = AgencyPlaceTO_base.ToString(),
                AgencyWPTO = AgencyWPTO_base.ToString(),
                AgencyTotalTO = AgencyTotalTO_base.ToString(),
                AgencyCommIncome = AgencyCommIncome_base.ToString(),
                AgencyPTWL = AgencyPTWL_base.ToString(),

                IBCWinTO = IBCWinTO_base.ToString(),
                IBCPlaceTO = IBCPlaceTO_base.ToString(),
                IBCWPTO = IBCWPTO_base.ToString(),
                IBCTotalTO = IBCTotalTO_base.ToString(),
                IBCCommIncome = IBCCommIncome_base.ToString(),
                IBCToteFee = IBCToteFee_base.ToString(),
                IBCCommExpense = IBCCommExpense_base.ToString(),
                IBCPTWL = IBCPTWL_base.ToString(),
                IBCNetIncome = IBCNetIncome_base.ToString(),

                PartnerWinTO = PartnerWinTO_base.ToString(),
                PartnerPlaceTO = PartnerPlaceTO_base.ToString(),
                PartnerWPTO = PartnerWPTO_base.ToString(),
                PartnerTotalTO = PartnerTotalTO_base.ToString(),
                PartnerDividendWon = PartnerDividendWon_base.ToString(),
                PartnerCommExpense = PartnerCommExpense_base.ToString(),
                PartnerToteFee = PartnerToteFee_base.ToString(),
                PartnerAmountDue = PartnerAmountDue_base.ToString()
            });

            #endregion
            #endregion

            #region grant total grouped meeting
            var distinctMeetings = GetDistinctMeeting(dataTable);


            #region var pool grant total grouped by meetings
            string CurrencyIdPoolCurr = string.Empty;
            string CurrencyPoolCurr = string.Empty;
            string NamePoolCurr = string.Empty;

            decimal AgencyWinTOPoolCurr = 0;
            decimal AgencyPlaceTOPoolCurr = 0;
            decimal AgencyWPTOPoolCurr = 0;
            decimal AgencyTotalTOPoolCurr = 0;
            decimal AgencyCommIncomePoolCurr = 0;
            decimal AgencyPTWLPoolCurr = 0;

            decimal IBCWinTOPoolCurr = 0;
            decimal IBCPlaceTOPoolCurr = 0;
            decimal IBCWPTOPoolCurr = 0;
            decimal IBCTotalTOPoolCurr = 0;
            decimal IBCCommIncomePoolCurr = 0;
            decimal IBCToteFeePoolCurr = 0;
            decimal IBCCommExpensePoolCurr = 0;
            decimal IBCPTWLPoolCurr = 0;
            decimal IBCNetIncomePoolCurr = 0;

            decimal PartnerWinTOPoolCurr = 0;
            decimal PartnerPlaceTOPoolCurr = 0;
            decimal PartnerWPTOPoolCurr = 0;
            decimal PartnerTotalTOPoolCurr = 0;
            decimal PartnerDividendWonPoolCurr = 0;
            decimal PartnerCommExpensePoolCurr = 0;
            decimal PartnerToteFeePoolCurr = 0;
            decimal PartnerAmountDuePoolCurr = 0;
            #endregion

            #region var base grant total grouped by meetings
            string CurrencyIdBase = string.Empty;
            string CurrencyBase = string.Empty;
            string NameBase = string.Empty;

            decimal AgencyWinTOBase = 0;
            decimal AgencyPlaceTOBase = 0;
            decimal AgencyWPTOBase = 0;
            decimal AgencyTotalTOBase = 0;
            decimal AgencyCommIncomeBase = 0;
            decimal AgencyPTWLBase = 0;

            decimal IBCWinTOBase = 0;
            decimal IBCPlaceTOBase = 0;
            decimal IBCWPTOBase = 0;
            decimal IBCTotalTOBase = 0;
            decimal IBCCommIncomeBase = 0;
            decimal IBCToteFeeBase = 0;
            decimal IBCCommExpenseBase = 0;
            decimal IBCPTWLBase = 0;
            decimal IBCNetIncomeBase = 0;

            decimal PartnerWinTOBase = 0;
            decimal PartnerPlaceTOBase = 0;
            decimal PartnerWPTOBase = 0;
            decimal PartnerTotalTOBase = 0;
            decimal PartnerDividendWonBase = 0;
            decimal PartnerCommExpenseBase = 0;
            decimal PartnerToteFeeBase = 0;
            decimal PartnerAmountDueBase = 0;
            #endregion

            foreach (var meeting in distinctMeetings)
            {
                var racingWinlossRowBoPool = new RacingWinlossRowBO();
                var racingWinlossRowBoBase = new RacingWinlossRowBO();
                var meeting1 = meeting;

                #region var pool grant total grouped by meetings
                CurrencyIdPoolCurr = string.Empty;
                CurrencyPoolCurr = string.Empty;
                NamePoolCurr = string.Empty;

                AgencyWinTOPoolCurr = 0;
                AgencyPlaceTOPoolCurr = 0;
                AgencyWPTOPoolCurr = 0;
                AgencyTotalTOPoolCurr = 0;
                AgencyCommIncomePoolCurr = 0;
                AgencyPTWLPoolCurr = 0;

                IBCWinTOPoolCurr = 0;
                IBCPlaceTOPoolCurr = 0;
                IBCWPTOPoolCurr = 0;
                IBCTotalTOPoolCurr = 0;
                IBCCommIncomePoolCurr = 0;
                IBCToteFeePoolCurr = 0;
                IBCCommExpensePoolCurr = 0;
                IBCPTWLPoolCurr = 0;
                IBCNetIncomePoolCurr = 0;

                PartnerWinTOPoolCurr = 0;
                PartnerPlaceTOPoolCurr = 0;
                PartnerWPTOPoolCurr = 0;
                PartnerTotalTOPoolCurr = 0;
                PartnerDividendWonPoolCurr = 0;
                PartnerCommExpensePoolCurr = 0;
                PartnerToteFeePoolCurr = 0;
                PartnerAmountDuePoolCurr = 0;
                #endregion

                #region var base grant total grouped by meetings
                CurrencyIdBase = string.Empty;
                CurrencyBase = string.Empty;
                NameBase = string.Empty;

                AgencyWinTOBase = 0;
                AgencyPlaceTOBase = 0;
                AgencyWPTOBase = 0;
                AgencyTotalTOBase = 0;
                AgencyCommIncomeBase = 0;
                AgencyPTWLBase = 0;

                IBCWinTOBase = 0;
                IBCPlaceTOBase = 0;
                IBCWPTOBase = 0;
                IBCTotalTOBase = 0;
                IBCCommIncomeBase = 0;
                IBCToteFeeBase = 0;
                IBCCommExpenseBase = 0;
                IBCPTWLBase = 0;
                IBCNetIncomeBase = 0;

                PartnerWinTOBase = 0;
                PartnerPlaceTOBase = 0;
                PartnerWPTOBase = 0;
                PartnerTotalTOBase = 0;
                PartnerDividendWonBase = 0;
                PartnerCommExpenseBase = 0;
                PartnerToteFeeBase = 0;
                PartnerAmountDueBase = 0;
                #endregion

                foreach (var dataRow in
                    dataTable.Rows.Cast<DataRow>().Where(dataRow => dataRow["MeetingId"].ToString() == meeting1.MeetingId.ToString()))
                {
                    #region pool grant total grouped by meetings
                    CurrencyIdPoolCurr = poolCurrency;
                    CurrencyPoolCurr = CacheUtils.GetExchangeName(CastAide.AsInt32(poolCurrency));
                    NamePoolCurr = CacheRef.GetLeagueName(dataRow["MeetingId"]);

                    AgencyWinTOPoolCurr += CastAide.AsDecimal(dataRow["winTO_Agency_PoolCurr"]);
                    AgencyPlaceTOPoolCurr += CastAide.AsDecimal(dataRow["placeTO_Agency_PoolCurr"]);
                    AgencyWPTOPoolCurr += CastAide.AsDecimal(dataRow["winplaceTO_Agency_PoolCurr"]);
                    AgencyTotalTOPoolCurr += CastAide.AsDecimal(dataRow["totalTO_Agency_PoolCurr"]);
                    AgencyCommIncomePoolCurr += CastAide.AsDecimal(dataRow["commIncome_Agency_PoolCurr"]);
                    AgencyPTWLPoolCurr += CastAide.AsDecimal(dataRow["PTWL_Agency_PoolCurr"]);

                    IBCWinTOPoolCurr += CastAide.AsDecimal(dataRow["winTO_IBC_PoolCurr"]);
                    IBCPlaceTOPoolCurr += CastAide.AsDecimal(dataRow["placeTO_IBC_PoolCurr"]);
                    IBCWPTOPoolCurr += CastAide.AsDecimal(dataRow["winplaceTO_IBC_PoolCurr"]);
                    IBCTotalTOPoolCurr += CastAide.AsDecimal(dataRow["totalTO_IBC_PoolCurr"]);
                    IBCCommIncomePoolCurr += CastAide.AsDecimal(dataRow["commIncome_IBC_PoolCurr"]);
                    IBCToteFeePoolCurr += CastAide.AsDecimal(dataRow["FeeDue2_PoolCurr"]);
                    IBCCommExpensePoolCurr += CastAide.AsDecimal(dataRow["commIncome_Agency_PoolCurr"]);
                    IBCPTWLPoolCurr += CastAide.AsDecimal(dataRow["PTWL_IBC_PoolCurr"]);
                    IBCNetIncomePoolCurr += CastAide.AsDecimal(dataRow["netIncome_IBC_PoolCurr"]);

                    PartnerWinTOPoolCurr += CastAide.AsDecimal(dataRow["winTO_Partner_PoolCurr"]);
                    PartnerPlaceTOPoolCurr += CastAide.AsDecimal(dataRow["placeTO_Partner_PoolCurr"]);
                    PartnerWPTOPoolCurr += CastAide.AsDecimal(dataRow["winPlaceTO_Partner_PoolCurr"]);
                    PartnerTotalTOPoolCurr += CastAide.AsDecimal(dataRow["totalTO_Partner_PoolCurr"]);
                    PartnerDividendWonPoolCurr += CastAide.AsDecimal(dataRow["dividendWon_PoolCurr"]);
                    PartnerCommExpensePoolCurr += CastAide.AsDecimal(dataRow["commIncome_IBC_PoolCurr"]);
                    PartnerToteFeePoolCurr += CastAide.AsDecimal(dataRow["FeeDue2_PoolCurr"]);
                    PartnerAmountDuePoolCurr += CastAide.AsDecimal(dataRow["AmountDue_PoolCurr"]);
                    #endregion

                    //---------------------------------------------------------------------------------------------------------
                    #region base grant total grouped by meetings
                    CurrencyIdBase = "2";
                    CurrencyBase = "Base (RM)";
                    NameBase = CacheRef.GetLeagueName(dataRow["MeetingId"]);

                    AgencyWinTOBase += CastAide.AsDecimal(dataRow["winTO_Agency"]);
                    AgencyPlaceTOBase += CastAide.AsDecimal(dataRow["placeTO_Agency"]);
                    AgencyWPTOBase += CastAide.AsDecimal(dataRow["winplaceTO_Agency"]);
                    AgencyTotalTOBase += CastAide.AsDecimal(dataRow["totalTO_Agency"]);
                    AgencyCommIncomeBase += CastAide.AsDecimal(dataRow["commIncome_Agency"]);
                    AgencyPTWLBase += CastAide.AsDecimal(dataRow["PTWL_Agency"]);

                    IBCWinTOBase += CastAide.AsDecimal(dataRow["winTO_IBC"]);
                    IBCPlaceTOBase += CastAide.AsDecimal(dataRow["placeTO_IBC"]);
                    IBCWPTOBase += CastAide.AsDecimal(dataRow["winplaceTO_IBC"]);
                    IBCTotalTOBase += CastAide.AsDecimal(dataRow["totalTO_IBC"]);
                    IBCCommIncomeBase += CastAide.AsDecimal(dataRow["commIncome_IBC"]);
                    IBCToteFeeBase += CastAide.AsDecimal(dataRow["FeeDue2"]);
                    IBCCommExpenseBase += CastAide.AsDecimal(dataRow["commIncome_Agency"]);
                    IBCPTWLBase += CastAide.AsDecimal(dataRow["PTWL_IBC"]);
                    IBCNetIncomeBase += CastAide.AsDecimal(dataRow["netIncome_IBC"]);

                    PartnerWinTOBase += CastAide.AsDecimal(dataRow["winTO_Partner"]);
                    PartnerPlaceTOBase += CastAide.AsDecimal(dataRow["placeTO_Partner"]);
                    PartnerWPTOBase += CastAide.AsDecimal(dataRow["winPlaceTO_Partner"]);
                    PartnerTotalTOBase += CastAide.AsDecimal(dataRow["totalTO_Partner"]);
                    PartnerDividendWonBase += CastAide.AsDecimal(dataRow["dividendWon"]);
                    PartnerCommExpenseBase += CastAide.AsDecimal(dataRow["commIncome_IBC"]);
                    PartnerToteFeeBase += CastAide.AsDecimal(dataRow["FeeDue2"]);
                    PartnerAmountDueBase += CastAide.AsDecimal(dataRow["AmountDue"]);
                    #endregion
                }
                #region pool grant total grouped by meetings
                racingWinlossRowBoPool.CurrencyId = CurrencyIdPoolCurr;
                racingWinlossRowBoPool.Currency = CurrencyPoolCurr;
                racingWinlossRowBoPool.Name = NamePoolCurr;

                racingWinlossRowBoPool.AgencyWinTO = NumberFormat.DecFormat(AgencyWinTOPoolCurr, 2);
                racingWinlossRowBoPool.AgencyPlaceTO = NumberFormat.DecFormat(AgencyPlaceTOPoolCurr, 2);
                racingWinlossRowBoPool.AgencyWPTO = NumberFormat.DecFormat(AgencyWPTOPoolCurr, 2);
                racingWinlossRowBoPool.AgencyTotalTO = NumberFormat.DecFormat(AgencyTotalTOPoolCurr, 2);
                racingWinlossRowBoPool.AgencyCommIncome = NumberFormat.DecFormat(AgencyCommIncomePoolCurr, 2);
                racingWinlossRowBoPool.AgencyPTWL = NumberFormat.DecFormat(AgencyPTWLPoolCurr, 2);

                racingWinlossRowBoPool.IBCWinTO = NumberFormat.DecFormat(IBCWinTOPoolCurr, 2);
                racingWinlossRowBoPool.IBCPlaceTO = NumberFormat.DecFormat(IBCPlaceTOPoolCurr, 2);
                racingWinlossRowBoPool.IBCWPTO = NumberFormat.DecFormat(IBCWPTOPoolCurr, 2);
                racingWinlossRowBoPool.IBCTotalTO = NumberFormat.DecFormat(IBCTotalTOPoolCurr, 2);
                racingWinlossRowBoPool.IBCCommIncome = NumberFormat.DecFormat(IBCCommIncomePoolCurr, 2);
                racingWinlossRowBoPool.IBCToteFee = NumberFormat.DecFormat(IBCToteFeePoolCurr, 2);
                racingWinlossRowBoPool.IBCCommExpense = NumberFormat.DecFormat(IBCCommExpensePoolCurr, 2);
                racingWinlossRowBoPool.IBCPTWL = NumberFormat.DecFormat(IBCPTWLPoolCurr, 2);
                racingWinlossRowBoPool.IBCNetIncome = NumberFormat.DecFormat(IBCNetIncomePoolCurr, 2);

                racingWinlossRowBoPool.PartnerWinTO = NumberFormat.DecFormat(PartnerWinTOPoolCurr, 2);
                racingWinlossRowBoPool.PartnerPlaceTO = NumberFormat.DecFormat(PartnerPlaceTOPoolCurr, 2);
                racingWinlossRowBoPool.PartnerWPTO = NumberFormat.DecFormat(PartnerWPTOPoolCurr, 2);
                racingWinlossRowBoPool.PartnerTotalTO = NumberFormat.DecFormat(PartnerTotalTOPoolCurr, 2);
                racingWinlossRowBoPool.PartnerDividendWon = NumberFormat.DecFormat(PartnerDividendWonPoolCurr, 2);
                racingWinlossRowBoPool.PartnerCommExpense = NumberFormat.DecFormat(PartnerCommExpensePoolCurr, 2);
                racingWinlossRowBoPool.PartnerToteFee = NumberFormat.DecFormat(PartnerToteFeePoolCurr, 2);
                racingWinlossRowBoPool.PartnerAmountDue = NumberFormat.DecFormat(PartnerAmountDuePoolCurr, 2);
                #endregion

                #region base grant total grouped by meetings
                racingWinlossRowBoBase.CurrencyId = CurrencyIdBase;
                racingWinlossRowBoBase.Currency = CurrencyBase;
                racingWinlossRowBoBase.Name = NameBase;

                racingWinlossRowBoBase.AgencyWinTO = NumberFormat.DecFormat(AgencyWinTOBase, 2);
                racingWinlossRowBoBase.AgencyPlaceTO = NumberFormat.DecFormat(AgencyPlaceTOBase, 2);
                racingWinlossRowBoBase.AgencyWPTO = NumberFormat.DecFormat(AgencyWPTOBase, 2);
                racingWinlossRowBoBase.AgencyTotalTO = NumberFormat.DecFormat(AgencyTotalTOBase, 2);
                racingWinlossRowBoBase.AgencyCommIncome = NumberFormat.DecFormat(AgencyCommIncomeBase, 2);
                racingWinlossRowBoBase.AgencyPTWL = NumberFormat.DecFormat(AgencyPTWLBase, 2);

                racingWinlossRowBoBase.IBCWinTO = NumberFormat.DecFormat(IBCWinTOBase, 2);
                racingWinlossRowBoBase.IBCPlaceTO = NumberFormat.DecFormat(IBCPlaceTOBase, 2);
                racingWinlossRowBoBase.IBCWPTO = NumberFormat.DecFormat(IBCWPTOBase, 2);
                racingWinlossRowBoBase.IBCTotalTO = NumberFormat.DecFormat(IBCTotalTOBase, 2);
                racingWinlossRowBoBase.IBCCommIncome = NumberFormat.DecFormat(IBCCommIncomeBase, 2);
                racingWinlossRowBoBase.IBCToteFee = NumberFormat.DecFormat(IBCToteFeeBase, 2);
                racingWinlossRowBoBase.IBCCommExpense = NumberFormat.DecFormat(IBCCommExpenseBase, 2);
                racingWinlossRowBoBase.IBCPTWL = NumberFormat.DecFormat(IBCPTWLBase, 2);
                racingWinlossRowBoBase.IBCNetIncome = NumberFormat.DecFormat(IBCNetIncomeBase, 2);

                racingWinlossRowBoBase.PartnerWinTO = NumberFormat.DecFormat(PartnerWinTOBase, 2);
                racingWinlossRowBoBase.PartnerPlaceTO = NumberFormat.DecFormat(PartnerPlaceTOBase, 2);
                racingWinlossRowBoBase.PartnerWPTO = NumberFormat.DecFormat(PartnerWPTOBase, 2);
                racingWinlossRowBoBase.PartnerTotalTO = NumberFormat.DecFormat(PartnerTotalTOBase, 2);
                racingWinlossRowBoBase.PartnerDividendWon = NumberFormat.DecFormat(PartnerDividendWonBase, 2);
                racingWinlossRowBoBase.PartnerCommExpense = NumberFormat.DecFormat(PartnerCommExpenseBase, 2);
                racingWinlossRowBoBase.PartnerToteFee = NumberFormat.DecFormat(PartnerToteFeeBase, 2);
                racingWinlossRowBoBase.PartnerAmountDue = NumberFormat.DecFormat(PartnerAmountDueBase, 2);
                #endregion
                racingDetailsBo.Totals.Add(racingWinlossRowBoPool);
                //racingDetailsBo.Totals.Add(racingWinlossRowBoBase);
            }
            #endregion

            #region Calculate Grand Total
            
            var grand = grandTotal.OrderByDescending(s => s.Currency).Select(s=>s.Currency).Distinct().ToArray();
            foreach (string currency in grand)
            {
                racingDetailsBo.GrandTotals.Add(new RacingWinlossRowBO()
                                               {
                                                   Name="Grand Total",
                                                    Currency = currency,
                                                    
                                                    AgencyWinTO = NumberFormat.DecFormat( grandTotal.Where(s=>s.Currency == currency).Sum(s=> CastAide.AsDecimal(s.AgencyWinTO)),2),
                                                    AgencyPlaceTO = NumberFormat.DecFormat( grandTotal.Where(s=>s.Currency == currency).Sum(s=> CastAide.AsDecimal(s.AgencyPlaceTO)),2), 
                                                    AgencyWPTO = NumberFormat.DecFormat( grandTotal.Where(s=>s.Currency == currency).Sum(s=> CastAide.AsDecimal(s.AgencyWPTO)),2), 
                                                    AgencyTotalTO = NumberFormat.DecFormat( grandTotal.Where(s=>s.Currency == currency).Sum(s=> CastAide.AsDecimal(s.AgencyTotalTO)),2), 
                                                    AgencyCommIncome = NumberFormat.DecFormat( grandTotal.Where(s=>s.Currency == currency).Sum(s=> CastAide.AsDecimal(s.AgencyCommIncome)),2), 
                                                    AgencyPTWL = NumberFormat.DecFormat( grandTotal.Where(s=>s.Currency == currency).Sum(s=> CastAide.AsDecimal(s.AgencyPTWL)),2), 
                                                   
                                                    IBCWinTO = NumberFormat.DecFormat( grandTotal.Where(s=>s.Currency == currency).Sum(s=> CastAide.AsDecimal(s.IBCWinTO)),2), 
                                                    IBCPlaceTO = NumberFormat.DecFormat( grandTotal.Where(s=>s.Currency == currency).Sum(s=> CastAide.AsDecimal(s.IBCPlaceTO)),2), 
                                                    IBCWPTO = NumberFormat.DecFormat( grandTotal.Where(s=>s.Currency == currency).Sum(s=> CastAide.AsDecimal(s.IBCWPTO)),2), 
                                                    IBCTotalTO = NumberFormat.DecFormat( grandTotal.Where(s=>s.Currency == currency).Sum(s=> CastAide.AsDecimal(s.IBCTotalTO)),2), 
                                                    IBCCommIncome = NumberFormat.DecFormat( grandTotal.Where(s=>s.Currency == currency).Sum(s=> CastAide.AsDecimal(s.IBCCommIncome)),2), 
                                                    IBCCommExpense = NumberFormat.DecFormat( grandTotal.Where(s=>s.Currency == currency).Sum(s=> CastAide.AsDecimal(s.IBCCommExpense)),2), 
                                                    IBCNetIncome = NumberFormat.DecFormat( grandTotal.Where(s=>s.Currency == currency).Sum(s=> CastAide.AsDecimal(s.IBCNetIncome)),2), 
                                                    IBCPTWL = NumberFormat.DecFormat( grandTotal.Where(s=>s.Currency == currency).Sum(s=> CastAide.AsDecimal(s.IBCPTWL)),2), 
                                                    IBCToteFee = NumberFormat.DecFormat( grandTotal.Where(s=>s.Currency == currency).Sum(s=> CastAide.AsDecimal(s.IBCToteFee)),2), 
                                                   
                                                    PartnerWinTO = NumberFormat.DecFormat( grandTotal.Where(s=>s.Currency == currency).Sum(s=> CastAide.AsDecimal(s.PartnerWinTO)),2), 
                                                    PartnerPlaceTO = NumberFormat.DecFormat( grandTotal.Where(s=>s.Currency == currency).Sum(s=> CastAide.AsDecimal(s.PartnerPlaceTO)),2), 
                                                    PartnerWPTO = NumberFormat.DecFormat( grandTotal.Where(s=>s.Currency == currency).Sum(s=> CastAide.AsDecimal(s.PartnerWPTO)),2), 
                                                    PartnerTotalTO = NumberFormat.DecFormat( grandTotal.Where(s=>s.Currency == currency).Sum(s=> CastAide.AsDecimal(s.PartnerTotalTO)),2), 
                                                    PartnerDividendWon = NumberFormat.DecFormat( grandTotal.Where(s=>s.Currency == currency).Sum(s=> CastAide.AsDecimal(s.PartnerDividendWon)),2), 
                                                    PartnerCommExpense =  NumberFormat.DecFormat( grandTotal.Where(s=>s.Currency == currency).Sum(s=> CastAide.AsDecimal(s.PartnerCommExpense)),2),
                                                    PartnerToteFee = NumberFormat.DecFormat(grandTotal.Where(s => s.Currency == currency).Sum(s => CastAide.AsDecimal(s.PartnerToteFee)), 2),
                                                    PartnerAmountDue = NumberFormat.DecFormat(grandTotal.Where(s => s.Currency == currency).Sum(s => CastAide.AsDecimal(s.PartnerAmountDue)), 2), 
                                               });
            }
            #endregion

            SessionExtension.AddSession(HttpContext.Current.Session, string.Format(_winLossRacingReportDetailSessionKey, poolCurrency, partnerId), racingDetailsBo);
            return racingDetailsBo;
        }

        public RacingWinlossDetailsBO GetWinlossRacingReportDetailsNew(string winlossDate, string poolCurrency, string partnerId)
        {

            DataTable dataTable = _reportRacingDao.WinlostRacingTote_DetailsNew(winlossDate, poolCurrency, partnerId);

            if (dataTable == null)
                return new RacingWinlossDetailsBO();

            #region Variable List Object


            var arrCurrencies = GetDistinctCurrency(dataTable);

            var racingDetailsBo = new RacingWinlossDetailsBO();
            racingDetailsBo.Items = new List<RacingWinlossCurrencyBO>();
            racingDetailsBo.Totals = new List<RacingWinlossRowBO>();
            racingDetailsBo.GrandTotals = new List<RacingWinlossRowBO>();
            #endregion

            #region Currency

            #region variable

            decimal AgencyWinTO;
            decimal AgencyPlaceTO;
            decimal AgencyWPTO;
            decimal AgencyTotalTO;
            decimal AgencyCommIncome;
            decimal AgencyPTWL;

            decimal IBCWinTO;
            decimal IBCPlaceTO;
            decimal IBCWPTO;
            decimal IBCTotalTO;
            decimal IBCCommIncome;
            decimal IBCToteFee;
            decimal IBCCommExpense;
            decimal IBCPTWL;
            decimal IBCNetIncome;

            decimal PartnerWinTO;
            decimal PartnerPlaceTO;
            decimal PartnerWPTO;
            decimal PartnerTotalTO;
            decimal PartnerDividendWon;
            decimal PartnerCommExpense;
            decimal PartnerToteFee;
            decimal PartnerAmountDue;

            //currency origin
            decimal AgencyWinTO_origin = 0;
            decimal AgencyPlaceTO_origin = 0;
            decimal AgencyWPTO_origin = 0;
            decimal AgencyTotalTO_origin = 0;
            decimal AgencyCommIncome_origin = 0;
            decimal AgencyPTWL_origin = 0;

            decimal IBCWinTO_origin = 0;
            decimal IBCPlaceTO_origin = 0;
            decimal IBCWPTO_origin = 0;
            decimal IBCTotalTO_origin = 0;
            decimal IBCCommIncome_origin = 0;
            decimal IBCToteFee_origin = 0;
            decimal IBCCommExpense_origin = 0;
            decimal IBCPTWL_origin = 0;
            decimal IBCNetIncome_origin = 0;

            decimal PartnerWinTO_origin = 0;
            decimal PartnerPlaceTO_origin = 0;
            decimal PartnerWPTO_origin = 0;
            decimal PartnerTotalTO_origin = 0;
            decimal PartnerDividendWon_origin = 0;
            decimal PartnerCommExpense_origin = 0;
            decimal PartnerToteFee_origin = 0;
            decimal PartnerAmountDue_origin = 0;

            //Pool total
            decimal AgencyWinTO_pool = 0;
            decimal AgencyPlaceTO_pool = 0;
            decimal AgencyWPTO_pool = 0;
            decimal AgencyTotalTO_pool = 0;
            decimal AgencyCommIncome_pool = 0;
            decimal AgencyPTWL_pool = 0;

            decimal IBCWinTO_pool = 0;
            decimal IBCPlaceTO_pool = 0;
            decimal IBCWPTO_pool = 0;
            decimal IBCTotalTO_pool = 0;
            decimal IBCCommIncome_pool = 0;
            decimal IBCToteFee_pool = 0;
            decimal IBCCommExpense_pool = 0;
            decimal IBCPTWL_pool = 0;
            decimal IBCNetIncome_pool = 0;

            decimal PartnerWinTO_pool = 0;
            decimal PartnerPlaceTO_pool = 0;
            decimal PartnerWPTO_pool = 0;
            decimal PartnerTotalTO_pool = 0;
            decimal PartnerDividendWon_pool = 0;
            decimal PartnerCommExpense_pool = 0;
            decimal PartnerToteFee_pool = 0;
            decimal PartnerAmountDue_pool = 0;

            //Base total
            decimal AgencyWinTO_base = 0;
            decimal AgencyPlaceTO_base = 0;
            decimal AgencyWPTO_base = 0;
            decimal AgencyTotalTO_base = 0;
            decimal AgencyCommIncome_base = 0;
            decimal AgencyPTWL_base = 0;

            decimal IBCWinTO_base = 0;
            decimal IBCPlaceTO_base = 0;
            decimal IBCWPTO_base = 0;
            decimal IBCTotalTO_base = 0;
            decimal IBCCommIncome_base = 0;
            decimal IBCToteFee_base = 0;
            decimal IBCCommExpense_base = 0;
            decimal IBCPTWL_base = 0;
            decimal IBCNetIncome_base = 0;

            decimal PartnerWinTO_base = 0;
            decimal PartnerPlaceTO_base = 0;
            decimal PartnerWPTO_base = 0;
            decimal PartnerTotalTO_base = 0;
            decimal PartnerDividendWon_base = 0;
            decimal PartnerCommExpense_base = 0;
            decimal PartnerToteFee_base = 0;
            decimal PartnerAmountDue_base = 0;
            #endregion
            #endregion

            List<RacingWinlossRowBO> grandTotal = new List<RacingWinlossRowBO>();
            foreach (MeetingCurr curr in arrCurrencies)
            {
                #region GrandTotal
                #region PoolCurr
                grandTotal.Add(new RacingWinlossRowBO()
                {
                    CurrencyId = poolCurrency,
                    Currency = CacheUtils.GetExchangeName(CastAide.AsInt32(poolCurrency)),

                    AgencyWinTO = AgencyWinTO_pool.ToString(),
                    AgencyPlaceTO = AgencyPlaceTO_pool.ToString(),
                    AgencyWPTO = AgencyWPTO_pool.ToString(),
                    AgencyTotalTO = AgencyTotalTO_pool.ToString(),
                    AgencyCommIncome = AgencyCommIncome_pool.ToString(),
                    AgencyPTWL = AgencyPTWL_pool.ToString(),

                    IBCWinTO = IBCWinTO_pool.ToString(),
                    IBCPlaceTO = IBCPlaceTO_pool.ToString(),
                    IBCWPTO = IBCWPTO_pool.ToString(),
                    IBCTotalTO = IBCTotalTO_pool.ToString(),
                    IBCCommIncome = IBCCommIncome_pool.ToString(),
                    IBCToteFee = IBCToteFee_pool.ToString(),
                    IBCCommExpense = IBCCommExpense_pool.ToString(),
                    IBCPTWL = IBCPTWL_pool.ToString(),
                    IBCNetIncome = IBCNetIncome_pool.ToString(),

                    PartnerWinTO = PartnerWinTO_pool.ToString(),
                    PartnerPlaceTO = PartnerPlaceTO_pool.ToString(),
                    PartnerWPTO = PartnerWPTO_pool.ToString(),
                    PartnerTotalTO = PartnerTotalTO_pool.ToString(),
                    PartnerDividendWon = PartnerDividendWon_pool.ToString(),
                    PartnerCommExpense = PartnerCommExpense_pool.ToString(),
                    PartnerToteFee = PartnerToteFee_pool.ToString(),
                    PartnerAmountDue = PartnerAmountDue_pool.ToString()
                });

                #endregion

                #region Base
                grandTotal.Add(new RacingWinlossRowBO()
                {
                    CurrencyId = "2",
                    Currency = "Base (RM)",

                    AgencyWinTO = AgencyWinTO_base.ToString(),
                    AgencyPlaceTO = AgencyPlaceTO_base.ToString(),
                    AgencyWPTO = AgencyWPTO_base.ToString(),
                    AgencyTotalTO = AgencyTotalTO_base.ToString(),
                    AgencyCommIncome = AgencyCommIncome_base.ToString(),
                    AgencyPTWL = AgencyPTWL_base.ToString(),

                    IBCWinTO = IBCWinTO_base.ToString(),
                    IBCPlaceTO = IBCPlaceTO_base.ToString(),
                    IBCWPTO = IBCWPTO_base.ToString(),
                    IBCTotalTO = IBCTotalTO_base.ToString(),
                    IBCCommIncome = IBCCommIncome_base.ToString(),
                    IBCToteFee = IBCToteFee_base.ToString(),
                    IBCCommExpense = IBCCommExpense_base.ToString(),
                    IBCPTWL = IBCPTWL_base.ToString(),
                    IBCNetIncome = IBCNetIncome_base.ToString(),

                    PartnerWinTO = PartnerWinTO_base.ToString(),
                    PartnerPlaceTO = PartnerPlaceTO_base.ToString(),
                    PartnerWPTO = PartnerWPTO_base.ToString(),
                    PartnerTotalTO = PartnerTotalTO_base.ToString(),
                    PartnerDividendWon = PartnerDividendWon_base.ToString(),
                    PartnerCommExpense = PartnerCommExpense_base.ToString(),
                    PartnerToteFee = PartnerToteFee_base.ToString(),
                    PartnerAmountDue = PartnerAmountDue_base.ToString()
                });

                #endregion
                #endregion

                #region assign default
                #region origin
                AgencyWinTO_origin = 0;
                AgencyPlaceTO_origin = 0;
                AgencyWPTO_origin = 0;
                AgencyTotalTO_origin = 0;
                AgencyCommIncome_origin = 0;
                AgencyPTWL_origin = 0;

                IBCWinTO_origin = 0;
                IBCPlaceTO_origin = 0;
                IBCWPTO_origin = 0;
                IBCTotalTO_origin = 0;
                IBCCommIncome_origin = 0;
                IBCToteFee_origin = 0;
                IBCCommExpense_origin = 0;
                IBCPTWL_origin = 0;
                IBCNetIncome_origin = 0;

                PartnerWinTO_origin = 0;
                PartnerPlaceTO_origin = 0;
                PartnerWPTO_origin = 0;
                PartnerTotalTO_origin = 0;
                PartnerDividendWon_origin = 0;
                PartnerCommExpense_origin = 0;
                PartnerToteFee_origin = 0;
                PartnerAmountDue_origin = 0;
                #endregion
                #region Pool
                AgencyWinTO_pool = 0;
                AgencyPlaceTO_pool = 0;
                AgencyWPTO_pool = 0;
                AgencyTotalTO_pool = 0;
                AgencyCommIncome_pool = 0;
                AgencyPTWL_pool = 0;

                IBCWinTO_pool = 0;
                IBCPlaceTO_pool = 0;
                IBCWPTO_pool = 0;
                IBCTotalTO_pool = 0;
                IBCCommIncome_pool = 0;
                IBCToteFee_pool = 0;
                IBCCommExpense_pool = 0;
                IBCPTWL_pool = 0;
                IBCNetIncome_pool = 0;

                PartnerWinTO_pool = 0;
                PartnerPlaceTO_pool = 0;
                PartnerWPTO_pool = 0;
                PartnerTotalTO_pool = 0;
                PartnerDividendWon_pool = 0;
                PartnerCommExpense_pool = 0;
                PartnerToteFee_pool = 0;
                PartnerAmountDue_pool = 0;
                #endregion

                #region Base
                AgencyWinTO_base = 0;
                AgencyPlaceTO_base = 0;
                AgencyWPTO_base = 0;
                AgencyTotalTO_base = 0;
                AgencyCommIncome_base = 0;
                AgencyPTWL_base = 0;

                IBCWinTO_base = 0;
                IBCPlaceTO_base = 0;
                IBCWPTO_base = 0;
                IBCTotalTO_base = 0;
                IBCCommIncome_base = 0;
                IBCToteFee_base = 0;
                IBCCommExpense_base = 0;
                IBCPTWL_base = 0;
                IBCNetIncome_base = 0;

                PartnerWinTO_base = 0;
                PartnerPlaceTO_base = 0;
                PartnerWPTO_base = 0;
                PartnerTotalTO_base = 0;
                PartnerDividendWon_base = 0;
                PartnerCommExpense_base = 0;
                PartnerToteFee_base = 0;
                PartnerAmountDue_base = 0;
                #endregion
                #endregion

                var arrDistinctMeeting = GetDistinctMeetingCurrency(dataTable, curr.CurrencyId);

                var lstCurrency = new RacingWinlossCurrencyBO();
                lstCurrency.Items = new List<RacingWinlossMeetingBO>();
                lstCurrency.Totals = new List<RacingWinlossRowBO>();

                //Meeting by Currency
                int[] arrBaseCurr = { 2 };
                int currentCurr = 0;
                foreach (MeetingCurr meetingCurr in arrDistinctMeeting)
                {
                    currentCurr = meetingCurr.CurrencyId;
                    DataRow[] rows = dataTable.Select("currency = " + curr.CurrencyId + " and meetingid = " + meetingCurr.MeetingId);

                    var lstMeeting = new RacingWinlossMeetingBO();
                    lstMeeting.Items = new List<RacingWinlossRowBO>();
                    lstMeeting.Totals = new List<RacingWinlossRowBO>();

                    foreach (DataRow dataRow in rows)
                    {
                        #region row
                        var row = new RacingWinlossRowBO()
                        {
                            CurrencyId = CastAide.AsString(dataRow["currency"]),
                            Currency = CastAide.AsString(dataRow["currencyName"]),
                            Id = CastAide.AsString(dataRow["MeetingId"]),
                            Name = CacheRef.GetLeagueName(dataRow["MeetingId"]),
                            MeetingID = dataRow["MeetingId"].ToString(),
                            RaceNo = CastAide.AsString(dataRow["RaceNo"]),

                            AgencyWinTO = NumberFormat.DecFormat(dataRow["winTO_Agency_Origin"], 2),
                            AgencyPlaceTO = NumberFormat.DecFormat(dataRow["placeTO_Agency_Origin"], 2),
                            AgencyWPTO = NumberFormat.DecFormat(dataRow["winplaceTO_Agency_Origin"], 2),
                            AgencyTotalTO = NumberFormat.DecFormat(dataRow["totalTO_Agency_Origin"], 2),
                            AgencyCommIncome = NumberFormat.DecFormat(dataRow["commIncome_Agency_Origin"], 2),
                            AgencyPTWL = NumberFormat.DecFormat(dataRow["PTWL_Agency_Origin"], 2),

                            IBCWinTO = NumberFormat.DecFormat(dataRow["winTO_IBC_Origin"], 2),
                            IBCPlaceTO = NumberFormat.DecFormat(dataRow["placeTO_IBC_Origin"], 2),
                            IBCWPTO = NumberFormat.DecFormat(dataRow["winplaceTO_IBC_Origin"], 2),
                            IBCTotalTO = NumberFormat.DecFormat(dataRow["totalTO_IBC_Origin"], 2),
                            IBCCommIncome = NumberFormat.DecFormat(dataRow["commIncome_IBC_Origin"], 2),
                            IBCCommExpense = NumberFormat.DecFormat(dataRow["commIncome_Agency_Origin"], 2),
                            IBCNetIncome = NumberFormat.DecFormat(dataRow["netIncome_IBC_Origin"], 2),
                            IBCPTWL = NumberFormat.DecFormat(dataRow["PTWL_IBC_Origin"], 2),
                            IBCToteFee = NumberFormat.DecFormat(dataRow["FeeDue2_Origin"], 2),

                            PartnerWinTO = NumberFormat.DecFormat(dataRow["winTO_Partner_Origin"], 2),
                            PartnerPlaceTO = NumberFormat.DecFormat(dataRow["placeTO_Partner_Origin"], 2),
                            PartnerWPTO = NumberFormat.DecFormat(dataRow["winplaceTO_Partner_Origin"], 2),
                            PartnerTotalTO = NumberFormat.DecFormat(dataRow["totalTO_Partner_Origin"], 2),
                            PartnerDividendWon = NumberFormat.DecFormat(dataRow["dividendWon_Origin"], 2),
                            PartnerAmountDue = NumberFormat.DecFormat(dataRow["AmountDue_Origin"], 2),
                            PartnerCommExpense = NumberFormat.DecFormat(dataRow["commIncome_IBC_Origin"], 2),
                            PartnerToteFee = NumberFormat.DecFormat(dataRow["FeeDue2_Origin"], 2),
                        };
                        #endregion

                        lstMeeting.Items.Add(row);

                        #region Total Curr Origin
                        AgencyWinTO_origin += CastAide.AsDecimal(dataRow["winTO_Agency_Origin"].ToString());
                        AgencyPlaceTO_origin += CastAide.AsDecimal(dataRow["placeTO_Agency_Origin"].ToString());
                        AgencyWPTO_origin += CastAide.AsDecimal(dataRow["winplaceTO_Agency_Origin"].ToString());
                        AgencyTotalTO_origin += CastAide.AsDecimal(dataRow["totalTO_Agency_Origin"].ToString());
                        AgencyCommIncome_origin += CastAide.AsDecimal(dataRow["commIncome_Agency_Origin"].ToString());
                        AgencyPTWL_origin += CastAide.AsDecimal(dataRow["PTWL_Agency_Origin"].ToString());

                        IBCWinTO_origin += CastAide.AsDecimal(dataRow["winTO_IBC_Origin"].ToString());
                        IBCPlaceTO_origin += CastAide.AsDecimal(dataRow["placeTO_IBC_Origin"].ToString());
                        IBCWPTO_origin += CastAide.AsDecimal(dataRow["winplaceTO_IBC_Origin"].ToString());
                        IBCTotalTO_origin += CastAide.AsDecimal(dataRow["totalTO_IBC_Origin"].ToString());
                        IBCCommIncome_origin += CastAide.AsDecimal(dataRow["commIncome_IBC_Origin"].ToString());
                        IBCToteFee_origin += CastAide.AsDecimal(dataRow["FeeDue2_Origin"].ToString());
                        IBCCommExpense_origin += CastAide.AsDecimal(dataRow["commIncome_Agency_Origin"].ToString());
                        IBCPTWL_origin += CastAide.AsDecimal(dataRow["PTWL_IBC_Origin"].ToString());
                        IBCNetIncome_origin += CastAide.AsDecimal(dataRow["netIncome_IBC_Origin"].ToString());

                        PartnerWinTO_origin += CastAide.AsDecimal(dataRow["winTO_Partner_Origin"].ToString());
                        PartnerPlaceTO_origin += CastAide.AsDecimal(dataRow["placeTO_Partner_Origin"].ToString());
                        PartnerWPTO_origin += CastAide.AsDecimal(dataRow["winPlaceTO_Partner_Origin"].ToString());
                        PartnerTotalTO_origin += CastAide.AsDecimal(dataRow["totalTO_Partner_Origin"].ToString());
                        PartnerDividendWon_origin += CastAide.AsDecimal(dataRow["dividendWon_Origin"].ToString());
                        PartnerCommExpense_origin += CastAide.AsDecimal(dataRow["commIncome_IBC_Origin"].ToString());
                        PartnerToteFee_origin += CastAide.AsDecimal(dataRow["FeeDue2_Origin"].ToString());
                        PartnerAmountDue_origin += CastAide.AsDecimal(dataRow["AmountDue_Origin"].ToString());
                        #endregion

                        #region PoolCurr
                        AgencyWinTO_pool += CastAide.AsDecimal(dataRow["winTO_Agency_PoolCurr"].ToString());
                        AgencyPlaceTO_pool += CastAide.AsDecimal(dataRow["placeTO_Agency_PoolCurr"].ToString());
                        AgencyWPTO_pool += CastAide.AsDecimal(dataRow["winplaceTO_Agency_PoolCurr"].ToString());
                        AgencyTotalTO_pool += CastAide.AsDecimal(dataRow["totalTO_Agency_PoolCurr"].ToString());
                        AgencyCommIncome_pool += CastAide.AsDecimal(dataRow["commIncome_Agency_PoolCurr"].ToString());
                        AgencyPTWL_pool += CastAide.AsDecimal(dataRow["PTWL_Agency_PoolCurr"].ToString());

                        IBCWinTO_pool += CastAide.AsDecimal(dataRow["winTO_IBC_PoolCurr"].ToString());
                        IBCPlaceTO_pool += CastAide.AsDecimal(dataRow["placeTO_IBC_PoolCurr"].ToString());
                        IBCWPTO_pool += CastAide.AsDecimal(dataRow["winplaceTO_IBC_PoolCurr"].ToString());
                        IBCTotalTO_pool += CastAide.AsDecimal(dataRow["totalTO_IBC_PoolCurr"].ToString());
                        IBCCommIncome_pool += CastAide.AsDecimal(dataRow["commIncome_IBC_PoolCurr"].ToString());
                        IBCToteFee_pool += CastAide.AsDecimal(dataRow["FeeDue2_PoolCurr"].ToString());
                        IBCCommExpense_pool += CastAide.AsDecimal(dataRow["commIncome_Agency_PoolCurr"].ToString());
                        IBCPTWL_pool += CastAide.AsDecimal(dataRow["PTWL_IBC_PoolCurr"].ToString());
                        IBCNetIncome_pool += CastAide.AsDecimal(dataRow["netIncome_IBC_PoolCurr"].ToString());

                        PartnerWinTO_pool += CastAide.AsDecimal(dataRow["winTO_Partner_PoolCurr"].ToString());
                        PartnerPlaceTO_pool += CastAide.AsDecimal(dataRow["placeTO_Partner_PoolCurr"].ToString());
                        PartnerWPTO_pool += CastAide.AsDecimal(dataRow["winPlaceTO_Partner_PoolCurr"].ToString());
                        PartnerTotalTO_pool += CastAide.AsDecimal(dataRow["totalTO_Partner_PoolCurr"].ToString());
                        PartnerDividendWon_pool += CastAide.AsDecimal(dataRow["dividendWon_PoolCurr"].ToString());
                        PartnerCommExpense_pool += CastAide.AsDecimal(dataRow["commIncome_IBC_PoolCurr"].ToString());
                        PartnerToteFee_pool += CastAide.AsDecimal(dataRow["FeeDue2_PoolCurr"].ToString());
                        PartnerAmountDue_pool += CastAide.AsDecimal(dataRow["AmountDue_PoolCurr"].ToString());
                        #endregion

                        #region base
                        AgencyWinTO_base += CastAide.AsDecimal(dataRow["winTO_Agency"].ToString()); ;
                        AgencyPlaceTO_base += CastAide.AsDecimal(dataRow["placeTO_Agency"].ToString()); ;
                        AgencyWPTO_base += CastAide.AsDecimal(dataRow["winplaceTO_Agency"].ToString());
                        AgencyTotalTO_base += CastAide.AsDecimal(dataRow["totalTO_Agency"].ToString());
                        AgencyCommIncome_base += CastAide.AsDecimal(dataRow["commIncome_Agency"].ToString());
                        AgencyPTWL_base += CastAide.AsDecimal(dataRow["PTWL_Agency"].ToString());

                        IBCWinTO_base += CastAide.AsDecimal(dataRow["winTO_IBC"].ToString());
                        IBCPlaceTO_base += CastAide.AsDecimal(dataRow["placeTO_IBC"].ToString());
                        IBCWPTO_base += CastAide.AsDecimal(dataRow["winplaceTO_IBC"].ToString());
                        IBCTotalTO_base += CastAide.AsDecimal(dataRow["totalTO_IBC"].ToString());
                        IBCCommIncome_base += CastAide.AsDecimal(dataRow["commIncome_IBC"].ToString());
                        IBCToteFee_base += CastAide.AsDecimal(dataRow["FeeDue2"].ToString());
                        IBCCommExpense_base += CastAide.AsDecimal(dataRow["commIncome_Agency"].ToString());
                        IBCPTWL_base += CastAide.AsDecimal(dataRow["PTWL_IBC"].ToString());
                        IBCNetIncome_base += CastAide.AsDecimal(dataRow["netIncome_IBC"].ToString());

                        PartnerWinTO_base += CastAide.AsDecimal(dataRow["winTO_Partner"].ToString());
                        PartnerPlaceTO_base += CastAide.AsDecimal(dataRow["placeTO_Partner"].ToString());
                        PartnerWPTO_base += CastAide.AsDecimal(dataRow["winPlaceTO_Partner"].ToString());
                        PartnerTotalTO_base += CastAide.AsDecimal(dataRow["totalTO_Partner"].ToString());
                        PartnerDividendWon_base += CastAide.AsDecimal(dataRow["dividendWon"].ToString());
                        PartnerCommExpense_base += CastAide.AsDecimal(dataRow["commIncome_IBC"].ToString());
                        PartnerToteFee_base += CastAide.AsDecimal(dataRow["FeeDue2"].ToString());
                        PartnerAmountDue_base += CastAide.AsDecimal(dataRow["AmountDue"].ToString());
                        #endregion
                    }

                    lstCurrency.Items.Add(lstMeeting);

                    #region Totals Meeting Currency
                    lstMeeting.Totals.Add(new RacingWinlossRowBO()
                    {
                        Currency = CacheUtils.GetExchangeName(meetingCurr.CurrencyId),
                        Name = CacheRef.GetLeagueName(meetingCurr.MeetingId) + " - " + CacheUtils.GetExchangeName(meetingCurr.CurrencyId) + " Total",
                        MeetingID = meetingCurr.MeetingId.ToString(),

                        AgencyWinTO = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.AgencyWinTO)), 2),
                        AgencyPlaceTO = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.AgencyPlaceTO)), 2),
                        AgencyWPTO = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.AgencyWPTO)), 2),
                        AgencyTotalTO = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.AgencyTotalTO)), 2),
                        AgencyCommIncome = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.AgencyCommIncome)), 2),
                        AgencyPTWL = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.AgencyPTWL)), 2),

                        IBCWinTO = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.IBCWinTO)), 2),
                        IBCPlaceTO = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.IBCPlaceTO)), 2),
                        IBCWPTO = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.IBCWPTO)), 2),
                        IBCTotalTO = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.IBCTotalTO)), 2),
                        IBCCommIncome = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.IBCCommIncome)), 2),
                        IBCCommExpense = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.IBCCommExpense)), 2),
                        IBCNetIncome = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.IBCNetIncome)), 2),
                        IBCPTWL = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.IBCPTWL)), 2),
                        IBCToteFee = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.IBCToteFee)), 2),

                        PartnerWinTO = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.PartnerWinTO)), 2),
                        PartnerPlaceTO = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.PartnerPlaceTO)), 2),
                        PartnerWPTO = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.PartnerWPTO)), 2),
                        PartnerTotalTO = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.PartnerTotalTO)), 2),
                        PartnerDividendWon = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.PartnerDividendWon)), 2),
                        PartnerCommExpense = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.PartnerCommExpense)), 2),
                        PartnerToteFee = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.PartnerToteFee)), 2),
                        PartnerAmountDue = NumberFormat.DecFormat(lstMeeting.Items.Sum(s => CastAide.AsDecimal(s.PartnerAmountDue)), 2),
                    });



                    #endregion

                }


                #region Total Currency

                #region origin
                var originCurr = new RacingWinlossRowBO()
                {
                    Currency = CacheUtils.GetExchangeName(curr.CurrencyId),
                    Name = CacheUtils.GetExchangeName(curr.CurrencyId) + " Total",

                    AgencyWinTO = NumberFormat.DecFormat(AgencyWinTO_origin, 2),
                    AgencyPlaceTO = NumberFormat.DecFormat(AgencyPlaceTO_origin, 2),
                    AgencyWPTO = NumberFormat.DecFormat(AgencyWPTO_origin, 2),
                    AgencyTotalTO = NumberFormat.DecFormat(AgencyTotalTO_origin, 2),
                    AgencyCommIncome = NumberFormat.DecFormat(AgencyCommIncome_origin, 2),
                    AgencyPTWL = NumberFormat.DecFormat(AgencyPTWL_origin, 2),

                    IBCWinTO = NumberFormat.DecFormat(IBCWinTO_origin, 2),
                    IBCPlaceTO = NumberFormat.DecFormat(IBCPlaceTO_origin, 2),
                    IBCWPTO = NumberFormat.DecFormat(IBCWPTO_origin, 2),
                    IBCTotalTO = NumberFormat.DecFormat(IBCTotalTO_origin, 2),
                    IBCCommIncome = NumberFormat.DecFormat(IBCCommIncome_origin, 2),
                    IBCCommExpense = NumberFormat.DecFormat(IBCCommExpense_origin, 2),
                    IBCNetIncome = NumberFormat.DecFormat(IBCNetIncome_origin, 2),
                    IBCPTWL = NumberFormat.DecFormat(IBCPTWL_origin, 2),
                    IBCToteFee = NumberFormat.DecFormat(IBCToteFee_origin, 2),

                    PartnerWinTO = NumberFormat.DecFormat(PartnerWinTO_origin, 2),
                    PartnerPlaceTO = NumberFormat.DecFormat(PartnerPlaceTO_origin, 2),
                    PartnerWPTO = NumberFormat.DecFormat(PartnerWPTO_origin, 2),
                    PartnerTotalTO = NumberFormat.DecFormat(PartnerTotalTO_origin, 2),
                    PartnerDividendWon = NumberFormat.DecFormat(PartnerDividendWon_origin, 2),
                    PartnerCommExpense = NumberFormat.DecFormat(PartnerCommExpense_origin, 2),
                    PartnerToteFee = NumberFormat.DecFormat(PartnerToteFee_origin, 2),
                    PartnerAmountDue = NumberFormat.DecFormat(PartnerAmountDue_origin, 2),

                };
                lstCurrency.Totals.Add(originCurr);
                #endregion

                #region base
                if (!arrBaseCurr.Contains(currentCurr))
                {
                    var baseCurr = new RacingWinlossRowBO()
                    {
                        Currency = "Base (RM)",
                        Name = CacheUtils.GetExchangeName(curr.CurrencyId) + " Total",
                        MeetingID = curr.MeetingId.ToString(),

                        AgencyWinTO = NumberFormat.DecFormat(AgencyWinTO_base, 2),
                        AgencyPlaceTO = NumberFormat.DecFormat(AgencyPlaceTO_base, 2),
                        AgencyWPTO = NumberFormat.DecFormat(AgencyWPTO_base, 2),
                        AgencyTotalTO = NumberFormat.DecFormat(AgencyTotalTO_base, 2),
                        AgencyCommIncome = NumberFormat.DecFormat(AgencyCommIncome_base, 2),
                        AgencyPTWL = NumberFormat.DecFormat(AgencyPTWL_base, 2),

                        IBCWinTO = NumberFormat.DecFormat(IBCWinTO_base, 2),
                        IBCPlaceTO = NumberFormat.DecFormat(IBCPlaceTO_base, 2),
                        IBCWPTO = NumberFormat.DecFormat(IBCWPTO_base, 2),
                        IBCTotalTO = NumberFormat.DecFormat(IBCTotalTO_base, 2),
                        IBCCommIncome = NumberFormat.DecFormat(IBCCommIncome_base, 2),
                        IBCCommExpense = NumberFormat.DecFormat(IBCCommExpense_base, 2),
                        IBCNetIncome = NumberFormat.DecFormat(IBCNetIncome_base, 2),
                        IBCPTWL = NumberFormat.DecFormat(IBCPTWL_base, 2),
                        IBCToteFee = NumberFormat.DecFormat(IBCToteFee_base, 2),

                        PartnerWinTO = NumberFormat.DecFormat(PartnerWinTO_base, 2),
                        PartnerPlaceTO = NumberFormat.DecFormat(PartnerPlaceTO_base, 2),
                        PartnerWPTO = NumberFormat.DecFormat(PartnerWPTO_base, 2),
                        PartnerTotalTO = NumberFormat.DecFormat(PartnerTotalTO_base, 2),
                        PartnerDividendWon = NumberFormat.DecFormat(PartnerDividendWon_base, 2),
                        PartnerCommExpense = NumberFormat.DecFormat(PartnerCommExpense_base, 2),
                        PartnerToteFee = NumberFormat.DecFormat(PartnerToteFee_base, 2),
                        PartnerAmountDue = NumberFormat.DecFormat(PartnerAmountDue_base, 2),
                    };
                    lstCurrency.Totals.Add(baseCurr);
                }

                #endregion
                #endregion

                racingDetailsBo.Items.Add(lstCurrency);

            }

            #region GrandTotal
            #region PoolCurr
            grandTotal.Add(new RacingWinlossRowBO()
            {
                CurrencyId = poolCurrency,
                Currency = CacheUtils.GetExchangeName(CastAide.AsInt32(poolCurrency)),

                AgencyWinTO = AgencyWinTO_pool.ToString(),
                AgencyPlaceTO = AgencyPlaceTO_pool.ToString(),
                AgencyWPTO = AgencyWPTO_pool.ToString(),
                AgencyTotalTO = AgencyTotalTO_pool.ToString(),
                AgencyCommIncome = AgencyCommIncome_pool.ToString(),
                AgencyPTWL = AgencyPTWL_pool.ToString(),

                IBCWinTO = IBCWinTO_pool.ToString(),
                IBCPlaceTO = IBCPlaceTO_pool.ToString(),
                IBCWPTO = IBCWPTO_pool.ToString(),
                IBCTotalTO = IBCTotalTO_pool.ToString(),
                IBCCommIncome = IBCCommIncome_pool.ToString(),
                IBCToteFee = IBCToteFee_pool.ToString(),
                IBCCommExpense = IBCCommExpense_pool.ToString(),
                IBCPTWL = IBCPTWL_pool.ToString(),
                IBCNetIncome = IBCNetIncome_pool.ToString(),

                PartnerWinTO = PartnerWinTO_pool.ToString(),
                PartnerPlaceTO = PartnerPlaceTO_pool.ToString(),
                PartnerWPTO = PartnerWPTO_pool.ToString(),
                PartnerTotalTO = PartnerTotalTO_pool.ToString(),
                PartnerDividendWon = PartnerDividendWon_pool.ToString(),
                PartnerCommExpense = PartnerCommExpense_pool.ToString(),
                PartnerToteFee = PartnerToteFee_pool.ToString(),
                PartnerAmountDue = PartnerAmountDue_pool.ToString()
            });

            #endregion

            #region Base
            grandTotal.Add(new RacingWinlossRowBO()
            {
                CurrencyId = "2",
                Currency = "Base (RM)",

                AgencyWinTO = AgencyWinTO_base.ToString(),
                AgencyPlaceTO = AgencyPlaceTO_base.ToString(),
                AgencyWPTO = AgencyWPTO_base.ToString(),
                AgencyTotalTO = AgencyTotalTO_base.ToString(),
                AgencyCommIncome = AgencyCommIncome_base.ToString(),
                AgencyPTWL = AgencyPTWL_base.ToString(),

                IBCWinTO = IBCWinTO_base.ToString(),
                IBCPlaceTO = IBCPlaceTO_base.ToString(),
                IBCWPTO = IBCWPTO_base.ToString(),
                IBCTotalTO = IBCTotalTO_base.ToString(),
                IBCCommIncome = IBCCommIncome_base.ToString(),
                IBCToteFee = IBCToteFee_base.ToString(),
                IBCCommExpense = IBCCommExpense_base.ToString(),
                IBCPTWL = IBCPTWL_base.ToString(),
                IBCNetIncome = IBCNetIncome_base.ToString(),

                PartnerWinTO = PartnerWinTO_base.ToString(),
                PartnerPlaceTO = PartnerPlaceTO_base.ToString(),
                PartnerWPTO = PartnerWPTO_base.ToString(),
                PartnerTotalTO = PartnerTotalTO_base.ToString(),
                PartnerDividendWon = PartnerDividendWon_base.ToString(),
                PartnerCommExpense = PartnerCommExpense_base.ToString(),
                PartnerToteFee = PartnerToteFee_base.ToString(),
                PartnerAmountDue = PartnerAmountDue_base.ToString()
            });

            #endregion
            #endregion

            #region grant total grouped meeting
            var distinctMeetings = GetDistinctMeeting(dataTable);


            #region var pool grant total grouped by meetings
            string CurrencyIdPoolCurr = string.Empty;
            string CurrencyPoolCurr = string.Empty;
            string NamePoolCurr = string.Empty;

            decimal AgencyWinTOPoolCurr = 0;
            decimal AgencyPlaceTOPoolCurr = 0;
            decimal AgencyWPTOPoolCurr = 0;
            decimal AgencyTotalTOPoolCurr = 0;
            decimal AgencyCommIncomePoolCurr = 0;
            decimal AgencyPTWLPoolCurr = 0;

            decimal IBCWinTOPoolCurr = 0;
            decimal IBCPlaceTOPoolCurr = 0;
            decimal IBCWPTOPoolCurr = 0;
            decimal IBCTotalTOPoolCurr = 0;
            decimal IBCCommIncomePoolCurr = 0;
            decimal IBCToteFeePoolCurr = 0;
            decimal IBCCommExpensePoolCurr = 0;
            decimal IBCPTWLPoolCurr = 0;
            decimal IBCNetIncomePoolCurr = 0;

            decimal PartnerWinTOPoolCurr = 0;
            decimal PartnerPlaceTOPoolCurr = 0;
            decimal PartnerWPTOPoolCurr = 0;
            decimal PartnerTotalTOPoolCurr = 0;
            decimal PartnerDividendWonPoolCurr = 0;
            decimal PartnerCommExpensePoolCurr = 0;
            decimal PartnerToteFeePoolCurr = 0;
            decimal PartnerAmountDuePoolCurr = 0;
            #endregion

            #region var base grant total grouped by meetings
            string CurrencyIdBase = string.Empty;
            string CurrencyBase = string.Empty;
            string NameBase = string.Empty;

            decimal AgencyWinTOBase = 0;
            decimal AgencyPlaceTOBase = 0;
            decimal AgencyWPTOBase = 0;
            decimal AgencyTotalTOBase = 0;
            decimal AgencyCommIncomeBase = 0;
            decimal AgencyPTWLBase = 0;

            decimal IBCWinTOBase = 0;
            decimal IBCPlaceTOBase = 0;
            decimal IBCWPTOBase = 0;
            decimal IBCTotalTOBase = 0;
            decimal IBCCommIncomeBase = 0;
            decimal IBCToteFeeBase = 0;
            decimal IBCCommExpenseBase = 0;
            decimal IBCPTWLBase = 0;
            decimal IBCNetIncomeBase = 0;

            decimal PartnerWinTOBase = 0;
            decimal PartnerPlaceTOBase = 0;
            decimal PartnerWPTOBase = 0;
            decimal PartnerTotalTOBase = 0;
            decimal PartnerDividendWonBase = 0;
            decimal PartnerCommExpenseBase = 0;
            decimal PartnerToteFeeBase = 0;
            decimal PartnerAmountDueBase = 0;
            #endregion

            foreach (var meeting in distinctMeetings)
            {
                var racingWinlossRowBoPool = new RacingWinlossRowBO();
                var racingWinlossRowBoBase = new RacingWinlossRowBO();
                var meeting1 = meeting;

                #region var pool grant total grouped by meetings
                CurrencyIdPoolCurr = string.Empty;
                CurrencyPoolCurr = string.Empty;
                NamePoolCurr = string.Empty;

                AgencyWinTOPoolCurr = 0;
                AgencyPlaceTOPoolCurr = 0;
                AgencyWPTOPoolCurr = 0;
                AgencyTotalTOPoolCurr = 0;
                AgencyCommIncomePoolCurr = 0;
                AgencyPTWLPoolCurr = 0;

                IBCWinTOPoolCurr = 0;
                IBCPlaceTOPoolCurr = 0;
                IBCWPTOPoolCurr = 0;
                IBCTotalTOPoolCurr = 0;
                IBCCommIncomePoolCurr = 0;
                IBCToteFeePoolCurr = 0;
                IBCCommExpensePoolCurr = 0;
                IBCPTWLPoolCurr = 0;
                IBCNetIncomePoolCurr = 0;

                PartnerWinTOPoolCurr = 0;
                PartnerPlaceTOPoolCurr = 0;
                PartnerWPTOPoolCurr = 0;
                PartnerTotalTOPoolCurr = 0;
                PartnerDividendWonPoolCurr = 0;
                PartnerCommExpensePoolCurr = 0;
                PartnerToteFeePoolCurr = 0;
                PartnerAmountDuePoolCurr = 0;
                #endregion

                #region var base grant total grouped by meetings
                CurrencyIdBase = string.Empty;
                CurrencyBase = string.Empty;
                NameBase = string.Empty;

                AgencyWinTOBase = 0;
                AgencyPlaceTOBase = 0;
                AgencyWPTOBase = 0;
                AgencyTotalTOBase = 0;
                AgencyCommIncomeBase = 0;
                AgencyPTWLBase = 0;

                IBCWinTOBase = 0;
                IBCPlaceTOBase = 0;
                IBCWPTOBase = 0;
                IBCTotalTOBase = 0;
                IBCCommIncomeBase = 0;
                IBCToteFeeBase = 0;
                IBCCommExpenseBase = 0;
                IBCPTWLBase = 0;
                IBCNetIncomeBase = 0;

                PartnerWinTOBase = 0;
                PartnerPlaceTOBase = 0;
                PartnerWPTOBase = 0;
                PartnerTotalTOBase = 0;
                PartnerDividendWonBase = 0;
                PartnerCommExpenseBase = 0;
                PartnerToteFeeBase = 0;
                PartnerAmountDueBase = 0;
                #endregion

                foreach (var dataRow in
                    dataTable.Rows.Cast<DataRow>().Where(dataRow => dataRow["MeetingId"].ToString() == meeting1.MeetingId.ToString()))
                {
                    #region pool grant total grouped by meetings
                    CurrencyIdPoolCurr = poolCurrency;
                    CurrencyPoolCurr = CacheUtils.GetExchangeName(CastAide.AsInt32(poolCurrency));
                    NamePoolCurr = CacheRef.GetLeagueName(dataRow["MeetingId"]);

                    AgencyWinTOPoolCurr += CastAide.AsDecimal(dataRow["winTO_Agency_PoolCurr"]);
                    AgencyPlaceTOPoolCurr += CastAide.AsDecimal(dataRow["placeTO_Agency_PoolCurr"]);
                    AgencyWPTOPoolCurr += CastAide.AsDecimal(dataRow["winplaceTO_Agency_PoolCurr"]);
                    AgencyTotalTOPoolCurr += CastAide.AsDecimal(dataRow["totalTO_Agency_PoolCurr"]);
                    AgencyCommIncomePoolCurr += CastAide.AsDecimal(dataRow["commIncome_Agency_PoolCurr"]);
                    AgencyPTWLPoolCurr += CastAide.AsDecimal(dataRow["PTWL_Agency_PoolCurr"]);

                    IBCWinTOPoolCurr += CastAide.AsDecimal(dataRow["winTO_IBC_PoolCurr"]);
                    IBCPlaceTOPoolCurr += CastAide.AsDecimal(dataRow["placeTO_IBC_PoolCurr"]);
                    IBCWPTOPoolCurr += CastAide.AsDecimal(dataRow["winplaceTO_IBC_PoolCurr"]);
                    IBCTotalTOPoolCurr += CastAide.AsDecimal(dataRow["totalTO_IBC_PoolCurr"]);
                    IBCCommIncomePoolCurr += CastAide.AsDecimal(dataRow["commIncome_IBC_PoolCurr"]);
                    IBCToteFeePoolCurr += CastAide.AsDecimal(dataRow["FeeDue2_PoolCurr"]);
                    IBCCommExpensePoolCurr += CastAide.AsDecimal(dataRow["commIncome_Agency_PoolCurr"]);
                    IBCPTWLPoolCurr += CastAide.AsDecimal(dataRow["PTWL_IBC_PoolCurr"]);
                    IBCNetIncomePoolCurr += CastAide.AsDecimal(dataRow["netIncome_IBC_PoolCurr"]);

                    PartnerWinTOPoolCurr += CastAide.AsDecimal(dataRow["winTO_Partner_PoolCurr"]);
                    PartnerPlaceTOPoolCurr += CastAide.AsDecimal(dataRow["placeTO_Partner_PoolCurr"]);
                    PartnerWPTOPoolCurr += CastAide.AsDecimal(dataRow["winPlaceTO_Partner_PoolCurr"]);
                    PartnerTotalTOPoolCurr += CastAide.AsDecimal(dataRow["totalTO_Partner_PoolCurr"]);
                    PartnerDividendWonPoolCurr += CastAide.AsDecimal(dataRow["dividendWon_PoolCurr"]);
                    PartnerCommExpensePoolCurr += CastAide.AsDecimal(dataRow["commIncome_IBC_PoolCurr"]);
                    PartnerToteFeePoolCurr += CastAide.AsDecimal(dataRow["FeeDue2_PoolCurr"]);
                    PartnerAmountDuePoolCurr += CastAide.AsDecimal(dataRow["AmountDue_PoolCurr"]);
                    #endregion

                    //---------------------------------------------------------------------------------------------------------
                    #region base grant total grouped by meetings
                    CurrencyIdBase = "2";
                    CurrencyBase = "Base (RM)";
                    NameBase = CacheRef.GetLeagueName(dataRow["MeetingId"]);

                    AgencyWinTOBase += CastAide.AsDecimal(dataRow["winTO_Agency"]);
                    AgencyPlaceTOBase += CastAide.AsDecimal(dataRow["placeTO_Agency"]);
                    AgencyWPTOBase += CastAide.AsDecimal(dataRow["winplaceTO_Agency"]);
                    AgencyTotalTOBase += CastAide.AsDecimal(dataRow["totalTO_Agency"]);
                    AgencyCommIncomeBase += CastAide.AsDecimal(dataRow["commIncome_Agency"]);
                    AgencyPTWLBase += CastAide.AsDecimal(dataRow["PTWL_Agency"]);

                    IBCWinTOBase += CastAide.AsDecimal(dataRow["winTO_IBC"]);
                    IBCPlaceTOBase += CastAide.AsDecimal(dataRow["placeTO_IBC"]);
                    IBCWPTOBase += CastAide.AsDecimal(dataRow["winplaceTO_IBC"]);
                    IBCTotalTOBase += CastAide.AsDecimal(dataRow["totalTO_IBC"]);
                    IBCCommIncomeBase += CastAide.AsDecimal(dataRow["commIncome_IBC"]);
                    IBCToteFeeBase += CastAide.AsDecimal(dataRow["FeeDue2"]);
                    IBCCommExpenseBase += CastAide.AsDecimal(dataRow["commIncome_Agency"]);
                    IBCPTWLBase += CastAide.AsDecimal(dataRow["PTWL_IBC"]);
                    IBCNetIncomeBase += CastAide.AsDecimal(dataRow["netIncome_IBC"]);

                    PartnerWinTOBase += CastAide.AsDecimal(dataRow["winTO_Partner"]);
                    PartnerPlaceTOBase += CastAide.AsDecimal(dataRow["placeTO_Partner"]);
                    PartnerWPTOBase += CastAide.AsDecimal(dataRow["winPlaceTO_Partner"]);
                    PartnerTotalTOBase += CastAide.AsDecimal(dataRow["totalTO_Partner"]);
                    PartnerDividendWonBase += CastAide.AsDecimal(dataRow["dividendWon"]);
                    PartnerCommExpenseBase += CastAide.AsDecimal(dataRow["commIncome_IBC"]);
                    PartnerToteFeeBase += CastAide.AsDecimal(dataRow["FeeDue2"]);
                    PartnerAmountDueBase += CastAide.AsDecimal(dataRow["AmountDue"]);
                    #endregion
                }
                #region pool grant total grouped by meetings
                racingWinlossRowBoPool.CurrencyId = CurrencyIdPoolCurr;
                racingWinlossRowBoPool.Currency = CurrencyPoolCurr;
                racingWinlossRowBoPool.Name = NamePoolCurr;

                racingWinlossRowBoPool.AgencyWinTO = NumberFormat.DecFormat(AgencyWinTOPoolCurr, 2);
                racingWinlossRowBoPool.AgencyPlaceTO = NumberFormat.DecFormat(AgencyPlaceTOPoolCurr, 2);
                racingWinlossRowBoPool.AgencyWPTO = NumberFormat.DecFormat(AgencyWPTOPoolCurr, 2);
                racingWinlossRowBoPool.AgencyTotalTO = NumberFormat.DecFormat(AgencyTotalTOPoolCurr, 2);
                racingWinlossRowBoPool.AgencyCommIncome = NumberFormat.DecFormat(AgencyCommIncomePoolCurr, 2);
                racingWinlossRowBoPool.AgencyPTWL = NumberFormat.DecFormat(AgencyPTWLPoolCurr, 2);

                racingWinlossRowBoPool.IBCWinTO = NumberFormat.DecFormat(IBCWinTOPoolCurr, 2);
                racingWinlossRowBoPool.IBCPlaceTO = NumberFormat.DecFormat(IBCPlaceTOPoolCurr, 2);
                racingWinlossRowBoPool.IBCWPTO = NumberFormat.DecFormat(IBCWPTOPoolCurr, 2);
                racingWinlossRowBoPool.IBCTotalTO = NumberFormat.DecFormat(IBCTotalTOPoolCurr, 2);
                racingWinlossRowBoPool.IBCCommIncome = NumberFormat.DecFormat(IBCCommIncomePoolCurr, 2);
                racingWinlossRowBoPool.IBCToteFee = NumberFormat.DecFormat(IBCToteFeePoolCurr, 2);
                racingWinlossRowBoPool.IBCCommExpense = NumberFormat.DecFormat(IBCCommExpensePoolCurr, 2);
                racingWinlossRowBoPool.IBCPTWL = NumberFormat.DecFormat(IBCPTWLPoolCurr, 2);
                racingWinlossRowBoPool.IBCNetIncome = NumberFormat.DecFormat(IBCNetIncomePoolCurr, 2);

                racingWinlossRowBoPool.PartnerWinTO = NumberFormat.DecFormat(PartnerWinTOPoolCurr, 2);
                racingWinlossRowBoPool.PartnerPlaceTO = NumberFormat.DecFormat(PartnerPlaceTOPoolCurr, 2);
                racingWinlossRowBoPool.PartnerWPTO = NumberFormat.DecFormat(PartnerWPTOPoolCurr, 2);
                racingWinlossRowBoPool.PartnerTotalTO = NumberFormat.DecFormat(PartnerTotalTOPoolCurr, 2);
                racingWinlossRowBoPool.PartnerDividendWon = NumberFormat.DecFormat(PartnerDividendWonPoolCurr, 2);
                racingWinlossRowBoPool.PartnerCommExpense = NumberFormat.DecFormat(PartnerCommExpensePoolCurr, 2);
                racingWinlossRowBoPool.PartnerToteFee = NumberFormat.DecFormat(PartnerToteFeePoolCurr, 2);
                racingWinlossRowBoPool.PartnerAmountDue = NumberFormat.DecFormat(PartnerAmountDuePoolCurr, 2);
                #endregion

                #region base grant total grouped by meetings
                racingWinlossRowBoBase.CurrencyId = CurrencyIdBase;
                racingWinlossRowBoBase.Currency = CurrencyBase;
                racingWinlossRowBoBase.Name = NameBase;

                racingWinlossRowBoBase.AgencyWinTO = NumberFormat.DecFormat(AgencyWinTOBase, 2);
                racingWinlossRowBoBase.AgencyPlaceTO = NumberFormat.DecFormat(AgencyPlaceTOBase, 2);
                racingWinlossRowBoBase.AgencyWPTO = NumberFormat.DecFormat(AgencyWPTOBase, 2);
                racingWinlossRowBoBase.AgencyTotalTO = NumberFormat.DecFormat(AgencyTotalTOBase, 2);
                racingWinlossRowBoBase.AgencyCommIncome = NumberFormat.DecFormat(AgencyCommIncomeBase, 2);
                racingWinlossRowBoBase.AgencyPTWL = NumberFormat.DecFormat(AgencyPTWLBase, 2);

                racingWinlossRowBoBase.IBCWinTO = NumberFormat.DecFormat(IBCWinTOBase, 2);
                racingWinlossRowBoBase.IBCPlaceTO = NumberFormat.DecFormat(IBCPlaceTOBase, 2);
                racingWinlossRowBoBase.IBCWPTO = NumberFormat.DecFormat(IBCWPTOBase, 2);
                racingWinlossRowBoBase.IBCTotalTO = NumberFormat.DecFormat(IBCTotalTOBase, 2);
                racingWinlossRowBoBase.IBCCommIncome = NumberFormat.DecFormat(IBCCommIncomeBase, 2);
                racingWinlossRowBoBase.IBCToteFee = NumberFormat.DecFormat(IBCToteFeeBase, 2);
                racingWinlossRowBoBase.IBCCommExpense = NumberFormat.DecFormat(IBCCommExpenseBase, 2);
                racingWinlossRowBoBase.IBCPTWL = NumberFormat.DecFormat(IBCPTWLBase, 2);
                racingWinlossRowBoBase.IBCNetIncome = NumberFormat.DecFormat(IBCNetIncomeBase, 2);

                racingWinlossRowBoBase.PartnerWinTO = NumberFormat.DecFormat(PartnerWinTOBase, 2);
                racingWinlossRowBoBase.PartnerPlaceTO = NumberFormat.DecFormat(PartnerPlaceTOBase, 2);
                racingWinlossRowBoBase.PartnerWPTO = NumberFormat.DecFormat(PartnerWPTOBase, 2);
                racingWinlossRowBoBase.PartnerTotalTO = NumberFormat.DecFormat(PartnerTotalTOBase, 2);
                racingWinlossRowBoBase.PartnerDividendWon = NumberFormat.DecFormat(PartnerDividendWonBase, 2);
                racingWinlossRowBoBase.PartnerCommExpense = NumberFormat.DecFormat(PartnerCommExpenseBase, 2);
                racingWinlossRowBoBase.PartnerToteFee = NumberFormat.DecFormat(PartnerToteFeeBase, 2);
                racingWinlossRowBoBase.PartnerAmountDue = NumberFormat.DecFormat(PartnerAmountDueBase, 2);
                #endregion
                racingDetailsBo.Totals.Add(racingWinlossRowBoPool);
                //racingDetailsBo.Totals.Add(racingWinlossRowBoBase);
            }
            #endregion

            #region Calculate Grand Total

            var grand = grandTotal.OrderByDescending(s => s.Currency).Select(s => s.Currency).Distinct().ToArray();
            foreach (string currency in grand)
            {
                racingDetailsBo.GrandTotals.Add(new RacingWinlossRowBO()
                {
                    Name = "Grand Total",
                    Currency = currency,

                    AgencyWinTO = NumberFormat.DecFormat(grandTotal.Where(s => s.Currency == currency).Sum(s => CastAide.AsDecimal(s.AgencyWinTO)), 2),
                    AgencyPlaceTO = NumberFormat.DecFormat(grandTotal.Where(s => s.Currency == currency).Sum(s => CastAide.AsDecimal(s.AgencyPlaceTO)), 2),
                    AgencyWPTO = NumberFormat.DecFormat(grandTotal.Where(s => s.Currency == currency).Sum(s => CastAide.AsDecimal(s.AgencyWPTO)), 2),
                    AgencyTotalTO = NumberFormat.DecFormat(grandTotal.Where(s => s.Currency == currency).Sum(s => CastAide.AsDecimal(s.AgencyTotalTO)), 2),
                    AgencyCommIncome = NumberFormat.DecFormat(grandTotal.Where(s => s.Currency == currency).Sum(s => CastAide.AsDecimal(s.AgencyCommIncome)), 2),
                    AgencyPTWL = NumberFormat.DecFormat(grandTotal.Where(s => s.Currency == currency).Sum(s => CastAide.AsDecimal(s.AgencyPTWL)), 2),

                    IBCWinTO = NumberFormat.DecFormat(grandTotal.Where(s => s.Currency == currency).Sum(s => CastAide.AsDecimal(s.IBCWinTO)), 2),
                    IBCPlaceTO = NumberFormat.DecFormat(grandTotal.Where(s => s.Currency == currency).Sum(s => CastAide.AsDecimal(s.IBCPlaceTO)), 2),
                    IBCWPTO = NumberFormat.DecFormat(grandTotal.Where(s => s.Currency == currency).Sum(s => CastAide.AsDecimal(s.IBCWPTO)), 2),
                    IBCTotalTO = NumberFormat.DecFormat(grandTotal.Where(s => s.Currency == currency).Sum(s => CastAide.AsDecimal(s.IBCTotalTO)), 2),
                    IBCCommIncome = NumberFormat.DecFormat(grandTotal.Where(s => s.Currency == currency).Sum(s => CastAide.AsDecimal(s.IBCCommIncome)), 2),
                    IBCCommExpense = NumberFormat.DecFormat(grandTotal.Where(s => s.Currency == currency).Sum(s => CastAide.AsDecimal(s.IBCCommExpense)), 2),
                    IBCNetIncome = NumberFormat.DecFormat(grandTotal.Where(s => s.Currency == currency).Sum(s => CastAide.AsDecimal(s.IBCNetIncome)), 2),
                    IBCPTWL = NumberFormat.DecFormat(grandTotal.Where(s => s.Currency == currency).Sum(s => CastAide.AsDecimal(s.IBCPTWL)), 2),
                    IBCToteFee = NumberFormat.DecFormat(grandTotal.Where(s => s.Currency == currency).Sum(s => CastAide.AsDecimal(s.IBCToteFee)), 2),

                    PartnerWinTO = NumberFormat.DecFormat(grandTotal.Where(s => s.Currency == currency).Sum(s => CastAide.AsDecimal(s.PartnerWinTO)), 2),
                    PartnerPlaceTO = NumberFormat.DecFormat(grandTotal.Where(s => s.Currency == currency).Sum(s => CastAide.AsDecimal(s.PartnerPlaceTO)), 2),
                    PartnerWPTO = NumberFormat.DecFormat(grandTotal.Where(s => s.Currency == currency).Sum(s => CastAide.AsDecimal(s.PartnerWPTO)), 2),
                    PartnerTotalTO = NumberFormat.DecFormat(grandTotal.Where(s => s.Currency == currency).Sum(s => CastAide.AsDecimal(s.PartnerTotalTO)), 2),
                    PartnerDividendWon = NumberFormat.DecFormat(grandTotal.Where(s => s.Currency == currency).Sum(s => CastAide.AsDecimal(s.PartnerDividendWon)), 2),
                    PartnerCommExpense = NumberFormat.DecFormat(grandTotal.Where(s => s.Currency == currency).Sum(s => CastAide.AsDecimal(s.PartnerCommExpense)), 2),
                    PartnerToteFee = NumberFormat.DecFormat(grandTotal.Where(s => s.Currency == currency).Sum(s => CastAide.AsDecimal(s.PartnerToteFee)), 2),
                    PartnerAmountDue = NumberFormat.DecFormat(grandTotal.Where(s => s.Currency == currency).Sum(s => CastAide.AsDecimal(s.PartnerAmountDue)), 2),
                });
            }
            #endregion

            SessionExtension.AddSession(HttpContext.Current.Session, string.Format(_winLossRacingReportDetailSessionKey, poolCurrency, partnerId), racingDetailsBo);
            return racingDetailsBo;
        }
        #endregion

        #region GetWinlossRacingBetlist
        public RacingWinlossBetlistBO GetWinlossRacingBetlist(string winlossdate, string leagueid, string raceNo, string currency)
        {
            RacingWinlossBetlistBO racingWinlossBetlistBo = new RacingWinlossBetlistBO();
            racingWinlossBetlistBo.Items = new List<RacingWinlossBetlistRowBO>();
            racingWinlossBetlistBo.Totals = new RacingWinlossBetlistRowBO();

            DataTable dataTable = _reportRacingDao.WinlostRacingToteBetlist(winlossdate, leagueid, raceNo, currency);

            DataRow[] rows = dataTable.Select();
            int index = 0;
            foreach (DataRow row in rows)
            {
                index++;
                #region row
                var betlistRow = new RacingWinlossBetlistRowBO()
                                                     {
                                                         No = NumberFormat.DecFormat(index,0),
                                                         Member = CastAide.AsString(row["username"]),
                                                         RefNo = CastAide.AsString(row["transid"]),
                                                         TransTime = CastAide.AsDateTime(row["transdate"]).ToString(),
                                                         Stake = NumberFormat.DecFormat(row["stake"], 2),
                                                         MemberWL = NumberFormat.DecFormat(row["winlost"], 2),
                                                         MemberCom = NumberFormat.DecFormat(row["PlayerComm"], 2),

                                                         AgencyWL = NumberFormat.DecFormat(row["WLPTAgency"], 2),
                                                         AgencyCom = NumberFormat.DecFormat(row["CommIncomeAgency"], 2),

                                                         IBCWL = NumberFormat.DecFormat(row["WLPTIBC"], 2),
                                                         IBCCom = NumberFormat.DecFormat(row["CommIncomeIBC"], 2),

                                                         Dividend = NumberFormat.DecFormat(row["dividend"], 2),

                                                         IsShowOtExRate = CastAide.AsBoolean(row["isShowOTExRate"]),

                                                         

                                                         HorseBet = row["HorseNo"] + " - " + CacheRef.GetHorseTeamName(row["HorseId"]),
                                                         Bettype = CacheRef.GetBettype(row["bettype"]),
                                                         BettypeId = CastAide.AsString(row["bettype"]),
                                                         ExRateName = "(Ex.Rate 2)",
                                                         ExRate = NumberFormat.DecFormat(row["virtualrate"], 2),
                                                };

                if (CastAide.AsInt32(row["bettype"]) == 31 || CastAide.AsInt32(row["bettype"]) == 32)
                {
                    betlistRow.Status = CastAide.AsString(row["status"]);
                    if (betlistRow.Status.Equals(Status.WIN) || betlistRow.Status.Equals(Status.Refund))
                        betlistRow.Odds = NumberFormat.DecFormat(row["Odds"], 2);
                    else
                        betlistRow.Odds = "-";
                }
                else
                {
                    betlistRow.WinStatus = CastAide.AsString(row["winStatus"]);
                    betlistRow.PlaceStatus = CastAide.AsString(row["placeStatus"]);
                    betlistRow.winOdds = betlistRow.WinStatus.Equals(Status.WIN)
                                             ? NumberFormat.DecFormat(row["WinOdds"], 2)
                                             : "0.00";
                    betlistRow.placeOdds = betlistRow.WinStatus.Equals(Status.WIN) || betlistRow.WinStatus.Equals(Status.LOSE)
                                               ? NumberFormat.DecFormat(row["PlaceOdds"], 2)
                                               : "0.00";
                    if (!row.IsNull("status"))
                        betlistRow.Status = CastAide.AsString(row["status"]);
                    else
                        betlistRow.Status = String.Empty;

                    if (!row.IsNull("Odds"))
                        betlistRow.Odds = NumberFormat.DecFormat(row["Odds"], 2);
                    else
                        betlistRow.Odds = string.Empty;
                }
                #endregion

                racingWinlossBetlistBo.Items.Add(betlistRow);


            }

            #region Total
            racingWinlossBetlistBo.Totals = new RacingWinlossBetlistRowBO()
                                                {
                                                    Member = "Total",

                                                    Stake = NumberFormat.DecFormat( racingWinlossBetlistBo.Items.Sum(s=> CastAide.AsDecimal( s.Stake)),2),
                                                    MemberWL = NumberFormat.DecFormat(racingWinlossBetlistBo.Items.Sum(s => CastAide.AsDecimal(s.MemberWL)), 2),
                                                    AgencyWL = NumberFormat.DecFormat(racingWinlossBetlistBo.Items.Sum(s => CastAide.AsDecimal(s.AgencyWL)), 2),
                                                    IBCWL = NumberFormat.DecFormat(racingWinlossBetlistBo.Items.Sum(s => CastAide.AsDecimal(s.IBCWL)), 2),

                                                    MemberCom = NumberFormat.DecFormat(racingWinlossBetlistBo.Items.Sum(s => CastAide.AsDecimal(s.MemberCom)), 2),
                                                    AgencyCom = NumberFormat.DecFormat(racingWinlossBetlistBo.Items.Sum(s => CastAide.AsDecimal(s.AgencyCom)), 2),
                                                    IBCCom = NumberFormat.DecFormat(racingWinlossBetlistBo.Items.Sum(s => CastAide.AsDecimal(s.IBCCom)), 2),

                                                    Dividend = NumberFormat.DecFormat(racingWinlossBetlistBo.Items.Sum(s => CastAide.AsDecimal(s.Dividend)), 2),  
                                                };
            #endregion

            return racingWinlossBetlistBo;
        }

        public RacingWinlossBetlistBO GetWinlossRacingBetlistNew(string winlossdate, string leagueid, string raceNo, string currency)
        {
            RacingWinlossBetlistBO racingWinlossBetlistBo = new RacingWinlossBetlistBO();
            racingWinlossBetlistBo.Items = new List<RacingWinlossBetlistRowBO>();
            racingWinlossBetlistBo.Totals = new RacingWinlossBetlistRowBO();

            DataTable dataTable = _reportRacingDao.WinlostRacingToteBetlistNew(winlossdate, leagueid, raceNo, currency);

            DataRow[] rows = dataTable.Select();
            int index = 0;
            foreach (DataRow row in rows)
            {
                index++;
                #region row
                var betlistRow = new RacingWinlossBetlistRowBO()
                {
                    No = NumberFormat.DecFormat(index, 0),
                    Member = CastAide.AsString(row["username"]),
                    RefNo = CastAide.AsString(row["transid"]),
                    TransTime = CastAide.AsDateTime(row["transdate"]).ToString(),
                    Stake = NumberFormat.DecFormat(row["stake"], 2),
                    MemberWL = NumberFormat.DecFormat(row["winlost"], 2),
                    MemberCom = NumberFormat.DecFormat(row["PlayerComm"], 2),

                    AgencyWL = NumberFormat.DecFormat(row["WLPTAgency"], 2),
                    AgencyCom = NumberFormat.DecFormat(row["CommIncomeAgency"], 2),

                    IBCWL = NumberFormat.DecFormat(row["WLPTIBC"], 2),
                    IBCCom = NumberFormat.DecFormat(row["CommIncomeIBC"], 2),

                    Dividend = NumberFormat.DecFormat(row["dividend"], 2),

                    IsShowOtExRate = CastAide.AsBoolean(row["isShowOTExRate"]),



                    HorseBet = row["HorseNo"] + " - " + CacheRef.GetHorseTeamName(row["HorseId"]),
                    Bettype = CacheRef.GetBettype(row["bettype"]),
                    BettypeId = CastAide.AsString(row["bettype"]),
                    ExRateName = "(Ex.Rate 2)",
                    ExRate = NumberFormat.DecFormat(row["virtualrate"], 2),
                };

                if (CastAide.AsInt32(row["bettype"]) == 31 || CastAide.AsInt32(row["bettype"]) == 32)
                {
                    betlistRow.Status = CastAide.AsString(row["status"]);
                    if (betlistRow.Status.Equals(Status.WIN) || betlistRow.Status.Equals(Status.Refund))
                        betlistRow.Odds = NumberFormat.DecFormat(row["Odds"], 2);
                    else
                        betlistRow.Odds = "-";
                }
                else
                {
                    betlistRow.WinStatus = CastAide.AsString(row["winStatus"]);
                    betlistRow.PlaceStatus = CastAide.AsString(row["placeStatus"]);
                    betlistRow.winOdds = betlistRow.WinStatus.Equals(Status.WIN)
                                             ? NumberFormat.DecFormat(row["WinOdds"], 2)
                                             : "0.00";
                    betlistRow.placeOdds = betlistRow.WinStatus.Equals(Status.WIN) || betlistRow.WinStatus.Equals(Status.LOSE)
                                               ? NumberFormat.DecFormat(row["PlaceOdds"], 2)
                                               : "0.00";
                    if (!row.IsNull("status"))
                        betlistRow.Status = CastAide.AsString(row["status"]);
                    else
                        betlistRow.Status = String.Empty;

                    if (!row.IsNull("Odds"))
                        betlistRow.Odds = NumberFormat.DecFormat(row["Odds"], 2);
                    else
                        betlistRow.Odds = string.Empty;
                }
                #endregion

                racingWinlossBetlistBo.Items.Add(betlistRow);


            }

            #region Total
            racingWinlossBetlistBo.Totals = new RacingWinlossBetlistRowBO()
            {
                Member = "Total",

                Stake = NumberFormat.DecFormat(racingWinlossBetlistBo.Items.Sum(s => CastAide.AsDecimal(s.Stake)), 2),
                MemberWL = NumberFormat.DecFormat(racingWinlossBetlistBo.Items.Sum(s => CastAide.AsDecimal(s.MemberWL)), 2),
                AgencyWL = NumberFormat.DecFormat(racingWinlossBetlistBo.Items.Sum(s => CastAide.AsDecimal(s.AgencyWL)), 2),
                IBCWL = NumberFormat.DecFormat(racingWinlossBetlistBo.Items.Sum(s => CastAide.AsDecimal(s.IBCWL)), 2),

                MemberCom = NumberFormat.DecFormat(racingWinlossBetlistBo.Items.Sum(s => CastAide.AsDecimal(s.MemberCom)), 2),
                AgencyCom = NumberFormat.DecFormat(racingWinlossBetlistBo.Items.Sum(s => CastAide.AsDecimal(s.AgencyCom)), 2),
                IBCCom = NumberFormat.DecFormat(racingWinlossBetlistBo.Items.Sum(s => CastAide.AsDecimal(s.IBCCom)), 2),

                Dividend = NumberFormat.DecFormat(racingWinlossBetlistBo.Items.Sum(s => CastAide.AsDecimal(s.Dividend)), 2),
            };
            #endregion

            return racingWinlossBetlistBo;
        }
        #endregion

        #region Export 2 excel

        private static List<string> GetDistinctMeeting(List<RacingWinlossRowBO> pool)
        {
            var listDistinctMeeting = new List<string>();
            foreach (RacingWinlossRowBO racingWinlossRowBo in pool)
            {
                if (!CheckItemExistedInArray(listDistinctMeeting, racingWinlossRowBo.Name))
                    listDistinctMeeting.Add(racingWinlossRowBo.Name);
            }
            return listDistinctMeeting;
        }

        private static bool CheckItemExistedInArray(List<string> arr, string value)
        {
            foreach (string item in arr)
            {
                if (item == value) return true;
            }
            return false;
        }

        

        #region Racing Group
        //public void ExportToExcelWinLossRacingGroup(string fromDate, string toDate)
        //{
        //    try
        //    {
        //        RacingGroupMain(fromDate, toDate, true, false);
        //    }
        //    catch(Exception ex)
        //    {
        //        
        //        {
        //            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        //        }
        //    }
        //}

        private void RacingGroupMain(string fromDate, string toDate, bool isExport, bool isSendMail, string To, string Cc, string bcc, string Subject, string Content, ref bool isSendMailSuccess)
        {
            #region local variables
            var startCol = 0;
            var startRow = 3;
            var nextRow = 0;
            var reportName = string.Format("{0}-From-{1}-To-{2}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRacingGroupDaily").Replace(" ", ""), fromDate, toDate);
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRacingGroupDaily"));
            #endregion

            #region Assign Header
            #region report title
            wsReport.Cells[startRow - 3, startCol].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells[startRow - 3, startCol].Value = string.Format("{0} - From {1} To {2}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRacingGroupDaily"), fromDate, toDate);
            #endregion
            RacingGroupAssignHeader(ref wsReport, startRow, startCol);
            #endregion
            #region Assign Content
            var getSession = SessionExtension.GetSession(HttpContext.Current.Session, _winLossRacingGroupReportSessionKey)
                != null ? (RacingWinlossGroupDTO)SessionExtension.GetSession(HttpContext.Current.Session, _winLossRacingGroupReportSessionKey) :
                GetRacingWinlossGroup(fromDate, toDate);
            if (getSession != null && getSession.Items.Count > 0)
            {
                foreach (RacingWinlossGroupDayBO item in getSession.Items)
                {
                    var totalList = new List<RacingWinlossGroupRowBO>();
                    totalList.Add(item.Total);

                    RacingGroupAssignContent(ref wsReport, startRow, startCol, ref nextRow,
                        item.Items, false, false);
                    RacingGroupAssignContent(ref wsReport, startRow, startCol, ref nextRow,
                        totalList, true, false);
                }
                if (getSession.Total != null)
                    RacingGroupAssignContent(ref wsReport, startRow, startCol, ref nextRow,
                        getSession.Total, false, true);
            }
            #endregion

            #region response
            if (isExport)
            {
                var response = HttpContext.Current.Response;
                response.Clear();
                response.ContentType = "application/vnd.ms-excel";
                response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
                excelFile.SaveXls(response.OutputStream);
            }
            else if (isSendMail)
            {
                var memoryStream = new MemoryStream();
                excelFile.SaveXls(memoryStream);
                #region mail setting
                var title = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRacingGroupDaily");
                var emailSubject = string.Format("{0} from {1} to {2}", title, fromDate, toDate);
                var emailContent = string.Format("{0}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRacingGroupDailyMailContent"));
                var mailSetting = MailSection.Instance[MailSection.Setting.Send]["RacingGroup"].Mails["GroupDaily"];
                #endregion

                var mailAttachment = new Sunplus.Accounting.Core.Extensions.MailAttachment()
                                         {
                                             Content = memoryStream,
                                             ContentType = "application/ms-excel",
                                             FileName = string.Format("{0}.xls", reportName)
                                         };

                isSendMailSuccess = SendMailExtension.AccSendMail(MailSection.HostName.Gmail, To, Cc, bcc, title, Subject, Content, mailAttachment);
            }
            #endregion
        }

        private void RacingGroupMain(string fromDate, string toDate, bool isExport, bool isSendMail) 
        {
            #region local variables
            var startCol = 0;
            var startRow = 3;
            var nextRow = 0;
            var reportName = string.Format("{0}-From-{1}-To-{2}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRacingGroupDaily").Replace(" ", ""), fromDate, toDate);
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRacingGroupDaily"));
            #endregion

            #region Assign Header
            #region report title
            wsReport.Cells[startRow - 3, startCol].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells[startRow - 3, startCol].Value = string.Format("{0} - From {1} To {2}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRacingGroupDaily"), fromDate, toDate);
            #endregion
            RacingGroupAssignHeader(ref wsReport, startRow, startCol);
            #endregion
            #region Assign Content
            var getSession = SessionExtension.GetSession(HttpContext.Current.Session, _winLossRacingGroupReportSessionKey)
                != null ? (RacingWinlossGroupDTO)SessionExtension.GetSession(HttpContext.Current.Session, _winLossRacingGroupReportSessionKey) :
                GetRacingWinlossGroup(fromDate, toDate);
            if (getSession != null && getSession.Items.Count > 0)
            {
                foreach (RacingWinlossGroupDayBO item in getSession.Items)
                {
                    var totalList = new List<RacingWinlossGroupRowBO>();
                    totalList.Add(item.Total);

                    RacingGroupAssignContent(ref wsReport, startRow, startCol, ref nextRow,
                        item.Items, false, false);
                    RacingGroupAssignContent(ref wsReport, startRow, startCol, ref nextRow,
                        totalList, true, false);
                }
                if (getSession.Total != null)
                    RacingGroupAssignContent(ref wsReport, startRow, startCol, ref nextRow,
                        getSession.Total, false, true);
            }
            #endregion

            #region response
            if (isExport)
            {
                var response = HttpContext.Current.Response;
                response.Clear();
                response.ContentType = "application/vnd.ms-excel";
                response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
                excelFile.SaveXls(response.OutputStream);
            }
            else if (isSendMail)
            {
                var memoryStream = new MemoryStream();
                excelFile.SaveXls(memoryStream);
                #region mail setting
                var title = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRacingGroupDaily");
                var emailSubject = string.Format("{0} from {1} to {2}", title, fromDate, toDate);
                var emailContent = string.Format("{0}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRacingGroupDailyMailContent"));
                var mailSetting = MailSection.Instance[MailSection.Setting.Send]["RacingGroup"].Mails["GroupDaily"];
                #endregion
                

                var mailAttachment = new Sunplus.Accounting.Core.Extensions.MailAttachment()
                {
                    Content = memoryStream,
                    ContentType = "application/ms-excel",
                    FileName = string.Format("{0}.xls", reportName)
                };
                SendMailExtension.AccSendMail(MailSection.HostName.Gmail, mailSetting.To, mailSetting.Cc, title, emailSubject,
                                              emailContent, mailAttachment);
            }
            #endregion
        }
        private void RacingGroupAssignHeader(ref ExcelWorksheet wsReport, int startRow, int startCol)
        {
            #region local variables
            var dateTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Date");
            var partnerTypeTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "PartnerType");
            var totalInComeTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "TotalIncome");
            var groupTotalInCome = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "GroupTotalInCome");
            var turnOverTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "TurnOver");
            var ibcTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblIbc");
            var onetwoBetTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lbl12Bet");
            var partnerTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPartner");
            var agencyTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAgency");
            var totalToTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblTotalTO");
            var ptTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPT");
            var netCommTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblNetComm");
            var devidendWonTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDividendWon");
            var commExenseTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCommExpense");
            var feeTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblFee");
            var amountDueTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAmountDue");
            var commTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblComm");
            var nextCol = 1;
            var bgHeaderNormalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var bgHeaderIbcFormat = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(130, 199, 255));
            #endregion

            #region first row
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Style = bgHeaderIbcFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Value = string.Empty;
            nextCol++;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 2, startCol + nextCol + 3).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 2, startCol + nextCol + 3).Style = bgHeaderNormalFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 2, startCol + (nextCol += 3)).Value = string.Format("{0} (RM)", turnOverTitle);
            //nextCol++;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 2, startCol + nextCol + 3).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 2, startCol + nextCol + 3).Style = bgHeaderIbcFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 2, startCol + (nextCol += 3)).Value = string.Format("{0} (RM)", ibcTitle);
            //nextCol++;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 2, startCol + nextCol + 3).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 2, startCol + nextCol + 3).Style = bgHeaderNormalFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 2, startCol + (nextCol += 3)).Value = string.Format("{0} (RM)", onetwoBetTitle);

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol+1, startRow -1, startCol + nextCol + 1).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 1, startCol + nextCol + 1).Style = bgHeaderIbcFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol+1, startRow -1, startCol + (nextCol += 1)).Value = string.Format("{0}", groupTotalInCome);

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol+1, startRow - 2, startCol + nextCol + 5).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 2, startCol + nextCol + 5).Style = bgHeaderNormalFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol+1, startRow - 2, startCol + (nextCol + 5)).Value = string.Format("{0} (RM)", partnerTitle);
            #endregion

            #region second row
            nextCol = 1;
            wsReport.Cells[startRow - 1, startCol].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow - 1, startCol].Value = dateTitle;
            wsReport.Cells[startRow - 1, startCol + nextCol].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = partnerTypeTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = agencyTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = ibcTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = partnerTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = totalToTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = ptTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = netCommTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = totalInComeTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = ptTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = commTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = totalInComeTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 2)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = turnOverTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = devidendWonTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = commExenseTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = feeTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = amountDueTitle;
            #endregion

        }
        private void RacingGroupAssignContent(ref ExcelWorksheet wsReport, int startRow, 
            int startCol, ref int nextRow, List<RacingWinlossGroupRowBO> reportContent, bool isTotal, bool isFooter)
        {
            #region local variables
            //var bgColor = isFooter ? Color.FromArgb(205, 222, 255) : isTotal ? Color.FromArgb(211, 225, 200) : Color.FromArgb(255, 255, 255);
            var bgColor = isFooter ? Color.FromArgb(211, 225, 200) : Color.FromArgb(255, 255, 255);            
            var bgGrandTotalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(205, 222, 255), true, false);
            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", bgColor, isTotal || isFooter ? true : false, false);
            var nextCol = 1;
            var currentRow = 0;
            #endregion

            foreach (var item in reportContent)
            {
                nextCol = 1;
                bgFormat = (isFooter && currentRow == reportContent.Count - 1) ? bgGrandTotalFormat : bgFormat;

                #region left columns
                if (isFooter || isTotal)
                {
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (nextCol)).Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (nextCol)).Style = bgFormat;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (nextCol)).Value = item.Name;
                }
                else
                {
                    if (currentRow == 0)
                    {
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow + reportContent.Count - 1, startCol).Merged = true;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow + reportContent.Count - 1, startCol).Style = bgFormat;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + (nextRow + reportContent.Count - 1), startCol).Value = item.WinlostDate;
                    }
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style = bgFormat;
                    wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = item.Name;
                }
                #endregion

                #region right columns
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.TurnoverAgency);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.TurnoverIBC);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.Turnover3rd);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.TurnoverTotalTO);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.IBCPT);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.IBCNetComm);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.IBCTotalIncome);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.One2BetPT);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.One2BetComm);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.One2BetTotalIncome);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.GroupTotalIncome);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.PartnerTurnover);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.PartnerDividendWon);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.PartnerCommExpense);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.PartnerFees);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol++)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(item.PartnerAmountDueto);
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style = bgFormat;
                #endregion

                currentRow++;
                nextRow++;
            }
        }
        #endregion

        #region Racing Group For Tote
        public void ExportToExcelWinLossRacingGroupForTote(string fromDate, string toDate)
        {
            RacingGroupForToteMain(fromDate, toDate, true, false);
        }

        private void RacingGroupForToteMain(string fromDate, string toDate, bool isExport, bool isSendMail)
        {
            #region local variables
            var startCol = 0;
            var startRow = 3;
            var nextRow = 0;
            var title = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRacingGroupDailyForTote");
            var reportName = string.Format("{0}-From-{1}-To-{2}", title.Replace(" ", ""), fromDate, toDate);
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(title);
            #endregion

            #region Assign Header
            #region report title
            wsReport.Cells[startRow - 3, startCol].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells[startRow - 3, startCol].Value = string.Format("{0} - From {1} To {2}", title, fromDate, toDate);
            #endregion
            RacingGroupForToteAssignHeader(ref wsReport, startRow, startCol);
            #endregion
            #region Assign Content
            var getSession = SessionExtension.GetSession(HttpContext.Current.Session, _winLossRacingGroupReportSessionKey)
                != null ? (RacingWinlossGroupDayBO)SessionExtension.GetSession(HttpContext.Current.Session, _winLossRacingGroupForToteReportSessionKey) :
                GetRacingWinlossGroupForTote(fromDate, toDate);
            if (getSession != null && getSession.Items.Count > 0)
            {

                RacingGroupForToteAssignContent(ref wsReport, startRow, startCol, ref nextRow, getSession.Items
                        , false, false);

                if (getSession.Total != null)
                {
                    var total = new List<RacingWinlossGroupRowBO>();
                    total.Add(getSession.Total);

                    RacingGroupForToteAssignContent(ref wsReport, startRow, startCol, ref nextRow,
                                                    total, false, true);
                }
            }
            #endregion

            #region response
            if (isExport)
            {
                var response = HttpContext.Current.Response;
                response.Clear();
                response.ContentType = "application/vnd.ms-excel";
                response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
                excelFile.SaveXls(response.OutputStream);
            }
            else if (isSendMail)
            {
                var memoryStream = new MemoryStream();
                excelFile.SaveXls(memoryStream);
                #region mail setting
                
                var emailSubject = string.Format("{0} from {1} to {2}", title, fromDate, toDate);
                var emailContent = string.Format("{0}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRacingGroupDailyMailContent"));
                var mailSetting = MailSection.Instance[MailSection.Setting.Send]["RacingGroup"].Mails["GroupDailyForTote"];
                #endregion

                var mailAttachment = new Sunplus.Accounting.Core.Extensions.MailAttachment()
                {
                    Content = memoryStream,
                    ContentType = "application/ms-excel",
                    FileName = string.Format("{0}.xls", reportName)
                };
                SendMailExtension.AccSendMail(MailSection.HostName.Gmail, mailSetting.To, mailSetting.Cc, title,
                                              emailSubject, emailContent, mailAttachment);
            }
            #endregion
        }

        private void RacingGroupForToteMain(string fromDate, string toDate, bool isExport, bool isSendMail, string To, string Cc, string bcc, string Subject, string Content, ref bool isSendMailSuccess)
        {
            #region local variables
            var startCol = 0;
            var startRow = 3;
            var nextRow = 0;
            var title = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRacingGroupDailyForTote");
            var reportName = string.Format("{0}-From-{1}-To-{2}", title.Replace(" ", ""), fromDate, toDate);
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(title);
            #endregion

            #region Assign Header
            #region report title
            wsReport.Cells[startRow - 3, startCol].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells[startRow - 3, startCol].Value = string.Format("{0} - From {1} To {2}", title, fromDate, toDate);
            #endregion
            RacingGroupForToteAssignHeader(ref wsReport, startRow, startCol);
            #endregion
            #region Assign Content
            var getSession = SessionExtension.GetSession(HttpContext.Current.Session, _winLossRacingGroupReportSessionKey)
                != null ? (RacingWinlossGroupDayBO)SessionExtension.GetSession(HttpContext.Current.Session, _winLossRacingGroupForToteReportSessionKey) :
                GetRacingWinlossGroupForTote(fromDate, toDate);
            if (getSession != null && getSession.Items.Count > 0)
            {

                RacingGroupForToteAssignContent(ref wsReport, startRow, startCol, ref nextRow, getSession.Items
                        , false, false);

                if (getSession.Total != null)
                {
                    var total = new List<RacingWinlossGroupRowBO>();
                    total.Add(getSession.Total);

                    RacingGroupForToteAssignContent(ref wsReport, startRow, startCol, ref nextRow,
                                                    total, false, true);
                }
            }
            #endregion

            #region response
            if (isExport)
            {
                var response = HttpContext.Current.Response;
                response.Clear();
                response.ContentType = "application/vnd.ms-excel";
                response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
                excelFile.SaveXls(response.OutputStream);
            }
            else if (isSendMail)
            {
                var memoryStream = new MemoryStream();
                excelFile.SaveXls(memoryStream);
                #region mail setting

                var emailSubject = string.Format("{0} from {1} to {2}", title, fromDate, toDate);
                var emailContent = string.Format("{0}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRacingGroupDailyMailContent"));
                var mailSetting = MailSection.Instance[MailSection.Setting.Send]["RacingGroup"].Mails["GroupDailyForTote"];
                #endregion

                var mailAttachment = new Sunplus.Accounting.Core.Extensions.MailAttachment()
                {
                    Content = memoryStream,
                    ContentType = "application/ms-excel",
                    FileName = string.Format("{0}.xls", reportName)
                };
                isSendMailSuccess = SendMailExtension.AccSendMail(MailSection.HostName.Gmail, To, Cc, bcc, title,
                                              Subject, Content, mailAttachment);
            }
            #endregion
        }

        private void RacingGroupForToteAssignHeader(ref ExcelWorksheet wsReport, int startRow, int startCol)
        {
            #region local variables
            var dateTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Date");
            var totalInComeTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "TotalIncome");
            var groupTotalInCome = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "GroupTotalInCome");
            var turnOverTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "TurnOver");
            var ibcTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblIbc");
            var onetwoBetTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lbl12Bet");
            var agencyTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAgency");
            var totalToTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblTotalTO");
            var ptTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPT");
            var netCommTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblNetComm");
            var commTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblComm");
            var bettypeTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Bettype");
            var partnerTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPartner");
            var nextCol = 1;
            var bgHeaderNormalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var bgHeaderIbcFormat = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(130, 199, 255));
            #endregion

            #region first row
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Style = bgHeaderIbcFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Value = string.Empty;
            nextCol++;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 2, startCol + nextCol + 3).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 2, startCol + nextCol + 3).Style = bgHeaderNormalFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol, startRow - 2, startCol + (nextCol += 3)).Value = string.Format("{0} (RM)", turnOverTitle);
            //nextCol++;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 2, startCol + nextCol + 3).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 2, startCol + nextCol + 3).Style = bgHeaderIbcFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 2, startCol + (nextCol += 3)).Value = string.Format("{0} (RM)", ibcTitle);
            //nextCol++;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 2, startCol + nextCol + 3).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 2, startCol + nextCol + 3).Style = bgHeaderNormalFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 2, startCol + (nextCol += 3)).Value = string.Format("{0} (RM)", onetwoBetTitle);

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 1, startCol + nextCol + 1).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 1, startCol + nextCol + 1).Style = bgHeaderIbcFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + nextCol + 1, startRow - 1, startCol + (nextCol += 1)).Value = string.Format("{0}", groupTotalInCome);
            #endregion

            #region second row
            nextCol = 1;
            wsReport.Cells[startRow - 1, startCol].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow - 1, startCol].Value = dateTitle;
            wsReport.Cells[startRow - 1, startCol + nextCol].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = bettypeTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = agencyTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = ibcTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = partnerTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = totalToTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = ptTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = netCommTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderIbcFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = totalInComeTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = ptTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = commTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = totalInComeTitle;
            #endregion

        }
        private void RacingGroupForToteAssignContent(ref ExcelWorksheet wsReport, int startRow,
            int startCol, ref int nextRow, List<RacingWinlossGroupRowBO> reportContent, bool isTotal, bool isFooter)
        {
            #region local variables
            var bgColor = isFooter ? Color.FromArgb(205, 222, 255) : Color.FromArgb(255, 255, 255);
            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", bgColor, isTotal || isFooter ? true : false, false);
            var nextCol = 1;
            var currentRow = 0;
            ExcelWorksheet wsWinLoss = wsReport;
            var nextrow = nextRow;
            #endregion

            foreach (var item in reportContent)
            {
                nextCol = 1;
                #region left columns
                if (isFooter || isTotal)
                {
                    wsWinLoss.Cells.GetSubrangeAbsolute(startRow + nextrow, startCol, startRow + nextrow, startCol + (nextCol)).Merged = true;
                    wsWinLoss.Cells.GetSubrangeAbsolute(startRow + nextrow, startCol, startRow + nextrow, startCol + (nextCol)).Style = bgFormat;
                    wsWinLoss.Cells.GetSubrangeAbsolute(startRow + nextrow, startCol, startRow + nextrow, startCol + (nextCol)).Value = item.Name;
                }
                else
                {
                    wsWinLoss.Cells[startRow + nextrow, startCol].Style = bgFormat;
                    wsWinLoss.Cells[startRow + nextrow, startCol].Value = item.WinlostDate;
                    wsWinLoss.Cells[startRow + nextrow, startCol + (nextCol)].Style = bgFormat;
                    wsWinLoss.Cells[startRow + nextrow, startCol + nextCol].Value = item.Name;
                }

                #endregion

                #region right columns
                wsWinLoss.Cells[startRow + nextrow, startCol + (nextCol++)].Style = bgFormat;
                wsWinLoss.Cells[startRow + nextrow, startCol + nextCol].Value = CastAide.AsDouble(item.TurnoverAgency);
                wsWinLoss.Cells[startRow + nextrow, startCol + (nextCol++)].Style = bgFormat;
                wsWinLoss.Cells[startRow + nextrow, startCol + nextCol].Value = CastAide.AsDouble(item.TurnoverIBC);
                wsWinLoss.Cells[startRow + nextrow, startCol + (nextCol++)].Style = bgFormat;
                wsWinLoss.Cells[startRow + nextrow, startCol + nextCol].Value = CastAide.AsDouble(item.Turnover3rd);
                wsWinLoss.Cells[startRow + nextrow, startCol + (nextCol++)].Style = bgFormat;
                wsWinLoss.Cells[startRow + nextrow, startCol + nextCol].Value = CastAide.AsDouble(item.TurnoverTotalTO);

                wsWinLoss.Cells[startRow + nextrow, startCol + (nextCol++)].Style = bgFormat;
                wsWinLoss.Cells[startRow + nextrow, startCol + nextCol].Value = CastAide.AsDouble(item.IBCPT);
                wsWinLoss.Cells[startRow + nextrow, startCol + (nextCol++)].Style = bgFormat;
                wsWinLoss.Cells[startRow + nextrow, startCol + nextCol].Value = CastAide.AsDouble(item.IBCNetComm);
                wsWinLoss.Cells[startRow + nextrow, startCol + (nextCol++)].Style = bgFormat;
                wsWinLoss.Cells[startRow + nextrow, startCol + nextCol].Value = CastAide.AsDouble(item.IBCTotalIncome);


                wsWinLoss.Cells[startRow + nextrow, startCol + (nextCol++)].Style = bgFormat;
                wsWinLoss.Cells[startRow + nextrow, startCol + nextCol].Value = CastAide.AsDouble(item.One2BetPT);
                wsWinLoss.Cells[startRow + nextrow, startCol + (nextCol++)].Style = bgFormat;
                wsWinLoss.Cells[startRow + nextrow, startCol + nextCol].Value = CastAide.AsDouble(item.One2BetComm);
                wsWinLoss.Cells[startRow + nextrow, startCol + (nextCol++)].Style = bgFormat;
                wsWinLoss.Cells[startRow + nextrow, startCol + nextCol].Value = CastAide.AsDouble(item.One2BetTotalIncome);

                wsWinLoss.Cells[startRow + nextrow, startCol + (nextCol++)].Style = bgFormat;
                wsWinLoss.Cells[startRow + nextrow, startCol + nextCol].Value = CastAide.AsDouble(item.GroupTotalIncome);

                wsWinLoss.Cells[startRow + nextrow, startCol + (nextCol)].Style = bgFormat;
                #endregion

                currentRow++;
                nextrow++;
            }

            nextRow = nextrow;
            wsReport = wsWinLoss;
        }

        #endregion
        #endregion

        #region GetRacingWinlossGroup
        public RacingWinlossGroupDTO GetRacingWinlossGroup(string fromdate, string todate)
        {
            RacingWinlossGroupDTO wl = new RacingWinlossGroupDTO();
            wl.Items = new List<RacingWinlossGroupDayBO>();
            wl.Total = new List<RacingWinlossGroupRowBO>();
            
            try
            {
                DataTable dataTable = _reportRacingDao.WinlostRacing_Group(fromdate, todate);
                List<string> dates = dataTable.AsEnumerable().Select(s => CastAide.AsDateTime(s["winlostdate"]).ToShortDateString()).Distinct().ToList();
                List<RacingWinlossGroupRowBO> grandTotalByPartner = new List<RacingWinlossGroupRowBO>();
                
                foreach (string date in dates)
                {
                    var groupDate = new RacingWinlossGroupDayBO();
                    groupDate.Items = new List<RacingWinlossGroupRowBO>();

                    DataRow [] partnersByDate = dataTable.Select("winlostdate='" + CastAide.AsDateTime( date )+ "'");
                    foreach (DataRow row in partnersByDate)
                    {

                        var r = new RacingWinlossGroupRowBO()
                                                {
                                                    #region AssginRow
                                                    WinlostDate = date,
                                                    Name = CastAide.AsString(row["partnerName"]),
                                                    TurnoverAgency = NumberFormat.DecFormat(row["turnoverAgents"], 2),
                                                    TurnoverIBC = NumberFormat.DecFormat(row["turnoverHouse"], 2),
                                                    Turnover3rd = NumberFormat.DecFormat(row["turnover3rd"], 2),
                                                    TurnoverTotalTO = NumberFormat.DecFormat(row["totalTurnover"], 2),

                                                    IBCPT = NumberFormat.DecFormat(row["incomeHousePT"], 2),
                                                    IBCNetComm = NumberFormat.DecFormat(row["incomeHouseNetComm"], 2),
                                                    IBCTotalIncome = NumberFormat.DecFormat(row["totalHouseIncome"], 2),

                                                    One2BetPT = NumberFormat.DecFormat(row["income12BETPT"], 2),
                                                    One2BetComm = NumberFormat.DecFormat(row["income12BETComm"], 2),
                                                    One2BetTotalIncome = NumberFormat.DecFormat(row["total12BETIncome"], 2),
                                                    GroupTotalIncome = NumberFormat.DecFormat(row["groupTotalIncome"], 2),

                                                    PartnerTurnover = NumberFormat.DecFormat(row["turnover3rd"], 2),
                                                    PartnerDividendWon = NumberFormat.DecFormat(row["DividendWon3rd"], 2),
                                                    PartnerCommExpense = NumberFormat.DecFormat(row["ExpComm3rd"], 2),
                                                    PartnerFees = NumberFormat.DecFormat(row["Fees3rd"], 2),
                                                    PartnerAmountDueto = NumberFormat.DecFormat(row["AmountDue23rd"], 2),
                                                    #endregion
                                                };

                        groupDate.Items.Add(r);
                        grandTotalByPartner.Add(r);
                    }
                    #region Sub Total by Date
                    groupDate.Total = new RacingWinlossGroupRowBO()
                                          {
                                              Name = "Total",
                                              TurnoverAgency = NumberFormat.DecFormat( groupDate.Items.Sum(s=> CastAide.AsDecimal(s.TurnoverAgency)),2),
                                              TurnoverIBC = NumberFormat.DecFormat(groupDate.Items.Sum(s => CastAide.AsDecimal(s.TurnoverIBC)), 2),
                                              Turnover3rd = NumberFormat.DecFormat(groupDate.Items.Sum(s => CastAide.AsDecimal(s.Turnover3rd)), 2),
                                              TurnoverTotalTO = NumberFormat.DecFormat(groupDate.Items.Sum(s => CastAide.AsDecimal(s.TurnoverTotalTO)), 2),
                                              IBCPT = NumberFormat.DecFormat(groupDate.Items.Sum(s => CastAide.AsDecimal(s.IBCPT)), 2),
                                              IBCNetComm = NumberFormat.DecFormat(groupDate.Items.Sum(s => CastAide.AsDecimal(s.IBCNetComm)), 2),
                                              IBCTotalIncome = NumberFormat.DecFormat(groupDate.Items.Sum(s => CastAide.AsDecimal(s.IBCTotalIncome)), 2),
                                              One2BetPT = NumberFormat.DecFormat(groupDate.Items.Sum(s => CastAide.AsDecimal(s.One2BetPT)), 2),
                                              One2BetComm = NumberFormat.DecFormat(groupDate.Items.Sum(s => CastAide.AsDecimal(s.One2BetComm)), 2),
                                              One2BetTotalIncome = NumberFormat.DecFormat(groupDate.Items.Sum(s => CastAide.AsDecimal(s.One2BetTotalIncome)), 2),
                                              GroupTotalIncome = NumberFormat.DecFormat(groupDate.Items.Sum(s => CastAide.AsDecimal(s.GroupTotalIncome)), 2),
                                              PartnerTurnover = NumberFormat.DecFormat(groupDate.Items.Sum(s => CastAide.AsDecimal(s.PartnerTurnover)), 2),
                                              PartnerDividendWon = NumberFormat.DecFormat(groupDate.Items.Sum(s => CastAide.AsDecimal(s.PartnerDividendWon)), 2),
                                              PartnerCommExpense = NumberFormat.DecFormat(groupDate.Items.Sum(s => CastAide.AsDecimal(s.PartnerCommExpense)), 2),
                                              PartnerFees = NumberFormat.DecFormat(groupDate.Items.Sum(s => CastAide.AsDecimal(s.PartnerFees)), 2),
                                              PartnerAmountDueto = NumberFormat.DecFormat(groupDate.Items.Sum(s => CastAide.AsDecimal(s.PartnerAmountDueto)), 2),
                                          };
                    #endregion

                    wl.Items.Add(groupDate);
                }

                #region Total by Partner

                List<string> partners = grandTotalByPartner.Select(s => s.Name).Distinct().ToList();
                foreach (string partner in partners)
                {
                    wl.Total.Add(new RacingWinlossGroupRowBO()
                                     {
                                         Name = partner + " Total",
                                         TurnoverAgency = NumberFormat.DecFormat(grandTotalByPartner.Where(s=>s.Name == partner).Sum(s=> CastAide.AsDecimal( s.TurnoverAgency)), 2),
                                         TurnoverIBC = NumberFormat.DecFormat(grandTotalByPartner.Where(s => s.Name == partner).Sum(s => CastAide.AsDecimal(s.TurnoverIBC)), 2),
                                         Turnover3rd = NumberFormat.DecFormat(grandTotalByPartner.Where(s => s.Name == partner).Sum(s => CastAide.AsDecimal(s.Turnover3rd)), 2),
                                         TurnoverTotalTO = NumberFormat.DecFormat(grandTotalByPartner.Where(s => s.Name == partner).Sum(s => CastAide.AsDecimal(s.TurnoverTotalTO)), 2),
                                         IBCPT = NumberFormat.DecFormat(grandTotalByPartner.Where(s => s.Name == partner).Sum(s => CastAide.AsDecimal(s.IBCPT)), 2),
                                         IBCNetComm = NumberFormat.DecFormat(grandTotalByPartner.Where(s => s.Name == partner).Sum(s => CastAide.AsDecimal(s.IBCNetComm)), 2),
                                         IBCTotalIncome = NumberFormat.DecFormat(grandTotalByPartner.Where(s => s.Name == partner).Sum(s => CastAide.AsDecimal(s.IBCTotalIncome)), 2),
                                         One2BetPT = NumberFormat.DecFormat(grandTotalByPartner.Where(s => s.Name == partner).Sum(s => CastAide.AsDecimal(s.One2BetPT)), 2),
                                         One2BetComm = NumberFormat.DecFormat(grandTotalByPartner.Where(s => s.Name == partner).Sum(s => CastAide.AsDecimal(s.One2BetComm)), 2),
                                         One2BetTotalIncome = NumberFormat.DecFormat(grandTotalByPartner.Where(s => s.Name == partner).Sum(s => CastAide.AsDecimal(s.One2BetTotalIncome)), 2),
                                         GroupTotalIncome = NumberFormat.DecFormat(grandTotalByPartner.Where(s => s.Name == partner).Sum(s => CastAide.AsDecimal(s.GroupTotalIncome)), 2),
                                         PartnerTurnover = NumberFormat.DecFormat(grandTotalByPartner.Where(s => s.Name == partner).Sum(s => CastAide.AsDecimal(s.PartnerTurnover)), 2),
                                         PartnerDividendWon = NumberFormat.DecFormat(grandTotalByPartner.Where(s => s.Name == partner).Sum(s => CastAide.AsDecimal(s.PartnerDividendWon)), 2),
                                         PartnerCommExpense = NumberFormat.DecFormat(grandTotalByPartner.Where(s => s.Name == partner).Sum(s => CastAide.AsDecimal(s.PartnerCommExpense)), 2),
                                         PartnerFees = NumberFormat.DecFormat(grandTotalByPartner.Where(s => s.Name == partner).Sum(s => CastAide.AsDecimal(s.PartnerFees)), 2),
                                         PartnerAmountDueto = NumberFormat.DecFormat(grandTotalByPartner.Where(s => s.Name == partner).Sum(s => CastAide.AsDecimal(s.PartnerAmountDueto)), 2), 
                                     });
                }

                wl.Total.Add(new RacingWinlossGroupRowBO()
                                 {
                                     Name = "Grand Total",
                                     TurnoverAgency = NumberFormat.DecFormat(grandTotalByPartner.Sum(s => CastAide.AsDecimal(s.TurnoverAgency)), 2),
                                     TurnoverIBC = NumberFormat.DecFormat(grandTotalByPartner.Sum(s => CastAide.AsDecimal(s.TurnoverIBC)), 2),
                                     Turnover3rd = NumberFormat.DecFormat(grandTotalByPartner.Sum(s => CastAide.AsDecimal(s.Turnover3rd)), 2),
                                     TurnoverTotalTO = NumberFormat.DecFormat(grandTotalByPartner.Sum(s => CastAide.AsDecimal(s.TurnoverTotalTO)), 2),
                                     IBCPT = NumberFormat.DecFormat(grandTotalByPartner.Sum(s => CastAide.AsDecimal(s.IBCPT)), 2),
                                     IBCNetComm = NumberFormat.DecFormat(grandTotalByPartner.Sum(s => CastAide.AsDecimal(s.IBCNetComm)), 2),
                                     IBCTotalIncome = NumberFormat.DecFormat(grandTotalByPartner.Sum(s => CastAide.AsDecimal(s.IBCTotalIncome)), 2),
                                     One2BetPT = NumberFormat.DecFormat(grandTotalByPartner.Sum(s => CastAide.AsDecimal(s.One2BetPT)), 2),
                                     One2BetComm = NumberFormat.DecFormat(grandTotalByPartner.Sum(s => CastAide.AsDecimal(s.One2BetComm)), 2),
                                     One2BetTotalIncome = NumberFormat.DecFormat(grandTotalByPartner.Sum(s => CastAide.AsDecimal(s.One2BetTotalIncome)), 2),
                                     GroupTotalIncome = NumberFormat.DecFormat(grandTotalByPartner.Sum(s => CastAide.AsDecimal(s.GroupTotalIncome)), 2),
                                     PartnerTurnover = NumberFormat.DecFormat(grandTotalByPartner.Sum(s => CastAide.AsDecimal(s.PartnerTurnover)), 2),
                                     PartnerDividendWon = NumberFormat.DecFormat(grandTotalByPartner.Sum(s => CastAide.AsDecimal(s.PartnerDividendWon)), 2),
                                     PartnerCommExpense = NumberFormat.DecFormat(grandTotalByPartner.Sum(s => CastAide.AsDecimal(s.PartnerCommExpense)), 2),
                                     PartnerFees = NumberFormat.DecFormat(grandTotalByPartner.Sum(s => CastAide.AsDecimal(s.PartnerFees)), 2),
                                     PartnerAmountDueto = NumberFormat.DecFormat(grandTotalByPartner.Sum(s => CastAide.AsDecimal(s.PartnerAmountDueto)), 2), 
                                 });

                #endregion
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }

            SessionExtension.AddSession(HttpContext.Current.Session, _winLossRacingGroupReportSessionKey, wl);
            return wl;
        }
        #endregion

        #region GetRacingWinlossGroupForTote
        public RacingWinlossGroupDayBO GetRacingWinlossGroupForTote(string fromdate, string todate)
        {
            RacingWinlossGroupDayBO wl = new RacingWinlossGroupDayBO();
            wl.Items = new List<RacingWinlossGroupRowBO>();
            wl.Total = new RacingWinlossGroupRowBO();

            try
            {
                DataTable dataTable = _reportRacingDao.WinlostRacing_Group(fromdate, todate);
                DataRow[] partnersByDate = dataTable.Select("partnername='RACE-PHU'");
                foreach (DataRow row in partnersByDate)
                {

                    var r = new RacingWinlossGroupRowBO()
                    {
                        #region AssginRow
                        WinlostDate = CastAide.AsDateTime(row["winlostdate"]).ToShortDateString(),
                        Name = CastAide.AsString(row["partnerName"]),
                        TurnoverAgency = NumberFormat.DecFormat(row["turnoverAgents"], 2),
                        TurnoverIBC = NumberFormat.DecFormat(row["turnoverHouse"], 2),
                        Turnover3rd = NumberFormat.DecFormat(row["turnover3rd"], 2),
                        TurnoverTotalTO = NumberFormat.DecFormat(row["totalTurnover"], 2),
                        IBCPT = NumberFormat.DecFormat(row["incomeHousePT"], 2),
                        IBCNetComm = NumberFormat.DecFormat(row["incomeHouseNetComm"], 2),
                        IBCTotalIncome = NumberFormat.DecFormat(row["totalHouseIncome"], 2),
                        One2BetPT = NumberFormat.DecFormat(row["income12BETPT"], 2),
                        One2BetComm = NumberFormat.DecFormat(row["income12BETComm"], 2),
                        One2BetTotalIncome = NumberFormat.DecFormat(row["total12BETIncome"], 2),
                        GroupTotalIncome = NumberFormat.DecFormat(row["groupTotalIncome"], 2),
                        PartnerTurnover = NumberFormat.DecFormat(row["turnover3rd"], 2),
                        PartnerDividendWon = NumberFormat.DecFormat(row["DividendWon3rd"], 2),
                        PartnerCommExpense = NumberFormat.DecFormat(row["ExpComm3rd"], 2),
                        PartnerFees = NumberFormat.DecFormat(row["Fees3rd"], 2),
                        PartnerAmountDueto = NumberFormat.DecFormat(row["AmountDue23rd"], 2),
                        #endregion
                    };

                    wl.Items.Add(r);
                }

                #region Total
                wl.Total = new RacingWinlossGroupRowBO()
                               {
                                  Name = "Grand Total",
                                  TurnoverAgency = NumberFormat.DecFormat(wl.Items.Sum(s=> CastAide.AsDecimal( s.TurnoverAgency)), 2), 
                                  TurnoverIBC = NumberFormat.DecFormat(wl.Items.Sum(s=> CastAide.AsDecimal( s.TurnoverIBC)), 2), 
                                  Turnover3rd = NumberFormat.DecFormat(wl.Items.Sum(s=> CastAide.AsDecimal( s.Turnover3rd)), 2), 
                                  TurnoverTotalTO = NumberFormat.DecFormat(wl.Items.Sum(s=> CastAide.AsDecimal( s.TurnoverTotalTO)), 2), 
                                  IBCPT = NumberFormat.DecFormat(wl.Items.Sum(s=> CastAide.AsDecimal( s.IBCPT)), 2), 
                                  IBCNetComm = NumberFormat.DecFormat(wl.Items.Sum(s=> CastAide.AsDecimal( s.IBCNetComm)), 2), 
                                  IBCTotalIncome = NumberFormat.DecFormat(wl.Items.Sum(s=> CastAide.AsDecimal( s.IBCTotalIncome)), 2), 
                                  One2BetPT = NumberFormat.DecFormat(wl.Items.Sum(s=> CastAide.AsDecimal( s.One2BetPT)), 2), 
                                  One2BetComm = NumberFormat.DecFormat(wl.Items.Sum(s=> CastAide.AsDecimal( s.One2BetComm)), 2), 
                                  One2BetTotalIncome = NumberFormat.DecFormat(wl.Items.Sum(s=> CastAide.AsDecimal( s.One2BetTotalIncome)), 2), 
                                  GroupTotalIncome = NumberFormat.DecFormat(wl.Items.Sum(s=> CastAide.AsDecimal( s.GroupTotalIncome)), 2), 
                                  PartnerTurnover = NumberFormat.DecFormat(wl.Items.Sum(s=> CastAide.AsDecimal( s.PartnerTurnover)), 2), 
                                  PartnerDividendWon = NumberFormat.DecFormat(wl.Items.Sum(s=> CastAide.AsDecimal( s.PartnerDividendWon)), 2), 
                                  PartnerCommExpense = NumberFormat.DecFormat(wl.Items.Sum(s=> CastAide.AsDecimal( s.PartnerCommExpense)), 2),
                                  PartnerFees = NumberFormat.DecFormat(wl.Items.Sum(s => CastAide.AsDecimal(s.PartnerFees)), 2),
                                  PartnerAmountDueto = NumberFormat.DecFormat(wl.Items.Sum(s => CastAide.AsDecimal(s.PartnerAmountDueto)), 2), 
                               };
                #endregion
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }

            SessionExtension.AddSession(HttpContext.Current.Session,_winLossRacingGroupForToteReportSessionKey, wl);
            return wl;
        }
        #endregion

        #region Send Mail
        public bool SendMailWinLossRacingGroup(string fromDate, string toDate) 
        {
            try
            {
                RacingGroupMain(fromDate, toDate, false, true);
                return true;
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return false;
            }
        }

        public SendMailResult SendMailWinLossRacingGroup(string fromDate, string toDate, List<MailFormat> mails, string Subject, string Content)
        {
            var sendMailResult = new SendMailResult();
            try
            {
                var isSuccess = false;
                if (mails != null)
                {
                    RacingGroupMain(fromDate, toDate, false, true, mails[0].To, mails[0].Cc,
                                    mails[0].Bcc, Subject, Content, ref isSuccess);
                }

                if (!isSuccess)
                {
                    sendMailResult.IsFail = true;
                    sendMailResult.Recivers = new List<SendMailToReciverResult>();
                    sendMailResult.Recivers.Add(new SendMailToReciverResult()
                                                    {
                                                        ErrorMessage =
                                                            string.Format("- {0}: {1}",
                                                                            ResourceExtension.GetResourceValue(
                                                                                ResourceExtension.Resources.Language,
                                                                                "lblRacingGroupDaily"),
                                                                            ResourceExtension.GetResourceValue(
                                                                                ResourceExtension.Resources.Language,
                                                                                "lblSendMailFail")),
                                                        Reciver =
                                                            ResourceExtension.GetResourceValue(
                                                                ResourceExtension.Resources.Language,
                                                                "lblRacingGroupDaily"),
                                                        IsSuccess = false
                                                    });
                }
                

            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }

            return sendMailResult;
        }

        public Mails GetDefaultToEmailAddress(int horseId)
        {
            var getGroupMail = "GroupDaily";
            switch (horseId)
            {
                case 0:
                    getGroupMail = "GroupDaily";
                    break;
                case (int)Constants.Sport.HorseRacing:
                    getGroupMail = "GroupDailyForTote";
                    break;
                default:
                    break;
            }

            var mailSetting = MailSection.Instance[MailSection.Setting.Send]["RacingGroup"].Mails[getGroupMail];
            var recivers = new Mails();
            recivers.Recivers=new List<MailFormat>();
            recivers.Recivers.Add(new MailFormat()
                                      {
                                          Bcc = mailSetting.Bcc,
                                          Cc = mailSetting.Cc,
                                          Reciver = mailSetting.Name,
                                          To = mailSetting.To
                                      });
            return recivers;
        }

        public bool SendMailWinLossRacingGroupForTote(string fromDate, string toDate)
        {
            try
            {
                RacingGroupForToteMain(fromDate, toDate, false, true);
                return true;
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return false;
            }
        }

        public SendMailResult SendMailWinLossRacingGroupForTote(string fromDate, string toDate, List<MailFormat> mails, string Subject, string Content)
        {
            var sendMailResult = new SendMailResult();
            try
            {
                var isSuccess = false;
                if (mails != null)
                {
                    RacingGroupForToteMain(fromDate, toDate, false, true, mails[0].To, mails[0].Cc, mails[0].Bcc, Subject, Content, ref isSuccess);
                }
                if(!isSuccess)
                {
                    sendMailResult.IsFail = true;
                    sendMailResult.Recivers = new List<SendMailToReciverResult>();
                    sendMailResult.Recivers.Add(new SendMailToReciverResult()
                    {
                        ErrorMessage =
                            string.Format("- {0}: {1}",
                                          ResourceExtension.GetResourceValue(
                                              ResourceExtension.Resources.Language,
                                              "lblRacingGroupDailyForTote"),
                                          ResourceExtension.GetResourceValue(
                                              ResourceExtension.Resources.Language,
                                              "lblSendMailFail")),
                        Reciver =
                            ResourceExtension.GetResourceValue(
                                ResourceExtension.Resources.Language,
                                "lblRacingGroupDailyForTote"),
                        IsSuccess = false
                    });
                }

            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                
            }

            return sendMailResult;
        }
        
        #endregion

    }

    class PartnerPoolCurr
    {
        public int PartnerId { get; set; }
        public int PoolCurr { get; set; }
    }
    class MeetingCurr
    {
        public int CurrencyId { get; set; }
        public int MeetingId { get; set; }
    }
}

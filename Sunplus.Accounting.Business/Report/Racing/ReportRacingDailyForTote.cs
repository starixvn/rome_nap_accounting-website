using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DNAide.Core;
using DNAide.Utils;
using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Entities.Report.Racebook;
using System.Web;

namespace Sunplus.Accounting.Business.Report.Racing
{
    public partial class ReportRacing
    {
        string ArrCurrencySubTotal_USD = "3,35,40,17,16";
        private int[] ArrUSD = { 3, 35, 40, 17, 16 };

        string ArrCurrencySubTotal_THB = "4,43";
        private int[] ArrTHB = {4, 43};

        string ArrCurrencySubTotal_IN = "15,22,33,34,47";
        private int[] ArrIN = {15, 22, 33, 34, 47};

        string strArrCurrencyOrder = "[2],[13],[6],[5],[15], [22],[33],[34], [47],[1],[4],[43],[3],[35],[40],[17],[16]";
        private string _RacingDailyWinLossForToteSessionKey = "racingdailywinlossfortote";
        #region SelWinlostHR_Details
        public RacingDailyWinLossDTO SelWinlostHR_Details(string wldate)
        {

            RacingDailyWinLossDTO racing = new RacingDailyWinLossDTO();
            racing.Items = new List<RacingDailyWinLossByCurrency>();
            racing.Totals = new List<RacingDailyWinLossRow>();

            try
            {
                DataTable dataTable = new DataTable();
                DataTable  dtWL = _reportRacingDao.SelWinlostHR_Details(wldate);

                 dtWL.Columns.Add("Order", typeof (int));
                int cid = 0, cid_p = 0;
                foreach (DataRow dr in dtWL.Rows)
                {
                    cid_p = CastAide.AsInt32(dr["currency"]);

                    if (strArrCurrencyOrder.Contains("[" + cid_p + "]"))
                    {
                        dr["Order"] = strArrCurrencyOrder.IndexOf("[" + cid_p + "]");
                    }
                    else
                    {
                        dr["Order"] = 1001;
                    }
                }

                DataView dv = dtWL.DefaultView;
                dv.Sort = "Order asc, currency asc, leagueid asc ";
                dataTable = dv.ToTable();

                List<int> Currencies = dataTable.AsEnumerable().OrderBy(l => l["Order"]).Select(s => CastAide.AsInt32(s["currency"])).Distinct().ToList();

                RacingDailyWinLossByCurrency byAllCurrencyBase = new RacingDailyWinLossByCurrency();
                byAllCurrencyBase.Items = new List<RacingDailyWinLossByMeetingCurrency>();
                byAllCurrencyBase.Totals = new List<RacingDailyWinLossRow>();
                
                int CountUSD = 0;
                int CountTHB = 0;
                int CountIN = 0;
                int TotalCurrUSD = 0;
                int TotalCurrTHB = 0;
                int TotalCurrIN = 0;

                
                TotalCurrIN = Currencies.Intersect(ArrIN).ToList().Count;
                TotalCurrTHB = Currencies.Intersect(ArrTHB).ToList().Count;
                TotalCurrUSD = Currencies.Intersect(ArrUSD).ToList().Count;
                
                
                foreach (int currency in Currencies)
                {
                    #region Check Is need to calculate group total currency
                    if(ArrCurrencySubTotal_USD.Split(',').Where(s=>s == currency.ToString()).Count()>0)
                    {
                        CountUSD++;
                    }
                    if (ArrCurrencySubTotal_THB.Split(',').Where(s => s == currency.ToString()).Count() > 0)
                    {
                        CountTHB++;
                    }
                    if (ArrCurrencySubTotal_IN.Split(',').Where(s => s == currency.ToString()).Count() > 0)
                    {
                        CountIN++;
                    }
                    #endregion

                    RacingDailyWinLossByCurrency byCurrencyOrigin = new RacingDailyWinLossByCurrency();
                    byCurrencyOrigin.Items = new List<RacingDailyWinLossByMeetingCurrency>();
                    byCurrencyOrigin.Totals = new List<RacingDailyWinLossRow>();
                    byCurrencyOrigin.GroupTotals = new List<RacingDailyWinLossRow>();

                    RacingDailyWinLossByCurrency byCurrencyBase = new RacingDailyWinLossByCurrency();
                    byCurrencyBase.Items = new List<RacingDailyWinLossByMeetingCurrency>();
                    byCurrencyBase.Totals = new List<RacingDailyWinLossRow>();

                    List<int> MeetingIds =
                        dataTable.AsEnumerable().Where(s => CastAide.AsInt32(s["currency"]) == currency).Select(
                            l => CastAide.AsInt32(l["leagueid"])).Distinct().ToList();

                    foreach (int meetingId in MeetingIds)
                    {
                        RacingDailyWinLossByMeetingCurrency meetingCurrencyOrigin = new RacingDailyWinLossByMeetingCurrency();
                        meetingCurrencyOrigin.Items = new List<RacingDailyWinLossRow>();
                        meetingCurrencyOrigin.Totals = new List<RacingDailyWinLossRow>();

                        RacingDailyWinLossByMeetingCurrency meetingCurrencyBase = new RacingDailyWinLossByMeetingCurrency();
                        meetingCurrencyBase.Items = new List<RacingDailyWinLossRow>();
                        meetingCurrencyBase.Totals = new List<RacingDailyWinLossRow>();

                        DataRow[] rows = dataTable.Select("currency=" + currency + " and leagueid=" + meetingId);

                        foreach (DataRow row in rows)
                        {
                            #region Origin
                            meetingCurrencyOrigin.Items.Add(new RacingDailyWinLossRow()
                            {
                                Currency = CastAide.AsString(row["currencyName"]),
                                CurrencyId = currency.ToString(),

                                Meeting = CacheRef.GetLeagueName(meetingId),
                                MeetingId = meetingId.ToString(),
                                Race = CastAide.AsString(row["groupmatch"]),

                                AgWinTO = NumberFormat.DecFormat(row["winTO_Agency_Origin"], 2),
                                AgPlaceTO = NumberFormat.DecFormat(row["placeTO_Agency_Origin"], 2),
                                AgWPTO = NumberFormat.DecFormat(row["winplaceTO_Agency_Origin"], 2),
                                AgTotal = NumberFormat.DecFormat(row["totalTOAgency_Origin"], 2),
                                AgCommIncome = NumberFormat.DecFormat(row["commIncomeAgency_Origin"], 2),
                                AgPTWL = NumberFormat.DecFormat(row["PTWLAgency_Origin"], 2),

                                IBCWinTO = NumberFormat.DecFormat(row["winTO_IBC_Origin"], 2),
                                IBCPlaceTO = NumberFormat.DecFormat(row["placeTO_IBC_Origin"], 2),
                                IBCWPTO = NumberFormat.DecFormat(row["winplaceTO_IBC_Origin"], 2),
                                IBCTotal = NumberFormat.DecFormat(row["totalTOIBC_Origin"], 2),
                                IBCCommIncome = NumberFormat.DecFormat(row["commIncomeIBC_Origin"], 2),
                                IBCCommExpense = NumberFormat.DecFormat(row["commIncomeAgency_Origin"], 2),
                                IBCAmountFee = NumberFormat.DecFormat(row["FeeDue2_Origin"], 2),
                                IBCPT = NumberFormat.DecFormat(row["PTWLIBC_Origin"], 2),
                                IBCNetIncome = NumberFormat.DecFormat(row["netIncomeIBC_Origin"], 2),

                                PhuWinTO = NumberFormat.DecFormat(row["winTOPhum_Origin"], 2),
                                PhuPlaceTO = NumberFormat.DecFormat(row["placeTOPhum_Origin"], 2),
                                PhuWPTO = NumberFormat.DecFormat(row["winplaceTOPhum_Origin"], 2),
                                PhuTotal = NumberFormat.DecFormat(row["totalTOPhum_Origin"], 2),
                                PhuDividendWon = NumberFormat.DecFormat(row["dividendWon_Origin"], 2),
                                PhuCommExpense = NumberFormat.DecFormat(row["commIncomeIBC_Origin"], 2),
                                PhuAmountFee = NumberFormat.DecFormat(row["FeeDue2_Origin"], 2),
                                PhuAmountDue = NumberFormat.DecFormat(row["AmountDue_Origin"], 2),
                                
                            });
                            #endregion

                            #region Base
                            meetingCurrencyBase.Items.Add(new RacingDailyWinLossRow()
                            {
                                Currency = CastAide.AsString(row["currencyName"]),
                                CurrencyId = currency.ToString(),

                                Meeting = CacheRef.GetLeagueName(meetingId),
                                MeetingId = meetingId.ToString(),
                                Race = CastAide.AsString(row["groupmatch"]),

                                AgWinTO = NumberFormat.DecFormat(row["winTO_Agency"], 2),
                                AgPlaceTO = NumberFormat.DecFormat(row["placeTO_Agency"], 2),
                                AgWPTO = NumberFormat.DecFormat(row["winplaceTO_Agency"], 2),
                                AgTotal = NumberFormat.DecFormat(row["totalTOAgency"], 2),
                                AgCommIncome = NumberFormat.DecFormat(row["commIncomeAgency"], 2),
                                AgPTWL = NumberFormat.DecFormat(row["PTWLAgency"], 2),

                                IBCWinTO = NumberFormat.DecFormat(row["winTO_IBC"], 2),
                                IBCPlaceTO = NumberFormat.DecFormat(row["placeTO_IBC"], 2),
                                IBCWPTO = NumberFormat.DecFormat(row["winplaceTO_IBC"], 2),
                                IBCTotal = NumberFormat.DecFormat(row["totalTOIBC"], 2),
                                IBCCommIncome = NumberFormat.DecFormat(row["commIncomeIBC"], 2),
                                IBCCommExpense = NumberFormat.DecFormat(row["commIncomeAgency"], 2),
                                IBCAmountFee = NumberFormat.DecFormat(row["FeeDue2"], 2),
                                IBCPT = NumberFormat.DecFormat(row["PTWLIBC"], 2),
                                IBCNetIncome = NumberFormat.DecFormat(row["netIncomeIBC"], 2),

                                PhuWinTO = NumberFormat.DecFormat(row["winTOPhum"], 2),
                                PhuPlaceTO = NumberFormat.DecFormat(row["placeTOPhum"], 2),
                                PhuWPTO = NumberFormat.DecFormat(row["winplaceTOPhum"], 2),
                                PhuTotal = NumberFormat.DecFormat(row["totalTOPhum"], 2),
                                PhuDividendWon = NumberFormat.DecFormat(row["dividendWon"], 2),
                                PhuCommExpense = NumberFormat.DecFormat(row["commIncomeIBC"], 2),
                                PhuAmountFee = NumberFormat.DecFormat(row["FeeDue2"], 2),
                                PhuAmountDue = NumberFormat.DecFormat(row["AmountDue"], 2),

                            });
                            #endregion
                        }

                        #region Total Origin
                        meetingCurrencyOrigin.Totals.Add(new RacingDailyWinLossRow()
                                {
                                    Currency = CacheUtils.GetExchangeName(currency),
                                    CurrencyId = currency.ToString(),
                                    Meeting = CacheRef.GetLeagueName(meetingId),
                                    MeetingId = meetingId.ToString(),

                                    AgWinTO = NumberFormat.DecFormat(meetingCurrencyOrigin.Items.Sum(s=> CastAide.AsDecimal( s.AgWinTO)), 2),
                                    AgPlaceTO = NumberFormat.DecFormat(meetingCurrencyOrigin.Items.Sum(s => CastAide.AsDecimal(s.AgPlaceTO)), 2),
                                    AgWPTO = NumberFormat.DecFormat(meetingCurrencyOrigin.Items.Sum(s => CastAide.AsDecimal(s.AgWPTO)), 2),
                                    AgTotal = NumberFormat.DecFormat(meetingCurrencyOrigin.Items.Sum(s => CastAide.AsDecimal(s.AgTotal)), 2),
                                    AgCommIncome = NumberFormat.DecFormat(meetingCurrencyOrigin.Items.Sum(s => CastAide.AsDecimal(s.AgCommIncome)), 2),
                                    AgPTWL = NumberFormat.DecFormat(meetingCurrencyOrigin.Items.Sum(s => CastAide.AsDecimal(s.AgPTWL)), 2),

                                    IBCWinTO = NumberFormat.DecFormat(meetingCurrencyOrigin.Items.Sum(s => CastAide.AsDecimal(s.IBCWinTO)), 2),
                                    IBCPlaceTO = NumberFormat.DecFormat(meetingCurrencyOrigin.Items.Sum(s => CastAide.AsDecimal(s.IBCPlaceTO)), 2),
                                    IBCWPTO = NumberFormat.DecFormat(meetingCurrencyOrigin.Items.Sum(s => CastAide.AsDecimal(s.IBCWPTO)), 2),
                                    IBCTotal = NumberFormat.DecFormat(meetingCurrencyOrigin.Items.Sum(s => CastAide.AsDecimal(s.IBCTotal)), 2),
                                    IBCCommIncome = NumberFormat.DecFormat(meetingCurrencyOrigin.Items.Sum(s => CastAide.AsDecimal(s.IBCCommIncome)), 2),
                                    IBCCommExpense = NumberFormat.DecFormat(meetingCurrencyOrigin.Items.Sum(s => CastAide.AsDecimal(s.IBCCommExpense)), 2),
                                    IBCAmountFee = NumberFormat.DecFormat(meetingCurrencyOrigin.Items.Sum(s => CastAide.AsDecimal(s.IBCAmountFee)), 2),
                                    IBCPT = NumberFormat.DecFormat(meetingCurrencyOrigin.Items.Sum(s => CastAide.AsDecimal(s.IBCPT)), 2),
                                    IBCNetIncome = NumberFormat.DecFormat(meetingCurrencyOrigin.Items.Sum(s => CastAide.AsDecimal(s.IBCNetIncome)), 2),

                                    PhuWinTO = NumberFormat.DecFormat(meetingCurrencyOrigin.Items.Sum(s => CastAide.AsDecimal(s.PhuWinTO)), 2),
                                    PhuPlaceTO = NumberFormat.DecFormat(meetingCurrencyOrigin.Items.Sum(s => CastAide.AsDecimal(s.PhuPlaceTO)), 2),
                                    PhuWPTO = NumberFormat.DecFormat(meetingCurrencyOrigin.Items.Sum(s => CastAide.AsDecimal(s.PhuWPTO)), 2),
                                    PhuTotal = NumberFormat.DecFormat(meetingCurrencyOrigin.Items.Sum(s => CastAide.AsDecimal(s.PhuTotal)), 2),
                                    PhuDividendWon = NumberFormat.DecFormat(meetingCurrencyOrigin.Items.Sum(s => CastAide.AsDecimal(s.PhuDividendWon)), 2),
                                    PhuCommExpense = NumberFormat.DecFormat(meetingCurrencyOrigin.Items.Sum(s => CastAide.AsDecimal(s.PhuCommExpense)), 2),
                                    PhuAmountFee = NumberFormat.DecFormat(meetingCurrencyOrigin.Items.Sum(s => CastAide.AsDecimal(s.PhuAmountFee)), 2),
                                    PhuAmountDue = NumberFormat.DecFormat(meetingCurrencyOrigin.Items.Sum(s => CastAide.AsDecimal(s.PhuAmountDue)), 2),
                                });
                        #endregion

                        byCurrencyOrigin.Items.Add(meetingCurrencyOrigin);
                        byCurrencyBase.Items.Add(meetingCurrencyBase);
                        byAllCurrencyBase.Items.Add(meetingCurrencyBase);
                    }

                    #region Total Origin currency
                    byCurrencyOrigin.Totals.Add(new RacingDailyWinLossRow()
                        {
                            CurrencyId = currency.ToString(),
                            Currency = CacheUtils.GetExchangeName(currency),
                           

                            AgWinTO = NumberFormat.DecFormat( byCurrencyOrigin.Items.Sum(s=>s.Items.Sum(l=> CastAide.AsDecimal(l.AgWinTO)))  , 2),
                            AgPlaceTO = NumberFormat.DecFormat(byCurrencyOrigin.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.AgPlaceTO))), 2),
                            AgWPTO = NumberFormat.DecFormat(byCurrencyOrigin.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.AgWPTO))), 2),
                            AgTotal = NumberFormat.DecFormat(byCurrencyOrigin.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.AgTotal))), 2),
                            AgCommIncome = NumberFormat.DecFormat(byCurrencyOrigin.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.AgCommIncome))), 2),
                            AgPTWL = NumberFormat.DecFormat(byCurrencyOrigin.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.AgPTWL))), 2),

                            IBCWinTO = NumberFormat.DecFormat(byCurrencyOrigin.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.IBCWinTO))), 2),
                            IBCPlaceTO = NumberFormat.DecFormat(byCurrencyOrigin.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.IBCPlaceTO))), 2),
                            IBCWPTO = NumberFormat.DecFormat(byCurrencyOrigin.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.IBCWPTO))), 2),
                            IBCTotal = NumberFormat.DecFormat(byCurrencyOrigin.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.IBCTotal))), 2),
                            IBCCommIncome = NumberFormat.DecFormat(byCurrencyOrigin.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.IBCCommIncome))), 2),
                            IBCCommExpense = NumberFormat.DecFormat(byCurrencyOrigin.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.IBCCommExpense))), 2),
                            IBCAmountFee = NumberFormat.DecFormat(byCurrencyOrigin.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.IBCAmountFee))), 2),
                            IBCPT = NumberFormat.DecFormat(byCurrencyOrigin.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.IBCPT))), 2),
                            IBCNetIncome = NumberFormat.DecFormat(byCurrencyOrigin.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.IBCNetIncome))), 2),

                            PhuWinTO = NumberFormat.DecFormat(byCurrencyOrigin.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.PhuWinTO))), 2),
                            PhuPlaceTO = NumberFormat.DecFormat(byCurrencyOrigin.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.PhuPlaceTO))), 2),
                            PhuWPTO = NumberFormat.DecFormat(byCurrencyOrigin.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.PhuWPTO))), 2),
                            PhuTotal = NumberFormat.DecFormat(byCurrencyOrigin.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.PhuTotal))), 2),
                            PhuDividendWon = NumberFormat.DecFormat(byCurrencyOrigin.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.PhuDividendWon))), 2),
                            PhuCommExpense = NumberFormat.DecFormat(byCurrencyOrigin.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.PhuCommExpense))), 2),
                            PhuAmountFee = NumberFormat.DecFormat(byCurrencyOrigin.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.PhuAmountFee))), 2),
                            PhuAmountDue = NumberFormat.DecFormat(byCurrencyOrigin.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.PhuAmountDue))), 2),
                        });
                    #endregion

                    #region Total Base currency
                    if (currency != 2)
                    {
                        byCurrencyOrigin.Totals.Add(new RacingDailyWinLossRow()
                        {
                            CurrencyId = "2",
                            Currency = "Base (RM)",

                            AgWinTO = NumberFormat.DecFormat(byCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.AgWinTO))), 2),
                            AgPlaceTO = NumberFormat.DecFormat(byCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.AgPlaceTO))), 2),
                            AgWPTO = NumberFormat.DecFormat(byCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.AgWPTO))), 2),
                            AgTotal = NumberFormat.DecFormat(byCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.AgTotal))), 2),
                            AgCommIncome = NumberFormat.DecFormat(byCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.AgCommIncome))), 2),
                            AgPTWL = NumberFormat.DecFormat(byCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.AgPTWL))), 2),

                            IBCWinTO = NumberFormat.DecFormat(byCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.IBCWinTO))), 2),
                            IBCPlaceTO = NumberFormat.DecFormat(byCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.IBCPlaceTO))), 2),
                            IBCWPTO = NumberFormat.DecFormat(byCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.IBCWPTO))), 2),
                            IBCTotal = NumberFormat.DecFormat(byCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.IBCTotal))), 2),
                            IBCCommIncome = NumberFormat.DecFormat(byCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.IBCCommIncome))), 2),
                            IBCCommExpense = NumberFormat.DecFormat(byCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.IBCCommExpense))), 2),
                            IBCAmountFee = NumberFormat.DecFormat(byCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.IBCAmountFee))), 2),
                            IBCPT = NumberFormat.DecFormat(byCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.IBCPT))), 2),
                            IBCNetIncome = NumberFormat.DecFormat(byCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.IBCNetIncome))), 2),

                            PhuWinTO = NumberFormat.DecFormat(byCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.PhuWinTO))), 2),
                            PhuPlaceTO = NumberFormat.DecFormat(byCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.PhuPlaceTO))), 2),
                            PhuWPTO = NumberFormat.DecFormat(byCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.PhuWPTO))), 2),
                            PhuTotal = NumberFormat.DecFormat(byCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.PhuTotal))), 2),
                            PhuDividendWon = NumberFormat.DecFormat(byCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.PhuDividendWon))), 2),
                            PhuCommExpense = NumberFormat.DecFormat(byCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.PhuCommExpense))), 2),
                            PhuAmountFee = NumberFormat.DecFormat(byCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.PhuAmountFee))), 2),
                            PhuAmountDue = NumberFormat.DecFormat(byCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.PhuAmountDue))), 2),
                        });
                    }
                    #endregion

                    #region Sub Total By Group Currency

                    string expression = "";

                    #region USD
                    if (CountUSD == TotalCurrUSD && TotalCurrUSD>=2)
                    {
                        //reset counter
                        CountUSD = -100;
                        expression = "currency in (" + ArrCurrencySubTotal_USD + ")";

                        #region Origin
                        byCurrencyOrigin.GroupTotals.Add(new RacingDailyWinLossRow()
                        {
                            CurrencyId = currency.ToString(),
                            Currency = CacheUtils.GetExchangeName(currency),

                            AgWinTO = NumberFormat.DecFormat(dataTable.Compute("sum(winTO_Agency_Origin)", expression), 2),
                            AgPlaceTO = NumberFormat.DecFormat(dataTable.Compute("sum(placeTO_Agency_Origin)", expression), 2),
                            AgWPTO = NumberFormat.DecFormat(dataTable.Compute("sum(winplaceTO_Agency_Origin)", expression), 2),
                            AgTotal = NumberFormat.DecFormat(dataTable.Compute("sum(totalTOAgency_Origin)", expression), 2),
                            AgCommIncome = NumberFormat.DecFormat(dataTable.Compute("sum(commIncomeAgency_Origin)", expression), 2),
                            AgPTWL = NumberFormat.DecFormat(dataTable.Compute("sum(PTWLAgency_Origin)", expression), 2),

                            IBCWinTO = NumberFormat.DecFormat(dataTable.Compute("sum(winTO_IBC_Origin)", expression), 2),
                            IBCPlaceTO = NumberFormat.DecFormat(dataTable.Compute("sum(placeTO_IBC_Origin)", expression), 2),
                            IBCWPTO = NumberFormat.DecFormat(dataTable.Compute("sum(winplaceTO_IBC_Origin)", expression), 2),
                            IBCTotal = NumberFormat.DecFormat(dataTable.Compute("sum(totalTOIBC_Origin)", expression), 2),
                            IBCCommIncome = NumberFormat.DecFormat(dataTable.Compute("sum(commIncomeIBC_Origin)", expression), 2),
                            IBCCommExpense = NumberFormat.DecFormat(dataTable.Compute("sum(commIncomeAgency_Origin)", expression), 2),
                            IBCAmountFee = NumberFormat.DecFormat(dataTable.Compute("sum(FeeDue2_Origin)", expression), 2),
                            IBCPT = NumberFormat.DecFormat(dataTable.Compute("sum(PTWLIBC_Origin)", expression), 2),
                            IBCNetIncome = NumberFormat.DecFormat(dataTable.Compute("sum(netIncomeIBC_Origin)", expression), 2),

                            PhuWinTO = NumberFormat.DecFormat(dataTable.Compute("sum(winTOPhum_Origin)", expression), 2),
                            PhuPlaceTO = NumberFormat.DecFormat(dataTable.Compute("sum(placeTOPhum_Origin)", expression), 2),
                            PhuWPTO = NumberFormat.DecFormat(dataTable.Compute("sum(winplaceTOPhum_Origin)", expression), 2),
                            PhuTotal = NumberFormat.DecFormat(dataTable.Compute("sum(totalTOPhum_Origin)", expression), 2),
                            PhuDividendWon = NumberFormat.DecFormat(dataTable.Compute("sum(dividendWon_Origin)", expression), 2),
                            PhuCommExpense = NumberFormat.DecFormat(dataTable.Compute("sum(commIncomeIBC_Origin)", expression), 2),
                            PhuAmountFee = NumberFormat.DecFormat(dataTable.Compute("sum(FeeDue2_Origin)", expression), 2),
                            PhuAmountDue = NumberFormat.DecFormat(dataTable.Compute("sum(AmountDue_Origin)", expression), 2),

                        });
                        #endregion

                        #region Base RM
                        byCurrencyOrigin.GroupTotals.Add(new RacingDailyWinLossRow()
                        {
                            CurrencyId = "2",
                            Currency = "Base (RM)",

                            AgWinTO = NumberFormat.DecFormat(dataTable.Compute("sum(winTO_Agency)", expression), 2),
                            AgPlaceTO = NumberFormat.DecFormat(dataTable.Compute("sum(placeTO_Agency)", expression), 2),
                            AgWPTO = NumberFormat.DecFormat(dataTable.Compute("sum(winplaceTO_Agency)", expression), 2),
                            AgTotal = NumberFormat.DecFormat(dataTable.Compute("sum(totalTOAgency)", expression), 2),
                            AgCommIncome = NumberFormat.DecFormat(dataTable.Compute("sum(commIncomeAgency)", expression), 2),
                            AgPTWL = NumberFormat.DecFormat(dataTable.Compute("sum(PTWLAgency)", expression), 2),

                            IBCWinTO = NumberFormat.DecFormat(dataTable.Compute("sum(winTO_IBC)", expression), 2),
                            IBCPlaceTO = NumberFormat.DecFormat(dataTable.Compute("sum(placeTO_IBC)", expression), 2),
                            IBCWPTO = NumberFormat.DecFormat(dataTable.Compute("sum(winplaceTO_IBC)", expression), 2),
                            IBCTotal = NumberFormat.DecFormat(dataTable.Compute("sum(totalTOIBC)", expression), 2),
                            IBCCommIncome = NumberFormat.DecFormat(dataTable.Compute("sum(commIncomeIBC)", expression), 2),
                            IBCCommExpense = NumberFormat.DecFormat(dataTable.Compute("sum(commIncomeAgency)", expression), 2),
                            IBCAmountFee = NumberFormat.DecFormat(dataTable.Compute("sum(FeeDue2)", expression), 2),
                            IBCPT = NumberFormat.DecFormat(dataTable.Compute("sum(PTWLIBC)", expression), 2),
                            IBCNetIncome = NumberFormat.DecFormat(dataTable.Compute("sum(netIncomeIBC)", expression), 2),

                            PhuWinTO = NumberFormat.DecFormat(dataTable.Compute("sum(winTOPhum)", expression), 2),
                            PhuPlaceTO = NumberFormat.DecFormat(dataTable.Compute("sum(placeTOPhum)", expression), 2),
                            PhuWPTO = NumberFormat.DecFormat(dataTable.Compute("sum(winplaceTOPhum)", expression), 2),
                            PhuTotal = NumberFormat.DecFormat(dataTable.Compute("sum(totalTOPhum)", expression), 2),
                            PhuDividendWon = NumberFormat.DecFormat(dataTable.Compute("sum(dividendWon)", expression), 2),
                            PhuCommExpense = NumberFormat.DecFormat(dataTable.Compute("sum(commIncomeIBC)", expression), 2),
                            PhuAmountFee = NumberFormat.DecFormat(dataTable.Compute("sum(FeeDue2)", expression), 2),
                            PhuAmountDue = NumberFormat.DecFormat(dataTable.Compute("sum(AmountDue)", expression), 2),

                        });
                        #endregion
                    }
                    #endregion

                    #region THB
                    if (CountTHB == TotalCurrTHB && TotalCurrTHB >= 2)
                    {
                        //reset counter
                        CountTHB = -100;
                        expression = "currency in (" + ArrCurrencySubTotal_THB + ")";

                        #region Origin
                        byCurrencyOrigin.GroupTotals.Add(new RacingDailyWinLossRow()
                        {
                            CurrencyId = currency.ToString(),
                            Currency = CacheUtils.GetExchangeName(currency),

                            AgWinTO = NumberFormat.DecFormat(dataTable.Compute("sum(winTO_Agency_Origin)", expression), 2),
                            AgPlaceTO = NumberFormat.DecFormat(dataTable.Compute("sum(placeTO_Agency_Origin)", expression), 2),
                            AgWPTO = NumberFormat.DecFormat(dataTable.Compute("sum(winplaceTO_Agency_Origin)", expression), 2),
                            AgTotal = NumberFormat.DecFormat(dataTable.Compute("sum(totalTOAgency_Origin)", expression), 2),
                            AgCommIncome = NumberFormat.DecFormat(dataTable.Compute("sum(commIncomeAgency_Origin)", expression), 2),
                            AgPTWL = NumberFormat.DecFormat(dataTable.Compute("sum(PTWLAgency_Origin)", expression), 2),

                            IBCWinTO = NumberFormat.DecFormat(dataTable.Compute("sum(winTO_IBC_Origin)", expression), 2),
                            IBCPlaceTO = NumberFormat.DecFormat(dataTable.Compute("sum(placeTO_IBC_Origin)", expression), 2),
                            IBCWPTO = NumberFormat.DecFormat(dataTable.Compute("sum(winplaceTO_IBC_Origin)", expression), 2),
                            IBCTotal = NumberFormat.DecFormat(dataTable.Compute("sum(totalTOIBC_Origin)", expression), 2),
                            IBCCommIncome = NumberFormat.DecFormat(dataTable.Compute("sum(commIncomeIBC_Origin)", expression), 2),
                            IBCCommExpense = NumberFormat.DecFormat(dataTable.Compute("sum(commIncomeAgency_Origin)", expression), 2),
                            IBCAmountFee = NumberFormat.DecFormat(dataTable.Compute("sum(FeeDue2_Origin)", expression), 2),
                            IBCPT = NumberFormat.DecFormat(dataTable.Compute("sum(PTWLIBC_Origin)", expression), 2),
                            IBCNetIncome = NumberFormat.DecFormat(dataTable.Compute("sum(netIncomeIBC_Origin)", expression), 2),

                            PhuWinTO = NumberFormat.DecFormat(dataTable.Compute("sum(winTOPhum_Origin)", expression), 2),
                            PhuPlaceTO = NumberFormat.DecFormat(dataTable.Compute("sum(placeTOPhum_Origin)", expression), 2),
                            PhuWPTO = NumberFormat.DecFormat(dataTable.Compute("sum(winplaceTOPhum_Origin)", expression), 2),
                            PhuTotal = NumberFormat.DecFormat(dataTable.Compute("sum(totalTOPhum_Origin)", expression), 2),
                            PhuDividendWon = NumberFormat.DecFormat(dataTable.Compute("sum(dividendWon_Origin)", expression), 2),
                            PhuCommExpense = NumberFormat.DecFormat(dataTable.Compute("sum(commIncomeIBC_Origin)", expression), 2),
                            PhuAmountFee = NumberFormat.DecFormat(dataTable.Compute("sum(FeeDue2_Origin)", expression), 2),
                            PhuAmountDue = NumberFormat.DecFormat(dataTable.Compute("sum(AmountDue_Origin)", expression), 2),

                        });
                        #endregion

                        #region Base RM
                        byCurrencyOrigin.GroupTotals.Add(new RacingDailyWinLossRow()
                        {
                            CurrencyId = "2",
                            Currency = "Base (RM)",

                            AgWinTO = NumberFormat.DecFormat(dataTable.Compute("sum(winTO_Agency)", expression), 2),
                            AgPlaceTO = NumberFormat.DecFormat(dataTable.Compute("sum(placeTO_Agency)", expression), 2),
                            AgWPTO = NumberFormat.DecFormat(dataTable.Compute("sum(winplaceTO_Agency)", expression), 2),
                            AgTotal = NumberFormat.DecFormat(dataTable.Compute("sum(totalTOAgency)", expression), 2),
                            AgCommIncome = NumberFormat.DecFormat(dataTable.Compute("sum(commIncomeAgency)", expression), 2),
                            AgPTWL = NumberFormat.DecFormat(dataTable.Compute("sum(PTWLAgency)", expression), 2),

                            IBCWinTO = NumberFormat.DecFormat(dataTable.Compute("sum(winTO_IBC)", expression), 2),
                            IBCPlaceTO = NumberFormat.DecFormat(dataTable.Compute("sum(placeTO_IBC)", expression), 2),
                            IBCWPTO = NumberFormat.DecFormat(dataTable.Compute("sum(winplaceTO_IBC)", expression), 2),
                            IBCTotal = NumberFormat.DecFormat(dataTable.Compute("sum(totalTOIBC)", expression), 2),
                            IBCCommIncome = NumberFormat.DecFormat(dataTable.Compute("sum(commIncomeIBC)", expression), 2),
                            IBCCommExpense = NumberFormat.DecFormat(dataTable.Compute("sum(commIncomeAgency)", expression), 2),
                            IBCAmountFee = NumberFormat.DecFormat(dataTable.Compute("sum(FeeDue2)", expression), 2),
                            IBCPT = NumberFormat.DecFormat(dataTable.Compute("sum(PTWLIBC)", expression), 2),
                            IBCNetIncome = NumberFormat.DecFormat(dataTable.Compute("sum(netIncomeIBC)", expression), 2),

                            PhuWinTO = NumberFormat.DecFormat(dataTable.Compute("sum(winTOPhum)", expression), 2),
                            PhuPlaceTO = NumberFormat.DecFormat(dataTable.Compute("sum(placeTOPhum)", expression), 2),
                            PhuWPTO = NumberFormat.DecFormat(dataTable.Compute("sum(winplaceTOPhum)", expression), 2),
                            PhuTotal = NumberFormat.DecFormat(dataTable.Compute("sum(totalTOPhum)", expression), 2),
                            PhuDividendWon = NumberFormat.DecFormat(dataTable.Compute("sum(dividendWon)", expression), 2),
                            PhuCommExpense = NumberFormat.DecFormat(dataTable.Compute("sum(commIncomeIBC)", expression), 2),
                            PhuAmountFee = NumberFormat.DecFormat(dataTable.Compute("sum(FeeDue2)", expression), 2),
                            PhuAmountDue = NumberFormat.DecFormat(dataTable.Compute("sum(AmountDue)", expression), 2),

                        });
                        #endregion
                    }
                    #endregion

                    #region IN
                    if (CountIN == TotalCurrIN && TotalCurrIN>=2)
                    {
                        //reset counter
                        CountIN = -100;
                        expression = "currency in (" + ArrCurrencySubTotal_IN + ")";

                        #region Origin
                        byCurrencyOrigin.GroupTotals.Add(new RacingDailyWinLossRow()
                        {
                            CurrencyId = currency.ToString(),
                            Currency = CacheUtils.GetExchangeName(currency),

                            AgWinTO = NumberFormat.DecFormat(dataTable.Compute("sum(winTO_Agency_Origin)", expression), 2),
                            AgPlaceTO = NumberFormat.DecFormat(dataTable.Compute("sum(placeTO_Agency_Origin)", expression), 2),
                            AgWPTO = NumberFormat.DecFormat(dataTable.Compute("sum(winplaceTO_Agency_Origin)", expression), 2),
                            AgTotal = NumberFormat.DecFormat(dataTable.Compute("sum(totalTOAgency_Origin)", expression), 2),
                            AgCommIncome = NumberFormat.DecFormat(dataTable.Compute("sum(commIncomeAgency_Origin)", expression), 2),
                            AgPTWL = NumberFormat.DecFormat(dataTable.Compute("sum(PTWLAgency_Origin)", expression), 2),

                            IBCWinTO = NumberFormat.DecFormat(dataTable.Compute("sum(winTO_IBC_Origin)", expression), 2),
                            IBCPlaceTO = NumberFormat.DecFormat(dataTable.Compute("sum(placeTO_IBC_Origin)", expression), 2),
                            IBCWPTO = NumberFormat.DecFormat(dataTable.Compute("sum(winplaceTO_IBC_Origin)", expression), 2),
                            IBCTotal = NumberFormat.DecFormat(dataTable.Compute("sum(totalTOIBC_Origin)", expression), 2),
                            IBCCommIncome = NumberFormat.DecFormat(dataTable.Compute("sum(commIncomeIBC_Origin)", expression), 2),
                            IBCCommExpense = NumberFormat.DecFormat(dataTable.Compute("sum(commIncomeAgency_Origin)", expression), 2),
                            IBCAmountFee = NumberFormat.DecFormat(dataTable.Compute("sum(FeeDue2_Origin)", expression), 2),
                            IBCPT = NumberFormat.DecFormat(dataTable.Compute("sum(PTWLIBC_Origin)", expression), 2),
                            IBCNetIncome = NumberFormat.DecFormat(dataTable.Compute("sum(netIncomeIBC_Origin)", expression), 2),

                            PhuWinTO = NumberFormat.DecFormat(dataTable.Compute("sum(winTOPhum_Origin)", expression), 2),
                            PhuPlaceTO = NumberFormat.DecFormat(dataTable.Compute("sum(placeTOPhum_Origin)", expression), 2),
                            PhuWPTO = NumberFormat.DecFormat(dataTable.Compute("sum(winplaceTOPhum_Origin)", expression), 2),
                            PhuTotal = NumberFormat.DecFormat(dataTable.Compute("sum(totalTOPhum_Origin)", expression), 2),
                            PhuDividendWon = NumberFormat.DecFormat(dataTable.Compute("sum(dividendWon_Origin)", expression), 2),
                            PhuCommExpense = NumberFormat.DecFormat(dataTable.Compute("sum(commIncomeIBC_Origin)", expression), 2),
                            PhuAmountFee = NumberFormat.DecFormat(dataTable.Compute("sum(FeeDue2_Origin)", expression), 2),
                            PhuAmountDue = NumberFormat.DecFormat(dataTable.Compute("sum(AmountDue_Origin)", expression), 2),

                        });
                        #endregion

                        #region Base RM
                        byCurrencyOrigin.GroupTotals.Add(new RacingDailyWinLossRow()
                        {
                            CurrencyId = "2",
                            Currency = "Base (RM)",

                            AgWinTO = NumberFormat.DecFormat(dataTable.Compute("sum(winTO_Agency)", expression), 2),
                            AgPlaceTO = NumberFormat.DecFormat(dataTable.Compute("sum(placeTO_Agency)", expression), 2),
                            AgWPTO = NumberFormat.DecFormat(dataTable.Compute("sum(winplaceTO_Agency)", expression), 2),
                            AgTotal = NumberFormat.DecFormat(dataTable.Compute("sum(totalTOAgency)", expression), 2),
                            AgCommIncome = NumberFormat.DecFormat(dataTable.Compute("sum(commIncomeAgency)", expression), 2),
                            AgPTWL = NumberFormat.DecFormat(dataTable.Compute("sum(PTWLAgency)", expression), 2),

                            IBCWinTO = NumberFormat.DecFormat(dataTable.Compute("sum(winTO_IBC)", expression), 2),
                            IBCPlaceTO = NumberFormat.DecFormat(dataTable.Compute("sum(placeTO_IBC)", expression), 2),
                            IBCWPTO = NumberFormat.DecFormat(dataTable.Compute("sum(winplaceTO_IBC)", expression), 2),
                            IBCTotal = NumberFormat.DecFormat(dataTable.Compute("sum(totalTOIBC)", expression), 2),
                            IBCCommIncome = NumberFormat.DecFormat(dataTable.Compute("sum(commIncomeIBC)", expression), 2),
                            IBCCommExpense = NumberFormat.DecFormat(dataTable.Compute("sum(commIncomeAgency)", expression), 2),
                            IBCAmountFee = NumberFormat.DecFormat(dataTable.Compute("sum(FeeDue2)", expression), 2),
                            IBCPT = NumberFormat.DecFormat(dataTable.Compute("sum(PTWLIBC)", expression), 2),
                            IBCNetIncome = NumberFormat.DecFormat(dataTable.Compute("sum(netIncomeIBC)", expression), 2),

                            PhuWinTO = NumberFormat.DecFormat(dataTable.Compute("sum(winTOPhum)", expression), 2),
                            PhuPlaceTO = NumberFormat.DecFormat(dataTable.Compute("sum(placeTOPhum)", expression), 2),
                            PhuWPTO = NumberFormat.DecFormat(dataTable.Compute("sum(winplaceTOPhum)", expression), 2),
                            PhuTotal = NumberFormat.DecFormat(dataTable.Compute("sum(totalTOPhum)", expression), 2),
                            PhuDividendWon = NumberFormat.DecFormat(dataTable.Compute("sum(dividendWon)", expression), 2),
                            PhuCommExpense = NumberFormat.DecFormat(dataTable.Compute("sum(commIncomeIBC)", expression), 2),
                            PhuAmountFee = NumberFormat.DecFormat(dataTable.Compute("sum(FeeDue2)", expression), 2),
                            PhuAmountDue = NumberFormat.DecFormat(dataTable.Compute("sum(AmountDue)", expression), 2),

                        });
                        #endregion
                    }
                    #endregion

                    #endregion

                    racing.Items.Add(byCurrencyOrigin);
                }

                #region Grand Total Base RM
                racing.Totals.Add(new RacingDailyWinLossRow()
                    {
                        Currency = "Base (RM)",

                        AgWinTO = NumberFormat.DecFormat(byAllCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.AgWinTO))), 2),
                        AgPlaceTO = NumberFormat.DecFormat(byAllCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.AgPlaceTO))), 2),
                        AgWPTO = NumberFormat.DecFormat(byAllCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.AgWPTO))), 2),
                        AgTotal = NumberFormat.DecFormat(byAllCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.AgTotal))), 2),
                        AgCommIncome = NumberFormat.DecFormat(byAllCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.AgCommIncome))), 2),
                        AgPTWL = NumberFormat.DecFormat(byAllCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.AgPTWL))), 2),

                        IBCWinTO = NumberFormat.DecFormat(byAllCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.IBCWinTO))), 2),
                        IBCPlaceTO = NumberFormat.DecFormat(byAllCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.IBCPlaceTO))), 2),
                        IBCWPTO = NumberFormat.DecFormat(byAllCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.IBCWPTO))), 2),
                        IBCTotal = NumberFormat.DecFormat(byAllCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.IBCTotal))), 2),
                        IBCCommIncome = NumberFormat.DecFormat(byAllCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.IBCCommIncome))), 2),
                        IBCCommExpense = NumberFormat.DecFormat(byAllCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.IBCCommExpense))), 2),
                        IBCAmountFee = NumberFormat.DecFormat(byAllCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.IBCAmountFee))), 2),
                        IBCPT = NumberFormat.DecFormat(byAllCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.IBCPT))), 2),
                        IBCNetIncome = NumberFormat.DecFormat(byAllCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.IBCNetIncome))), 2),

                        PhuWinTO = NumberFormat.DecFormat(byAllCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.PhuWinTO))), 2),
                        PhuPlaceTO = NumberFormat.DecFormat(byAllCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.PhuPlaceTO))), 2),
                        PhuWPTO = NumberFormat.DecFormat(byAllCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.PhuWPTO))), 2),
                        PhuTotal = NumberFormat.DecFormat(byAllCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.PhuTotal))), 2),
                        PhuDividendWon = NumberFormat.DecFormat(byAllCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.PhuDividendWon))), 2),
                        PhuCommExpense = NumberFormat.DecFormat(byAllCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.PhuCommExpense))), 2),
                        PhuAmountFee = NumberFormat.DecFormat(byAllCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.PhuAmountFee))), 2),
                        PhuAmountDue = NumberFormat.DecFormat(byAllCurrencyBase.Items.Sum(s => s.Items.Sum(l => CastAide.AsDecimal(l.PhuAmountDue))), 2),
                    });
                #endregion

                SessionExtension.AddSession(HttpContext.Current.Session, _RacingDailyWinLossForToteSessionKey, racing);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return racing;
        }
        #endregion
    }

   
}

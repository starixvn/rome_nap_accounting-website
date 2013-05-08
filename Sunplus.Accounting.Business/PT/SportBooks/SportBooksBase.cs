using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DNAide.Web;

using DNAide.Core;
using IBCUtils;
using System.Text;
using Sunplus.Accounting.Business.Customer;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Web;
using Sunplus.Core.LoggingLib;

using Sunplus.Accounting.Core.Helper;
using Sunplus.Bingo.Core;

/// <summary>
/// Summary description for SportBooksManager
/// </summary>

namespace Sunplus.Accounting.Business.PT.SportBooks
{
    public abstract class SportBooksBase
    {

        public virtual string GetSuperPtSetting(DataRow data, int custId)
        {
            throw new NotImplementedException();
        }
        public virtual string GetMasterPtSetting_AddNew(DataRow data, ref string strHidden)
        {
            throw new NotImplementedException();
        }
        public virtual string GetMasterPtSetting_Edit(DataRow data, ref string strHidden)
        {
            throw new NotImplementedException();
        }
        public virtual string GetAgentPtSetting_AddNew(DataRow data, ref string strHidden)
        {
            throw new NotImplementedException();
        }
        public virtual string GetAgentPtSetting_Edit(DataRow data, bool IsEnabledAutoMaster, ref string strHidden)
        {
            throw new NotImplementedException();
        }
        public virtual string GetMemberPtSetting(DataRow data, bool IsAddNew)
        {
            throw new NotImplementedException();
        }
        public virtual string GetSuperOnMemberPtSetting(DataRow data, ref string strHidden)
        {
            throw new NotImplementedException();
        }
        public virtual string GetMasterOnMemberPtSetting(DataRow data, bool isEnabledAutoMaster, ref string strHidden)
        {
            throw new NotImplementedException();
        }

        public virtual string GetBetSetting(int sporttype, DataRow data, bool IsEditMem)
        {
            StringBuilder html = new StringBuilder();
            try
            {
                string tpl = WebUtils.LoadTemplate("Customer/v2/BetSetting.htm");

                TemplateHandler.Assign(ref tpl, "{sporttype}", sporttype);
                TemplateHandler.Assign(ref tpl, "{minbet-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMinBet"));
                TemplateHandler.Assign(ref tpl, "{maxbet-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMaxBet"));
                TemplateHandler.Assign(ref tpl, "{maxpermatch-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMaxPerMatch"));
                TemplateHandler.Assign(ref tpl, "{bet-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblBetSetting"));

                double minbet = 0, maxbet = 0, maxpermatch = 0;
                if (data != null)
                {
                    if (sporttype ==1)
                    {
                        minbet = CastAide.AsDouble(data["minbet"]);
                        maxbet = CastAide.AsDouble(data["maxbet"]);
                        maxpermatch = CastAide.AsDouble(data["maxpermatch"]);
                    }
                    else
                    {
                        minbet = CastAide.AsDouble(data["minbetv" + sporttype]);
                        maxbet = CastAide.AsDouble(data["maxbetv" + sporttype]);
                        maxpermatch = CastAide.AsDouble(data["maxpermatch" + sporttype]);
                    }
                }

                TemplateHandler.Assign(ref tpl, "{minbet" + sporttype + "-val}", FormatHandler.FormatNumber1(minbet));
                TemplateHandler.Assign(ref tpl, "{maxbet" + sporttype + "-val}", FormatHandler.FormatNumber1(maxbet));
                TemplateHandler.Assign(ref tpl, "{maxpermatch" + sporttype + "-val}", FormatHandler.FormatNumber1(maxpermatch));

                TemplateHandler.Assign(ref tpl, "{mbMin}", "hide");
                TemplateHandler.Assign(ref tpl, "{mbMax}", "hide");
                TemplateHandler.Assign(ref tpl, "{mpmMin}", "hide");
                TemplateHandler.Assign(ref tpl, "{mpmMax}", "hide");

                html.Append(tpl);
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            return html.ToString();
        }

        public virtual void GetLimitMinMaxBet(DataRow dr, ref string tpl, string sportTypeId)
        {
            try
            {
                string minbetMin;
                string maxbetMax;
                string maxpermatchMin;
                string maxpermatchMax;

                if (dr != null)
                {
                    minbetMin = FormatHandler.FormatNumber1(dr["MinBetv" + sportTypeId + "_Max"]);
                    maxbetMax = FormatHandler.FormatNumber1(dr["MaxBetv" + sportTypeId + "_Min"]);
                    maxpermatchMin = FormatHandler.FormatNumber1(dr["MaxPerMatchv" + sportTypeId + "_Min"]);
                    maxpermatchMax = FormatHandler.FormatNumber1(dr["MaxPerMatchv" + sportTypeId + "_Max"]);
                }
                else
                {
                    minbetMin = "0";
                    maxbetMax = "0";
                    maxpermatchMin = "0";
                    maxpermatchMax = "0";
                }
                TemplateHandler.Assign(ref tpl, "{minbet-min-" + sportTypeId + "}", minbetMin);
                TemplateHandler.Assign(ref tpl, "{maxbet-max-" + sportTypeId + "}", maxbetMax);
                TemplateHandler.Assign(ref tpl, "{maxpermatch-min-" + sportTypeId + "}", maxpermatchMin);
                TemplateHandler.Assign(ref tpl, "{maxpermatch-max-" + sportTypeId + "}", maxpermatchMax);
                if (minbetMin == "0")
                    TemplateHandler.Assign(ref tpl, "{mbMin}", "hidden");
                else
                    TemplateHandler.Assign(ref tpl, "{mbMin}", "minmaxbet");

                if (maxbetMax == "0")
                    TemplateHandler.Assign(ref tpl, "{mbMax}", "hidden");
                else
                    TemplateHandler.Assign(ref tpl, "{mbMax}", "minmaxbet");

                if (maxpermatchMin == "0")
                    TemplateHandler.Assign(ref tpl, "{mpmMin}", "hidden");
                else
                    TemplateHandler.Assign(ref tpl, "{mpmMin}", "minmaxbet");

                if (maxpermatchMax == "0")
                    TemplateHandler.Assign(ref tpl, "{mpmMax}", "hidden");
                else
                    TemplateHandler.Assign(ref tpl, "{mpmMax}", "minmaxbet");
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
        }

        public virtual string GetMinPTSetting(DataRow dr, ref string strHidden, string prefix)
        {
            throw new NotImplementedException();
        }

        public virtual void GetNumberCom(DataRow dr, int RoleId, bool IsAddNew, ref string strAccInfo)
        {
            StringBuilder html = new StringBuilder();
            try
            {
                if (BingoSettings.NumberGame.Enabled)                
                {
                    double max = 0, current = 0;
                    
                    switch (RoleId)
                    { 
                        case 4:
                            if (IsAddNew)
                            {
                                current = 0;
                                max = BingoSettings.MaxCommission;
                            }
                            else
                            {
                                max = CastAide.AsDouble(dr["discountnumber_max"]);
                                current = CastAide.AsDouble(dr["discountnumber"]);
                            }
                            break;
                        case 3:
                        case 2:
                            if (IsAddNew)
                                current = max = CastAide.AsDouble(dr["discountnumber"]);
                            else
                            {
                                max = CastAide.AsDouble(dr["discountnumber_max"]);
                                current = CastAide.AsDouble(dr["discountnumber"]);        
                            }
                            break;
                        case 1:
                            if (IsAddNew)
                                current = max = CastAide.AsDouble(dr["discountnumber"]);
                            else
                            {
                                max = CastAide.AsDouble(dr["discountnumber"]);
                                current = CastAide.AsDouble(dr["playerdiscountnumber"]);                             
                            }
                            TemplateHandler.Assign(ref strAccInfo, "DiscountNB", max);
                            break;
                    }
                    
                    html.Append(CustomerUtils.GetListGroup(max, current));
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref strAccInfo, "game-comm");
                }

                TemplateHandler.Assign(ref strAccInfo, "listGameCommValue", html.ToString());
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            
        }

        public virtual void GetMaxPerBall(DataRow dr, int RoleId, bool IsAddNew, ref string strAccInfo)
        {
            //IBCUtils.FormatHandler.FormatNumber1(minbet)
            try
            {                
                if (BingoSettings.NumberGame.Enabled)
                {
                    string tpl = WebUtils.LoadTemplate("Customer/v2/NumberGame/MaxPerBall.htm");
                    TemplateHandler.Assign(ref tpl, "RoleId", RoleId);
                    switch (RoleId)
                    {
                        case 4:
                            if (IsAddNew)
                            {
                                TemplateHandler.Assign(ref tpl, "txtMaxPerBall-val", 0);
                                TemplateHandler.Assign(ref tpl, "mbMin", "hide");
                            }
                            else
                            {
                                TemplateHandler.Assign(ref tpl, "txtMaxPerBall-val", FormatHandler.FormatNumber1(dr["maxperball161"]));
                                TemplateHandler.Assign(ref tpl, "mbMin", "hide");
                            }
                            break;
                        case 3:
                        case 2:
                        case 1:
                            //decimal MaxBet = 0, MaxPerMatch = 0;
                            //MaxBet = CastAide.AsDecimal(dr["maxbetv161_Min"]);
                            //MaxPerMatch = CastAide.AsDecimal(dr["MaxPerMatchv161_Min"]);
                            var maxPerBall = CastAide.AsDecimal(dr["MaxPerBallv161_Max"]);

                            if (IsAddNew)
                            {
                                TemplateHandler.Assign(ref tpl, "txtMaxPerBall-val", FormatHandler.FormatNumber1(dr["MaxPerBallv161_Max"]));
                                TemplateHandler.Assign(ref tpl, "maxbet-161", FormatHandler.FormatNumber1(dr["maxbetv161_Min"]));
                                TemplateHandler.Assign(ref tpl, "maxpermatch-161", FormatHandler.FormatNumber1(maxPerBall));
                                
                                //if(MaxBet == 0 && MaxPerMatch == 0)
                                //    TemplateHandler.Assign(ref tpl, "mbMin", "hide");
                                //else
                                //    TemplateHandler.Assign(ref tpl, "mbMin", "");

                                TemplateHandler.Assign(ref tpl, "mbMin", maxPerBall > 0 ? "" : "hide");
                            }
                            else
                            {

                                TemplateHandler.Assign(ref tpl, "txtMaxPerBall-val", FormatHandler.FormatNumber1(dr["maxperballv161"]));
                                TemplateHandler.Assign(ref tpl, "maxbet-161", FormatHandler.FormatNumber1(dr["maxbetv161"]));
                                TemplateHandler.Assign(ref tpl, "maxpermatch-161", FormatHandler.FormatNumber1(maxPerBall));

                                TemplateHandler.Assign(ref tpl, "mbMin", maxPerBall > 0 ? "" : "hide");
                            }
                            break;
                            
                    }
                    TemplateHandler.Assign(ref strAccInfo, "MaxPerBall", tpl);
                }
                else
                {
                    TemplateHandler.Assign(ref strAccInfo, "MaxPerBall", "");
                }
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
        }
        
    }

    

 
    


}

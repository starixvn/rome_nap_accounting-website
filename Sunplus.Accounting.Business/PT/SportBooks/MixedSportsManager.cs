using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web;
using DNAide.Core;
using DNAide.Web;
using IBCUtils;
using Sunplus.Accounting.Business.Customer;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Web;

namespace Sunplus.Accounting.Business.PT.SportBooks
{
    public class MixedSportsManager : SportBooksBase
    {
        #region varialbes

        private bool _isUpdated = false;
        private const int SportType = Constants.Sport.OtherSports;
        private static readonly int BetType = Constants.BetType.MixParlay;
        private const string Disabled = "disabled='disabled'";
        private const string Check = "checked='checked'";
        private const string BetTypeName = "MixParlay";

        #endregion varialbes

        public MixedSportsManager()
        {
        }

        #region private methods

        private static string GetPt(string pt)
        {
            return pt ?? "0";
        }

        #endregion private methods

        #region Get Header

        //this function for Edit PT
        private static string GetGameHeader()
        {
            var tpl = WebUtils.LoadTemplate("Customer/v2/MixedSports/MixedSportsPTHeader.htm");
            var html = new StringBuilder();
            html.Append(tpl);
            return html.ToString();
        }

        //this function for Edit Min-Max PT, Edit Super/Master pt on member
        private static string GetPTHeaderForEdit()
        {
            var tpl = WebUtils.LoadTemplate("Customer/v2/MixedSports/EditPTHeader.htm");
            TemplateHandler.Assign(ref tpl, "{mixparlay}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "MixParlay"));
            var html = new StringBuilder();
            html.Append(tpl);

            return html.ToString();
        }

        #endregion Get Header

        #region GetBetSetting

        public override string GetBetSetting(int sporttype, DataRow data, bool isEditMem)
        {
            #region private variables

            var html = new StringBuilder();
            var tpl = WebUtils.LoadTemplate("Customer/v2/BetSetting.htm");
            var min = "0";
            var max = "0";
            var maxpermatch = "0";
            var msSportType = string.Format("{0}_{1}", sporttype, BetType);

            #endregion private variables

            try
            {
                #region assign header

                TemplateHandler.Assign(ref tpl, "sporttype", msSportType);
                TemplateHandler.Assign(ref tpl, "{minbet-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMinBet"));
                TemplateHandler.Assign(ref tpl, "{maxbet-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMaxBet"));
                TemplateHandler.Assign(ref tpl, "{maxpermatch-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMaxPerMatch"));
                TemplateHandler.Assign(ref tpl, "{bet-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblBetSetting"));

                #endregion assign header

                #region get values

                if (data != null)
                {
                    if (_isUpdated)
                    {
                        min = DataFormat.FormatNumber1(data["minbetv" + sporttype + "mp"]);
                        max = DataFormat.FormatNumber1(data["maxbetv" + sporttype + "mp"]);
                        maxpermatch = (data.Table.Columns.Contains("maxpermatchv" + sporttype + "mp") ?
                            DataFormat.FormatNumber1(data["maxpermatchv" + sporttype + "mp"]) :
                            DataFormat.FormatNumber1(data["maxpermatch" + sporttype + "mp"]));
                    }
                    else
                    {
                        min = DataFormat.FormatNumber1(data["minbetv" + sporttype + "mp_max"]);
                        max = DataFormat.FormatNumber1(data["maxbetv" + sporttype + "mp_min"]);
                        maxpermatch = DataFormat.FormatNumber1(data["maxpermatchv" + sporttype + "mp_max"]);
                    }
                }
                GetLimitMinMaxBet(data, ref tpl, sporttype.ToString());

                #endregion get values
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            finally
            {
                TemplateHandler.Assign(ref tpl, "minbet" + msSportType + "-val", min);
                TemplateHandler.Assign(ref tpl, "maxbet" + msSportType + "-val", max);
                TemplateHandler.Assign(ref tpl, "maxpermatch" + msSportType + "-val", maxpermatch);
                html.Append(tpl);
            }
            return html.ToString();
        }

        public override void GetLimitMinMaxBet(DataRow dr, ref string tpl, string sportTypeId)
        {
            #region private variables

            var minbetMin = "0";
            var maxbetMax = "0";
            var maxpermatchMin = "0";
            var maxpermatchMax = "0";
            var msSportType = string.Format("{0}_{1}", SportType, BetType);

            #endregion private variables

            try
            {
                if (dr != null)
                {
                    minbetMin = dr.Table.Columns.Contains("MinBetv" + sportTypeId + "mp_Max") ? DataFormat.FormatNumber1(dr["MinBetv" + sportTypeId + "mp_Max"]) : "0";
                    maxbetMax = dr.Table.Columns.Contains("MaxBetv" + sportTypeId + "mp_Min") ? DataFormat.FormatNumber1(dr["MaxBetv" + sportTypeId + "mp_Min"]) : "0";
                    maxpermatchMin = dr.Table.Columns.Contains("MaxPerMatchv" + sportTypeId + "mp_Min") ? DataFormat.FormatNumber1(dr["MaxPerMatchv" + sportTypeId + "mp_Min"]) : "0";
                    maxpermatchMax = dr.Table.Columns.Contains("MaxPerMatchv" + sportTypeId + "mp_Max") ? DataFormat.FormatNumber1(dr["MaxPerMatchv" + sportTypeId + "mp_Max"]) : "0";
                }
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            finally
            {
                TemplateHandler.Assign(ref tpl, "{mbMin}", minbetMin == "0" ? "hidden" : "minmaxbet");
                TemplateHandler.Assign(ref tpl, "{mbMax}", maxbetMax == "0" ? "hidden" : "minmaxbet");
                TemplateHandler.Assign(ref tpl, "{mpmMin}", maxpermatchMin == "0" ? "hidden" : "minmaxbet");
                TemplateHandler.Assign(ref tpl, "{mpmMax}", maxpermatchMax == "0" ? "hidden" : "minmaxbet");

                TemplateHandler.Assign(ref tpl, "{minbet-min-" + msSportType + "}", minbetMin);
                TemplateHandler.Assign(ref tpl, "{maxbet-max-" + msSportType + "}", maxbetMax);
                TemplateHandler.Assign(ref tpl, "{maxpermatch-min-" + msSportType + "}", maxpermatchMin);
                TemplateHandler.Assign(ref tpl, "{maxpermatch-max-" + msSportType + "}", maxpermatchMax);
            }
        }

        #endregion GetBetSetting

        #region Super

        public override string GetSuperPtSetting(DataRow data, int custId)
        {
            #region private varialbes

            var html = new StringBuilder();
            var tplGameHeader = GetGameHeader();
            _isUpdated = true;
            var tplBetSetting = GetBetSetting(SportType, data, false);
            var tpl = WebUtils.LoadTemplate("Customer/v2/MixedSports/SuperPTRow.htm");

            double sMin = 0, sMax = 0.9, sB = 0.9;
            double mMin = 0, mMax = 0.9, mB = 0.9;
            double aMin = 0, aMax = 0.9, aB = 0.9;

            #endregion private varialbes

            try
            {
                #region assign info for header & betsetting

                TemplateHandler.Assign(ref tpl, "{ptheader}", tplGameHeader);
                TemplateHandler.Assign(ref tpl, "{betsetting}", tplBetSetting);

                TemplateHandler.Assign(ref tpl, "{super-position}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "SuperMaxPosition"));
                TemplateHandler.Assign(ref tpl, "{master-position}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "MasterMaxPosition"));
                TemplateHandler.Assign(ref tpl, "{agent-position}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "AgentMaxPosition"));
                TemplateHandler.Assign(ref tpl, "{colTitleSpan}", string.Empty);
                TemplateHandler.Assign(ref tpl, "{tblWidth}", "width:550px");

                #endregion assign info for header & betsetting

                if (data != null && custId > 0)
                {
                    //Super
                    sMin = CastAide.AsDouble(data["v" + SportType + "spositiontaking_" + BetType + "_min"]);
                    sMax = CastAide.AsDouble(data["v" + SportType + "spositiontaking_" + BetType + "_max"]);
                    sB = CastAide.AsDouble(data["v" + SportType + "spositiontaking_" + BetType]);
                    //Master
                    mB = CastAide.AsDouble(data["v" + SportType + "mpositiontaking_" + BetType]);
                    mMax = sB;
                    //Agent
                    aB = CastAide.AsDouble(data["v" + SportType + "apositiontaking_" + BetType]);
                    aMax = mB;
                }
                //set position taken value for Super
                TemplateHandler.Assign(ref tpl, "{listSMixParlayValue}", CustomerUtils.GetListPositionTaking(sMin, sMax, sB));
                //set position taken value for Master
                TemplateHandler.Assign(ref tpl, "{listMMixParlayValue}", CustomerUtils.GetListPositionTaking(mMin, mMax, mB));
                //set position taken value for Agent
                TemplateHandler.Assign(ref tpl, "{listAMixParlayValue}", CustomerUtils.GetListPositionTaking(aMin, aMax, aB));
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            finally
            {
                html.Append(tpl);
            }

            return html.ToString();
        }

        public void AddParamSuperPT(ref Hashtable param, HttpContext context)
        {
            #region private varialbes

            var sPT = "0";
            var mPT = "0";
            var aPT = "0";
            var minbet = "0";
            var maxbet = "0";
            var maxpermatch = "0";

            #endregion private varialbes

            try
            {
                #region get values

                if (!string.IsNullOrEmpty(context.Request["chkCopyAll"])) // copy all from soccer handicap
                {
                    minbet = context.Request["txtMinbet1"];
                    maxbet = context.Request["txtMaxbet1"];
                    maxpermatch = context.Request["txtMaxPerMatch1"];

                    sPT = context.Request["listSHdpD1"];//super
                    mPT = context.Request["listMHdpD1"];//master
                    aPT = context.Request["listAHdpD1"];//agent
                }
                else if (!string.IsNullOrEmpty(context.Request["chkCopyBasket"]))//copy from basketball
                {
                    minbet = context.Request["txtMinbet2"];
                    maxbet = context.Request["txtMaxbet2"];
                    maxpermatch = context.Request["txtMaxPerMatch2"];

                    sPT = context.Request["listSHdpB2"];//super
                    mPT = context.Request["listMHdpB2"];//master
                    aPT = context.Request["listAHdpB2"];//agent
                }
                else
                {
                    var msSportType = string.Format("{0}_{1}", Constants.Sport.OtherSports, Constants.BetType.MixParlay);

                    minbet = context.Request["txtMinbet" + msSportType];
                    maxbet = context.Request["txtMaxbet" + msSportType];
                    maxpermatch = context.Request["txtMaxPerMatch" + msSportType];

                    sPT = context.Request["listSMixParlay"];//super
                    mPT = context.Request["listMMixParlay"];//master
                    aPT = context.Request["listAMixParlay"];//agent
                }

                #endregion get values
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            finally
            {
                #region assign data

                param.Add(string.Format("minbetv{0}mp", SportType), minbet);//money
                param.Add(string.Format("maxbetv{0}mp", SportType), maxbet);//money
                param.Add(string.Format("maxpermatchv{0}mp", SportType), maxpermatch);//money

                param.Add("v" + SportType + "spositiontaking_" + BetType, sPT);//smallmoney
                param.Add("v" + SportType + "mpositiontaking_" + BetType, mPT);//smallmoney
                param.Add("v" + SportType + "apositiontaking_" + BetType, aPT);//smallmoney

                #endregion assign data
            }
        }

        #endregion Super

        #region Master

        #region Master Add new

        public override string GetMasterPtSetting_AddNew(DataRow data, ref string strHidden)
        {
            #region private variables

            _isUpdated = false;

            var html = new StringBuilder();
            var tpl = WebUtils.LoadTemplate("Customer/v2/MixedSports/MasterPTRow.htm");

            var tplGameHeader = GetGameHeader();
            var tplBetSetting = GetBetSetting(SportType, data, false);

            double sMin = 0, sMax = 0;
            double mMin = 0, mMax = 0, mMax2 = 0, msMin = 0, msMax = 0;
            double sAuto = 0;

            var hiddenHash = new Hashtable();

            #endregion private variables

            try
            {
                #region assign header and betsetting

                TemplateHandler.Assign(ref tpl, "{ptheader}", tplGameHeader);
                TemplateHandler.Assign(ref tpl, "{betsetting}", tplBetSetting);
                //
                TemplateHandler.Assign(ref tpl, "{super-position}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblSuperPresetPosition"));
                TemplateHandler.Assign(ref tpl, "{master-position}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMasterPresetPosition"));
                TemplateHandler.Assign(ref tpl, "{auto-super-position}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAutoSuperPresetPosition"));
                TemplateHandler.Assign(ref tpl, "{tblWidth}", "width:550px");
                TemplateHandler.Assign(ref tpl, "{colTitleSpan}", string.Empty);

                #endregion assign header and betsetting

                #region assign data

                if (data != null)
                {
                    //super
                    sMax = CastAide.AsDouble(data["v" + SportType + "spositiontaking_" + BetType]);

                    //master
                    msMin = CastAide.AsDouble(data["v" + SportType + "mspositiontaking_" + BetType + "_min"]);
                    msMax = CastAide.AsDouble(data["v" + SportType + "mspositiontaking_" + BetType + "_max"]);
                    mMax2 = CastAide.AsDouble(data["v" + SportType + "mpositiontaking_" + BetType + "_max"]);
                    mMax = msMax - sMax;

                    //super auto
                    sAuto = CastAide.AsDouble(data["auto_v" + SportType + "spositiontaking_" + BetType]);
                }

                //add hidden values
                hiddenHash.Add("maxMS" + BetTypeName, msMax);
                hiddenHash.Add("minMS" + BetTypeName, msMin);
                hiddenHash.Add("maxM" + BetTypeName, mMax2);
                //set position taken value for Super
                TemplateHandler.Assign(ref tpl, "{listSMixParlayValue}", CustomerUtils.GetListPositionTaking(sMin, sMax, sMax));
                //set position taken value for Master
                TemplateHandler.Assign(ref tpl, "{listMMixParlayValue}", CustomerUtils.GetListPositionTaking(mMin, mMax, mMax));
                //set position taken value for Agent
                TemplateHandler.Assign(ref tpl, "{listSAutoMixParlayValue}", CustomerUtils.GetListPositionTaking(0, sAuto, 0));
                //auto checked
                TemplateHandler.Assign(ref tpl, "{statuslstSAutoMixParlay}", Disabled);

                strHidden = CustomerUtils.AssignMultipleHidden(hiddenHash);

                #endregion assign data
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            finally
            {
                html.Append(tpl);
            }

            return html.ToString();
        }

        #endregion Master Add new

        #region Master Edit

        public override string GetMasterPtSetting_Edit(DataRow data, ref string strHidden)
        {
            #region private variables

            _isUpdated = true;
            var html = new StringBuilder();
            var tpl = WebUtils.LoadTemplate("Customer/v2/MixedSports/MasterPTRow.htm");

            var tplGameHeader = GetGameHeader();
            var tplBetSetting = GetBetSetting(SportType, data, false);

            double sMin = 0, sMax = 0, s = 0;
            double mMin = 0, mMax = 0, m = 0, mMax2 = 0, msMin = 0, msMax = 0;
            double sAuto = 0, sAutoMax = 0;
            var isAuto = false;

            var hiddenHash = new Hashtable();

            #endregion private variables

            try
            {
                #region assign info for header & betsetting

                TemplateHandler.Assign(ref tpl, "{ptheader}", tplGameHeader);
                TemplateHandler.Assign(ref tpl, "{betsetting}", tplBetSetting);

                TemplateHandler.Assign(ref tpl, "{super-position}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblSuperPresetPosition"));
                TemplateHandler.Assign(ref tpl, "{master-position}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMasterPresetPosition"));
                TemplateHandler.Assign(ref tpl, "{auto-super-position}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAutoSuperPresetPosition"));
                TemplateHandler.Assign(ref tpl, "{tblWidth}", "width:700px");
                TemplateHandler.Assign(ref tpl, "{colTitleSpan}", string.Empty);

                #endregion assign info for header & betsetting

                #region assign data

                if (data != null)
                {
                    isAuto = CastAide.AsBoolean(data["isauto_v" + SportType + "spositiontaking_" + BetType]);
                    //Master preset posiotion
                    mMax2 = CastAide.AsDouble(data["v" + SportType + "mpositiontaking_" + BetType + "_max"]);
                    msMax = CastAide.AsDouble(data["v" + SportType + "mspositiontaking_" + BetType + "_max"]);
                    msMin = CastAide.AsDouble(data["v" + SportType + "mspositiontaking_" + BetType + "_min"]);
                    mMin = CastAide.AsDouble(data["v" + SportType + "mpositiontaking_" + BetType + "_min"]);
                    m = CastAide.AsDouble(data["v" + SportType + "mpositiontaking_" + BetType]);

                    s = CastAide.AsDouble(data["v" + SportType + "spositiontaking_" + BetType]);

                    mMax = isAuto ? msMax : msMax - s;
                    mMax = mMax > mMax2 ? mMax2 : mMax;
                    //Super preset position
                    sMax = CastAide.AsDouble(data["v" + SportType + "spositiontaking_" + BetType + "_max"]);
                    sMax = sMax - mMin;
                    //Auto Super preset
                    sAuto = CastAide.AsDouble(data["auto_v" + SportType + "spositiontaking_" + BetType]);
                    sAutoMax = CastAide.AsDouble(data["auto_v" + SportType + "spositiontaking_" + BetType + "_max"]);
                    //------------
                    //sHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1_max"))
                    //------------
                }

                if (isAuto)
                {
                    TemplateHandler.Assign(ref tpl, "statuslstS" + BetTypeName, Disabled);
                    TemplateHandler.Assign(ref tpl, "statuschkSAuto" + BetTypeName, Check);
                    TemplateHandler.Assign(ref tpl, "statuslstSAuto" + BetTypeName, string.Empty);
                }
                else
                {
                    TemplateHandler.Assign(ref tpl, "statuslstS" + BetTypeName, string.Empty);
                    TemplateHandler.Assign(ref tpl, "statuschkSAuto" + BetTypeName, string.Empty);
                    TemplateHandler.Assign(ref tpl, "statuslstSAuto" + BetTypeName, Disabled);
                }

                //add hidden values
                hiddenHash.Add("maxMS" + BetTypeName, msMax);
                hiddenHash.Add("minMS" + BetTypeName, msMin);
                hiddenHash.Add("maxM" + BetTypeName, mMax2);
                hiddenHash.Add("isAuto" + BetTypeName, isAuto);

                //set position taken value for Super
                TemplateHandler.Assign(ref tpl, "{listSMixParlayValue}", CustomerUtils.GetListPositionTaking(sMin, sMax, s));
                //set position taken value for Master
                TemplateHandler.Assign(ref tpl, "{listMMixParlayValue}", CustomerUtils.GetListPositionTaking(mMin, mMax, m));
                //set position taken value for Agent
                TemplateHandler.Assign(ref tpl, "{listSAutoMixParlayValue}", CustomerUtils.GetListPositionTaking(0, sAutoMax, sAuto));

                strHidden = CustomerUtils.AssignMultipleHidden(hiddenHash);

                #endregion assign data
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            finally { html.Append(tpl); }

            return html.ToString();
        }

        #endregion Master Edit

        public void AddParamMasterPT(ref Hashtable param, HttpContext context)
        {
            #region private varialbes

            var minbet = "0";
            var maxbet = "0";
            var maxpermatch = "0";

            var sPresetPosition = "0";
            var mPresetPosition = "0";
            var sAutoPrestPosition = "0";
            var isAuto = 0;

            #endregion private varialbes

            try
            {
                #region get data

                if (string.IsNullOrEmpty(context.Request["chkUpdPT" + SportType]))
                {
                    param.Add(string.Format("updatepositiontakingv{0}mp", SportType), 1);

                    if (!string.IsNullOrEmpty(context.Request["chkCopyAll"])) // copy all from soccer handicap
                    {
                        minbet = context.Request["txtMinbet1"];
                        maxbet = context.Request["txtMaxbet1"];
                        maxpermatch = context.Request["txtMaxPerMatch1"];

                        mPresetPosition = context.Request["listM1HdpD"];//master preset
                        isAuto = string.IsNullOrEmpty(context.Request["chkAuto1HdpD"]) ? 0 : 1;
                        sPresetPosition = isAuto == 0 ? context.Request["listS1HdpD"] : "0";//super preset
                        sAutoPrestPosition = isAuto == 0 ? "0" : context.Request["autoS1HdpD"];//super auto preset
                    }
                    else if (!string.IsNullOrEmpty(context.Request["chkCopyBasket"]))//copy from basketball
                    {
                        minbet = context.Request["txtMinbet2"];
                        maxbet = context.Request["txtMaxbet2"];
                        maxpermatch = context.Request["txtMaxPerMatch2"];

                        mPresetPosition = context.Request["listM2Hdp"];//master preset
                        isAuto = string.IsNullOrEmpty(context.Request["chkAuto2Hdp"]) ? 0 : 1;
                        sPresetPosition = isAuto == 0 ? context.Request["listS2Hdp"] : "0";//super preset
                        sAutoPrestPosition = isAuto == 0 ? "0" : context.Request["autoS2Hdp"]; //super auto preset
                    }
                    else
                    {
                        minbet = context.Request["txtMinbet99_9"];
                        maxbet = context.Request["txtMaxbet99_9"];
                        maxpermatch = context.Request["txtMaxPerMatch99_9"];

                        mPresetPosition = context.Request["listMMixParlay"];//master preset
                        isAuto = string.IsNullOrEmpty(context.Request["chkMixParlay"]) ? 0 : 1;
                        sPresetPosition = isAuto == 0 ? context.Request["listSMixParlay"] : "0";//super preset
                        sAutoPrestPosition = isAuto == 0 ? "0" : context.Request["autoSMixParlay"];//super auto preset
                    }
                }
                else
                {
                    param.Add(string.Format("updatepositiontakingv{0}mp", SportType), 0);
                }

                #endregion get data
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            finally
            {
                #region assign data for param

                param.Add(string.Format("updateminmaxbetv{0}mp", SportType),
                          string.IsNullOrEmpty(context.Request["chkUpdInfo" + SportType]) ? 1 : 0);

                param.Add(string.Format("minbetv{0}mp", SportType), minbet);//money
                param.Add(string.Format("maxbetv{0}mp", SportType), maxbet);//money
                param.Add(string.Format("maxpermatchv{0}mp", SportType), maxpermatch);//money

                param.Add(string.Format("v{0}spositiontaking_{1}", SportType, BetType), sPresetPosition);
                param.Add(string.Format("v{0}mpositiontaking_{1}", SportType, BetType), mPresetPosition);
                param.Add(string.Format("auto_v{0}spositiontaking_{1}", SportType, BetType), sAutoPrestPosition);
                param.Add(string.Format("isauto_v{0}spositiontaking_{1}", SportType, BetType), isAuto);

                #endregion assign data for param
            }
        }

        #endregion Master

        #region Agent

        #region Agent Add New

        public override string GetAgentPtSetting_AddNew(DataRow data, ref string strHidden)
        {
            #region private varialbes

            _isUpdated = false;
            var html = new StringBuilder();
            var tpl = WebUtils.LoadTemplate("Customer/v2/MixedSports/AgentPTRow.htm");

            var tplGameHeader = GetGameHeader();
            var tplBetSetting = GetBetSetting(SportType, data, false);

            double aMin = 0, aMax = 0, a = 0, aMax2 = 0, maMin = 0;
            double mMin = 0, mMax = 0, m = 0;

            var hiddenHash = new Hashtable();

            #endregion private varialbes

            try
            {
                #region assign header and betsetting

                TemplateHandler.Assign(ref tpl, "{ptheader}", tplGameHeader);
                TemplateHandler.Assign(ref tpl, "{betsetting}", tplBetSetting);
                //
                TemplateHandler.Assign(ref tpl, "{agent-position}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAgentPosition"));
                TemplateHandler.Assign(ref tpl, "{master-position}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMasterPresetPosition"));
                TemplateHandler.Assign(ref tpl, "{auto-master-position}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAutoMasterResestPosition"));
                //
                TemplateHandler.Assign(ref tpl, "{tblWidth}", "width:700px");
                TemplateHandler.Assign(ref tpl, "{colTitleSpan}", string.Empty);

                #endregion assign header and betsetting

                if (data != null)
                {
                    //master postion
                    mMax = CastAide.AsDouble(data["v" + SportType + "mpositiontaking_" + BetType]);
                    m = mMax;
                    maMin = CastAide.AsDouble(data["v" + SportType + "mpositiontaking_" + BetType + "_min"]);
                    //agent preset position
                    //aMin = 0;
                    aMax2 = CastAide.AsDouble(data["v" + SportType + "apositiontaking_" + BetType + "_max"]);
                    aMax = mMax - m > aMax2 ? aMax2 : mMax - m;
                    a = aMax;
                }

                #region assign data and setting properties of Super/Master/Agent

                //add hidden values
                hiddenHash.Add("minM" + BetTypeName, maMin);
                hiddenHash.Add("maxA" + BetTypeName, ((mMax > aMax2) ? aMax2 : mMax));
                hiddenHash.Add("maxM" + BetTypeName, mMax);

                TemplateHandler.Assign(ref tpl, "{listAMixParlayValue}", CustomerUtils.GetListPositionTaking(aMin, aMax, a));
                TemplateHandler.Assign(ref tpl, "{listMMixParlayValue}", CustomerUtils.GetListPositionTaking(mMin, mMax, m));
                TemplateHandler.Assign(ref tpl, "{listSAutoMixParlayValue}", CustomerUtils.GetListPositionTaking(0, mMax, 0));
                TemplateHandler.Assign(ref tpl, "{statuslstSMixParlay}", string.Empty);
                TemplateHandler.Assign(ref tpl, "{statuschkMixParlay}", string.Empty);
                TemplateHandler.Assign(ref tpl, "{statuslstSAutoMixParlay}", "disabled='disabled'");

                strHidden = CustomerUtils.AssignMultipleHidden(hiddenHash);

                #endregion assign data and setting properties of Super/Master/Agent
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            finally
            {
                html.Append(tpl);
            }
            return html.ToString();
        }

        #endregion Agent Add New

        #region Agent Edit

        public override string GetAgentPtSetting_Edit(DataRow data, bool isEnabledAutoMaster, ref string strHidden)
        {
            #region private varialbes

            _isUpdated = true;
            var html = new StringBuilder();
            var tpl = WebUtils.LoadTemplate("Customer/v2/MixedSports/AgentPTRow.htm");

            var tplGameHeader = GetGameHeader();
            var tplBetSetting = GetBetSetting(SportType, data, false);

            double aMin = 0, aMax = 0, a = 0, aMax2 = 0;
            double mMin = 0, mMax = 0, m = 0, maMin = 0, maMax = 0, mAuto = 0, mAutoMax = 0;
            var isAuto = false;

            var hiddenHash = new Hashtable();

            #endregion private varialbes

            try
            {
                #region assign header and betsetting

                TemplateHandler.Assign(ref tpl, "{ptheader}", tplGameHeader);
                TemplateHandler.Assign(ref tpl, "{betsetting}", tplBetSetting);
                //
                TemplateHandler.Assign(ref tpl, "{agent-position}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAgentPosition"));
                TemplateHandler.Assign(ref tpl, "{master-position}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMasterPresetPosition"));
                TemplateHandler.Assign(ref tpl, "{auto-master-position}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAutoMasterResestPosition"));
                //
                TemplateHandler.Assign(ref tpl, "{tblWidth}", "width:700px");
                TemplateHandler.Assign(ref tpl, "{colTitleSpan}", string.Empty);

                #endregion assign header and betsetting

                #region get data

                if (data != null)
                {
                    //Master preset position
                    m = CastAide.AsDouble(data.Table.Columns.Contains("v" + SportType + "mpositiontaking_" + BetType) ?
                        data["v" + SportType + "mpositiontaking_" + BetType] : 0);
                    maMin = CastAide.AsDouble(data.Table.Columns.Contains("min_v" + SportType + "mapositiontaking_" + BetType) ?
                        data["min_v" + SportType + "mapositiontaking_" + BetType] : 0);
                    maMax = CastAide.AsDouble(data.Table.Columns.Contains("max_v" + SportType + "mapositiontaking_" + BetType) ?
                        data["max_v" + SportType + "mapositiontaking_" + BetType] : 0);
                    //Agent position
                    a = CastAide.AsDouble(data.Table.Columns.Contains("v" + SportType + "apositiontaking_" + BetType) ?
                        data["v" + SportType + "apositiontaking_" + BetType] : 0);
                    aMax2 = CastAide.AsDouble(data.Table.Columns.Contains("v" + SportType + "apositiontaking_" + BetType + "_max") ?
                        data["v" + SportType + "apositiontaking_" + BetType + "_max"] : 0);
                    aMin = CastAide.AsDouble(data.Table.Columns.Contains("v" + SportType + "apositiontaking_" + BetType + "_min") ?
                        data["v" + SportType + "apositiontaking_" + BetType + "_min"] : 0);
                    aMax = maMax - m > aMax2 ? aMax2 : maMax - m;
                    mMax = maMax - aMin;
                    //Master auto preset
                    mAuto = CastAide.AsDouble(data.Table.Columns.Contains("auto_v" + SportType + "mpositiontaking_" + BetType) ?
                        data["auto_v" + SportType + "mpositiontaking_" + BetType] : 0);
                    mAutoMax = CastAide.AsDouble(data.Table.Columns.Contains("auto_v" + SportType + "mpositiontaking_" + BetType + "_max") ?
                        data["auto_v" + SportType + "mpositiontaking_" + BetType + "_max"] : 0);
                    isAuto = CastAide.AsBoolean(data.Table.Columns.Contains("isauto_v" + SportType + "mpositiontaking_" + BetType) ?
                        data["isauto_v" + SportType + "mpositiontaking_" + BetType] : 0);
                }
                //add hidden values
                hiddenHash.Add("minM" + BetTypeName, maMin);
                hiddenHash.Add("maxA" + BetTypeName, (aMax2 > maMax) ? maMax : aMax2);
                hiddenHash.Add("maxM" + BetTypeName, maMax);
                hiddenHash.Add("isAuto" + BetTypeName, isAuto);

                #endregion get data

                #region assign data

                //agent
                TemplateHandler.Assign(ref tpl, "{listAMixParlayValue}", CustomerUtils.GetListPositionTaking(aMin, aMax, a));
                //master
                TemplateHandler.Assign(ref tpl, "{listMMixParlayValue}", CustomerUtils.GetListPositionTaking(mMin, mMax, m));
                //auto preset
                if (isEnabledAutoMaster)
                    TemplateHandler.Assign(ref tpl, "{listSAutoMixParlayValue}", CustomerUtils.GetListPositionTaking(0, mAutoMax, mAuto));

                if (isAuto)
                {
                    TemplateHandler.Assign(ref tpl, "{statuslstSMixParlay}", Disabled);
                    TemplateHandler.Assign(ref tpl, "{statuschkMixParlay}", Check);
                    TemplateHandler.Assign(ref tpl, "{statuslstSAutoMixParlay}", string.Empty);
                }
                else
                {
                    TemplateHandler.Assign(ref tpl, "{statuslstSMixParlay}", string.Empty);
                    TemplateHandler.Assign(ref tpl, "{statuschkMixParlay}", string.Empty);
                    TemplateHandler.Assign(ref tpl, "{statuslstSAutoMixParlay}", Disabled);
                }

                #endregion assign data

                strHidden = CustomerUtils.AssignMultipleHidden(hiddenHash);
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            finally
            {
                html.Append(tpl);
            }
            return html.ToString();
        }

        #endregion Agent Edit

        public void AddParamAgentPT(ref Hashtable param, HttpContext context)
        {
            #region private varialbes

            var minbet = "0";
            var maxbet = "0";
            var maxpermatch = "0";

            var aPT = "0";
            var mPT = "0";
            var mAuto = "0";
            var isAuto = 0;

            #endregion private varialbes

            try
            {
                #region get data

                if (string.IsNullOrEmpty(context.Request["chkUpdPT" + SportType]))
                {
                    param.Add(string.Format("updatepositiontakingv{0}mp", SportType), 1);

                    if (!string.IsNullOrEmpty(context.Request["chkCopyAll"])) // copy all from soccer handicap
                    {
                        minbet = context.Request["txtMinbet1"];
                        maxbet = context.Request["txtMaxbet1"];
                        maxpermatch = context.Request["txtMaxPerMatch1"];

                        aPT = context.Request["listM1HdpD"];
                        isAuto = string.IsNullOrEmpty(context.Request["chkAuto1HdpD"]) ? 0 : 1;
                        mPT = isAuto == 0 ? context.Request["listS1HdpD"] : "0";
                        mAuto = isAuto == 0 ? "0" : context.Request["autoS1HdpD"];
                    }
                    else if (!string.IsNullOrEmpty(context.Request["chkCopyBasket"]))//copy from basketball
                    {
                        minbet = context.Request["txtMinbet2"];
                        maxbet = context.Request["txtMaxbet2"];
                        maxpermatch = context.Request["txtMaxPerMatch2"];

                        aPT = context.Request["listM2Hdp"];
                        isAuto = string.IsNullOrEmpty(context.Request["chkAuto2Hdp"]) ? 0 : 1;
                        mPT = isAuto == 0 ? context.Request["listS2Hdp"] : "0";
                        mAuto = isAuto == 0 ? "0" : context.Request["autoS2Hdp"];
                    }
                    else
                    {
                        minbet = context.Request["txtMinbet99_9"];
                        maxbet = context.Request["txtMaxbet99_9"];
                        maxpermatch = context.Request["txtMaxPerMatch99_9"];

                        aPT = context.Request["listMMixParlay"];
                        isAuto = string.IsNullOrEmpty(context.Request["chkAutoMixParlay"]) ? 0 : 1;
                        mPT = isAuto == 0 ? context.Request["listSMixParlay"] : "0";
                        mAuto = isAuto == 0 ? "0" : context.Request["autoSMixParlay"];
                    }
                }
                else
                {
                    param.Add(string.Format("updatepositiontakingv{0}mp", SportType), 0);
                }

                #endregion get data
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            finally
            {
                #region assign values for parameters

                param.Add(string.Format("updateminmaxbetv{0}mp", SportType),
                          !string.IsNullOrEmpty(context.Request["chkUpdInfo" + SportType]) ? 0 : 1);

                param.Add(string.Format("minbetv{0}mp", SportType), minbet);//money
                param.Add(string.Format("maxbetv{0}mp", SportType), maxbet);//money
                param.Add(string.Format("maxpermatchv{0}mp", SportType), maxpermatch);//money

                param.Add(string.Format("v{0}apositiontaking_{1}", SportType, BetType), aPT);
                param.Add(string.Format("v{0}mpositiontaking_{1}", SportType, BetType), mPT);
                //check
                if (!WebSettings.disableAutoM)
                {
                    param.Add(string.Format("auto_v{0}mpositiontaking_{1}", SportType, BetType), mAuto);
                    param.Add(string.Format("isauto_v{0}mpositiontaking_{1}", SportType, BetType), isAuto);
                }
                //just for test, remove it after test
                if (!param.ContainsKey("minbetv161"))
                {
                    param.Add("discountnumber", 0);
                    new NumberGameManager().AddParamAgentPT(ref param, context, false, false);
                }

                #endregion assign values for parameters
            }
        }

        #endregion Agent

        #region Member

        public override string GetMemberPtSetting(DataRow data, bool isAddNew)
        {
            #region private varialbes

            _isUpdated = isAddNew ? false : true;
            var html = new StringBuilder();
            var tpl = WebUtils.LoadTemplate("Customer/v2/MixedSports/MemberPTRow.htm");

            double mMin = 0, mMax = 0, m = 0;
            var tplGameHeader = GetGameHeader();
            var tplBetSetting = GetBetSetting(SportType, data, !isAddNew);

            #endregion private varialbes

            try
            {
                #region assign header and betsetting

                TemplateHandler.Assign(ref tpl, "{ptheader}", tplGameHeader);
                TemplateHandler.Assign(ref tpl, "{betsetting}", tplBetSetting);
                TemplateHandler.Assign(ref tpl, "{tblWidth}", "width:700px");
                TemplateHandler.RemoveBlock(ref tpl, "tdLeft");
                TemplateHandler.Assign(ref tpl, "{colTitleSpan}", "colspan='2'");

                #endregion assign header and betsetting

                if (data != null)
                {
                    if (isAddNew)
                    {
                        mMin = CastAide.AsDouble(data["v" + SportType + "apositiontaking_" + BetType + "_min"]);
                        mMax = CastAide.AsDouble(data["v" + SportType + "apositiontaking_" + BetType]);
                        m = mMax;
                    }
                    else
                    {
                        mMin = CastAide.AsDouble(data["minv" + SportType + "pos_" + BetType]);
                        mMax = CastAide.AsDouble(data["maxv" + SportType + "pos_" + BetType]);
                        m = CastAide.AsDouble(data["v" + SportType + "apositiontaking_" + BetType]);
                    }
                }

                TemplateHandler.Assign(ref tpl, "{listMMixParlayValue}", CustomerUtils.GetListPositionTaking(mMin, mMax, m));
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            finally
            {
                html.Append(tpl);
            }
            return html.ToString();
        }

        public void AddParamMemberPT(ref Hashtable param, string minBet, string maxBet, string maxPerMatch,
            string agentPt, byte updatePt, byte updateMinMax)
        {
            try
            {
                param.Add(string.Format("updatepositiontakingv{0}mp", SportType), updatePt);
                param.Add(string.Format("updateminmaxbetv{0}mp", SportType), updateMinMax);

                param.Add(string.Format("minbetv{0}mp", SportType), minBet);//money
                param.Add(string.Format("maxbetv{0}mp", SportType), maxBet);//money
                param.Add(string.Format("maxpermatchv{0}mp", SportType), maxPerMatch);//money
                param.Add(string.Format("v{0}apositiontaking_{1}", SportType, BetType), agentPt);
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
        }

        #endregion Member

        #region Min/Max PT; Update Super/Master PT on Member

        #region super pt on member

        public string GetSuperEditOnMemberInfo(DataRow data, ref string hiddenMixedSports)
        {
            #region private variables

            var html = new StringBuilder();
            var hidden = new Hashtable();
            var tplHeader = GetPTHeaderForEdit();
            var tpl = WebUtils.LoadTemplate("Customer/v2/MixedSports/EditSuperOnMemberRow.htm");
            double sMin = 0, sMax = 0, s = 0;
            double m = 0, a = 0;
            double sAuto = 0, sAutoMax = 0;
            bool isAutoMember = false, isAutoMaster = false;

            #endregion private variables

            #region assign info for header

            TemplateHandler.Assign(ref tpl, "{ptheader}", tplHeader);
            TemplateHandler.Assign(ref tpl, "{agent-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Agent"));
            TemplateHandler.Assign(ref tpl, "{master-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Master"));
            TemplateHandler.Assign(ref tpl, "{super-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Super"));
            TemplateHandler.Assign(ref tpl, "{superauto-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "SuperAuto"));

            #endregion assign info for header

            try
            {
                if (data != null)
                {
                    //super sHdpMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_1"))
                    sMin = CastAide.AsDouble(data[string.Format("minv{0}pos_{1}", SportType, BetType)]);
                    sMax = CastAide.AsDouble(data[string.Format("maxv{0}pos_{1}", SportType, BetType)]);
                    s = CastAide.AsDouble(data[string.Format("v{0}spositiontaking_{1}", SportType, BetType)]);
                    //master
                    m = CastAide.AsDouble(data[string.Format("v{0}mpositiontaking_{1}", SportType, BetType)]);
                    //agent
                    a = CastAide.AsDouble(data[string.Format("v{0}apositiontaking_{1}", SportType, BetType)]);
                    //super auto
                    sAuto = CastAide.AsDouble(data[string.Format("auto_v{0}spositiontaking_{1}", SportType, BetType)]);
                    sAutoMax = CastAide.AsDouble(data[string.Format("maxautov{0}pos_{1}", SportType, BetType)]);

                    isAutoMember =
                        CastAide.AsBoolean(data[string.Format("isauto_v{0}spositiontaking_mem_{1}", SportType, BetType)]);
                    isAutoMaster =
                        CastAide.AsBoolean(data[string.Format("isauto_v{0}spositiontaking_{1}", SportType, BetType)]);
                }
                hidden.Add("isAutoMixParlay", isAutoMember);
                hidden.Add("isAuto_VMixParlay", isAutoMaster);
                hiddenMixedSports = CustomerUtils.AssignMultipleHidden(hidden);

                #region assign detail info

                if (isAutoMaster)
                {
                    TemplateHandler.RemoveBlock(ref tpl, "none-sauto");
                    TemplateHandler.Assign(ref tpl, "{statuschkSAutoMixParlay}", isAutoMember ? Check : string.Empty);
                    TemplateHandler.Assign(ref tpl, "{statuslstSAutoMixParlay}", isAutoMember ? string.Empty : Disabled);
                    TemplateHandler.Assign(ref tpl, "{s-mp-val}", s);
                }
                else if (isAutoMember)
                {
                    TemplateHandler.RemoveBlock(ref tpl, "sauto");
                    TemplateHandler.Assign(ref tpl, "{statuschkSAutoMixParlay}", Check);
                    TemplateHandler.Assign(ref tpl, "{statuslstSMixParlay}", Disabled);
                    TemplateHandler.Assign(ref tpl, "{statuslstSAutoMixParlay}", string.Empty);

                    TemplateHandler.Assign(ref tpl, "{listSMixParlayValue}",
                                           CustomerUtils.GetListPositionTaking(sMin, sMax, s));
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref tpl, "sauto");
                    TemplateHandler.Assign(ref tpl, "{statuschkSAutoMixParlay}", string.Empty);
                    TemplateHandler.Assign(ref tpl, "{statuslstSMixParlay}", string.Empty);
                    TemplateHandler.Assign(ref tpl, "{statuslstSAutoMixParlay}", Disabled);

                    TemplateHandler.Assign(ref tpl, "{listSMixParlayValue}",
                                           CustomerUtils.GetListPositionTaking(sMin, sMax, s));
                }

                TemplateHandler.Assign(ref tpl, "{a-mp-val}", a);
                TemplateHandler.Assign(ref tpl, "{m-mp-val}", m);
                TemplateHandler.Assign(ref tpl, "{listSAutoMixParlayValue}",
                                       CustomerUtils.GetListPositionTaking(0, sAutoMax, sAuto));

                #endregion assign detail info
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            html.Append(tpl);
            return html.ToString();
        }

        public void AddParamSuperPTOnMember(ref Hashtable param, string auto, string s, bool isAuto)
        {
            try
            {
                param.Add(string.Format("v{0}spositiontaking_{1}", SportType, BetType), s);
                param.Add(string.Format("auto_v{0}spositiontaking_{1}", SportType, BetType), auto);
                param.Add(string.Format("isauto_v{0}spositiontaking_{1}", SportType, BetType), isAuto);
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
        }

        #endregion super pt on member

        #region min/max pt

        public string GetMinMaxPtForEdit(DataRow data, string prefix, ref string hiddenMixedSports)
        {
            #region private varialbes

            var html = new StringBuilder();
            var tplHeader = GetPTHeaderForEdit();
            var tpl = WebUtils.LoadTemplate("Customer/v2/MixedSports/EditMinMaxPTRow.htm");
            double mpMin = 0, mpMax = 0, mp = 0;
            var hidden = new Hashtable();

            #endregion private varialbes

            TemplateHandler.Assign(ref tpl, "{ptheader}", tplHeader);
            TemplateHandler.Assign(ref tpl, "{wPT}", "wPT70");
            try
            {
                #region assign data

                if (data != null)
                {
                    mpMax =
                        CastAide.AsDouble(data[string.Format("v{0}{1}positiontaking_{2}", SportType, prefix, BetType)]);
                    mp =
                        CastAide.AsDouble(
                            data[string.Format("v{0}{1}positiontaking_{2}_min", SportType, prefix, BetType)]);

                    hidden.Add("listMMixParlay" + prefix, mpMax);

                    if (prefix.Equals("s"))
                    {
                        TemplateHandler.RemoveBlock(ref tpl, "super-min");
                        TemplateHandler.RemoveBlock(ref tpl, "MinPT");

                        TemplateHandler.Assign(ref tpl, "{listSMixParlayValue}",
                                               CustomerUtils.GetListPositionTaking(mpMin, mpMax, mp));

                        //hiddenMixedSports = string.Empty;
                        hiddenMixedSports = CustomerUtils.AssignMultipleHidden(hidden);
                    }
                    else
                    {
                        TemplateHandler.Assign(ref tpl, "{super-position}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMaxPosition"));
                        TemplateHandler.Assign(ref tpl, "{master-position}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMinPosition"));

                        double mMpMax = 0.9, mMp = 0;
                        mMp =
                            FormatHandler.GetDouble(
                                data[string.Format("v{0}{1}positiontaking_{2}_max", SportType, prefix, BetType)]);
                        TemplateHandler.Assign(ref tpl, "{listSMixParlayValue}",
                                               CustomerUtils.GetListPositionTaking(mp, mMpMax, mMp));
                        TemplateHandler.Assign(ref tpl, "{listMMixParlayValue}",
                                               CustomerUtils.GetListPositionTaking(mpMin, mpMax, mp));

                        hiddenMixedSports = CustomerUtils.AssignMultipleHidden(hidden);
                    }
                }

                #endregion assign data
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }

            html.Append(tpl);
            return html.ToString();
        }

        public void AddParamMinMaxPT(ref Hashtable param, string type, string super, string master)
        {
            string pre1 = "max", pre2 = "min";
            if (type.Equals("super"))
            {
                pre1 = "min";
                pre2 = "max";
            }
            try
            {
                param.Add(string.Format("{0}v{1}positiontaking_{2}", pre1, SportType, BetType),
                          super);
                param.Add(string.Format("{0}v{1}positiontaking_{2}", pre2, SportType, BetType),
                          master);
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
        }

        #endregion min/max pt

        #region master pt on member

        public string GetMasterEditOnMemberInfo(DataRow data, ref string hiddenMixedSports, bool isEnableAutoMaster)
        {
            #region priavate variables

            var html = new StringBuilder();
            var tplHeader = GetPTHeaderForEdit();
            var tpl = WebUtils.LoadTemplate("Customer/v2/MixedSports/EditMasterOnMemberRow.htm");
            var hidden = new Hashtable();
            double a = 0;
            double mMin = 0, mMax = 0, m = 0;
            double mAutoMax = 0, mAuto = 0;
            bool isAutoMember = false, isAutoAgent = false;

            #endregion priavate variables

            #region assign header

            TemplateHandler.Assign(ref tpl, "{ptheader}", tplHeader);
            TemplateHandler.Assign(ref tpl, "{agent-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Agent"));
            TemplateHandler.Assign(ref tpl, "{master-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Master"));

            #endregion assign header

            try
            {
                #region get and assign data

                if (data != null)
                {
                    m = CastAide.AsDouble(data[string.Format("v{0}mpositiontaking_{1}", SportType, BetType)]);
                    if (Permissions.IsEnabledAutoMaster(isEnableAutoMaster))
                    {
                        isAutoMember =
                            CastAide.AsBoolean(
                                data[string.Format("isauto_v{0}mpositiontaking_mem_{1}", SportType, BetType)]);
                        isAutoAgent =
                            CastAide.AsBoolean(data[string.Format("isauto_v{0}mpositiontaking_{1}", SportType, BetType)]);
                    }
                    else
                        TemplateHandler.RemoveBlock(ref tpl, "MasterAuto");

                    mAuto =
                        CastAide.AsDouble(data[string.Format("auto_v{0}mpositiontaking_mem_{1}", SportType, BetType)]);
                    a = CastAide.AsDouble(data[string.Format("v{0}apositiontaking_{1}", SportType, BetType)]);
                    mMin = CastAide.AsDouble(data[string.Format("minv{0}pos_{1}", SportType, BetType)]);
                    mMax = CastAide.AsDouble(data[string.Format("maxv{0}pos_{1}", SportType, BetType)]);
                    mAutoMax =
                        CastAide.AsDouble(data[string.Format("maxauto_v{0}mpositiontaking_mem_{1}", SportType, BetType)]);
                }
                hidden.Add("isAuto_VMixParlayMember", isAutoMember);
                hidden.Add("isAuto_VMixParlayAgent", isAutoAgent);
                hiddenMixedSports = CustomerUtils.AssignMultipleHidden(hidden);

                if (isAutoAgent)
                {
                    TemplateHandler.Assign(ref tpl, "{statuslstMMixParlay}", isAutoMember ? Disabled : string.Empty);
                    TemplateHandler.Assign(ref tpl, "{statuschkMAutoMixParlay}", isAutoMember ? Check : string.Empty);
                    TemplateHandler.Assign(ref tpl, "{statuslstMAutoMixParlay}", isAutoMember ? string.Empty : Disabled);
                    TemplateHandler.Assign(ref tpl, "{m-mp-val}", m);
                    TemplateHandler.RemoveBlock(ref tpl, "none-mauto");
                }

                else if (isAutoMember)
                {
                    TemplateHandler.Assign(ref tpl, "{statuslstMMixParlay}", Disabled);
                    TemplateHandler.Assign(ref tpl, "{statuschkMAutoMixParlay}", Check);
                    TemplateHandler.Assign(ref tpl, "{statuslstMAutoMixParlay}", string.Empty);
                    TemplateHandler.Assign(ref tpl, "{listMMixParlayValue}",
                                           CustomerUtils.GetListPositionTaking(mMin, mMax, m));
                    TemplateHandler.RemoveBlock(ref tpl, "mauto");
                }
                else
                {
                    TemplateHandler.Assign(ref tpl, "{statuslstMMixParlay}", string.Empty);
                    TemplateHandler.Assign(ref tpl, "{statuschkMAutoMixParlay}", string.Empty);
                    TemplateHandler.Assign(ref tpl, "{statuslstMAutoMixParlay}", Disabled);
                    TemplateHandler.Assign(ref tpl, "{listMMixParlayValue}",
                                           CustomerUtils.GetListPositionTaking(mMin, mMax, m));

                    TemplateHandler.RemoveBlock(ref tpl, "mauto");
                }

                TemplateHandler.Assign(ref tpl, "{a-mp-val}", a);
                TemplateHandler.Assign(ref tpl, "{master-auto-text}",
                                       string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAuto"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Master")));
                TemplateHandler.Assign(ref tpl, "{listMAutoMixParlayValue}",
                                       CustomerUtils.GetListPositionTaking(0, mAutoMax, mAuto));

                #endregion get and assign data
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            html.Append(tpl);
            return html.ToString();
        }

        public void AddParamMasterPTOnMember(ref Hashtable param, double auto, double m, bool isAuto)
        {
            try
            {
                param.Add(string.Format("v{0}mpositiontaking_{1}", SportType, BetType), m);
                param.Add(string.Format("auto_v{0}mpositiontaking_mem_{1}", SportType, BetType), auto);
                param.Add(string.Format("isauto_v{0}mpositiontaking_mem_{1}", SportType, BetType), isAuto);
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
        }

        #endregion master pt on member

        public static void GetEditMinMaxValue(DataRow data, ref string tpl)
        {
            try
            {
                TemplateHandler.Assign(ref tpl, "minbet" + SportType + "mp-val",
                                       DataFormat.FormatNumber1(data[string.Format("minbetv{0}mp", SportType)]));
                TemplateHandler.Assign(ref tpl, "maxbet" + SportType + "mp-val",
                                       DataFormat.FormatNumber1(data[string.Format("maxbetv{0}mp", SportType)]));
                TemplateHandler.Assign(ref tpl, "maxpermatch" + SportType + "mp-val",
                                       DataFormat.FormatNumber1(data[string.Format("maxpermatchv{0}mp", SportType)]));
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
        }

        #endregion Min/Max PT; Update Super/Master PT on Member
    }
}
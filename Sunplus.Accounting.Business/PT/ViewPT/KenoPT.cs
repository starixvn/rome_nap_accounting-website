// -----------------------------------------------------------------------
// <copyright file="KenoPT.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Sunplus.Accounting.Business.PT.ViewPT
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Sunplus.Accounting.Entities.Customer.PT.ViewPT;
    using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
    using System.Data;
    using DNAide.Core;
    using Sunplus.Accounting.Core.Extensions;
    using Bettype = Sunplus.Accounting.Entities.Customer.PT.ViewPT.Bettype;
    using Sunplus.Accounting.Entities.PositionTaking.Racing;
    using DNAide.Utils;
    using Sunplus.Accounting.Business.Security;
    using Sunplus.Accounting.Core.Configurations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class KenoPT
    {
        private static string _lcsPt = "{0}positiontaking";

        public static Dictionary<int, string> Bettypes = new Dictionary<int, string>()
            {
                {1501, "Australia Acttab"},
                {1503, "Slovakia Eklub"},
                {1504, "Canada Keno"},
                {1505, "West Canada Keno"},
                {1507, "Massac"},
                {1508, "Ohio"},
                {1509, "Malta"}
            };

        public BettypeDTO GetBettypeByKeno()
        {
            var result = new BettypeDTO { BettypeList = new List<Bettype>() };

            foreach (var bettype in KenoPT.Bettypes)
            {
                result.BettypeList.Add(new Bettype()
                {
                    BettypeId = bettype.Key,
                    BettypeName = bettype.Value
                });
            }

            return result;
        }

        private static ViewPtDTO KenoPositionTaking(string userName, int? sportType, int? bettype)
        {
            var result = new ViewPtDTO();
            try
            {
                if (bettype.Equals(0))
                {
                }
                #region get data

                var dtPt = SelectCustomer.GetLiveCasinoPt(userName, sportType, bettype);

                #endregion get data

                #region assign data to ViewPtDTO


                if (dtPt != null && dtPt.ResultSet.Tables.Count > 1)
                {
                    var dtInfo = dtPt.ResultSet.Tables[0];
                    var masterDisableAutoPt = dtPt.ResultSet.Tables[1].Rows.Count > 0 ? CastAide.AsBoolean(dtPt.ResultSet.Tables[1].Rows[0]["DisableAutoMPT"], false) : false;
                    var data = new DataTable();
                    DataView dv = dtInfo.DefaultView;
                    dv.Sort = "roleid desc";
                    data = dv.ToTable();

                    result.IsCasino = false;
                    result.IsLiveCasino = true;
                    var drInfo = data.Rows[dtInfo.Rows.Count - 1];
                    var roleId = CastAide.AsInt32(drInfo["roleid"], 1);
                    result.CustomerInfo = new CustomerRelation { Upline = new List<CustomerInfo>() };
                    result.PotionTakingDetail = new List<PositionTakingDTO>();
                    //var masterDisableAutoPt =

                    #region assign Customer Info

                    #region agent

                    if (roleId < (int)CustInfo.Roles.Agent)
                    {
                        var agentInfo = new CustomerInfo()
                        {
                            CustId = CastAide.AsInt32(data.Rows[2]["custid"].ToString()),
                            UserName = data.Rows[2]["username"].ToString(),
                            RoleId = (int)CustInfo.Roles.Agent,
                            RoleName = ResourceExtension.GetResourceValue(
                                ResourceExtension.Resources.Language, "Agent")
                        };
                        result.CustomerInfo.Upline.Add(agentInfo);
                    }

                    #endregion agent

                    #region master

                    if (roleId < (int)CustInfo.Roles.Master)
                    {
                        var masterInfo = new CustomerInfo()
                        {
                            CustId = CastAide.AsInt32(data.Rows[1]["custid"].ToString()),
                            UserName = data.Rows[1]["username"].ToString(),
                            RoleId = (int)CustInfo.Roles.Master,
                            RoleName = ResourceExtension.GetResourceValue(
                                ResourceExtension.Resources.Language, "Master")
                        };
                        result.CustomerInfo.Upline.Add(masterInfo);
                    }

                    #endregion master

                    #region super

                    if (roleId < (int)CustInfo.Roles.Super)
                    {
                        var superInfo = new CustomerInfo()
                        {
                            CustId = CastAide.AsInt32(data.Rows[0]["custid"].ToString()),
                            UserName = data.Rows[0]["username"].ToString(),
                            RoleId = (int)CustInfo.Roles.Super,
                            RoleName = ResourceExtension.GetResourceValue(
                                ResourceExtension.Resources.Language, "Super")
                        };
                        result.CustomerInfo.Upline.Add(superInfo);
                    }

                    #endregion super

                    #region current user level

                    result.CustomerInfo.Customer = new CustomerInfo()
                    {
                        CustId = CastAide.AsInt32(drInfo["custid"].ToString()),
                        UserName = drInfo["username"].ToString(),
                        RoleId = (int)CustInfo.Roles.Member
                    };

                    #endregion current user level

                    #endregion assign Customer Info

                    #region assign PT

                    var ptCurrent = new PositionTakingDTO();
                    foreach (DataRow dr in data.Rows)
                    {
                        int currentRoleId = CastAide.AsInt16(dr["roleid"].ToString());
                        ptCurrent.PermistionEnableAutoMaster =
                            Permissions.IsEnabledAutoMaster(!masterDisableAutoPt);
                        ptCurrent.EnableAutoMaster = !WebSettings.disableAutoM;
                        ptCurrent.DisableFlagCss = (!ptCurrent.PermistionEnableAutoMaster ? "disableFlag" : "OpenFlag");
                        ptCurrent.DisableAutoCss = (!ptCurrent.PermistionEnableAutoMaster ? "disColorMPT" : "");

                        #region super

                        if (currentRoleId == (int)CustInfo.Roles.Super)
                        {
                            if (sportType == 202)
                            {
                                var bettypeTmp = KenoPT.Bettypes.SingleOrDefault(x => x.Key == bettype);
                                ptCurrent.SportName = "Keno(" + bettypeTmp.Value + ")  " + "[202-" + bettypeTmp.Key + "]";
                                ptCurrent.SportType = KenoPT.Bettypes.SingleOrDefault(x => x.Key == bettype).Value;
                                ptCurrent.TypeName = KenoPT.Bettypes.SingleOrDefault(x => x.Key == bettype).Value;
                            }
                            else
                            {
                                ptCurrent.SportName = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "liveCasinoTitle");
                                ptCurrent.SportType = string.Empty;
                                ptCurrent.TypeName = string.Empty;
                            }

                            var superPt = new PositionTakingInfo()
                            {
                                Super = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "s")], 2),
                                Master = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "m")], 2),
                                Agent = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "a")], 2),
                                MinPt = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "min")], 2),
                                Com = IBCUtils.FormatHandler.FormatNumber4(dr["commission"])
                            };
                            ptCurrent.Super = superPt;
                        }

                        #endregion super

                        #region master

                        if (currentRoleId == (int)CustInfo.Roles.Master)
                        {
                            var masterPt = new PositionTakingInfo()
                            {
                                Super = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "s")], 2),
                                Master = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "m")], 2),
                                MinPt = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "min")], 2),
                                SuperAuto = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "auto")], 2),
                                SuperIsAuto =
                                    (CastAide.AsBoolean(dr[string.Format(_lcsPt, "isauto")], false))
                                        ? " checked='checked'"
                                        : string.Empty,
                                Com = IBCUtils.FormatHandler.FormatNumber4(dr["commission"])
                            };
                            ptCurrent.Master = masterPt;
                        }

                        #endregion master

                        #region Agent

                        if (currentRoleId == (int)CustInfo.Roles.Agent)
                        {
                            var agentPt = new PositionTakingInfo()
                            {
                                Super = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "s")], 2),
                                Master = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "m")], 2),
                                Agent = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "a")], 2),
                                MinPt = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "min")], 2),
                                SuperAuto = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "auto")], 2),
                                SuperIsAuto =
                                    (CastAide.AsBoolean(dr[string.Format(_lcsPt, "isauto")], false))
                                        ? " checked='checked'"
                                        : string.Empty,
                                MasterAuto = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "autom")], 2),
                                MasterIsAuto = (CastAide.AsBoolean(dr[string.Format(_lcsPt, "isautom")], false))
                                        ? " checked='checked'"
                                        : string.Empty,
                                Com = IBCUtils.FormatHandler.FormatNumber4(dr["commission"])
                            };
                            ptCurrent.Agent = agentPt;
                        }

                        #endregion Agent

                        #region member

                        if (currentRoleId == (int)CustInfo.Roles.Member)
                        {
                            var memberPt = new PositionTakingInfo()
                            {
                                Super = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "s")], 2),
                                Master = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "m")], 2),
                                Agent = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "a")], 2),
                                SuperAuto = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "auto")], 2),
                                SuperIsAuto =
                                    (CastAide.AsBoolean(dr[string.Format(_lcsPt, "isauto")], false))
                                        ? " checked='checked'"
                                        : string.Empty,
                                MasterAuto = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "autom")], 2),
                                MasterIsAuto = (CastAide.AsBoolean(dr[string.Format(_lcsPt, "isautom")], false))
                                        ? " checked='checked'"
                                        : string.Empty,
                                Com = IBCUtils.FormatHandler.FormatNumber4(dr["commission"])
                            };
                            ptCurrent.Member = memberPt;
                        }

                        #endregion member

                        if (roleId != currentRoleId) continue;
                        result.PotionTakingDetail.Add(ptCurrent);
                        ptCurrent = new PositionTakingDTO();
                    }

                    #endregion assign PT
                }
                else
                {
                    result.ErrorMessage = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language,
                                                                             "thereisnodata");
                }

                #endregion assign data to ViewPtDTO
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }
    }
}

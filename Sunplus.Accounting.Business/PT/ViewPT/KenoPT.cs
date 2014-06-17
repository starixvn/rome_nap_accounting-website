// --------------------------------------------------------------------------
// <copyright file="KenoPT.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Sunplus.Accounting.Business.PT.ViewPT
{
    using System.Collections.Generic;
    using System.Data;
    using DNAide.Core;
    using DNAide.Data;
    using DNAide.Utils;
    using Fanex.Tarus.Infrastructure.Configurations;
    using Sunplus.Accounting.Business.Security;
    using Sunplus.Accounting.Core.Extensions;
    using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
    using Sunplus.Accounting.Entities.Criteria.ViewPT;
    using Sunplus.Accounting.Entities.Customer.PT.ViewPT;
    using Sunplus.Accounting.Entities.PositionTaking.Racing;
    using Bettype = Sunplus.Accounting.Entities.Customer.PT.ViewPT.Bettype;

    /// <summary>
    /// Keno Position Taking.
    /// </summary>
    public class KenoPT
    {
        public static Dictionary<int, string> Bettypes;

        public static byte NumberOfDecimalDigits = (byte)2;

        private IViewKenoPTRepository _viewKenoPTRepository;

        public KenoPT()
            : this(null)
        {
        }

        public KenoPT(IViewKenoPTRepository viewKenoPTRepository)
        {
            GetActiveBetTypes();
            _viewKenoPTRepository = viewKenoPTRepository ?? new ViewKenoPTRepository();
        }

        public void GetActiveBetTypes()
        {
            if (Bettypes != null)
            {
                return;
            }

            Bettypes = new Dictionary<int, string>();
            var kenoBetTypes = KenoBetTypesSection.Instance.BetTypes;
            foreach (KenoBetTypesElement betType in kenoBetTypes)
            {
                if (!betType.Active)
                {
                    continue;
                }

                Bettypes.Add(int.Parse(betType.Id), betType.Name);
            }
        }

        public BettypeDTO GetBettypeByKeno()
        {
            var result = new BettypeDTO
            {
                BettypeList = new List<Bettype>()
            };

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

        public ViewPtDTO ViewPositionTaking(ViewKenoPTCriteria criteria)
        {
            var result = new ViewPtDTO();
            var rawData = _viewKenoPTRepository.GetPT(criteria);

            if (rawData.Outputs != null &&
                CastAide.AsInt32(rawData.Outputs["errcode"], -1) != 0)
            {
                result.ErrorMessage = rawData.Outputs["errmsg"].ToString();
                return result;
            }

            if (rawData.ResultSet.Tables.Count == 0)
            {
                result.ErrorMessage = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata");
                return result;
            }

            var drCustInfo = rawData.ResultSet.Tables[0].Rows[0];
            var roleId = CastAide.AsInt32(drCustInfo["roleid"], 1);
            result.CustomerInfo = GetCustomerRelation(rawData);
            result.PotionTakingDetail = PositionTakingDetail(rawData, roleId, criteria.SportId);
            return result;
        }

        public CustomerRelation GetCustomerRelation(DbResult data)
        {
            var customerRelation = new CustomerRelation
            {
                Upline = new List<CustomerInfo>()
            };

            var drCustInfo = data.ResultSet.Tables[0].Rows[0];
            var roleId = CastAide.AsInt32(drCustInfo["roleid"], 1);
            if (roleId < (int)CustInfo.Roles.Agent)
            {
                var agentInfo = new CustomerInfo()
                {
                    CustId = CastAide.AsInt32(drCustInfo["AgID"].ToString()),
                    UserName = drCustInfo["Ag"].ToString(),
                    RoleId = (int)CustInfo.Roles.Agent,
                    RoleName = ResourceExtension.GetResourceValue(
                     ResourceExtension.Resources.Language, "Agent")
                };

                customerRelation.Upline.Add(agentInfo);
            }


            if (roleId < (int)CustInfo.Roles.Master)
            {
                var masterInfo = new CustomerInfo()
                {
                    CustId = CastAide.AsInt32(drCustInfo["MAgID"].ToString()),
                    UserName = drCustInfo["MAg"].ToString(),
                    RoleId = (int)CustInfo.Roles.Master,
                    RoleName = ResourceExtension.GetResourceValue(
                                         ResourceExtension.Resources.Language, "Master")
                };

                customerRelation.Upline.Add(masterInfo);
            }

            if (roleId < (int)CustInfo.Roles.Super)
            {
                var superInfo = new CustomerInfo()
                {
                    CustId = CastAide.AsInt32(drCustInfo["SAgID"].ToString()),
                    UserName = drCustInfo["SAg"].ToString(),
                    RoleId = (int)CustInfo.Roles.Super,
                    RoleName = ResourceExtension.GetResourceValue(
                                         ResourceExtension.Resources.Language, "Super")
                };
                customerRelation.Upline.Add(superInfo);
            }

            customerRelation.Customer = new CustomerInfo()
            {
                CustId = CastAide.AsInt32(drCustInfo["custid"].ToString()),
                UserName = drCustInfo["username"].ToString(),
                RoleId = (int)CustInfo.Roles.Member
            };

            return customerRelation;
        }

        private List<PositionTakingDTO> PositionTakingDetail(DbResult data, int roleId, int sportId)
        {
            var ptDetail = new List<PositionTakingDTO>();
            if (data == null || data.ResultSet.Tables.Count < 2)
            {
                return ptDetail;
            }

            var ptData = data.ResultSet.Tables[1];
            var ptInfo = data.ResultSet.Tables[0].Rows[0];
            string currencyColumnName = "currency";
            var currencyOfCustomer = ptInfo.Table.Columns.Contains(currencyColumnName) ?
                CastAide.AsInt32(ptInfo[currencyColumnName]) : 1;

            foreach (DataRow dr in ptData.Rows)
            {
                var ptCurrent = new PositionTakingDTO();
                var betType = CastAide.AsInt32(dr["bettype"]);
                var sportType = CastAide.AsInt32(dr["sporttype"].ToString());
                var currencyAllowForTote = string.Empty;

                AssignSportInfo(ref ptCurrent, dr, ptInfo);
                AssignPtDetail(ref ptCurrent, dr, roleId);
                ptDetail.Add(ptCurrent);
            }

            return ptDetail;
        }

        private void AssignSportInfo(
            ref PositionTakingDTO ptDTO,
            DataRow dr,
            DataRow ptInfo)
        {
            ptDTO.SportName = CastAide.AsString(dr["sportname"]);
            ptDTO.SportType = CastAide.AsString(dr["sporttype"]);
            ptDTO.Bettype = CastAide.AsString(dr["bettype"]);
            ptDTO.TypeName = CastAide.AsString(dr["typename"]);

            ptDTO.PermistionEnableAutoMaster = Permissions.IsEnabledAutoMaster(!CastAide.AsBoolean(ptInfo["disableautompt"], false));
            ptDTO.EnableAutoMaster = !Sunplus.Accounting.Core.Configurations.WebSettings.disableAutoM;
            ptDTO.DisableFlagCss = (!ptDTO.PermistionEnableAutoMaster ? "disableFlag" : "OpenFlag");
            ptDTO.DisableAutoCss = (!ptDTO.PermistionEnableAutoMaster ? "disColorMPT" : "");
        }

        private void AssignPtDetail(
            ref PositionTakingDTO ptDTO,
            DataRow dr,
            int roleId)
        {
            if (roleId < (int)CustInfo.Roles.Agent)
            {
                var memberPt = new PositionTakingInfo()
                {
                    Super = FormatPTValue(dr["s1"]),
                    Master = FormatPTValue(dr["m1"]),
                    Agent = FormatPTValue(dr["a1"]),
                    SuperAuto = FormatPTValue(dr["u1"]),
                    SuperIsAuto = CheckIsAutoBox(CastAide.AsBoolean(dr["i1"], false)),
                    MasterAuto = FormatPTValue(dr["um1"]),
                    MasterIsAuto = CheckIsAutoBox(CastAide.AsBoolean(dr["im1"], false)),
                    MaxPt = FormatPTValue(dr["l1"])
                };

                ptDTO.Member = memberPt;
            }

            if (roleId < (int)CustInfo.Roles.Master)
            {
                var agentPt = new PositionTakingInfo()
                {
                    Master = FormatPTValue(dr["m2"]),
                    Agent = FormatPTValue(dr["a2"]),
                    SuperAuto = FormatPTValue(dr["um2"]),
                    SuperIsAuto = CheckIsAutoBox(CastAide.AsBoolean(dr["im2"], false)),
                    MasterAuto = FormatPTValue(dr["um2"]),
                    MasterIsAuto = CheckIsAutoBox(CastAide.AsBoolean(dr["im2"], false)),
                    MinPt = FormatPTValue(dr["n2"]),
                    MaxPt = FormatPTValue(dr["l2"])
                };

                ptDTO.Agent = agentPt;
            }

            if (roleId < (int)CustInfo.Roles.Super)
            {
                var masterPt = new PositionTakingInfo()
                {
                    Super = FormatPTValue(dr["s3"]),
                    Master = FormatPTValue(dr["m3"]),
                    SuperAuto = FormatPTValue(dr["u3"]),
                    SuperIsAuto = CheckIsAutoBox(CastAide.AsBoolean(dr["i3"], false)),
                    MinPt = FormatPTValue(dr["n3"]),
                    MaxPt = FormatPTValue(dr["l3"])
                };

                ptDTO.Master = masterPt;
            }

            if (roleId <= (int)CustInfo.Roles.Super)
            {
                var superPt = new PositionTakingInfo()
                {
                    Super = FormatPTValue(dr["s4"]),
                    Master = FormatPTValue(dr["m4"]),
                    Agent = FormatPTValue(dr["a4"]),
                    MinPt = FormatPTValue(dr["n4"]),
                    MaxPt = FormatPTValue(dr["l4"])
                };

                ptDTO.Super = superPt;
            }
        }

        private string FormatPTValue(object ptValue)
        {
            return NumberFormat.DecFormat(ptValue, NumberOfDecimalDigits);
        }

        private string CheckIsAutoBox(bool isCheck)
        {
            return isCheck ? " checked='checked'" : string.Empty;
        }
    }
}

namespace Sunplus.Accounting.Business.PT.ViewPT
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using DNAide.Core;
    using DNAide.Data;
    using DNAide.Utils;
    using Fanex.Tarus.Infrastructure.Extensions;
    using Sunplus.Accounting.Business.Security;
    using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
    using Sunplus.Accounting.Entities.Criteria.ViewPT;
    using Sunplus.Accounting.Entities.Customer.PT.ViewPT;
    using Sunplus.Accounting.Entities.PositionTaking.Racing;

    public class GoldDeluxePT
    {
        private static byte NumberOfDecimalDigits
        {
            get
            {
                return (byte)2;
            }
        }

        public ViewPtDTO ViewPositionTaking(ViewGoldDeluxePTCriteria criteria)
        {
            var result = new ViewPtDTO();
            var rawData = new ViewGoldDeluxePTRepository().GetPT(criteria);

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

            var dataRowCustInfo = rawData.ResultSet.Tables[0].Rows[0];
            var roleId = CastAide.AsInt32(dataRowCustInfo["roleid"], 1);
            result.CustomerInfo = GetCustomerRelation(rawData);
            result.IsCasino = false;
            result.IsGoldDeluxe = true;
            result.PotionTakingDetail = PositionTakingDetail(rawData, roleId, criteria.SportId);
            return result;
        }

        /// <summary>
        /// Gets the customer relation.
        /// </summary>
        /// <param name="data">The data information.</param>
        /// <returns>The customer relation.</returns>
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public CustomerRelation GetCustomerRelation(DbResult data)
        {
            var customerRelation = new CustomerRelation
            {
                Upline = new List<CustomerInfo>()
            };

            var dataRowCustInfo = data.ResultSet.Tables[0].Rows[0];
            var roleId = CastAide.AsInt32(dataRowCustInfo["roleid"], 1);
            if (roleId < (int)CustInfo.Roles.Agent)
            {
                var agentInfo = new CustomerInfo()
                {
                    CustId = CastAide.AsInt32(dataRowCustInfo["AgID"].ToString()),
                    UserName = dataRowCustInfo["Ag"].ToString(),
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
                    CustId = CastAide.AsInt32(dataRowCustInfo["MAgID"].ToString()),
                    UserName = dataRowCustInfo["MAg"].ToString(),
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
                    CustId = CastAide.AsInt32(dataRowCustInfo["SAgID"].ToString()),
                    UserName = dataRowCustInfo["SAg"].ToString(),
                    RoleId = (int)CustInfo.Roles.Super,
                    RoleName = ResourceExtension.GetResourceValue(
                                         ResourceExtension.Resources.Language, "Super")
                };
                customerRelation.Upline.Add(superInfo);
            }

            customerRelation.Customer = new CustomerInfo()
            {
                CustId = CastAide.AsInt32(dataRowCustInfo["custid"].ToString()),
                UserName = dataRowCustInfo["username"].ToString(),
                RoleId = (int)CustInfo.Roles.Member
            };

            return customerRelation;
        }

        /// <summary>
        /// Gets the position taking detail.
        /// </summary>
        /// <param name="data">The data information.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <param name="sportId">The sport identifier.</param>
        /// <returns>The position taking detail.</returns>
        private List<PositionTakingDTO> PositionTakingDetail(DbResult data, int roleId, int sportId)
        {
            var positionTakingDetail = new List<PositionTakingDTO>();
            if (data == null || data.ResultSet.Tables.Count < 2)
            {
                return positionTakingDetail;
            }

            var positionTakingData = data.ResultSet.Tables[1];
            var positionTakingInfo = data.ResultSet.Tables[0].Rows[0];
            string currencyColumnName = "currency";
            var currencyOfCustomer = positionTakingInfo.Table.Columns.Contains(currencyColumnName) ?
                CastAide.AsInt32(positionTakingInfo[currencyColumnName]) : 1;

            foreach (DataRow dr in positionTakingData.Rows)
            {
                var positionTakingCurrent = new PositionTakingDTO();
                var betType = CastAide.AsInt32(dr["bettype"]);
                var sportType = CastAide.AsInt32(dr["sporttype"].ToString());
                var currencyAllowForTote = string.Empty;

                AssignSportInfo(ref positionTakingCurrent, dr, positionTakingInfo);
                AssignPtDetail(ref positionTakingCurrent, dr, roleId);
                positionTakingDetail.Add(positionTakingCurrent);
            }

            return positionTakingDetail;
        }

        /// <summary>
        /// Assigns the sport information.
        /// </summary>
        /// <param name="positionTakingDTO">The position taking DTO.</param>
        /// <param name="dr">The data row.</param>
        /// <param name="positionTakingInfo">The position taking information.</param>
        private void AssignSportInfo(
            ref PositionTakingDTO positionTakingDTO,
            DataRow dr,
            DataRow positionTakingInfo)
        {
            positionTakingDTO.SportName = CastAide.AsString(dr["sportname"]);
            positionTakingDTO.SportType = CastAide.AsString(dr["sporttype"]);
            positionTakingDTO.Bettype = CastAide.AsString(dr["bettype"]);
            positionTakingDTO.TypeName = CastAide.AsString(dr["typename"]);

            positionTakingDTO.PermistionEnableAutoMaster = Permissions.IsEnabledAutoMaster(!CastAide.AsBoolean(positionTakingInfo["disableautompt"], false));
            positionTakingDTO.EnableAutoMaster = !Sunplus.Accounting.Core.Configurations.WebSettings.disableAutoM;
            positionTakingDTO.DisableFlagCss = !positionTakingDTO.PermistionEnableAutoMaster ? "disableFlag" : "OpenFlag";
            positionTakingDTO.DisableAutoCss = !positionTakingDTO.PermistionEnableAutoMaster ? "disColorMPT" : string.Empty;
        }

        /// <summary>
        /// Assigns the position taking detail.
        /// </summary>
        /// <param name="positionTakingDTO">The position taking DTO.</param>
        /// <param name="dr">The data row.</param>
        /// <param name="roleId">The role identifier.</param>
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private void AssignPtDetail(
            ref PositionTakingDTO positionTakingDTO,
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
                    MaxPt = FormatPTValue(dr["l1"]),
                    Com = IBCUtils.FormatHandler.FormatNumber4(dr["playerdiscount"])
                };

                positionTakingDTO.Member = memberPt;
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
                    MaxPt = FormatPTValue(dr["l2"]),
                    Com = IBCUtils.FormatHandler.FormatNumber4(dr["discount"])
                };

                positionTakingDTO.Agent = agentPt;
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
                    MaxPt = FormatPTValue(dr["l3"]),
                    Com = IBCUtils.FormatHandler.FormatNumber4(dr["adiscount"])
                };

                positionTakingDTO.Master = masterPt;
            }

            if (roleId <= (int)CustInfo.Roles.Super)
            {
                var superPt = new PositionTakingInfo()
                {
                    Super = FormatPTValue(dr["s4"]),
                    Master = FormatPTValue(dr["m4"]),
                    Agent = FormatPTValue(dr["a4"]),
                    MinPt = FormatPTValue(dr["n4"]),
                    MaxPt = FormatPTValue(dr["l4"]),
                    Com = IBCUtils.FormatHandler.FormatNumber4(dr["sdiscount"])
                };

                positionTakingDTO.Super = superPt;
            }
        }

        private string FormatPTValue(object positionTakingValue)
        {
            return NumberFormat.DecFormat(positionTakingValue, NumberOfDecimalDigits);
        }

        private string CheckIsAutoBox(bool isCheck)
        {
            return isCheck ? " checked='checked'" : string.Empty;
        }
    }
}

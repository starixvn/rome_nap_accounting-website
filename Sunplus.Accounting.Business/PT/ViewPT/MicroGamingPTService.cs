namespace Sunplus.Accounting.Business.PT.ViewPT
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using DNAide.Data;
    using DNAide.Utils;
    using Fanex.Common;
    using Fanex.Tarus.Infrastructure.Configurations.MicroGamingBetTypes;
    using Fanex.Tarus.Infrastructure.Constants;
    using Sunplus.Accounting.Core.Extensions;
    using Sunplus.Accounting.DataAccess.IBCDLL.Customer.MicroGaming;
    using Sunplus.Accounting.Entities.Criteria.ViewPT;
    using Sunplus.Accounting.Entities.Customer.PT.ViewPT;

    public class MicroGamingPTService
    {
        private IMicroGamingPTRepository _viewPTRepository;

        public MicroGamingPTService()
            : this(null)
        {
        }

        public MicroGamingPTService(IMicroGamingPTRepository viewPTRepository)
        {
            _viewPTRepository = viewPTRepository ?? new MicroGamingPTRepository();
        }

        public static byte NumberOfDecimalDigits
        {
            get
            {
                return (byte)2;
            }
        }

        public ViewPtDTO ViewPositionTaking(ViewMicroGamingPTCriteria criteria)
        {
            var rawData = _viewPTRepository.GetPT(criteria);
            var viewPtDTO = new ViewPtDTO
            {
                ErrorMessage = Converter.ToInt(rawData.Outputs["errcode"], -1) == 0 ?
                    string.Empty :
                    rawData.Outputs["errmsg"].ToString(),
                IsMicroGaming = criteria.IsMicroGaming,
                IsAGCasino = criteria.IsAGCasino
            };

            if (rawData.ResultSet.Tables.Count == 0)
            {
                viewPtDTO.ErrorMessage = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata");
                return viewPtDTO;
            }

            var dataRowCustInfo = rawData.ResultSet.Tables[0].Rows[0];
            var roleId = Converter.ToInt(dataRowCustInfo["roleid"], 1);
            viewPtDTO.CustomerInfo = GetCustomerRelation(rawData);
            viewPtDTO.PotionTakingDetail = PositionTakingDetail(rawData, roleId, criteria.SportId);
            return viewPtDTO;
        }

        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        private CustomerRelation GetCustomerRelation(DbResult data)
        {
            var customerRelation = new CustomerRelation
            {
                Upline = new List<CustomerInfo>()
            };

            var dataRowCustInfo = data.ResultSet.Tables[0].Rows[0];
            var roleId = Converter.ToInt(dataRowCustInfo["roleid"], 1);
            if (roleId < RoleIdConstants.Agent)
            {
                var agentInfo = new CustomerInfo()
                {
                    CustId = Converter.ToInt(dataRowCustInfo["AgID"].ToString()),
                    UserName = dataRowCustInfo["Ag"].ToString(),
                    RoleId = RoleIdConstants.Agent,
                    RoleName = RoleIdConstants.RoleName(RoleIdConstants.Agent)
                };

                customerRelation.Upline.Add(agentInfo);
            }

            if (roleId < RoleIdConstants.Master)
            {
                var masterInfo = new CustomerInfo()
                {
                    CustId = Converter.ToInt(dataRowCustInfo["MAgID"].ToString()),
                    UserName = dataRowCustInfo["MAg"].ToString(),
                    RoleId = RoleIdConstants.Master,
                    RoleName = RoleIdConstants.RoleName(RoleIdConstants.Master)
                };

                customerRelation.Upline.Add(masterInfo);
            }

            if (roleId < RoleIdConstants.Super)
            {
                var superInfo = new CustomerInfo()
                {
                    CustId = Converter.ToInt(dataRowCustInfo["SAgID"].ToString()),
                    UserName = dataRowCustInfo["SAg"].ToString(),
                    RoleId = RoleIdConstants.Super,
                    RoleName = RoleIdConstants.RoleName(RoleIdConstants.Super)
                };
                customerRelation.Upline.Add(superInfo);
            }

            customerRelation.Customer = new CustomerInfo()
            {
                CustId = Converter.ToInt(dataRowCustInfo["custid"].ToString()),
                UserName = dataRowCustInfo["username"].ToString(),
                RoleId = RoleIdConstants.Member
            };

            return customerRelation;
        }

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
                Converter.ToInt(positionTakingInfo[currencyColumnName]) : 1;

            foreach (DataRow dr in positionTakingData.Rows)
            {
                var positionTakingCurrent = new PositionTakingDTO();
                var betType = Converter.ToInt(dr["bettype"]);
                var sportType = Converter.ToInt(dr["sporttype"].ToString());
                var currencyAllowForTote = string.Empty;

                AssignSportInfo(positionTakingCurrent, dr, positionTakingInfo);
                AssignPtDetail(positionTakingCurrent, dr, roleId);
                positionTakingDetail.Add(positionTakingCurrent);
            }

            return positionTakingDetail;
        }

        private void AssignSportInfo(
            PositionTakingDTO positionTakingDTO,
            DataRow dr,
            DataRow positionTakingInfo)
        {
            var betType = Converter.ToInt(dr["bettype"]);

            positionTakingDTO.SportName = Converter.ToString2(dr["sportname"]);
            positionTakingDTO.SportType = Converter.ToString2(dr["sporttype"]);
            positionTakingDTO.Bettype = Converter.ToString2(betType);
            positionTakingDTO.TypeName = string.Format(
                "{0} - {1}",
                Converter.ToString2(dr["typename"]),
                (new MicroGamingBetTypesSectionServices()).GetGameTypeName(betType));
            positionTakingDTO.EnableAutoMaster = !Sunplus.Accounting.Core.Configurations.WebSettings.disableAutoM;
            positionTakingDTO.DisableFlagCss = "disableFlag";
            positionTakingDTO.DisableAutoCss = "disColorMPT";
        }

        private void AssignPtDetail(
            PositionTakingDTO positionTakingDTO,
            DataRow dr,
            int roleId)
        {
            if (roleId < RoleIdConstants.Agent)
            {
                var memberPt = new PositionTakingInfo()
                {
                    Super = FormatPTValue(dr["s1"]),
                    Master = FormatPTValue(dr["m1"]),
                    Agent = FormatPTValue(dr["a1"]),
                    MaxPt = FormatPTValue(dr["l1"]),
                    Com = "0",
                    SuperAuto = "0.00",
                    SuperIsAuto = string.Empty,
                    MasterAuto = "0.00",
                    MasterIsAuto = string.Empty
                };

                positionTakingDTO.Member = memberPt;
            }

            if (roleId < RoleIdConstants.Master)
            {
                var agentPt = new PositionTakingInfo()
                {
                    Master = FormatPTValue(dr["m2"]),
                    Agent = FormatPTValue(dr["a2"]),
                    MinPt = FormatPTValue(dr["n2"]),
                    MaxPt = FormatPTValue(dr["l2"]),
                    Com = "0",
                    SuperAuto = "0.00",
                    SuperIsAuto = string.Empty,
                    MasterAuto = "0.00",
                    MasterIsAuto = string.Empty
                };

                positionTakingDTO.Agent = agentPt;
            }

            if (roleId < RoleIdConstants.Super)
            {
                var masterPt = new PositionTakingInfo()
                {
                    Super = FormatPTValue(dr["s3"]),
                    Master = FormatPTValue(dr["m3"]),
                    MinPt = FormatPTValue(dr["n3"]),
                    MaxPt = FormatPTValue(dr["l3"]),
                    Com = "0",
                    SuperAuto = "0.00",
                    SuperIsAuto = string.Empty,
                    MasterAuto = "0.00",
                    MasterIsAuto = string.Empty
                };

                positionTakingDTO.Master = masterPt;
            }

            if (roleId <= RoleIdConstants.Super)
            {
                var superPt = new PositionTakingInfo()
                {
                    Super = FormatPTValue(dr["s4"]),
                    Master = FormatPTValue(dr["m4"]),
                    Agent = FormatPTValue(dr["a4"]),
                    MinPt = FormatPTValue(dr["n4"]),
                    MaxPt = FormatPTValue(dr["l4"]),
                    Com = "0",
                    SuperAuto = "0.00",
                    SuperIsAuto = string.Empty,
                    MasterAuto = "0.00",
                    MasterIsAuto = string.Empty
                };

                positionTakingDTO.Super = superPt;
            }
        }

        private string FormatPTValue(object positionTakingValue)
        {
            return NumberFormat.DecFormat(positionTakingValue, NumberOfDecimalDigits);
        }
    }
}

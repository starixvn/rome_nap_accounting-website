using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DNAide.Core;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess.IBetAggregatorDAO;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.BetAggregator;

namespace Sunplus.Accounting.Business.BetAggregator
{
    public class LicenseeConfigManager : ILicenseeConfigManager
    {
        private readonly ILicenseeConfigDAO _licenseeConfigDao;

        public LicenseeConfigManager()
        {
        }

        public LicenseeConfigManager(ILicenseeConfigDAO licenseeConfigDAO)
        {
            _licenseeConfigDao = licenseeConfigDAO;
        }

        public List<LicenseeConfig> SelAllLicensee()
        {
            var listLicensee = new List<LicenseeConfig>();
            var dbResult = _licenseeConfigDao.SelectAllLicensee();
            foreach (DataRow row in dbResult.Rows)
            {
                var licenseeConfig = new LicenseeConfig();
                licenseeConfig.Id = CastAide.AsInt32(row["LicenseeID"]);
                licenseeConfig.Name = CastAide.AsString(row["LicenseeName"]);

                listLicensee.Add(licenseeConfig);
            }

            return listLicensee;
        }

        public List<LicenseeConfig> SelBaLicenseeList()
        {
            var listLicensee = new List<LicenseeConfig>();
            var dbResult = _licenseeConfigDao.SelectLicenseeConfigList();

            if (dbResult != null || dbResult.Rows.Count > 0)
            {
                foreach (DataRow row in dbResult.Rows)
                {
                    var licenComm = new LicenseeConfig
                    {
                        Id = CastAide.AsInt32(row["LicenseeID"]),
                        Name = CastAide.AsString(row["LicenseeName"]),
                        ValueGroupA = CastAide.AsDouble(row["groupa"]),
                        ValueGroupB = CastAide.AsDouble(row["groupb"]),
                        ValueGroupC = CastAide.AsDouble(row["groupc"]),
                        ValueGroupD = CastAide.AsDouble(row["groupd"]),
                        //ValueGroup = CastAide.AsString(row["ugroup"]),
                        Value1X2Comm = CastAide.AsDouble(row["1x2Comm"]),
                        ValueOtherComm = CastAide.AsDouble(row["OtherComm"]),
                        ModifyBy = CastAide.AsString(row["ModifyBy"]),
                        ModifyDate = CastAide.AsString(row["ModifyDate"]),
                        EnableBa = CastAide.AsBoolean(row["EnableBA"])
                    };

                    listLicensee.Add(licenComm);
                }
            }

            return listLicensee;
        }

        public List<LicenseeLog> SelBaLicenseeLogList(int licenseeId)
        {
            var LicenseeLogs = new List<LicenseeLog>();

            var dt = _licenseeConfigDao.SelLicenseeConfigCommLog(licenseeId);

            if (dt != null && dt.Rows.Count > 0)
            {
                List<string> licensees = dt.AsEnumerable().Select(s => CastAide.AsString(s["LicenseeName"])).Distinct().ToList();

                foreach (string licensee in licensees)
                {
                    var LicenseeLog = new LicenseeLog { Items = new List<LicenseeConfigLog>() };
                    DataRow[] rows = dt.Select("LicenseeName='" + licensee + "'");
                    LicenseeLog.Name = licensee;
                    foreach (DataRow row in rows)
                    {
                        var licenseeCommLog = new LicenseeConfigLog
                                                  {
                                                      Id = CastAide.AsInt32(row["LicenseeID"]),
                                                      Name = CastAide.AsString(row["LicenseeName"]),
                                                      ValueGroupA = CastAide.AsDouble(row["groupa"]),
                                                      ValueGroupB = CastAide.AsDouble(row["groupb"]),
                                                      ValueGroupC = CastAide.AsDouble(row["groupc"]),
                                                      ValueGroupD = CastAide.AsDouble(row["groupd"]),
                                                      //ValueGroup = CastAide.AsString(row["ugroup"]),
                                                      Value1X2Comm = CastAide.AsDouble(row["1x2Comm"]),
                                                      ValueOtherComm = CastAide.AsDouble(row["OtherComm"]),
                                                      ModifyBy = CastAide.AsString(row["ModifyBy"]),
                                                      ModifyDate = CastAide.AsString(row["ModifyDate"]),
                                                      EnableBa = CastAide.AsBoolean(row["EnableBA"]),
                                                      Description = CastAide.AsString(row["description"]),
                                                      // IP = CastAide.AsString(row["IP"])
                                                      IP = IpManagement.ReplaceVNIP(CastAide.AsString(row["IP"]))
                                                  };
                        LicenseeLog.Items.Add(licenseeCommLog);
                    }
                    LicenseeLogs.Add(LicenseeLog);
                }
            }
            return LicenseeLogs;
        }

        public Result InsertConfig(int licenseeId, double hdpComm, double oneTwoComm, double otherComm, double gameComm, bool enableBa)
        {
            var result = new Result();
            result = _licenseeConfigDao.InsertLicenseeConfig(licenseeId, hdpComm, oneTwoComm, otherComm, gameComm,
                                                             enableBa);
            return result;
        }

        public Result UpdateConfig(int licenseeId, double groupa, double groupb, double groupc, double groupd, double oneXtwoComm, double otherComm, bool enableBa)
        {
            var result = new Result();
            result = _licenseeConfigDao.UpdateLicenseeConfig(licenseeId, groupa, groupb, groupc, groupd, oneXtwoComm, otherComm,
                                                             enableBa);
            return result;
        }

        public Result DeleteConfig(int licenseeId)
        {
            var result = new Result();
            result = _licenseeConfigDao.DeleteLicenseeConfig(licenseeId);
            return result;
        }

        public int CheckDownlineBA(int licenseeId)
        {
            return _licenseeConfigDao.CheckDownlineBA(licenseeId);
        }

        public List<Sunplus.Accounting.Entities.BetAggregator.AmtLimitLicensee> GetAmtLimitLicensees()
        {
            var licenseeConfigDAO = CommonBootStrapper.Locator.GetInstance<ILicenseeConfigDAO>();
            var dataTable = licenseeConfigDAO.GetAmtLimitLicensees();
            if (dataTable == null) return null;
            var listResult = new List<Sunplus.Accounting.Entities.BetAggregator.AmtLimitLicensee>();
            foreach (DataRow row in dataTable.Rows)
            {
                listResult.Add(new Sunplus.Accounting.Entities.BetAggregator.AmtLimitLicensee
                {
                    LicenseeID = DNAide.Core.CastAide.AsInt32(row["LicenseeID"]),
                    LicenseeName = DNAide.Core.CastAide.AsString(row["LicenseeName"]),
                    LimitAmt = DNAide.Core.CastAide.AsDouble(row["LimitAmt"])
                });
            }
            return listResult;
        }

        public Result BALimitWarning(int licenseeID, string licenseeName, double limitAmt)
        {
            var licenseeConfigDAO = CommonBootStrapper.Locator.GetInstance<ILicenseeConfigDAO>();
            var retValue = new Result() { ErrorCode = 0, ErrorMessage = string.Empty };
            var result = licenseeConfigDAO.BALimitWarning(licenseeID, licenseeName, limitAmt);
            if (result != null)
            {

                var errMessage = DNAide.Core.CastAide.AsString(result.Outputs["errmsg"]);

                if (!string.IsNullOrEmpty(errMessage))
                {
                    if (result.ResultSet != null && result.ResultSet.Tables.Count > 0 && result.ResultSet.Tables[0].Rows.Count > 0)
                    {
                        var dataRow = result.ResultSet.Tables[0].Rows[0];
                        return new Result
                                   {
                                       ErrorCode = 1,
                                       ErrorMessage = dataRow["winlostamt"].ToString()
                                   };
                    }
                }

            }
            return retValue;
        }

        #region select currency group

        public BACurrenyGroupDTO SelBACurrencyGroupBy(int licenseeId)
        {
            var result = new BACurrenyGroupDTO();
            result.GroupSettings = new List<GroupSetting>();
            result.LicensesId = licenseeId.ToString();
            try
            {
                var groupsSetting = BASection.Instance[BASection.StoreCollection.CurrencyGroupSetting];
                var licenseeConfigDAO = CommonBootStrapper.Locator.GetInstance<ILicenseeConfigDAO>();
                var rs = licenseeConfigDAO.SelGroupCurrencyByLicensee(licenseeId);
                if (rs != null && rs.ResultSet.Tables != null && rs.ResultSet.Tables.Count > 0)
                {
                    var dtCurrency = rs.ResultSet.Tables[0];
                    var dtSetting = rs.ResultSet.Tables[1];
                    if (dtCurrency != null && dtCurrency.Rows.Count > 0)
                    {
                        #region assign all currencies of licensee

                        var currencyGroupInfo = new List<CurrencyInfo>();
                        currencyGroupInfo.Add(new CurrencyInfo()
                                                  {
                                                      Id = "0",
                                                      Name = "All",
                                                      Checked = false
                                                  });
                        foreach (DataRow dr in dtCurrency.Rows)
                        {
                            var currencyInfo = new CurrencyInfo()
                                                   {
                                                       Id = CastAide.AsString(dr["currency"]),
                                                       Name = CastAide.AsString(dr["currencyname"]),
                                                       Checked = false
                                                   };
                            currencyGroupInfo.Add(currencyInfo);
                        }

                        #endregion assign all currencies of licensee

                        #region get all groups and assign settign data

                        foreach (BAElement group in groupsSetting)
                        {
                            var groupId = group.Id;
                            var groupSetting = new GroupSetting()
                                                   {
                                                       Id = groupId,
                                                       GroupName = group.Title,
                                                       Currencies = new List<CurrencyInfo>()
                                                   };

                            var mappingSetting =
                                (from s in dtSetting.AsEnumerable()
                                 where
                                     s.Field<string>("ugroup").ToLower().Equals(groupId.ToLower()) &&
                                     s.Field<int>("Currency") != 0
                                 select s).ToList();

                            if (mappingSetting != null && mappingSetting.Count() > 0)
                            {
                                groupSetting.Currencies = new List<CurrencyInfo>();
                                foreach (var curr in currencyGroupInfo)
                                {
                                    var isChecked =
                                        (from r in mappingSetting
                                         where CastAide.AsInt32(r["currency"]) == CastAide.AsInt32(curr.Id)
                                         select r);

                                    groupSetting.Currencies.Add(new CurrencyInfo()
                                                                    {
                                                                        Id = curr.Id,
                                                                        Name = curr.Name,
                                                                        Checked =
                                                                            isChecked != null && isChecked.Count() > 0
                                                                                ? true
                                                                                : false
                                                                    });
                                }
                            }
                            else
                            {
                                groupSetting.Currencies = new List<CurrencyInfo>();
                                foreach (var curr in currencyGroupInfo)
                                {
                                    groupSetting.Currencies.Add(new CurrencyInfo()
                                                                    {
                                                                        Id = curr.Id,
                                                                        Name = curr.Name,
                                                                        Checked = false
                                                                    });
                                }
                            }

                            result.GroupSettings.Add(groupSetting);
                            groupSetting = new GroupSetting();
                            groupSetting.Currencies = new List<CurrencyInfo>();
                        }

                        #endregion get all groups and assign settign data
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        #endregion select currency group

        public Result BAUpdateGroupSetting(int licenseeId, string setting)
        {
            var licenseeConfigDAO = CommonBootStrapper.Locator.GetInstance<ILicenseeConfigDAO>();
            var result = new Result() { ErrorCode = 1, ErrorMessage = "Update Fail" };
            try
            {
                result = licenseeConfigDAO.UpdateGroupSettingByLicensee(licenseeId, setting);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }
    }
}
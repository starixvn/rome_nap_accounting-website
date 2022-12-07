using NPOI.SS.UserModel;

namespace Fanex.Tarus.Customer.Business.InitializingCustomerSetting
{
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting.Deposit;
    using Fanex.Tarus.Customer.DataAccess.InitializingCustomerSetting;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.InitializingCustomerSetting;
    using Fanex.Tarus.Customer.ViewModels.InitializingCustomerSetting;
    using NPOI.SS.UserModel;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public interface IInitializingDepositSettingService : IInitializingCreditSettingService
    {
        IEnumerable<BetSettingModel> GetBetSettings(int configId);

        ValidateBetSettingModel ParseBetSetting(HttpPostedFileBase request, int settingType);
    }
    public class InitializingDepositSettingService : InitializingCreditSettingService, IInitializingDepositSettingService
    {
        private const int OtherProductsSettingType = 2;
        private const int SportsbookSettingType = 10;
        private const int VirtualSport3SettingType = 11;
        private const int NumberGameSettingType = 12;

        public InitializingDepositSettingService() : base(new InittializingDepositSettingDataAccess())
        {
        }

        public InitializingDepositSettingService(InittializingDepositSettingDataAccess dataAccess) : base(dataAccess)
        {
        }

        public override InitializingCustomerSettingViewModel GetSetting()
        {
            var currencies = dataAccess.GetExchangeCurrency();
            var sites = dataAccess.GetSites();

            return dataAccess.GetSetting(new GetDepositSettingCriteria(), currencies, sites);
        }

        public IEnumerable<BetSettingModel> GetBetSettings(int configId)
        {
            return dataAccess.GetBetSetting(configId);
        }

        public override Result DeleteConfigSetting(int configId, int userId, string ip)
        {
            var criteria = new DeleteDepositConfigCriteria
            {
                ConfigId = configId,
                UserId = userId,
                IP = ip
            };

            return dataAccess.ExecuteNonQuery(criteria);
        }

        public override Result AbortConfigSetting(int configId, int userId, string ip)
        {
            var criteria = new UpdateDepositConfigStatusCriteria
            {
                ConfigId = configId,
                UserId = userId,
                Status = 3,
                IP = ip
            };

            return dataAccess.ExecuteNonQuery(criteria);
        }

        public override IEnumerable<InsertErrorModel> AddConfigSetting(ConfigModel request, string ip, int userId)
        {
            var criteria = new AddDepositSettingConfigCriteria(request, ip, userId)
            {
                IsConfirmed = true
            };

            return ProcessAddUpdateSetting(criteria, request);
        }

        public override IEnumerable<InsertErrorModel> UpdateConfigSetting(ConfigModel request, string ip, int userId)
        {
            request.BetSettings = CombineInputAndCurrentBetSetting(request).ToList();
            var criteria = new EditDepositSettingConfigCriteria(request, ip, userId)
            {
                IsConfirmed = true
            };

            return ProcessAddUpdateSetting(criteria, request);
        }
        

        public override ValidateResultModel ValidateAddConfigSetting(ConfigModel request, string ip, int userId)
        {
            var criteria = new AddDepositSettingConfigCriteria(request, ip, userId);

            return ProcessValidateSetting(criteria, request);
        }

        public override ValidateResultModel ValidateUpdateConfigSetting(ConfigModel request, string ip, int userId)
        {
            request.BetSettings = CombineInputAndCurrentBetSetting(request).ToList();
            var criteria = new EditDepositSettingConfigCriteria(request, ip, userId);

            return ProcessValidateSetting(criteria, request);
        }

        public ValidateBetSettingModel ParseBetSetting(HttpPostedFileBase request, int settingType)
        {
            var result = new ValidateBetSettingModel();
            var betSettings = new List<BetSettingModel>();
            int rowCount = 1;
            var currencies = dataAccess.GetExchangeCurrency(false);
            try
            {
                IWorkbook workbook = WorkbookFactory.Create(request.InputStream);
                ISheet sheet = workbook.GetSheetAt(0);
                IEnumerator rows = sheet.GetRowEnumerator();
                rows.MoveNext();
                while (rows.MoveNext())
                {
                    rowCount++;
                    IRow row = (IRow)rows.Current;
                    if (!IsHaveDataRow(row))
                    {
                        continue;
                    }

                    var sportType = int.Parse(row.GetCell(0).ToString());
                    var betType = int.Parse(row.GetCell(1).ToString());
                    var currencyCode = HttpUtility.HtmlEncode(row.GetCell(2).ToString().Replace(" ", string.Empty)).ToUpperInvariant();
                    var currencyName = row.GetCell(3).ToString().Replace(" ", string.Empty).ToUpperInvariant();
                    var minBet = GetCellDecimalValue(row.GetCell(4));
                    var maxBet = GetCellDecimalValue(row.GetCell(5));
                    var maxPerMatch = GetCellDecimalValue(row.GetCell(6));
                    var maxPerNumber = GetCellDecimalValue(row.GetCell(7));
                    var maxPayoutPerMatch = GetCellDecimalValue(row.GetCell(8));
                    var currency = currencies.FirstOrDefault(cur => cur.Name == currencyName)?.Id;

                    var betSetting = new BetSettingModel
                    {
                        RowPos = rowCount,
                        SettingType = settingType,
                        SportTypeId = sportType,
                        BetTypeId = betType,
                        CurrencyCode = currencyCode,
                        CurrencyName = currencyName,
                        MinBet = minBet,
                        MaxBet = maxBet,
                        MaxPerMatch = maxPerMatch,
                        MaxPerNumber = maxPerNumber,
                        MaxPayoutPerMatch = maxPayoutPerMatch,
                        Currency = currency ?? 0
                    };

                    betSettings.Add(betSetting);
                }

                result.BetSettings = betSettings;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = "Invalid file uploaded. Error at row: " + rowCount.ToString() + ". " + ex.Message;
                result.SettingType = settingType;
            }

            return result;
        }

        private decimal? GetCellDecimalValue(ICell cell)
        {
            if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) return null;

            if (cell.CellType == CellType.Numeric) return (decimal)cell.NumericCellValue;

            return decimal.Parse(cell.ToString());
        }

        protected override ValidateResultModel ProcessValidateSetting(ICriteria criteria, ConfigModel request)
        {
            if(request.BetSettings != null)
            {
                var validateBetSettingResult = ValidateBetSetting(request);
                if (!string.IsNullOrEmpty(validateBetSettingResult.ErrorMessage))
                {
                    return new ValidateResultModel()
                    {
                        BetSettingErrorMessage = validateBetSettingResult.ErrorMessage,
                        SettingType = validateBetSettingResult.SettingType
                    };

                }
            }

            return base.ProcessValidateSetting(criteria, request);
        }
        private ValidateBetSettingModel ValidateBetSetting(ConfigModel request)
        {
            var siteIds = new List<int>();
            var configCurrencyName = dataAccess.GetExchangeCurrency(false)
                .Where(cur => request.Config.CurrencyIdList.Contains(cur.Id))
                .Select(cur => cur.Name).ToList();

            var groupSettingBySettingType = request.Settings.Where(setting => setting.SettingName == SettingInforModel.BetSetting && setting.IsDefault)
                                                            .GroupBy(setting => setting.SettingType).ToDictionary(group => group.Key, group => group.ToList());

            foreach (var betSetting in request.BetSettings)
            {
                var checkRequiredFieldsResult = CheckRequiredField(betSetting);
                if (checkRequiredFieldsResult != string.Empty)
                {
                    return new ValidateBetSettingModel
                    {
                        ErrorMessage = checkRequiredFieldsResult,
                        SettingType = betSetting.SettingType
                    };
                }

                var checkMappingResult = CheckMapping(betSetting, configCurrencyName, groupSettingBySettingType[betSetting.SettingType]);
                if (checkMappingResult != string.Empty)
                {
                    return new ValidateBetSettingModel
                    {
                        ErrorMessage = checkMappingResult,
                        SettingType = betSetting.SettingType
                    };
                }
                var checkCorrectDataResult = CheckCorrectData(betSetting);
                if (checkCorrectDataResult != string.Empty)
                {
                    return new ValidateBetSettingModel
                    {
                        ErrorMessage = checkCorrectDataResult,
                        SettingType = betSetting.SettingType
                    };
                }
            }
            
            var checkMissingMappingResult = CheckMissingMapping(request.BetSettings, configCurrencyName, groupSettingBySettingType);
            if (checkMissingMappingResult.ErrorMessage != string.Empty)
            {
                return checkMissingMappingResult;
            }

            var checkDuplicateMappingResult = CheckDuplicateMapping(request.BetSettings);
            if (checkDuplicateMappingResult.ErrorMessage != string.Empty)
            {
                return checkDuplicateMappingResult;
            }

            return new ValidateBetSettingModel
            {
                ErrorMessage = string.Empty
            };
        }

        private bool IsHaveDataRow(IRow row)
        {
            for (int i = 0; i < 8; i++)
            {
                if (row.GetCell(i) != null && !string.IsNullOrWhiteSpace(row.GetCell(i).ToString()))
                {
                    return true;
                }
            }
            return false;
        }

        private string CheckRequiredField(BetSettingModel betSetting)
        {
            if (betSetting.SportTypeId == null)
            {
                return $"Row {betSetting.RowPos}: Sport Type is required info. Please check again.";
            }

            if (betSetting.BetTypeId == null)
            {
                return $"Row {betSetting.RowPos}: Bet Type is required info. Please check again.";
            }

            if (string.IsNullOrWhiteSpace(betSetting.CurrencyCode) || string.IsNullOrWhiteSpace(betSetting.CurrencyName))
            {
                return $"Row { betSetting.RowPos}: Currency is required info. Please check again.";
            }

            if (betSetting.MinBet == null)
            {
                return $"Row {betSetting.RowPos}: Min Bet is required info. Please check again.";
            }

            if (betSetting.MaxBet == null)
            {
                return $"Row {betSetting.RowPos}: Max Bet is required info. Please check again.";
            }

            if (betSetting.SettingType != VirtualSport3SettingType && betSetting.MaxPerMatch == null)
            {
                return $"Row {betSetting.RowPos}: Max per Match is required info. Please check again.";
            }

            if (betSetting.SettingType == NumberGameSettingType && betSetting.MaxPerNumber == null)
            {
                return $"Row {betSetting.RowPos}: Max per Number is required info. Please check again.";
            }

            if (betSetting.SettingType == SportsbookSettingType && betSetting.MaxPayoutPerMatch == null)
            {
                return $"Row {betSetting.RowPos}: Max Payout per Match is required info. Please check again.";
            }

            //extra data
            if (betSetting.SettingType == SportsbookSettingType && betSetting.MaxPerNumber != null)
            {
                return $"Row {betSetting.RowPos}: only need data for Sport Type, Bet Type, Currency Name, Min Bet, Max Bet, Max per Match, Max Payout per Match. Please check again.";
            }

            if (betSetting.SettingType == NumberGameSettingType && betSetting.MaxPayoutPerMatch != null)
            {
                return $"Row {betSetting.RowPos}: only need data for Sport Type, Bet Type, Currency Name, Min Bet, Max Bet, Max per Match, Max per Number. Please check again.";
            }

            if (betSetting.SettingType == VirtualSport3SettingType && (betSetting.MaxPerMatch != null || betSetting.MaxPerNumber != null || betSetting.MaxPayoutPerMatch != null))
            {
                return $"Row {betSetting.RowPos}: only need data for Sport Type, Bet Type, Currency Name, Min Bet, Max Bet. Please check again.";
            }

            if (betSetting.SettingType == OtherProductsSettingType && (betSetting.MaxPerNumber != null || betSetting.MaxPayoutPerMatch != null))
            {
                return $"Row {betSetting.RowPos}: only need data for Sport Type, Bet Type, Currency Name, Min Bet, Max Bet, Max per Match. Please check again.";
            }

            return string.Empty;
        }

        private string CheckMapping(BetSettingModel betSetting, List<string> currencyNames, List<SettingInforModel> settingInforModels)
        {
            if(!currencyNames.Contains(betSetting.CurrencyName.ToUpperInvariant()) ||
                !settingInforModels.Exists(setting => setting.SportType == betSetting.SportTypeId && setting.BetType == betSetting.BetTypeId))
            {
                return $"Row {betSetting.RowPos}: Sport Type/BetType/Currency in the upload file is invalid. Please check again.";
            }

            return string.Empty;
        }

        private ValidateBetSettingModel CheckMissingMapping(List<BetSettingModel> betSettings, List<string> currencyNames, Dictionary<int, List<SettingInforModel>>  groupSettingInforModels)
        {
            foreach (var key in groupSettingInforModels.Keys)
            {
                var settingInforModels = groupSettingInforModels[key];
                foreach (var setting in settingInforModels)
                {
                    foreach (var currency in currencyNames)
                    {
                        var missingCurrencies = new List<string>();
                        if (!betSettings.Exists(betSetting =>betSetting.SettingType == key && betSetting.CurrencyName.ToUpperInvariant() == currency && betSetting.SportTypeId == setting.SportType && betSetting.BetTypeId == setting.BetType))
                        {
                            missingCurrencies.Add(currency);
                        }

                        if (missingCurrencies.Count > 0)
                        {
                            return new ValidateBetSettingModel()
                            {
                                ErrorMessage = $"Config row {setting.Id}: missing row, currency name {string.Join(", ", missingCurrencies)}. Please check again.",
                                SettingType = key
                            };
                        }
                    }
                }
            }

            return new ValidateBetSettingModel()
            {
                ErrorMessage = string.Empty
            };
        }

        private ValidateBetSettingModel CheckDuplicateMapping(List<BetSettingModel> betSettings)
        {
            var groupBetSettings = betSettings.GroupBy(setting => setting.SettingType);
            foreach (var group in groupBetSettings)
            {
                var settings = group.ToList();
                var duplicateSettingsGroup = settings.GroupBy(setting => new { setting.SportTypeId, setting.BetTypeId, setting.CurrencyName });
                foreach (var settingGroup in duplicateSettingsGroup)
                {
                    if (settingGroup.Count() > 2)
                    {
                        var dublicateRowString = settingGroup.Select(setting => $"Row {setting.RowPos}");

                        return new ValidateBetSettingModel()
                        {
                            ErrorMessage = $"{string.Join(", ", dublicateRowString)} are duplicated. Please check again.",
                            SettingType = group.Key
                        };
                    }
                }
            }
            
            return new ValidateBetSettingModel()
            {
                ErrorMessage = string.Empty
            };
        }

        private string CheckCorrectData(BetSettingModel betSetting)
        {
            switch (betSetting.SettingType)
            {
                case SportsbookSettingType:
                    if(betSetting.MaxPayoutPerMatch < betSetting.MaxPerMatch
                        || betSetting.MaxPerMatch < betSetting.MaxBet
                        || betSetting.MaxBet < betSetting.MinBet
                        || betSetting.MinBet < 0)
                    {
                        return $"Row {betSetting.RowPos}: please follow the format \"0 <= Min Bet <= Max Bet <= Max per Match <= Max Payout per Match\".";
                    }
                    break;
                case NumberGameSettingType:
                    if (betSetting.MaxPerMatch < betSetting.MaxPerNumber
                        || betSetting.MaxPerNumber < betSetting.MaxBet
                        || betSetting.MaxBet < betSetting.MinBet
                        || betSetting.MinBet < 0)
                    {
                        return $"Row {betSetting.RowPos}: please follow the format \"0 <= Min Bet <= Max Bet <= Max per Number <= Max per Match\".";
                    }
                    break;
                case VirtualSport3SettingType:
                    if (betSetting.MaxBet < betSetting.MinBet
                        || betSetting.MinBet < 0)
                    {
                        return $"Row {betSetting.RowPos}: please follow the format \"0 <= Min Bet <= Max Bet\".";
                    }
                    break;
                case OtherProductsSettingType:
                    if (betSetting.MaxPerMatch < betSetting.MaxBet
                        || betSetting.MaxBet < betSetting.MinBet
                        || betSetting.MinBet < 0)
                    {
                        return $"Row {betSetting.RowPos}: please follow the format \"0 <= Min Bet <= Max Bet <= Max per Match\".";
                    }
                    break;
                default:
                    break;
            }

            return string.Empty;
        }

        private IEnumerable<BetSettingModel> CombineInputAndCurrentBetSetting(ConfigModel request)
        {
            var curBetSetting = GetBetSettings(request.Config.ConfigId);
            var betSettings = request.Settings.Where(st => st.SettingName == SettingInforModel.BetSetting);
            var isDefaultBetSetting = betSettings.Where(st => st.IsDefault).ToList();
            curBetSetting = curBetSetting.Where(st => isDefaultBetSetting.Exists(betSetting => betSetting.SettingType == st.SettingType && betSetting.SportType == st.SportTypeId && betSetting.BetType == st.BetTypeId));
            if (request.BetSettings != null && request.BetSettings.Count > 0)
            {
                var groupCurBetSetting = curBetSetting.GroupBy(setting => setting.SettingType).ToDictionary(gr => gr.Key, gr => gr.ToList());
                var inputBetSetting = request.BetSettings.GroupBy(setting => setting.SettingType);

                foreach (var group in inputBetSetting)
                {
                    if (groupCurBetSetting.ContainsKey(group.Key))
                    {
                        groupCurBetSetting[group.Key] = group.ToList();
                    } else
                    {
                        groupCurBetSetting.Add(group.Key, group.ToList());
                    }
                }

                return groupCurBetSetting.SelectMany(st => st.Value);
            }

            return curBetSetting;
        }
    }
}

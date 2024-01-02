namespace Fanex.Tarus.Customer.Business.InitializingCustomerSetting
{
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting.Deposit;
    using Fanex.Tarus.Customer.DataAccess.InitializingCustomerSetting;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.InitializingCustomerSetting;
    using Fanex.Tarus.Customer.Requests.InitializingCustomerSetting;
    using Fanex.Tarus.Customer.ViewModels.InitializingCustomerSetting;
    using Fanex.Tarus.Infrastructure.Common;
    using NPOI.SS.UserModel;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Mvc;

    public interface IInitializingDepositSettingService
    {
        InitializingCustomerSettingViewModel GetSetting();

        Result DeleteConfigSetting(int configId, int userId, string ip);

        Result AbortConfigSetting(int configId, int userId, string ip);

        ConfigSettingViewModel GetProductSportBettype();

        ConfigSettingViewModel GetConfigDetail(int configId, bool isView = false);

        ConfigSettingViewModel GenerateConfigSetting(GenerateConfigSettingRequest request);

        ValidateResultModel ValidateAddConfigSetting(ConfigModel request, string ip, int userId);

        IEnumerable<InsertErrorModel> AddConfigSetting(ConfigModel request, string ip, int userId);

        ValidateResultModel ValidateUpdateConfigSetting(ConfigModel request, string ip, int userId);

        IEnumerable<InsertErrorModel> UpdateConfigSetting(ConfigModel request, string ip, int userId);

        IEnumerable<BetSettingModel> GetBetSettings(int configId);

        ValidateBetSettingModel ParseBetSetting(HttpPostedFileBase request, int settingType);
    }
    public class InitializingDepositSettingService : IInitializingDepositSettingService
    {
        private const int OtherProductsSettingType = 2;
        private const int SportsbookSettingType = 10;
        private const int VirtualSport3SettingType = 11;
        private const int NumberGameSettingType = 12;

        private const string FormatDisplay = "[{0}] {1}";
        private const string ErrorMessage = "Row {0}: {1}";
        private const string HeadlineRegex = @"^[0-9a-zA-Z\s~,.?:;!|@#$%&*\(\)\{\}+-]+$";

        private readonly IInittializingDepositSettingDataAccess dataAccess;

        public InitializingDepositSettingService()
        {
            dataAccess = new InittializingDepositSettingDataAccess();
        }

        public InitializingDepositSettingService(InittializingDepositSettingDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public InitializingCustomerSettingViewModel GetSetting()
        {
            var currencies = dataAccess.GetExchangeCurrency();
            var sites = dataAccess.GetSites(AppSession.AccId);

            return dataAccess.GetSetting(new GetDepositSettingCriteria(), currencies, sites);
        }

        public IEnumerable<BetSettingModel> GetBetSettings(int configId)
        {
            return dataAccess.GetBetSetting(configId);
        }

        public Result DeleteConfigSetting(int configId, int userId, string ip)
        {
            var criteria = new DeleteDepositConfigCriteria
            {
                ConfigId = configId,
                UserId = userId,
                IP = ip
            };

            return dataAccess.ExecuteNonQuery(criteria);
        }

        public Result AbortConfigSetting(int configId, int userId, string ip)
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

        public IEnumerable<InsertErrorModel> AddConfigSetting(ConfigModel request, string ip, int userId)
        {
            var criteria = new AddDepositSettingConfigCriteria(request, ip, userId)
            {
                IsConfirmed = true
            };

            return ProcessAddUpdateSetting(criteria, request);
        }

        public IEnumerable<InsertErrorModel> UpdateConfigSetting(ConfigModel request, string ip, int userId)
        {
            request.BetSettings = CombineInputAndCurrentBetSetting(request).ToList();
            var criteria = new EditDepositSettingConfigCriteria(request, ip, userId)
            {
                IsConfirmed = true
            };

            return ProcessAddUpdateSetting(criteria, request);
        }

        private IEnumerable<InsertErrorModel> ProcessAddUpdateSetting(ICriteria criteria, ConfigModel request)
        {
            var resultMessages = dataAccess.AddUpdateConfigSetting(criteria);
            foreach (var msg in resultMessages)
            {
                msg.ErrorMsg = MappingMessageResult(msg, request.Settings);
            }

            return resultMessages;
        }


        public ValidateResultModel ValidateAddConfigSetting(ConfigModel request, string ip, int userId)
        {
            var criteria = new AddDepositSettingConfigCriteria(request, ip, userId);

            return ProcessValidateSetting(criteria, request);
        }

        public ValidateResultModel ValidateUpdateConfigSetting(ConfigModel request, string ip, int userId)
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
                result.ErrorMessage = "Invalid file uploaded. Error at row: " + rowCount.ToString() + ". ";
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

        private ValidateResultModel ProcessValidateSetting(ICriteria criteria, ConfigModel request)
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

            var result = ValidateHeadlines(request);

            if (!result.Errors.Any())
            {
                result = dataAccess.ValidateConfigSetting(criteria);
            }

            foreach (var msg in result.Errors)
            {
                msg.ErrorMsg = MappingMessageResult(msg, request.Settings);
            }

            return result;
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

            if (string.IsNullOrWhiteSpace(betSetting.CurrencyName))
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
                        var duplicatedRowString = settingGroup.Select(setting => $"Row {setting.RowPos}");

                        return new ValidateBetSettingModel()
                        {
                            ErrorMessage = $"{string.Join(", ", duplicatedRowString)} are duplicated. Please check again.",
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

        private ValidateResultModel ValidateHeadlines(ConfigModel request)
        {
            var result = new ValidateResultModel
            {
                Errors = new List<InsertErrorModel>()
            };

            foreach (var setting in request.Settings)
            {
                if (setting.IsRequiredHeadline)
                {
                    setting.SpecialRequirement = HttpUtility.HtmlDecode(setting.SpecialRequirement);
                    if (!string.IsNullOrEmpty(setting.SpecialRequirement)
                        && !Regex.IsMatch(setting.SpecialRequirement, HeadlineRegex))
                    {
                        result.Errors.Add(new InsertErrorModel
                        {
                            Id = setting.Id,
                            ErrorCode = -1,
                            ErrorMsg = "Invalid Headlines."
                        });
                    }
                }

            }
            return result;
        }

        private string MappingMessageResult(InsertErrorModel message, List<SettingInforModel> settings)
        {
            var setting = settings.FirstOrDefault(item => item.Id == message.Id);
            if (setting != null)
            {
                return string.Format(ErrorMessage, setting.Id, message.ErrorMsg);
            }

            return message.ErrorMsg;
        }

        public ConfigSettingViewModel GetProductSportBettype()
        {
            var productDcs = dataAccess.GetProductDcsModel();

            return new ConfigSettingViewModel
            {
                ProductModels = GenerateProductModels(productDcs)
            };
        }

        public ConfigSettingViewModel GetConfigDetail(int configId, bool isView = false)
        {
            var config = dataAccess.GetConfigDetail(new GetDepositConfigDetailCriteria(configId));
            if (config.Config == null)
            {
                return new ConfigSettingViewModel { Error = new Result(-1, "This row does not exist, please refresh again.") };
            }

            var productDcs = dataAccess.GetProductDcsModel();
            var allSettings = productDcs.Where(item => item.ProductId == config.Config.ProductId
                                                    && config.Config.SportTypeIdList.Contains(item.SportId)
                                                    && config.Config.BetTypeIdList.Contains(item.BettypeId));
            foreach (var setting in allSettings)
            {
                var existSetting = config.Settings.FirstOrDefault(cfs => cfs.SettingType == setting.SettingTypeId && cfs.SportType == setting.SportId && cfs.BetType == setting.BettypeId);
                if (existSetting == null)
                {
                    config.Settings.Add(new SettingInforModel(config.Settings.Count + 2, setting));
                }
                else
                {
                    existSetting.MaxDiscount = setting.MaxDiscount;
                    existSetting.IntervalValue = setting.IntervalValue;
                }
            }
            var sportbettypesetting = GetSportAndBettypeWithSetting(productDcs);
            var sites = dataAccess.GetSites(AppSession.AccId).ToList();
            var exchangeCurrencies = dataAccess.GetExchangeCurrency().ToList();
            var groupSettings = config.Settings.OrderBy(st => st.SettingOrder).ThenBy(st => st.SettingType).GroupBy(setting => setting.SettingType);

            var pos = 1;
            foreach (var group in groupSettings)
            {
                var selectedSetting = sportbettypesetting.FirstOrDefault(st => st.SettingName == group.FirstOrDefault().SettingName);
                foreach (var setting in group)
                {
                    setting.Id = pos;
                    setting.SportTypeOptions = BuildSportOptions(selectedSetting.Sports, setting.CopyFromSportType).ToList();
                    var selectedSport = selectedSetting.Sports.FirstOrDefault(sp => sp.SportId == setting.CopyFromSportType);
                    if (selectedSport == null)
                    {
                        selectedSport = selectedSetting.Sports.FirstOrDefault();
                        setting.CopyFromSportType = selectedSport.SportId;
                        setting.CopyFromBetType = selectedSport.Bettypes[0].BettypeId;
                    }
                    setting.BetTypeOptions = BuildBettypeOptions(selectedSport.Bettypes, setting.CopyFromBetType).ToList();
                    pos++;
                }
            }
            sites.RemoveAt(0);
            exchangeCurrencies.RemoveAt(0);
            var result = new ConfigSettingViewModel
            {
                ConfigDetail = config,
                SportBettypeBySettings = sportbettypesetting,
                GroupSettings = groupSettings,
                Sites = sites,
                Currencies = exchangeCurrencies
            };

            if (!isView)
            {
                result.ProductModels = GenerateProductModels(productDcs);
            }

            return result;
        }

        public ConfigSettingViewModel GenerateConfigSetting(GenerateConfigSettingRequest request)
        {
            var config = new SettingConfigModel()
            {
                ConfigId = request.ConfigId,
                BetTypeIdList = request.BettypeIdList,
                ProductId = request.ProductId,
                SportTypeIdList = request.SportIdList,
            };
            var productmatrix = dataAccess.GetProductDcsModel();
            var sportbettypesetting = GetSportAndBettypeWithSetting(productmatrix);

            var settings = productmatrix.Where(item => item.ProductId == request.ProductId
                                                    && request.SportIdList.Contains(item.SportId)
                                                    && request.BettypeIdList.Contains(item.BettypeId));
            var settingInfos = GenerateSettingInfoModel(settings.ToList()).ToList();
            var groupSettings = settingInfos.OrderBy(st => st.SettingOrder).ThenBy(st => st.SettingType).GroupBy(setting => setting.SettingType);

            var pos = 1;
            foreach (var settingGroup in groupSettings)
            {
                var sportBettype = sportbettypesetting.FirstOrDefault(st => st.SettingName == settingGroup.FirstOrDefault().SettingName);
                foreach (var setting in settingGroup)
                {
                    setting.Id = pos;
                    //setting.CopyFromSportType = sportBettype.Sports[0].SportId;
                    //setting.SportTypeOptions = BuildSportOptions(sportBettype.Sports, setting.CopyFromSportType).ToList();
                    //setting.CopyFromBetType = sportBettype.Sports[0].Bettypes[0].BettypeId;
                    //setting.BetTypeOptions = BuildBettypeOptions(sportBettype.Sports[0].Bettypes, setting.CopyFromBetType).ToList();
                    pos++;
                }
            }

            var configDetail = new ConfigModel
            {
                Config = config,
                Settings = settingInfos
            };

            var sites = dataAccess.GetSites(AppSession.AccId).ToList();

            var exchangeCurrencies = dataAccess.GetExchangeCurrency().ToList();

            sites.RemoveAt(0);
            exchangeCurrencies.RemoveAt(0);

            return new ConfigSettingViewModel
            {
                ConfigDetail = configDetail,
                Currencies = exchangeCurrencies,
                Sites = sites,
                SportBettypeBySettings = sportbettypesetting,
                GroupSettings = groupSettings
            };
        }
        
        private IEnumerable<ProductModel> GenerateProductModels(IEnumerable<ProductDcsModel> productDcsModels)
        {
            var result = new List<ProductModel>();
            foreach (var item in productDcsModels)
            {
                var product = new ProductModel(item.ProductId, string.Format(FormatDisplay, item.ProductId, item.ProductName));
                var sport = new SportModel(item.SportId, string.Format(FormatDisplay, item.SportId, item.SportName));
                var bettype = new BettypeModel(item.BettypeId, string.Format(FormatDisplay, item.BettypeId, item.BettypeName));
                var setting = new SettingTypeModel(item.SettingTypeId, item.SettingTypeName);
                var existProduct = result.FirstOrDefault(pd => pd.ProductId == item.ProductId);
                if (existProduct == null)
                {
                    bettype.SettingTypes.Add(setting);
                    sport.Bettypes.Add(bettype);
                    product.Sports.Add(sport);
                    result.Add(product);
                }
                else
                {
                    BuildExistSettingType(existProduct, sport, bettype, setting);
                }
            }

            return result;
        }

        private void BuildExistSettingType(ProductModel product, SportModel sport, BettypeModel bettype, SettingTypeModel setting)
        {
            var existSport = product.Sports.FirstOrDefault(sp => sp.SportId == sport.SportId);
            if (existSport == null)
            {
                bettype.SettingTypes.Add(setting);
                sport.Bettypes.Add(bettype);
                product.Sports.Add(sport);
            }
            else
            {
                var existBettype = existSport.Bettypes.FirstOrDefault(bt => bt.BettypeId == bettype.BettypeId);
                if (existBettype == null)
                {
                    bettype.SettingTypes.Add(setting);
                    existSport.Bettypes.Add(bettype);
                }
                else
                {
                    existBettype.SettingTypes.Add(setting);
                }
            }
        }

        private IEnumerable<SportBetTypeBySettingModel> GetSportAndBettypeWithSetting(IEnumerable<ProductDcsModel> productDcsModels)
        {
            var result = new List<SportBetTypeBySettingModel>();

            foreach (var item in productDcsModels)
            {
                var sport = new SportModel(item.SportId, string.Format(FormatDisplay, item.SportId, item.SportName));
                var bettype = new BettypeModel(item.BettypeId, string.Format(FormatDisplay, item.BettypeId, item.BettypeName));
                var setting = result.FirstOrDefault(x => x.SettingName == item.SettingTypeName);
                if (setting != null)
                {
                    var existSport = setting.Sports.FirstOrDefault(sp => sp.SportId == item.SportId);
                    if (existSport != null)
                    {
                        if (!existSport.Bettypes.Exists(bt => bt.BettypeId == item.BettypeId))
                        {
                            existSport.Bettypes.Add(bettype);
                        }
                    }
                    else
                    {
                        sport.Bettypes.Add(bettype);
                        setting.Sports.Add(sport);
                    }
                }
                else
                {
                    sport.Bettypes.Add(bettype);
                    result.Add(new SportBetTypeBySettingModel
                    {
                        SettingName = item.SettingTypeName,
                        Sports = new List<SportModel> { sport }
                    });
                }
            }

            foreach (var st in result)
            {
                if (st.SettingName == "Loss Limit")
                {
                    var winlossSetting = result.FirstOrDefault(rs => rs.SettingName == "Win Loss Limit");
                    if (winlossSetting != null)
                    {
                        GetSameSportBettypeFromSetting(st, winlossSetting);
                    }
                }

                if (st.SettingName == "Win Loss Limit")
                {
                    var lossSetting = result.FirstOrDefault(rs => rs.SettingName == "Loss Limit");
                    if (lossSetting != null)
                    {
                        GetSameSportBettypeFromSetting(st, lossSetting);
                    }
                }
            }

            return result;
        }
        private void GetSameSportBettypeFromSetting(SportBetTypeBySettingModel targetSetting, SportBetTypeBySettingModel sourceSetting)
        {
            foreach (var sp in sourceSetting.Sports)
            {
                if (!targetSetting.Sports.Exists(item => item.SportId == sp.SportId))
                {
                    targetSetting.Sports.Add(sp);
                }
            }
        }
        private IEnumerable<SettingInforModel> GenerateSettingInfoModel(List<ProductDcsModel> settings)
        {
            var result = new List<SettingInforModel>();
            for (int i = 0; i < settings.Count; i++)
            {
                result.Add(new SettingInforModel(i + 1, settings[i]));
            }

            return result;
        }

        private IEnumerable<SelectListItem> BuildSportOptions(IEnumerable<SportModel> sports, int selectedSport)
        {
            return sports.Select(sp => new SelectListItem { Value = sp.SportId.ToString(), Text = sp.SportName, Selected = selectedSport == sp.SportId });
        }

        private IEnumerable<SelectListItem> BuildBettypeOptions(IEnumerable<BettypeModel> bettypes, int selectedBettype)
        {
            return bettypes.Select(bt => new SelectListItem { Value = bt.BettypeId.ToString(), Text = bt.BettypeName, Selected = selectedBettype == bt.BettypeId });
        }
    }
}

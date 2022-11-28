namespace Fanex.Tarus.Customer.Business.InitializingCustomerSetting
{
    using Fanex.Tarus.Customer.ViewModels.InitializingCustomerSetting;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.InitializingCustomerSetting;
    using Fanex.Tarus.Customer.Requests.InitializingCustomerSetting;
    using Fanex.Tarus.Customer.DataAccess.InitializingCustomerSetting;
    using System.Linq;
    using System.Web.Mvc;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Web;
    using Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting.Credit;
    using Fanex.NAP.Framework.Data;

    public interface IInitializingCreditSettingService
    {
        InitializingCustomerSettingViewModel GetSetting();

        Result DeleteConfigSetting(int configId, int userId, string ip);

        Result AbortConfigSetting(int configId, int userId, string ip);

        ConfigSettingViewModel GetProductSportBettype();

        ConfigSettingViewModel GetConfigDetail(ICriteria criteria, bool isView = false);

        ConfigSettingViewModel GenerateConfigSetting(GenerateConfigSettingRequest request);

        ValidateResultModel ValidateAddConfigSetting(ConfigModel request, string ip, int userId);

        IEnumerable<InsertErrorModel> AddConfigSetting(ConfigModel request, string ip, int userId);

        ValidateResultModel ValidateUpdateConfigSetting(ConfigModel request, string ip, int userId);

        IEnumerable<InsertErrorModel> UpdateConfigSetting(ConfigModel request, string ip, int userId);
    }

    public class InitializingCreditSettingService : IInitializingCreditSettingService
    {
        protected readonly IInitializingSettingDataAccess dataAccess;

        private const string FormatDisplay = "[{0}] {1}";
        private const string ErrorMessage = "Row {0}: {1}";
        private const string HeadlineRegex = @"^[0-9a-zA-Z\s~,.?:;!|@#$%&*\(\)\{\}+-]+$";

        public InitializingCreditSettingService()
        {
            dataAccess = new InitializingCreditSettingDataAccess();
        }

        public InitializingCreditSettingService(IInitializingSettingDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public virtual Result AbortConfigSetting(int configId, int userId, string ip)
        {
            var criteria = new UpdateConfigStatusCriteria
            {
                ConfigId = configId,
                UserId = userId,
                Status = 3,
                IP = ip
            };

            return dataAccess.ExecuteNonQuery(criteria);
        }

        public virtual Result DeleteConfigSetting(int configId, int userId, string ip)
        {
            var criteria = new DeleteConfigCriteria
            {
                ConfigId = configId,
                UserId = userId,
                IP = ip
            };

            return dataAccess.ExecuteNonQuery(criteria);
        }

        public virtual ConfigSettingViewModel GetConfigDetail(ICriteria criteria, bool isView = false)
        {
            var config = dataAccess.GetConfigDetail(criteria);
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
            var sites = dataAccess.GetSites().ToList();
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

        public ConfigSettingViewModel GetProductSportBettype()
        {
            var productDcs = dataAccess.GetProductDcsModel();

            return new ConfigSettingViewModel
            {
                ProductModels = GenerateProductModels(productDcs)
            };
        }

        public virtual InitializingCustomerSettingViewModel GetSetting()
        {
            var currencies = dataAccess.GetExchangeCurrency();
            var sites = dataAccess.GetSites();
            return dataAccess.GetSetting(new GetSettingCriteria(), currencies, sites);
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
                    setting.CopyFromSportType = sportBettype.Sports[0].SportId;
                    setting.SportTypeOptions = BuildSportOptions(sportBettype.Sports, setting.CopyFromSportType).ToList();
                    setting.CopyFromBetType = sportBettype.Sports[0].Bettypes[0].BettypeId;
                    setting.BetTypeOptions = BuildBettypeOptions(sportBettype.Sports[0].Bettypes, setting.CopyFromBetType).ToList();
                    pos++;
                }
            }

            var configDetail = new ConfigModel
            {
                Config = config,
                Settings = settingInfos,
                BetSettings = new List<BetSettingModel>()
            };

            var sites = dataAccess.GetSites().ToList();

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

        private IEnumerable<SelectListItem> BuildSportOptions(IEnumerable<SportModel> sports, int selectedSport)
        {
            return sports.Select(sp => new SelectListItem { Value = sp.SportId.ToString(), Text = sp.SportName, Selected = selectedSport == sp.SportId });
        }

        private IEnumerable<SelectListItem> BuildBettypeOptions(IEnumerable<BettypeModel> bettypes, int selectedBettype)
        {
            return bettypes.Select(bt => new SelectListItem { Value = bt.BettypeId.ToString(), Text = bt.BettypeName, Selected = selectedBettype == bt.BettypeId });
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

        private string MappingMessageResult(InsertErrorModel message, List<SettingInforModel> settings)
        {
            var setting = settings.FirstOrDefault(item => item.Id == message.Id);
            if (setting != null)
            {
                return string.Format(ErrorMessage, setting.Id, message.ErrorMsg);
            }

            return message.ErrorMsg;
        }

        public virtual IEnumerable<InsertErrorModel> AddConfigSetting(ConfigModel request, string ip, int userId)
        {
            var criteria = new AddSettingConfigCriteria(request, ip, userId)
            {
                IsConfirmed = true
            };

            return ProcessAddUpdateSetting(criteria, request);
        }

        public virtual IEnumerable<InsertErrorModel> UpdateConfigSetting(ConfigModel request, string ip, int userId)
        {
            var criteria = new EditSettingConfigCriteria(request, ip, userId)
            {
                IsConfirmed = true
            };

            return ProcessAddUpdateSetting(criteria, request);
        }

        protected IEnumerable<InsertErrorModel> ProcessAddUpdateSetting(ICriteria criteria, ConfigModel request)
        {
            var resultMessages = dataAccess.AddUpdateConfigSetting(criteria);
            foreach (var msg in resultMessages)
            {
                msg.ErrorMsg = MappingMessageResult(msg, request.Settings);
            }

            return resultMessages;
        }

        public virtual ValidateResultModel ValidateAddConfigSetting(ConfigModel request, string ip, int userId)
        {
            var criteria = new AddSettingConfigCriteria(request, ip, userId);

            return ProcessValidateSetting(criteria, request);
        }

        public virtual ValidateResultModel ValidateUpdateConfigSetting(ConfigModel request, string ip, int userId)
        {
            var criteria = new EditSettingConfigCriteria(request, ip, userId);

            return ProcessValidateSetting(criteria, request);
        }

        protected virtual ValidateResultModel ProcessValidateSetting(ICriteria criteria, ConfigModel request)
        {
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
    }
}

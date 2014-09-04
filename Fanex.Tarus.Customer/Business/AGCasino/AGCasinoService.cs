// --------------------------------------------------------------------------
// <copyright file="AGCasinoService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Business.AGCasino
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Xml;
    using Fanex.Tarus.Customer.Criteria.AGCasino;
    using Fanex.Tarus.Customer.DataAccess.AGCasino;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.AGCasino;
    using Fanex.Tarus.Infrastructure.Configurations;
    using Fanex.Tarus.Infrastructure.Extensions;
    using Sunplus.Accounting.Business.Security;
    using Sunplus.Accounting.Core.Common;

    /// <summary>
    /// AG Casino Service.
    /// </summary>
    public class AGCasinoService : IAGCasinoService
    {
        private readonly IAGCasinoRepository _asianGamingCasinoRepository;

        public AGCasinoService()
            : this(null)
        {
        }

        public AGCasinoService(IAGCasinoRepository asianGamingCasinoRepository)
        {
            _asianGamingCasinoRepository = asianGamingCasinoRepository ?? new AGCasinoRepository();
        }

        /// <summary>
        /// Gets the customer status.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Customer Info.</returns>
        public AGCasinoCustomerInfo CustomerStatus(GetCustomerStatusCriteria criteria)
        {
            try
            {
                var customerSetting = _asianGamingCasinoRepository.CustomerStatus(criteria);
                customerSetting.ProductInfo = new List<ProductInfo> 
                { 
                    new ProductInfo
                    {
                        Id = "1",
                        Name = "AG Casino",
                        Enable = customerSetting.EnableAGCasino,
                        GameTypeGroup = new List<GameTypeGroup>(),
                        GameTypes = new List<GameType>()
                    }
                };

                return customerSetting;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }

        /// <summary>
        /// Updates the customer status.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Update Result.</returns>
        public Result UpdateCustomerStatus(UpdateCustomerStatusCriteria criteria)
        {
            try
            {
                Result updateResult = new Result();
                if (!Permissions.HasHostPermission(Enumerations.Functions.Customer.ViewInfo))
                {
                    updateResult.ErrorCode = 1;
                    updateResult.ErrorMessage = "You don't have permission on this function.";
                    return updateResult;
                }

                updateResult = _asianGamingCasinoRepository.UpdateCustomerStatus(criteria);
                if (updateResult.ErrorCode == 0)
                {
                    updateResult.ErrorMessage = "AG Casino status updated successfully";
                }

                return updateResult;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }

        /// <summary>
        /// Gets the customer setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// The customer setting.
        /// </returns>
        public AGCasinoCustomerInfo CustomerSetting(GetCustomerSettingCriteria criteria)
        {
            try
            {
                var customerSetting = _asianGamingCasinoRepository.CustomerSetting(criteria);
                customerSetting.SelectResult = new SelectResult();
                var apiResult = AGCasinoAPIResult(new GetAGCasinoAPIResultCriteria
                {
                    SiteName = customerSetting.SiteName,
                    Currency = customerSetting.ExchangeId
                });

                ParseAGCasinoAPIResult(apiResult, customerSetting);
                var apiHasData = customerSetting.SelectResult.Code == 0 &&
                    customerSetting.ProductInfo[0].GameTypeGroup.Count > 0;
                var hasChecked = !string.IsNullOrEmpty(customerSetting.AGCasinoCheckedGroup);
                if (!apiHasData && !hasChecked)
                {
                    customerSetting.SelectResult.Code = 1;
                    customerSetting.SelectResult.Message = "There is no bet setting group.";
                }
                else if (!apiHasData && hasChecked)
                {
                    customerSetting.SelectResult.Code = 1;
                    customerSetting.SelectResult.Message = "Data are not synchronized, please contact system admin.";
                }
                else
                {
                    MappingCheckedGroupCustomerSetting(customerSetting);
                }

                return customerSetting;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }

        /// <summary>
        /// Updates the customer setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Update Result.</returns>
        public Result UpdateCustomerSetting(UpdateCustomerSettingCriteria criteria)
        {
            try
            {
                Result updateResult = new Result();
                if (!Permissions.HasHostPermission(Enumerations.Functions.Customer.ViewInfo))
                {
                    updateResult.ErrorCode = 1;
                    updateResult.ErrorMessage = "You don't have permission on this function.";
                    return updateResult;
                }

                updateResult = _asianGamingCasinoRepository.UpdateCustomerSetting(criteria);
                if (updateResult.ErrorCode == 0)
                {
                    updateResult.ErrorMessage = "AG Casino setting updated successfully";
                }

                return updateResult;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }

        /// <summary>
        /// Gets Asian Gaming Casino API result.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Asian Gaming Casino API result.</returns>
        private string AGCasinoAPIResult(GetAGCasinoAPIResultCriteria criteria)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(WebSettings.AGCasinoAPILink);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";
                var parameters = "SiteName=" + criteria.SiteName + "&Currency=" + criteria.Currency;
                request.ContentLength = parameters.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    byte[] parameterBytes = Encoding.ASCII.GetBytes(parameters);
                    requestStream.Write(parameterBytes, 0, parameterBytes.Length);
                    requestStream.Close();
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(responseStream, Encoding.UTF8);
                return readStream.ReadToEnd();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Parses the ag casino API result.
        /// </summary>
        /// <param name="apiResult">The API result.</param>
        /// <param name="customerSetting">The customer setting.</param>
        private void ParseAGCasinoAPIResult(string apiResult, AGCasinoCustomerInfo customerSetting)
        {
            try
            {
                var asianGamingCasinoGameType = new ProductInfo
                {
                    Id = "1",
                    Name = "AG Casino",
                    Enable = customerSetting.EnableAGCasino,
                    GameTypeGroup = new List<GameTypeGroup>(),
                    GameTypes = new List<GameType>()
                };

                if (string.IsNullOrEmpty(apiResult))
                {
                    return;
                }

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(apiResult);
                string xpath = "/GroupBettingLimit";
                var groupBettingLimitNodes = xmlDoc.SelectNodes(xpath);
                foreach (XmlNode childrenNode in groupBettingLimitNodes)
                {
                    MappingGroupBettingLimitChildNodes(childrenNode, customerSetting, asianGamingCasinoGameType);
                }

                customerSetting.ProductInfo = new List<ProductInfo> { asianGamingCasinoGameType };
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        /// <summary>
        /// Mappings the group betting limit child nodes.
        /// </summary>
        /// <param name="xmlNode">The XML node.</param>
        /// <param name="customerSetting">The customer setting.</param>
        /// <param name="asianGamingCasinoGameType">Type of the asian gaming casino game.</param>
        private void MappingGroupBettingLimitChildNodes(
            XmlNode xmlNode,
            AGCasinoCustomerInfo customerSetting,
            ProductInfo asianGamingCasinoGameType)
        {
            foreach (XmlNode childrenNode in xmlNode)
            {
                switch (childrenNode.Name)
                {
                    case "BettingLimit":
                        MappingBettingLimitChildNodes(childrenNode, customerSetting, asianGamingCasinoGameType);
                        return;
                    case "ReturnCode":
                        customerSetting.SelectResult.Code = int.Parse(childrenNode.InnerText);
                        break;
                    case "ReturnMessage":
                        customerSetting.SelectResult.Message = childrenNode.InnerText;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Mappings the betting limit child nodes.
        /// </summary>
        /// <param name="bettingLimitNode">The betting limit node.</param>
        /// <param name="customerSetting">The customer setting.</param>
        /// <param name="asianGamingCasinoGameType">Type of the asian gaming casino game.</param>
        private void MappingBettingLimitChildNodes(
            XmlNode bettingLimitNode,
            AGCasinoCustomerInfo customerSetting,
            ProductInfo asianGamingCasinoGameType)
        {
            Dictionary<string, int> gameTypeGroup = GameTypeGroup(bettingLimitNode);
            foreach (var keyValue in gameTypeGroup)
            {
                asianGamingCasinoGameType.GameTypeGroup.Add(new GameTypeGroup
                {
                    Id = keyValue.Key,
                    Name = keyValue.Key
                });
            }

            foreach (XmlNode gameTypeNode in bettingLimitNode)
            {
                var gameType = new GameType
                {
                    Code = gameTypeNode.Attributes["GameType"].Value,
                    Name = gameTypeNode.Attributes["Name"].Value,
                    SettingLimitByGroup = InitListSettingLimit(asianGamingCasinoGameType.GameTypeGroup.Count)
                };

                gameType.Id = gameType.Name;
                foreach (XmlNode groupTypeNode in gameTypeNode)
                {
                    var groupName = groupTypeNode.Attributes["GroupType"].Value;
                    var groupIndex = gameTypeGroup[groupName];
                    gameType.SettingLimitByGroup[groupIndex] = ParseGroupTypeNode(groupTypeNode);
                }

                asianGamingCasinoGameType.GameTypes.Add(gameType);
            }
        }

        /// <summary>
        /// Gets the game type group.
        /// </summary>
        /// <param name="bettingLimitNode">The betting limit node.</param>
        /// <returns>The game type group.</returns>
        private Dictionary<string, int> GameTypeGroup(XmlNode bettingLimitNode)
        {
            Dictionary<string, int> gameTypeGroup = new Dictionary<string, int>();
            var index = 0;
            foreach (XmlNode gameTypeNode in bettingLimitNode)
            {
                foreach (XmlNode groupTypeNode in gameTypeNode)
                {
                    var groupName = groupTypeNode.Attributes["GroupType"].Value;
                    if (!gameTypeGroup.ContainsKey(groupName))
                    {
                        gameTypeGroup.Add(groupName, index++);
                    }
                }
            }

            return gameTypeGroup;
        }

        private List<SettingLimit> InitListSettingLimit(int numberOfGroup)
        {
            List<SettingLimit> settingLimitByGroup = new List<SettingLimit>();
            for (var i = 0; i < numberOfGroup; i++)
            {
                settingLimitByGroup.Add(null);
            }

            return settingLimitByGroup;
        }

        /// <summary>
        /// Parses the group type node.
        /// </summary>
        /// <param name="groupTypeNode">The group type node.</param>
        /// <returns>The Setting Limit.</returns>
        private SettingLimit ParseGroupTypeNode(XmlNode groupTypeNode)
        {
            SettingLimit settingLimit = new SettingLimit();
            var minNode = groupTypeNode["Min"];
            var maxNode = groupTypeNode["Max"];
            if (minNode == null || maxNode == null)
            {
                return settingLimit;
            }

            settingLimit.MinBet = ParseDecimalFromXML(minNode.InnerText);
            settingLimit.MaxBet = ParseDecimalFromXML(maxNode.InnerText);
            return settingLimit;
        }

        /// <summary>
        /// Parses the decimal from XML.
        /// </summary>
        /// <param name="xmlValue">The XML value.</param>
        /// <returns>The decimal value.</returns>
        private decimal ParseDecimalFromXML(string xmlValue)
        {
            if (string.IsNullOrEmpty(xmlValue))
            {
                return 0;
            }

            var decimalString = xmlValue.Replace(",", string.Empty);
            decimal decimalValue = 0;
            if (decimal.TryParse(decimalString, out decimalValue))
            {
                return decimalValue;
            }

            return 0;
        }

        /// <summary>
        /// Mappings the checked group customer setting.
        /// </summary>
        /// <param name="customerSetting">The customer setting.</param>
        private void MappingCheckedGroupCustomerSetting(AGCasinoCustomerInfo customerSetting)
        {
            var asianGamingProductInfo = customerSetting.ProductInfo[0];
            var checkGroupArray = customerSetting.AGCasinoCheckedGroup.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < asianGamingProductInfo.GameTypeGroup.Count; i++)
            {
                var group = asianGamingProductInfo.GameTypeGroup[i];
                var groupName = group.Name;
                customerSetting.ProductInfo[0].GameTypeGroup[i].Enable = checkGroupArray.Contains(groupName);
            }
        }
    }
}

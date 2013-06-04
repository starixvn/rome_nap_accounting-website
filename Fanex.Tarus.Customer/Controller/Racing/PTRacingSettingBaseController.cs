
namespace Fanex.Tarus.Customer.Controller.Racing
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.PTRacing.BusinessObjects;
    using Fanex.Tarus.Customer.Business.Racing;
    using Fanex.Tarus.Web.Controllers;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Sunplus.Accounting.Core.Extensions;
    using DNAide.Core;
using System.Text.RegularExpressions;

    public class PTRacingSettingBaseController : BaseController
    {
        protected static IRacingSettingService _racingSettingService;

        public PTRacingSettingBaseController(): this(null)
        { 
        }

        public PTRacingSettingBaseController(IRacingSettingService racingSettingService)
        {
            _racingSettingService = racingSettingService ?? new RacingSettingService();
        }

        [NonAction]
        protected JsonSerializerSettings GetJsonSerializerSetting()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return settings;
        }

        [NonAction]
        protected ICustomer DeserializeJsonString(string customerString)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            Customer customer =
                 JsonConvert.DeserializeObject<Customer>(customerString, settings);
            return customer;
        }

        protected IList<Result> ParseError(string errorCode, string errorMsg)
        {
            var errorResults = new List<Result>();

            if (!string.IsNullOrEmpty(errorCode))
            { 
                var arrErrorCode = Regex.Split(errorCode, "\a");
                var arrErrorMsgs = Regex.Split(errorMsg, "\a");
                if (arrErrorCode.Length > 0)
                {
                    var totalErrors = arrErrorCode.Length;
                    for (int i = 0; i < totalErrors; i++)
                    {
                        var errCode = CastAide.AsInt32(arrErrorCode[i]);
                        if (errCode == 0) { continue; }
                        var errMsg = errCode > 0
                                        ? arrErrorMsgs[i]
                                        : ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, errCode.ToString());

                        errorResults.Add(new Result()
                        {
                            ErrorCode = errCode.ToString(),
                            ErrorMessage = errMsg
                        });
                    }

                    if (errorResults.Count == 0)
                    {
                        errorResults.Add(new Result()
                        {
                            ErrorCode = "0",
                            ErrorMessage = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "0")
                        });
                    }
                }
            }
            else
            {
                errorResults.Add(new Result()
                {
                    ErrorCode = "1000",
                    ErrorMessage = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Unchanged")
                });
            }
            return errorResults;
        }
    }
}

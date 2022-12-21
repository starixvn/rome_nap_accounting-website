namespace Fanex.Tarus.Customer.Entities.InitializingCustomerSetting
{
    using System.Collections.Generic;

    public class ValidateResultModel
    {
        public bool IsWarning { get; set; }

        public List<InsertErrorModel> Errors { get; set; }

        public int SettingType { get; set; }

        public string BetSettingErrorMessage { get; set; }
    }
}

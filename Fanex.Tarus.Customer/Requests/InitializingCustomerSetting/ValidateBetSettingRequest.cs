namespace Fanex.Tarus.Customer.Requests.InitializingCustomerSetting
{
    using Fanex.Tarus.Customer.Entities.InitializingCustomerSetting;
    using System.Collections.Generic;
    using System.Web;
    
    public class ValidateBetSettingRequest
    {
        public HttpPostedFileBase UploadFile { get; set; }

        public IEnumerable<SettingInforModel> Settings { get; set; }

        public IEnumerable<int> CurrencyIdList { get; set; }
    }
}

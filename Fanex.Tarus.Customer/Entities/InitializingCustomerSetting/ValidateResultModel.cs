namespace Fanex.Tarus.Customer.Entities.InitializingCustomerSetting
{
    using System.Collections.Generic;

    public class ValidateResultModel
    {
        public bool IsWarning { get; set; }

        public List<InsertErrorModel> Errors { get; set; }
    }
}

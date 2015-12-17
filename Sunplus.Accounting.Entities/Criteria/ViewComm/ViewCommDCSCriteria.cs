namespace Sunplus.Accounting.Entities.Criteria.ViewPT
{
    using System.Collections;
    using Fanex.NAP.Framework.Data;

    public class ViewCommDCSCriteria : CriteriaBase
    {
        public string LanguageKey
        {
            get
            {
                return "EN";
            }
        }

        public override string GetSettingKey()
        {
            return "Acc_CustSetting_ProductNamesSelect";
        }
    }
}

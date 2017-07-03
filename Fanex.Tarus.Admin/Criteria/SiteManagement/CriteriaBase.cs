using System.Collections;

namespace Fanex.Tarus.Admin.Criteria.SiteManagement
{
    public interface CriteriaBase
    {
        Hashtable GetSPParams();

        string GetSettingKey();
    }
}

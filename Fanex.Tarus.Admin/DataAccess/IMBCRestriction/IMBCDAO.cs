namespace Fanex.Tarus.Admin.DataAccess.IMBCRestriction
{
    using Fanex.Data;
    using Sunplus.Accounting.Core.Common;
    using System.Collections;
    using System.Web;

    public class IMBCDAO
    {
        public bool IMBCRestrictionStatus()
        {
            using (ObjectDb db = new ObjectDb("Acc_CustSetting_SettingRestrictionSelect"))
            {
                using (var reader = db.ExecuteReader())
                {
                    if (reader != null && reader.Read())
                    {
                        return reader["isAllowed"].ToString().Equals("1") ? true : false;
                    }
                }

                return false;
            }
        }

        public bool UpdateIMBCRestrictionStatus(bool isAllowed)
        {
            using (ObjectDb db = new ObjectDb("Acc_CustSetting_SettingRestrictionUpdate"))
            {
                var param = new Hashtable();
                param.Add("isallowed", isAllowed);
                param.Add("userid", AppSession.AccId);
                param.Add("ip", HttpContext.Current.Request.UserHostAddress);
                db.AssignParamValues(param);

                db.ExecuteNonQuery();

                return true;
            }
        }
    }
}

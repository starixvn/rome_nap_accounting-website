namespace Fanex.Tarus.Admin.DataAccess.IMBCRestriction
{
    using System.Collections;
    using Fanex.Data;

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
                db.AssignParamValues(param);

                db.ExecuteNonQuery();

                return true;
            }
        }
    }
}

namespace Fanex.Tarus.Customer.DataAccess.MultipleUpdateSuperStatus
{
    using Fanex.CustomerSetting.Module.Core;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.Entities.MultipleUpdateSuperStatus;
    using Fanex.Tarus.Customer.ViewModels.MultipleUpdateSuperStatus;

    public interface IMultipleUpdateSuperStatusDataAccess
    {
        SupersStatusViewModel GetSupersStatus(ICriteria criteria);
    }

    public class MultipleUpdateSuperStatusDataAccess : IMultipleUpdateSuperStatusDataAccess
    {
        public SupersStatusViewModel GetSupersStatus(ICriteria criteria)
        {
            var result = new SupersStatusViewModel();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var reader = objDb.ExecuteReader(criteria.GetSpParams());

                while (reader.Read())
                {
                    result.Supers.Add(new SuperStatusEntity
                    {
                        CustId = Utils.ToInt32(reader["custid"]),
                        UserName = Utils.ToString(reader["username"]),
                        Site = Utils.ToString(reader["site"]),
                        Closed = Utils.ToBoolean(reader["closed"]),
                        Suspended = Utils.ToBoolean(reader["suspended"]),
                        Protected = Utils.ToBoolean(reader["IsProtectedMode"])
                    });
                }

                reader.Close();
                var outputs = objDb.GetOuputParams();
                result.ErrCode = Utils.ToInt32(outputs["errcode"]);
                result.ErrMsg = Utils.ToString(outputs["errmsg"]);
            }

            return result;
        }
    }
}

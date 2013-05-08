
namespace Fanex.Tarus.Customer.DataAccess.CustomerList
{
    using System;
    using System.Data;
    using Fanex.Data;
    using DNAide.Core;
    using System.Collections;
    public class CustomerListAccess : ICustomerListAccess
    {
        public DataSet GetCustomerList(string spName,Hashtable hash, ref int totalPages, ref int totalChildren)
        {
            using (ObjectDb db = new ObjectDb(spName, 1200))
            {
                db.AssignParamValues(hash);
                var result = db.ExecuteDataSet();

                totalPages = int.Parse(db.GetOuputParams()["totalpages"].ToString());
                totalChildren = int.Parse(db.GetOuputParams()["totalitems"].ToString());
                return result;
            }
        }
    }
}

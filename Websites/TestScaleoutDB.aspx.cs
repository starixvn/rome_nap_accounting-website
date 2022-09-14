using Fanex.Data;
using Fanex.Logging;
using Sunplus.Accounting.Core.Extensions;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.UI;

public partial class TestScaleoutDB : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    
        if (this.IsPostBack)
        {
            int creditCustid = 1176070;
            int depositCustid = 916498;
            //ExecSP(creditCustid);
            //ExecSP(depositCustid);
            for (int i = 0; i < 10; i++)
            {
                var tmp = i;
                ExecSP(tmp);
                //Task.Run(() => ExecSP(i));
            }

            //Task.Run(() => ExecSP(creditCustid));
            //Task.Run(() => ExecSP(depositCustid));
            //Task.Run(() => ExecSP(creditCustid));
            //Task.Run(() => ExecSP(depositCustid));
            //Task.Run(() => ExecSP(creditCustid));
            //Task.Run(() => ExecSP(depositCustid));
            //Task.Run(() => ExecSP(creditCustid));
            //Task.Run(() => ExecSP(depositCustid));
        }
    }

    private void ExecSP(int custId)
    {
        using (var objDb = new ObjectDb("Acc_BalanceMemberSel"))
        {
            LogService.Info("test Fanex.Data parallel: custid=" + custId);
            var param = new Hashtable();
            param.Add("custid", custId);
            //objDb.AssignParamValues(param);

            //string oldConnectionString = objDb.DbSetting.ConnectionString;
            //var siteConfigs = ObjectDbExtensions.GetSiteDBConfigByCustId(custId);
            //objDb.DbSetting.ConnectionString = BuildConnectionString(siteConfigs.AGIP, oldConnectionString);

            //objDb.ExecuteNonQuery();
            //objDb.DbSetting.ConnectionString = oldConnectionString;
            ExecuteDataSetScaleoutDB(objDb, custId);
        }
    }

    private int ExecuteDataSetScaleoutDB(ObjectDb objDb, int custId)
    {
        string oldConnectionString = objDb.DbSetting.ConnectionString;
        var siteConfigs = ObjectDbExtensions.GetSiteDBConfigByCustId(custId);
        //if (siteConfigs != null && siteConfigs.IsSiteDB)
        //{
        objDb.DbSetting.ConnectionString = BuildConnectionString(siteConfigs.AGIP, oldConnectionString);
        //}
        //using (var db = new ObjectDb("Acc_BalanceMemberSel"))
        //{
        //var param = new Hashtable();
        //param.Add("custid", custId);
        //objDb.AssignParamValues(param);

        int result = objDb.ExecuteNonQuery();
        objDb.DbSetting.ConnectionString = oldConnectionString;
        return result;
        //}
    }

    private static string BuildConnectionString(string serverIP, string connectionString)
    {
        var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
        sqlConnectionStringBuilder.DataSource = serverIP;
        return sqlConnectionStringBuilder.ToString();
    }
}
<%@ WebHandler Language="C#" Class="TestScrollTable" %>

using System;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data;

public class TestScrollTable : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        DataTable dt=new DataTable();
        dt = GenSampleData();
        JObject json = new JObject();
        
        json["Data"] = new JValue(JsonConvert.SerializeObject(dt));
        context.Response.Write(JsonConvert.SerializeObject(json));
        context.Response.End();
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    private DataTable GenSampleData()
    {
        DataTable tbl = new DataTable();
        tbl.Columns.Add("col1", typeof(string));
        tbl.Columns.Add("col2", typeof(string));
        tbl.Columns.Add("col3", typeof(string));
        tbl.Columns.Add("col4", typeof(string));
        tbl.Columns.Add("col5", typeof(string));

        for (int i = 1; i < 10; i++)
        {
            tbl.Rows.Add("Col 1:" + i, "Col 2:" + i, "Col 3:" + i, "Col 4:" + 1, "Col 5:" + i);
        }
        return tbl;
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Collections.ObjectModel;
using Fanex.Data;
using DNAide.Data;

public partial class Test_anything : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        using (ObjectDb objDB = new ObjectDb("Acc_ExchangeSel", 600))
        {
            var dataset = objDB.ExecuteDataSet();
            DbResult dbResult = new DbResult(objDB.GetOuputParams(), dataset);            
        }   
    }
}
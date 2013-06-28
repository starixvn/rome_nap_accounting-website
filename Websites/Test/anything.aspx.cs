using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Collections.ObjectModel;

public partial class Test_anything : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string s = DNAide.Utils.Encryption.CfsDeCrypt("i9hNnaZltx04XF6oTNOKTnvvtLC/zfxr");
      //  string pwd1 = "%bodbAdminNetLogin2587";
        string pwd2 = "casino168";
        string CfsEnCrypt = DNAide.Utils.Encryption.CfsEnCrypt(pwd2);
        Response.Write(CfsEnCrypt + "<br/>");

        string CfsDeCrypt = DNAide.Utils.Encryption.CfsDeCrypt(CfsEnCrypt);
        Response.Write(CfsDeCrypt + "<br/>"); 



    }
}
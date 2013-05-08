using System;
using IBCUtils;
using Fanex.Security;

public partial class Admin_Encryption : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {


    }
    protected void btnSC_Click(object sender, EventArgs e)
    {
        var md5 = Cryptography.ComputeHash("MD5", txtCustid.Value, "1qaw3+{:P)");
        var sc = Cryptography.ComputeHash("SHA1", txtSC.Value, md5);
        lblSc.InnerText = sc;
    }
    protected void btnEncrypt1_Click(object sender, EventArgs e)
    {
        lblEncrypt.InnerText = Encryption.CfsEnCode(txtEncryptValue.Value);
    }
}
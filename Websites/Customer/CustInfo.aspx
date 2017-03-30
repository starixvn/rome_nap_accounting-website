<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustInfo.aspx.cs" Inherits="Customer_CustInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <pre>
        <%=Newtonsoft.Json.JsonConvert.SerializeObject(new Sunplus.Accounting.DataAccess.APIClient.IBCLibProvider().GetBasicCustInfoIBC(Convert.ToInt32(Request.QueryString["custid"])), Newtonsoft.Json.Formatting.Indented) %>
    </pre>
</body>
</html>
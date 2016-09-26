<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UM.aspx.cs" Inherits="Areas_NG2_UM" %>
<%
    var service = new ONEbook.UM.ThirdParty.Services.Impl.UMService();
    var um = service.GetUnderMaintenanceTime(23);
    var UMfrom = um.From;
    var UMto = um.To;
 %>
<!DOCTYPE html>
<html>
    <head>
        <title>UM</title>
        <meta charset="utf-8" />
        <meta http-equiv="X-UA-Compatible" content="IE=edge">
        <meta name="viewport" content="width=device-width, initial-scale=1">
    </head>
    <body>

        <h1 class="title"><span style="color: #ff0000">Under Maintenance</span></h1>
        <hr />
        <h2 class="sub-title">Dear Valued Customers:</h2>
        <p>
            We are having server maintenance, website will be unavailable from <br />
            HK time <span  style="color: #ff0000"><%=UMfrom %> until <%=UMto%> </span>(GMT+ 8). Sorry for the inconvenience caused.
        </p>
        <h2 class="sub-title second">尊敬的客戶:</h2>
        <p>
            本公司將於香港時間 <span  style="color: #ff0000"><%=UMfrom%> 至 <%=UMto%> </span>(GMT +8) 對服務器進行維護，期間所有的網頁將無法登入，不便之處，敬請諒解
        </p>
    </body>
</html>
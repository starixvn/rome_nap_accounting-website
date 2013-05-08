<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DailyTransaction.aspx.cs"
    Inherits="DailyTransaction" Title="Daily Finance Transaction" Theme="" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" Theme="">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Daily Txn Info</title><meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link href="~/App_Themes/DailyTxn/DailyTxn.css" rel="stylesheet" type="text/css" />
    <link href="~/App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="~/App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="~/App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="~/App_Themes/Public/Pagination.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/ui.jqgrid.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        <%=sTitle%>
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box" id="bannerFilter" style="min-width:1010px;" >
                <table border="0px" cellpadding="0px" cellspacing="0px">
                    <tr>
                        <td class="b">
                            Date:&nbsp;
                        </td>
                        <td>
                            <%=strCalendar%>
                        </td>
                        <td class="b">
                            &nbsp;&nbsp;Status:&nbsp;
                        </td>
                        <td class="b">
                            <select id="ddlStatus">
                                <option value="1">All</option>
                                <option value="2">Verified</option>
                                <option value="0">Unverified</option>
                            </select>
                        </td>
                        <td class="b">
                            &nbsp;&nbsp;Type:&nbsp;
                        </td>
                        <td>
                            <%--<input type="text" maxlength="30" id="txtType" name="txtType" value="" class="txt" />--%>
                            <select id="txtType" style="width:90px;">
                                <option value="">All</option>
                                <%=bettypesList %>
                            </select>
                        </td>
                        <td class="b">
                            &nbsp;&nbsp;Creator:&nbsp;
                        </td>
                        <td>
                            <input type="text" id="txtCreator" maxlength="30" name="txtCreator" value="" class="txt" />&nbsp;&nbsp;
                        </td>
                        <td class="b">
                            &nbsp;&nbsp;Verified by:&nbsp;
                        </td>
                        <td>
                            <input type="text" id="txtVerifyBy" maxlength="30" name="txtVerifyBy" value="" class="txt" />&nbsp;&nbsp;
                        </td>
                        <td>
                            <input type="button" class="btn" onclick="GetDailyTransaction(1, 0);" value="Submit" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="bg_table" style="min-width:1010px">
                <div id="sContent">
                    Loading...</div>
            </div>
        </div>
    </div>
    


    <script src="../js/jQuery/jquery.min.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.json-1.3.js" type="text/javascript"></script>

    <script src="../js/jQuery/jquery.history.js" type="text/javascript"></script>

    <script src="../js/pager_v2.js" type="text/javascript"></script> 
	
    <script src="WebResources/js/DailyFinanceTrans.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>         
    <iframe id="frm" width="0" height="0"  frameborder="0" style="display:none"></iframe>
</body>
</html>

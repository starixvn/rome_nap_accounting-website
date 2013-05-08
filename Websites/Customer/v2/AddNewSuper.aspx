<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddNewSuper.aspx.cs" Inherits="Customer_V2_AddNewSuper"
    Title="Untitled Page" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />

    <script src="../../js/showhidepopup.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="../../js/BlockUI.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../js/common.js"></script>
    <script type="text/javascript" src="../../js/Customer.js"></script>
    <script src="../../js/PT/SuperPT/EditSuperPT.js?v=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../../js/PT/NumberGame/NumberGamePT.js?v=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="../../js/PT/MixedSports/MixedSportsPT.js?v=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.js?v=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(
            function () {
                SiteRendering('listSite', '../../');
            }
        );


        window.enableDeposit = <%=enableDeposit ? 1 : 0%> == 1 ? true : false;
    
    </script>
</head>
<body>
 <div class="main">
        <div class="maincontent">
              <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC fixfont">
                    <%=Strtitle%>
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                <div id="err" class="errMsg"></div>
                <div id="errMsg" class="error-message"></div>
            </div>
            <div class="bg_table">
                 <script src="../../js/ToolTip/wz_tooltip/wz_tooltip.js" type="text/javascript"></script>
	             <script src="../../js/ToolTip/wz_tooltip/tip_balloon2.js" type="text/javascript"></script>
                 <div align="left">
                    <iframe name="frInsert" frameborder="0" class="hidden" height="150px" width="800px">
                    </iframe>
                    <form name="fInsertNew" id="fInsertNew" action="../../Handlers/<%=ControlHandler %>" target="frInsert" method="post">
                    <input type="hidden" name="set" id="set" value="<%=fromSet %>" />
                    <input type="hidden" name="type" id="type" value="<%=type%>" />
                    <input type="hidden" name="uname" id="uname" value="<%=username%>" />
                    <input type="hidden" name="disHR" id="disHR" value="<%=disHR%>" />
                    <%=content.ToString() %>
                    </form>
                   <%-- <script type="text/javascript">
                        ShowError('<%=msg%>', 'err');
                    </script>--%>
                </div>
            </div>
        </div>
    </div>
  <script type="text/javascript">
      SetFirstLoadCurr();
      roleId = 4;
  </script>
</body>
</html>

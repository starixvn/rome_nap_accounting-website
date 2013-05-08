<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditAgent.aspx.vb" Inherits="Customer_V2_EditAgent" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Edit Agent</title>
     <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../js/common.js"></script>
    <script type="text/javascript" src="../../js/agent/Customer.js"></script>
    <script type="text/javascript" src="../../js/agent/MEditCustomer.js"></script>
    <script src="../../js/CustSetting.js" type="text/javascript"></script>   

    <script src="../../js/showhidepopup.js" type="text/javascript"></script>
    <script src="../../js/PT/NumberGame/NumberGamePT.js?v=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../../js/PT/MixedSports/MixedSportsPT.js?v=<%=WebSettings.Version%>" type="text/javascript"></script>
</head>
<body>
 <div class="maincontent">
     <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC fixfont">
                   Update Agent
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
    <div class="maincontent_box">
        <div id="err" class="errMsg">
        </div>
        <div id="errMsg" class="error-message">
        </div>
    </div>
    <div class="bg_table">
        <script src="../../js/ToolTip/wz_tooltip/wz_tooltip.js" type="text/javascript"></script>
        <script src="../../js/ToolTip/wz_tooltip/tip_balloon2.js" type="text/javascript"></script>
        <%VariableOfScript()%>
        <iframe id="frUpdate" name="frUpdate" style="DISPLAY:none"></iframe>
        <input type="hidden" name="roleId" id="roleId" value="2" />
        <input type="hidden" name="roleId2" id="roleId2" value="2" />
        <input type="hidden" id="hidEdit" name="hidEdit" value="1" />
        <input type="hidden" id="saveControlDisabled" name="saveControlDisabled"/>
        <input type="hidden" id="maxMemberPT" name="maxMemberPT" value="0" />	
        <form id="fUpdate" name="fUpdate" method="post" action="UpdateEditAgent.aspx" target="frUpdate">
	        <INPUT type="hidden" id="saveCustid" name="saveCustid" runat="server" />
	        <INPUT type="hidden" id="saveUsername" name="saveUsername" runat="server" />
	        <INPUT type="hidden" id="isAllowUpd" name="isAllowUpd" runat="server" /> 
	        <INPUT type="hidden" id="isMult" name="isMult" value="0">
	        <INPUT type="hidden" id="masterId" name="masterId" value="<%=masterId %>" />
	        <INPUT type="hidden" id="set" name="set" value="<%=fromSet %>" />
	        <input type="hidden" id="memberPT" name="memberPT"/>
	        <input type="hidden" id="isUpdateMember" name="isUpdateMember" value="0"/>
	        <div style="text-align:left;">
	        <asp:Literal id="htmlEditAgentContent" runat="server"></asp:Literal>
	        </div>
        </form>
  
    </div>
</div>

</body>
</html>

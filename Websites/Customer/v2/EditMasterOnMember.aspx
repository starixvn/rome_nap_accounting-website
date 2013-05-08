<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditMasterOnMember.aspx.vb" Inherits="Customer_V2_EditMasterOnMember" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Edit Master On Member</title>
     <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="../../js/BlockUI.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
	<script src="../../js/agent/Customer.js" type="text/javascript"></script>		
	<script src="../../js/agent/MasterEditMember.js?v=<%=WebSettings.Version%>" type="text/javascript"></script>

    <script src="../../js/PT/NumberGame/NumberGamePT.js?v=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../../js/PT/MixedSports/MixedSportsPT.js?v=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script type="text/javascript">
			var flag=false;
			function BtnOnClick()
			{
				if (obj("isAllowUpdPT").value=="0" || flag)
				{					
					
				}
				else {
				    
					flag=true;
					try{parent.BlockUI();}catch(e){}
					document.form.submit();
				}
}
	
		</script>
</head>
<body>
      <div class="maincontent">
          <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC fixfont">
                      Edit Master On Member
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
        <div class="maincontent_box">
            <div id="err" class="errMsg"></div>
        </div>
        <div class="bg_table">
            <iframe name="frUpdate" id="frUpdate" style="DISPLAY:none"></iframe>
		    <input type="hidden" id="roleId" name="roleId" value="3">
		    <form id="form" name="form" method="post" action="UpdateMasterOnMember.aspx" target="frUpdate">
			    <input id="isAllowUpdPT" name="isAllowUpdPT" type="hidden" runat="server"> 
			    <input id="isMult" name="isMult" type="hidden" runat="server">
			    <input id="saveCustid" type="hidden" runat="server" NAME="saveCustid"> 
			    <input id="saveUsername" type="hidden" runat="server" NAME="saveUsername">
			    <input id="saveAgentid" type="hidden" runat="server" NAME="saveAgentid">
			     <div style="text-align:left;">
			    <asp:Literal id="htmlSuperEditMember" runat="server"></asp:Literal></form>
		        </div>
		    <%=disabledBtn%>
		    <script type="text/javascript">
		        var msg = '<%=msg%>';
		        ShowError(msg, 'err');
		    </script>
        </div>
</body>
</html>

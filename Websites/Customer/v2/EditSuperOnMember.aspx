<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditSuperOnMember.aspx.vb" Inherits="Customer_V2_EditSuperOnMember" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Edit Super On Member</title>
    <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="../../js/BlockUI.js" type="text/javascript"></script>
    <script src="../../js/Common.js" type="text/javascript"></script>
	<script src="../../js/agent/Customer.js" type="text/javascript"></script>	
	<script src="../../js/agent/SuperEditMember.js?v=<%=WebSettings.Version%>" type="text/javascript"></script>

    <script src="../../js/PT/MixedSports/MixedSportsPT.js?v=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../../js/PT/NumberGame/NumberGamePT.js?v=<%=WebSettings.Version%>" type="text/javascript"></script>
	<script type="text/javascript">
	    var flag=false;
		function BtnOnClick()
		{								
			if(!flag)
			{	
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
                     Edit Super On Member
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
		    <form id="form" name="form" method="post" action="UpdateSuperOnMember.aspx" target="frUpdate">
		         <input type="hidden" id="hidEdit" name="hidEdit" value="1">
	            <input id="isMult" name="isMult" type="hidden" runat="server"> 
	            <input id="saveCustid" type="hidden" runat="server" name="saveCustid">
		        <input id="saveUsername" type="hidden" runat="server" name="saveUsername"> 
		        <input id="saveAgentid" type="hidden" runat="server" name="saveAgentid">
		        <div style="text-align:left;">
		        <asp:Literal id="htmlSuperEditMember" runat="server"></asp:Literal>
		        </div>
	        </form>
		    <script type="text/javascript">
		        var msg = '<%=msg%>';
		        ShowError(msg, 'err');
		    </script>
        </div>
    </div>
   
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConfirmEditMasterFI.aspx.cs" Inherits="Customer_V2_ConfirmEditMasterFI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>Select Member</title>
    <link href="../../App_Themes/Public/Public.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../../js/common.js"></script>
     <style type="text/css">
        input{width:40px;}
    </style>
    <script type="text/javascript">
        function GetMemberPT()
        {        
            var maxMember=Math.round(opener.memberPTMax * 100);   
            obj("ddlMemberPT").options.length=0;
            while(maxMember>=0)
            {
                var val=round(maxMember/100);              
                obj("ddlMemberPT").options.add(new Option(val,val));
                maxMember=maxMember-1;               
            }
        }
        function ButtonClick(str)
        {			
            opener.newspositiontaking = obj("ddlMemberPT").value;
            opener.updateallmembers = str;  
            opener.isOpenPopUp = 0;
            window.close();
            opener.UpdatePT();
        }     
        function DisableControls()
        {   		
            if (opener.PTUnBlockUI)
            {
                opener.PTUnBlockUI();
            }
        }
    </script>  
</head>
<body onload="GetMemberPT();" onunload="DisableControls();">
  <form id="fConfirmEditMaster">
    <table>
        <tr>
            <td class="l">
                <%=strMsg%>
            </td>
        </tr>
        <tr >
            <td class="l">
                <select id="ddlMemberPT" name="ddlMemberPT">
                </select>
            </td>
        </tr>
        <tr>
            <td class="l">
                <input type="button" id="btnYes" onclick="ButtonClick(1);" name="btnYes" value="Yes" class="buttonsite"/>
                <input type="button" id="btnNo" onclick="ButtonClick(0);" name="btnNo" value="No" class="buttonsite"/>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>

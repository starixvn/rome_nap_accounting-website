<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConfirmEditMasterHR.aspx.cs" Inherits="Customer_V2_ConfirmEditMasterHR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>Select Member</title>
    <script type="text/javascript" src="../../../js/common.js"></script>
     <style type="text/css">
        input{width:40px;}
    </style>
    <script type="text/javascript">
        function GetMemberPT()
        {        
            var maxMember=Math.round(opener.memberPTMax * 100);
            obj("ddlMemberPT").options.length = 0;
            //opener.BindSelectOptions(obj("ddlMemberPT"), opener.memberPTMax, 0, opener.memberPTMax, opener.interval_PT);
            while(maxMember>=0)
            {
                var val=round(maxMember/100);              
                obj("ddlMemberPT").options.add(new Option(val,val));
                maxMember=maxMember-10;               
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
            <td class="c">
                <%=strMsg%>
            </td>
        </tr>
        <tr class="c">
            <td>
                <select id="ddlMemberPT" name="ddlMemberPT">
                </select>
            </td>
        </tr>
        <tr>
            <td class="c">
                <input type="button" id="btnYes" onclick="ButtonClick(1);" name="btnYes" value="Yes"/>
                <input type="button" id="btnNo" onclick="ButtonClick(0);" name="btnNo" value="No"/>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>

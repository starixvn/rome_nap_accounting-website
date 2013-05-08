<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditVirtualExRate.aspx.cs" Inherits="Customer_EditVirtualExRate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Edit Virtual Exchange Rate</title>
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/common.js"></script> 
    
    <script src="../js/showhidepopup.js" type="text/javascript"></script>
     <script type="text/javascript">
        <%=strArrExRate%> 
        var isEdited=false;      
        function SubmitForm()
        {
            var isUpdate=confirm("Are you sure that you want to update this Virtual Exchange Rate value for this account and all of its downlines?")            
            if (isUpdate)
            {
                
                isEdited=true;
                document.fUpdate.submit();
                return true;
            }
            else
            {
                return false;
            }
        }
        function OnClose(isCustInfo)
        {   
           if (parent.ibcWnd)
            {
                parent.ibcWnd.close();
                 if(isEdited)
                    {
                      //parent.changeDisplayStatus();
                    }          
            }
            else if (window.opener)
            {
               window.close();
               //opener.changeDisplayStatus(); 
            }
        }
        
        function EnableClose()
        {
            obj("divEdit").style.display="none";
            obj("divClose").style.display="";
        }        
    </script>   
</head>
<body onunload="OnClose(<%=isCustInfo %>);" style="margin:5px 10px 5px 5px">
     <div class="main">
        <div class="maincontent">
            <div class="bg_table">
                <form name="fUpdate" action="../Handlers/UpdateVirtualRate.ashx" target="frUpdate" method="post">
                     <asp:Literal ID="editVirtualRateContent" runat="server"></asp:Literal>
                     <input type="hidden" name="hidExRate" id="hidExRate" value="<%=current%>"/>       
                </form>
                <iframe name="frUpdate" height="0" width="0" frameborder="0" style="display:none;"></iframe>
            </div>
        </div>
    </div>    
   
</body>
</html>

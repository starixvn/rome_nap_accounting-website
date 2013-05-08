<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BetList12BET.aspx.cs" Inherits="BetList_BetList12BET" Theme="" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title id="ReportTitle" runat="server"></title>
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
<link rel="Stylesheet" type ="text/css" href ="../App_Themes/Report/Betlist.css?v=4" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
<script src="../js/jQuery/jquery.js" type="text/javascript"></script>
<script src="../js/jQuery/scrolltable/webtoolkit.scrollabletable.js" type="text/javascript"></script>
<script src="../js/jQuery/scrolltable/webtoolkit.jscrollable.js" type="text/javascript"></script>
<script src="../js/jQuery/jquery.wresize.js" type="text/javascript"></script>
<script type="text/javascript" src="../js/CommonFxHeader.js?ver=<%=WebSettings.Version%>"></script>
<script type="text/javascript">
function ViewDNS(ip)
{
    url = 'http://anonym.to/?http://legacytools.dnsstuff.com/tools/ipall.ch?tool_id=67&token=&toolhandler_redirect=0&ip=' + ip + '&j=1';
    window.open(url,'DNS',' scrollbars=yes, toolbar=yes, menubar=yes,location=yes,status=yes,resizable=yes');
}
function obj(id)
{
    return document.getElementById(id);
}

function showMP(refno)
{
    var spEvent = obj('spEvent_' + refno);
    
    if(spEvent.className == 'hide')
    {
        if (spEvent.innerHTML == "")
        {
            $.ajax({
            url: '../Handlers/GenMixParlay.ashx?refno='  +  refno  + '&type=2' ,
            dataType:'html',
            success: function(data)
            {
                spEvent.innerHTML = data;
                spEvent.className = '';
                
            }
            });   
        }
        else
        {
            spEvent.className = '';
        }
    }
    else
    {
        spEvent.className = 'hide';
    }
}

</script>
</head>
<body>
    <asp:Literal ID="reportHeader" runat="server"></asp:Literal>
    <asp:Literal ID="reportContent" runat="server"></asp:Literal>
    <div id="sData"></div>
<script type="text/javascript">
Init(1200);
window.onresize = Resize;
</script>
</body>
</html>
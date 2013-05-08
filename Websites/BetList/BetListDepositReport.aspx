<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BetListDepositReport.aspx.cs"
    Inherits="BetListDepositReport" Theme="" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title id="ReportTitle" runat="server"></title>
    
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css" />
    
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link rel="Stylesheet" type="text/css" href="../App_Themes/Report/Betlist.css?v=4" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />

    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>

    <script src="../js/jQuery/scrolltable/webtoolkit.scrollabletable.js" type="text/javascript"></script>

    <script src="../js/jQuery/scrolltable/webtoolkit.jscrollable.js" type="text/javascript"></script>

    <script type="text/javascript" src="../js/CommonFxHeader.js?ver=<%=WebSettings.Version%>"></script>

    <script type="text/javascript" src="../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js"></script>
    
    <script type="text/javascript" src="../js/BlockUI.js"></script>
    <script src="../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    
    <script type="text/javascript" src="../js/dialogbox.js"></script>
    
    <script type="text/javascript" src="../js/BlockUI.js"></script>
    <script src="../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>

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

function showCMP(event,refno, listType) {
    var spEvent = obj('spEvent_' + refno);
    var target = event.target ? event.target : event.srcElement;
    $(target).parents("td:first").find("div").each(function(index, element){
        if ($(element).attr("id").indexOf("spEvent") >= 0) {
            spEvent = element;
        }
    });
    if (spEvent.className == 'hide') {
        if (spEvent.innerHTML == "") {
            BlockUI();
            $.ajax({
                url: '../Handlers/GenMixParlay.ashx?refno=' + refno + '&listType=' + listType + '&type=1',
                dataType: 'html',
                success: function (data) {
                    spEvent.innerHTML = data;
                    spEvent.className = '';
                    
                    $(event.target).parents("tr:first").find("td").each(function(index, element){
                        element.style.verticalAlign = "top";
                    });
                    
                    $.unblockUI();
                }
                , error: function() {
                    $.unblockUI();
                }
            });

        }
        else {
            spEvent.className = '';
            $(event.target).parents("tr:first").find("td").each(function(index, element){
                element.style.verticalAlign = "top";
            });
        }
    }
    else {
        spEvent.className = 'hide';
        $(event.target).parents("tr:first").find("td").each(function(index, element){
            element.style.verticalAlign = "middle";
        });
    }
}

function showDetailsCMP(refno, title, transid, betid) {
    //OpenPopup(url, title, width, height, modal, resizable, currWindow, duration, fnRecall, fnOnClose) {
    OpenPopup("../BetList/CombinationMPDetail.aspx?refno=" + refno + "&transid=" + transid + "&betid=" + betid, title, 720, 500, true, false, window, 0, null, null);
}

function cmpMouseOutDetailButton(event) {
    var target = event.target ? event.target : event.srcElement;
    $(target).parents("a:first").find("div").get(0).style.backgroundPosition = "0px 0px";
}

function cmpMouseOverDetailButton(event) {
    var target = event.target ? event.target : event.srcElement;
    $(target).parents("a:first").find("div").get(0).style.backgroundPosition = "0px -14px";
}

function showMP(refno)
{
    var spEvent = obj('spEvent_' + refno);
    
    if(spEvent.className == 'hide')
    {
        if (spEvent.innerHTML == "")
        {
            BlockUI();
            $.ajax({
                url: '../Handlers/GenMixParlay.ashx?refno='  +  refno  + '&type=2' ,
                dataType:'html',
                success: function(data)
                {
                    spEvent.innerHTML = data;
                    spEvent.className = '';
                    $.unblockUI();
                }
                , error: function() {
                    $.unblockUI();
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


function InitFixedHeader(widthmainpage) {

    return;
    if ($('#trFooter').length == 0) return;

    var ishide = $('#trFooter').is(':visible');

    //if (ishide) $('#trFooter').show();

    var topHeader = $('#trHeader').position().top;
    var topFooter = $('#trFooter').position().top;

    //if (ishide) $('#trFooter').hide();

    var innerHeight = window.innerHeight;
    var delta = 0;
    if (!innerHeight) {
        innerHeight = document.documentElement.clientHeight;
        delta = 0;
    }

    var RealHeight = $(window).height() - topHeader;

    if ((topFooter - topHeader) > $(window).height()) {

        jQuery('#table').Scrollable(RealHeight, widthmainpage);
    }
    else {
        $('#table .rspace').removeClass('rspace');
    }
}
    </script>

</head>
<body>
    <center>
        <asp:Literal ID="reportHeader" runat="server"></asp:Literal>
        <asp:Literal ID="reportContent" runat="server"></asp:Literal>
        <div id="sData">
        </div>
    </center>

    <script type="text/javascript">
    InitFixedHeader(800);
    </script>

</body>
</html>

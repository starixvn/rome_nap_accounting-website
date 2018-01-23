<%@ Page Language="C#" AutoEventWireup="true" CodeFile="__SuperAgentList.aspx.cs" Inherits="NewCustomer_SuperAgentList"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../App_Themes/Public/Common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Pagination.css" rel="stylesheet" type="text/css" /> 
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />

    <link href="../App_Themes/Customer/customerlist.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/ui.jqgrid.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/jQuery/jquery.js"></script>
    <script src="../js/pager_v2.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.history.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/common.js"></script>
    <script type="text/javascript" src="../js/Customer.js"></script>
    <script type="text/javascript" src="../js/ViewIPInfo.js"></script>
    <script src="../js/showhidepopup.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/master.agent.list.js"></script>
    <script runat="server">
        public string GetEditCustomerString()
        {
            if (WebSettings.IsShowCustomerSettingRenovation == 3)
            {
                return "EditSuper/EditSuperPage";
            }
            else
                return "Customer/v2/AddNewSuper.aspx";
        }
    </script>
    <script type="text/javascript">
        var currpage = 1;
        function reloadpage() {
            setTimeout("callAjax(getParams(currpage))", 2000);
        }
        
        function OpenEdit(uplineid, id, uname) {
            var url = "<%=GetEditCustomerString()%>?custid=" + id + "&uname=" + uname;
           var pageIndex="1";
           if (location.href.indexOf("#")>-1)
           {
            pageIndex=location.href.split("#")[1];
           }
           showPopup("Update Super Setting", url ,"","",980,650, false, 2000, function (){ callAjax(getParams(pageIndex));});
        }
        function On_Submit() {
            GenSuperListContent();
        }
        function GenSuperListContent() {
            callAjax(getParams("1"));
        }
        function On_KeyPress(e) {
            if (!e) e = window.event;
            var code = (e.keyCode) ? e.keyCode : e.which; 
            if (code == 13) {
                GenSuperListContent();
                return false;
            }
            return true;
        }
        function on_Blur() {
            if ($('#txtSearch').val().length == 0)
                $('#txtSearch').val('All');
        }

        function On_Focus() {
            if ($('#txtSearch').val() == 'All')
                $('#txtSearch').val('');
        }

        function pageload(hash) {
            if (hash) {
                // restore ajax loaded state
                if ($.browser.msie) {
                    hash = encodeURIComponent(hash);
                }
                var pageindex = hash.toString().replace(/Page/i, '');
                currpage = pageindex;
                GenSuperListContentByPager(pageindex);
            } else {
                // start page
            }
        }

        jQuery(document).ready(function() {
            GenSuperListContent();
            $.historyInit(pageload, "SuperAgentList.aspx");
           
        });

        function callAjax(postdata) {
            try{parent.BlockUI();}catch(e){}
            $.ajax({
                type: "POST",
                url: "../Services/WCF/Customer/ServiceCustomerList.svc/GenSuperListContent",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: postdata,
                success: function(rs){
                    BindData(rs);
                    MasterAgentList.init("supper");
                }
            });
        }

        function BindData(rs) {

           
            $("#strcontent").html(rs.toString());
            if (jQuery.browser.msie&& jQuery.browser.version.indexOf("7.")==0)
            {
                $("#btable").css("margin-top","-1px");
            }
            
            try{parent.jQuery.unblockUI();}catch(e){}
           
            //bind event for pager
            $(".pagenav_ibtn").click(function() {
                if (isNumberNotNull("txtpage")) {
                    var pageIndex = $("#txtpage").val();
                    currpage = pageIndex;
                    var hash = this.href + pageIndex;
                    hash = hash.replace(/^.*#/, '');
                    $.historyLoad(hash);
                    return false;
                }
            });

            $(".pagerlink").click(function() {
                var hash = this.href;
                hash = hash.replace(/^.*#/, '');
                $.historyLoad(hash);
                return false;
            });
        }
        

        function GenSuperListContentByPager(pageIndex) {
            if (pageIndex == null) {
                if (isNumberNotNull("txtpage"))
                    pageIndex = $("#txtpage").val();
                else
                    return;
            }
            if (pageIndex != "") {
                callAjax(getParams(pageIndex));
            }
        }
        function getParams(pageIndex) {
            var params = '';
            var txtSearch = $("#txtSearch");
            if (txtSearch.length > 0 && txtSearch.val() != "All")
                params += '{"searchText":"' + txtSearch.val() + '",';
            else
                params += '{';

            var listAlpha = $("#listAlpha");
            if (listAlpha.length > 0)
                if (listAlpha.val() != "All") params += '"selectAlpha":"' + listAlpha.val() + '",';

            var listCurrency = $("#listCurrency");
            if (listCurrency.length > 0)
                if (listCurrency.val() != "") params += '"selectCurreny":"' + listCurrency.val() + '",';

            params += '"type":' + '0' + ',';

            var listType = $("#listType");
            if (listType.length > 0)
                if (listType.val() != "") params += '"status":"' + listType.val() + '",';
            
                params += '"PageIndex":' + pageIndex + '}';
                return params;
        }
        var t;
        function showeditbox(id)
        {
         
            var parenttop=$("#" + id).parent().offset().top;
            var mtop=0;
            var mleft=0;
            if (parenttop>560)
            {
                mtop=-($("#" + id).height()+28);
            }
            if (jQuery.browser.msie&& jQuery.browser.version.indexOf("7.")==0)
            {
                mtop+=14;
                mleft-=28;
                $("#btable").css("margin-top","-2px");
            }
            $("#" + id).css("margin-top",mtop + "px");
            $("#" + id).css("margin-left",mleft + "px");
            $("#" + id).css("display","");
        }
        function hideeditbox(id)
        {
            t=setTimeout("hideit('" + id +  "')",200);
        }
        function editboxhover()
        {
            clearTimeout(t);
        }
        function hideit(id)
        {
            $("#" + id).css("display","none");
        }
        function editSetting(custId, roleId) {
            showPopup("<%= Resources.Language.PokerPageTitle %>"
                , "Customer/PT/Poker/SuperUpdate.aspx?custId=" + custId + "&roleId=" + roleId
                , 0, 0, 640, 480, false, 2000, null);
        }
        function liveCasinoSetting(custId, exchangeId, uplineId) {
            showPopup("<%= Resources.Language.liveCasinoTitle %>"
                , "LiveCasino/PTSetting/EditSuper.aspx?custId=" + custId + "&exchangeId=" + exchangeId
                , 0, 0, 640, 480, false, 2000, null);
        }
        function virtualSportsSetting(custId, uplineId, roleId) {
            showPopup("<%= Resources.Language.virtualsports %>"
                , "VirtualSportSuperPT/EditSuper?custId=" + custId + "&uplineId=" + uplineId + "&roleId=" + roleId
                , 0, 0, 860, 480, false, 2000, null);
        }
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
                    <asp:Literal ID="SuperAgentListHeaderStr" runat="server"></asp:Literal>
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
               <div id="searchbox"><%=strsearchbox%></div>
            </div>
            <div>
                 <div id="strcontent">Loading...</div>
            </div>
        </div>
    </div>
</body>
</html>

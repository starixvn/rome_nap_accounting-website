<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MasterAgentList.aspx.cs"
    Inherits="Customer_MasterAgentList" %>

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
    <script src="../js/jQuery/jquery.1.3.1.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/Customer.js"></script>
    <script type="text/javascript" src="../js/ViewIPInfo.js"></script>
    <script src="../js/pager_v2.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.history.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/common.js"></script>

    <script src="../js/showhidepopup.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/master.agent.list.js"></script>
     <script runat="server">
        public string GetEditCustomerString()
        {
            if (WebSettings.IsShowCustomerSettingRenovation == 3)
            {
                return "EditMaster/EditMasterPage";
            }
            else
                return "Customer/v2/EditMaster.aspx";
        }
    </script>
    <script type="text/javascript">
        jQuery(document).ready(function() {
            var superid = gup("superId");
            $("#superId").val(superid);
            $("#allowHR").val(gup("hr"));
            if (superid != "") {
                $("#searchbox").hide();
            }
            $.historyInit(pageload, "MasterAgentList.aspx");
            GenMasterListContent();
        })

        function OnChangeCurrency() {
            //obj("selectCurrency").innerHTML=obj("listCurrency").value;
        }
        
        function OpenEdit(superid, custid, uname) {
            var url = "<%=GetEditCustomerString()%>?custid=" + custid + "&superId=" + superid + "&uname=" + uname;
             var pageIndex="1";
           if (location.href.indexOf("#")>-1)
           {
            pageIndex=location.href.split("#")[1];
           }
            showPopup("Edit Master Setting", url ,"","",1000,650, false, 2000,  function (){ callAjax(getParams(pageIndex));});
        }
        function On_Submit() {
            GenMasterListContent()
        }

        function GenMasterListContent() {
            callAjax(getParams("1"));
        }

        function On_KeyPress(e) {
            if (!e) e = window.event;
            var code = (e.keyCode) ? e.keyCode : e.which; 
            if (code == 13) {
                On_Submit();
                return false;
            }
            return true;
        }
        function on_Blur() {
            if (obj('txtSearch').value.length == 0)
                obj('txtSearch').value = 'All';
        }

        function On_Focus() {
            if (obj('txtSearch').value == 'All')
                obj('txtSearch').value = '';
        }

        function pageload(hash) {
            if (hash) {
                // restore ajax loaded state
                if ($.browser.msie) {
                    hash = encodeURIComponent(hash);
                }
                var pageindex = hash.toString().replace(/Page/i, '');
                GenMasterListContentByPager(pageindex);
            } else {
                // start page
            }
        }

        function callAjax(postdata) {
            try{parent.BlockUI();}catch(e){}
            $.ajax({
                type: "POST",
                url: "../Services/WCF/Customer/ServiceCustomerList.svc/GenMasterListContent",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: postdata,
                success: function(rs){
                    BindData(rs);
                    MasterAgentList.init("master_agent");
                }
            });
        }

        function BindData(rs) {

            $("#strcontent").html(rs);
             if (jQuery.browser.msie&& jQuery.browser.version.indexOf("7.")==0)
            {
                $("#btable").css("margin-top","-1px");
            }

            
            try{parent.jQuery.unblockUI();}catch(e){}

            //bind event for pager
            $(".pagenav_ibtn").click(function() {
                if (isNumberNotNull("txtpage")) {
                    var pageIndex = $("#txtpage").val();
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


        function GenMasterListContentByPager(pageIndex) {
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
            var txtSearch=$("#txtSearch");
            if (txtSearch.length > 0 && txtSearch.val() != "All")
                params += '{"searchText":"' + txtSearch.val() + '",';
            else
                params += '{';
            
            var listAlpha=$("#listAlpha");
            if (listAlpha.length > 0)
                if (listAlpha.val() != "All") params += '"selectAlpha":"' + listAlpha.val() + '",';

            var listCurrency = $("#listCurrency");
            if (listCurrency.length > 0)
                if (listCurrency.val() != "") params += '"selectCurreny":"' + listCurrency.val() + '",';

            var userType = $("#userType");
            if (userType.length > 0)
                if (userType.val() != "") params += '"type":' + userType.val() + ',';
            
            var listType = $("#listType");
            if (listType.length > 0)
                if (listType.val() != "") params += '"status":"' + listType.val() + '",';
            
            var superId=$("#superId").val();
            if (superId != "") params += '"superId":"' + superId + '",';

            var hr = $("#allowHR").val();
            if (hr != "") params += '"hr":"' + hr + '",';

            var synccasinoUpline = gup("isSynccasino");
            if (synccasinoUpline != "") params += '"synccasinoUpline":"' + synccasinoUpline + '",';

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
                , "Customer/PT/Poker/MasterUpdate.aspx?custId=" + custId + "&roleId=" + roleId 
                , 0, 0, 640, 480, false, 2000, null);
        }
        function liveCasinoSetting(custId, exchangeId, uplineId) {
            showPopup("<%= Resources.Language.liveCasinoTitle %>"
                , "LiveCasino/PTSetting/EditMaster.aspx?custId=" + custId + "&uplineId=" + uplineId
                , 0, 0, 640, 480, false, 2000, null);
        }
        function virtualSportsSetting(custId, uplineId, roleId) {
            showPopup("<%= Resources.Language.virtualsports %>"
                , "VirtualSportMasterPT/EditMaster?custId=" + custId + "&uplineId=" + uplineId + "&roleId=" + roleId
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
                    Master List
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                <div id="searchbox">
                    <%=strsearchbox%></div>
            </div>
            <div>
                <div id="strcontent">
                    <table id="tblsuperlist" border="0"><tr><td>Loading...</td></tr></table>
                </div>
                 <form style="margin:0px; padding:0px;">
                    <input type="hidden" id="superId" />
                    <input type="hidden" id="allowHR" />
                </form>
            </div>
        </div>
    </div>
</body>
</html>

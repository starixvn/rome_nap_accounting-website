<%@ Page Language="C#" AutoEventWireup="true" CodeFile="__AgentList.aspx.cs" Inherits="Customer_AgentList"
    Title="Untitled Page" %>

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
    
    <script type="text/javascript" src="../js/common.js"></script>
    <script type="text/javascript" src="../js/Customer.js"></script>
    <script type="text/javascript" src="../js/ViewIPInfo.js"></script>
    <script type="text/javascript" src="../js/jQuery/jquery.js"></script>
    <script src="../js/pager_v2.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.history.js" type="text/javascript"></script>
    <script src="../js/showhidepopup.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/master.agent.list.js"></script>
      <script runat="server">
        public string GetEditCustomerString()
        {
            if (WebSettings.IsShowCustomerSettingRenovation == 3)
            {
                return "EditAgent/EditAgentPage";
            }
            else
                return "Customer/v2/EditAgent.aspx";
        }
    </script>
    <script type="text/javascript">
        jQuery(document).ready(function() {
            var masterid = gup("masterId");
            $("#masterId").val(masterid);
            $("#allowHR").val(gup("hr"));
            $.historyInit(pageload, "AgentList.aspx");
            GenAgentListContentByPager(1);
        })

        function pageload(hash) {
            if (hash) {
                // restore ajax loaded state
                if ($.browser.msie) {
                    hash = encodeURIComponent(hash);
                }
                var pageindex = hash.toString().replace(/Page/i, '');
                GenAgentListContentByPager(pageindex);
            } else {
                // start page
            }
        }


        function callAjax(postdata) {
            try{parent.BlockUI();}catch(e){}
            $.ajax({
                type: "POST",
                url: "../Services/WCF/Customer/ServiceCustomerList.svc/GenAgentListContent",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: postdata,
                success: function(rs){
                    BindData(rs);
                    MasterAgentList.init("agent");
                }
            });
        }

        function BindData(rs) {
            document.getElementById("strcontent").innerHTML = rs;
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


        function GenAgentListContentByPager(pageIndex) {
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

            var masterid = $("#masterId").val();
            if (masterid != "") params += '{"masterId":"' + masterid + '",';
            else
                params += '{';
                
            var hr = $("#allowHR").val();
            if (hr != "") params += '"hr":"' + hr + '",';

            var syncCasinoUpline = gup("isSynccasino");
            if (syncCasinoUpline != "") params += '"syncCasinoUpline":"' + syncCasinoUpline + '",';

            params += '"PageIndex":' + pageIndex + '}';

            return params;
        }
        

        function OpenEdit(masterId, custid, uname) {
            var url = "<%=GetEditCustomerString()%>?custId=" + custid + "&masterId=" + masterId + "&uname=" + uname;
            var pageIndex="1";
           if (location.href.indexOf("#")>-1)
           {
            pageIndex=location.href.split("#")[1];
           }
            showPopup("Update Agent Setting", url ,"","",980,650, false, 2000, function (){ callAjax(getParams(pageIndex));});
        }       
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
                , "Customer/PT/Poker/AgentUpdate.aspx?custId=" + custId + "&roleId=" + roleId
                , 0, 0, 640, 480, false, 2000, null);
        }
        function liveCasinoSetting(custId, exchangeId, uplineId) {
            showPopup("<%= Resources.Language.liveCasinoTitle %>"
                , "LiveCasino/PTSetting/EditAgent.aspx?custId=" + custId + "&uplineId=" + uplineId
                , 0, 0, 640, 480, false, 2000, null);
        }
        function virtualSportsSetting(custId, uplineId, roleId) {
            showPopup("<%= Resources.Language.virtualsports %>"
                , "VirtualSportAgentPT/EditAgent?custId=" + custId + "&uplineId=" + uplineId + "&roleId=" + roleId
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
                    Agent List
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
           <div>
                <div id="strcontent">
                    Loading...
                </div>
                <form>
                <input type="hidden" id="masterId" />
                <input type="hidden" id="allowHR" />
                </form>
               
            </div>
        </div>
    </div>
</body>
</html>

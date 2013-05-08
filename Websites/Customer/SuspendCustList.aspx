<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SuspendCustList.aspx.cs"
    Inherits="Customer_SuspendCustList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" Theme="">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../js/Controls/StyledCheckbox/form[1].css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Pagination.css" rel="stylesheet" type="text/css" /> 
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />

    <script src="../js/jQuery/jquery.min.js" type="text/javascript"></script>
    <script src="../js/pager_v2.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.history.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/common.js"></script>
    <script src="../js/Controls/StyledCheckbox/custom-form-elements.js" type="text/javascript"></script>
   
    <script type="text/javascript">
        function GetSuspendList() {
            $("#errMsg").html("");
            callAjax(getParams("1"));
            
        }
        function UpdateSuspend(chkObj, id) {
         
            var f = self.frames["frUpdateSuspend"];
            if (f) {
                var isChecked;
                var strC = confirm("Are you sure you want to update auto suspend for this User?");
                var url = "../Handlers/UpdateSuspendMember.ashx?custId=" + id;
               
                if (chkObj.checked) {
                    url += "&autoSuspend=0";
                    isChecked = false;
                }
                else {
                    url += "&autoSuspend=1";
                    isChecked = true;
                }
                
                if (strC) {
                    f.location.href = url;
                    chkObj.checked = isChecked;
                }
                else {
                    chkObj.checked = !isChecked;
                }
            }
        }
        function On_KeyPress(e) {
            if (!e) e = window.event;
            var code = (e.keyCode) ? e.keyCode : e.which; 
            if (code == 13) {
                GetSuspendList();
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
                GenSuspendContentByPager(pageindex);
            } else {
                // start page
            }
        }

        jQuery(document).ready(function() {
            $.historyInit(pageload, "SuspendCustList.aspx");
            GetSuspendList();
        });

        function callAjax(postdata) {
            try{parent.BlockUI();}catch(e){}
            $.ajax({
                type: "POST",
                url: "../Services/WCF/Customer/ServiceSuspend.svc/GenSuspendListContent",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: postdata,
                success: BindData
            });
        }

        function BindData(rs) {
            $("#strcontent").html("");
            $("#strcontent").append(rs);
            $("#divcontainer").removeClass("allhidden");
            Custom.init();//add styled checkbox
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


        function GenSuspendContentByPager(pageIndex) {
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
            if ($("#txtSearch").length > 0 && $('#txtSearch').val() != "All")
                params += '{"searchText":"' + $('#txtSearch').val() + '",';
            else
                params += '{';
            if ($("#listAlpha").length > 0)
                if ($('#listAlpha').val() != "All") params += '"selectAlpha":"' + $('#listAlpha').val() + '",';
            params += '"PageIndex":' + pageIndex + '}';
            return params;
        }
    </script>

</head>
<body >
<form name="fSearch" action="SuspendCustList.aspx" method="post">
    <div class="main">
        <div class="maincontent">
             <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC fixfont">
                    <asp:Literal ID="SuspendListContentHeaderStr" runat="server"></asp:Literal>
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                <div id="searchbox"><%=strsearchbox%></div>
            </div>
            <div id="divcontainer" class="bg_table allhidden">
                <div id="strcontent"></div>
                <iframe name="frUpdateSuspend" class="hidden"></iframe>
            </div>
        </div>
    </div>
     </form>
</body>
</html>

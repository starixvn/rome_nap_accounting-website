<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OperatorList.aspx.cs" Inherits="NewCustomer_OperatorList" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../App_Themes/Public/Common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Pagination.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />

    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.history.js" type="text/javascript"></script>
    <script src="../js/showhidepopup.js" type="text/javascript"></script>
    
    <script type="text/javascript" src="../js/common.js"></script>
    <script type="text/javascript" src="../js/Customer.js"></script>
    <script src="../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>    
    <script src="../js/pager_v2.js" type="text/javascript"></script>
  
    
    <script type="text/javascript">
        var currentpage = 1;
        function OpenEditOperator(id) {
            var url = "Customer/EditOperator.aspx?custId=" + id;
            var theWidth = window.innerWidth ? window.innerWidth : document.body.offsetWidth;
            showPopup("Edit Info", url ,"","",440,440, false, 2000, function (){GetOperatorList();});
        }

        function OpenDelOperator(id) {
            var strC = confirm("Are you sure you want to delete this Operator?");
            if (strC) {
                var f = self.frames["frUpdate"];
                if (f) {
                    f.location.href = "../Handlers/DeleteOperator.ashx?operatorId=" + id;
                }
            }
            else {
                return false;
            }
        }
        function GetOperatorList(obj) {
            if (obj != null) {
                GenOperatorListContentByPager(1);
            }
            else {
                GenOperatorListContentByPager(currentpage);
            }
            return true;
        }
        function On_KeyPress(e) {
            if (!e) e = window.event;
            var code = (e.keyCode) ? e.keyCode : e.which; 
            if (code == 13) {
                GetOperatorList();
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
                GenOperatorListContentByPager(pageindex);
            } else {
                // start page
            }
        }

        jQuery(document).ready(function() {
            $.historyInit(pageload, "OperatorList.aspx");
        });

        function callAjax(postdata) {
            try{parent.BlockUI();}catch(e){}
            $.ajax({
                type: "POST",
                url: "../Services/WCF/Customer/ServiceCustomerList.svc/GenOperatorListContent",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: postdata,
                success: BindData
            });
        }

        function BindData(rs) {
            document.getElementById("strcontent").innerHTML = rs;
            $("#divcontainer").removeClass("allhidden");
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
        function GenOperatorListContentByPager(pageIndex) {
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
            currentpage = pageIndex;
            return params;
        }
    </script>

</head>
<body>
    <form id="fSearch" name="fSearch" method="get" action="OperatorList.aspx">
    <div class="main">
        <div class="maincontent">
             <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL"></td>
                    <td class="maincontent_headerC"><asp:Literal ID="OperatorListHeaderStr" runat="server"></asp:Literal></td>
                    <td class="maincontent_headerR"></td>
                </tr>
            </table>
            <div class="maincontent_box">
                <div id="searchbox">
                    <asp:Literal ID="Search" runat="server"></asp:Literal>
                </div>
            </div>
            <div id="divcontainer" class="bg_table allhidden">
                <iframe name="frUpdate" frameborder="0" height="0" width="0"></iframe>
                <input type="hidden" name="strSearch" id="strSearch" value="Submit" />
                <div id="strcontent">
                    <asp:Literal ID="OperatorListContentHtml" runat="server"></asp:Literal>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>

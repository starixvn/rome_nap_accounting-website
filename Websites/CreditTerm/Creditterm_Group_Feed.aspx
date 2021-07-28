<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Creditterm_Group_Feed.aspx.cs"
    Inherits="CreditTerm_Creditterm_Group_Feed" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/image.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/CreditTerm/creditterm.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css?v1" rel="stylesheet" type="text/css" />

    <link href="../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet" type="text/css" />
    <link href="../js/jQuery/GridScroller/gridscroller.css" rel="stylesheet" type="text/css" />

</head>
<body style="overflow: scroll !important;">
    <div class="main" style="width: 908px">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL"></td>
                    <td class="maincontent_headerC">Credit Term - Edit Group</td>
                    <td class="maincontent_headerR"></td>
                    <td>
                        <a id="goBack" href="CreditTermGroupView.aspx" class="" title="Credit Term List">
                            <img alt="Credit Term List" src="../App_Themes/Images/go_back.png" border="0" height="20" /></a>
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                <div id="errMsg" style="white-space: pre-line"></div>
                <div id="errMsg1" style="white-space: pre-line"></div>
                <div id="clear2"></div>
                <%=strfilter%>
            </div>
            <div class="bg_table">
                <div style="width: 900px">
                    <div id="sContent">
                        <%=strContent%>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <form id="form" method="post" action="Creditterm_Group_Feed.aspx">
        <input type="hidden" id="IsExport" name="IsExport" value="0" />
        <input type="hidden" id="groupid" name="groupid" value="<%=selectedGroupId%>" />
        <input type="hidden" id="headline" name="headline" value="<%=selectedHeadline%>" />
    </form>
    <div id="confirmAddUsername" class="hidden">
        <p>Beside adding super(s) into the selected Headline Group, the following super(s) will be added into External Group with Primary Extra PT Group indicated.</p>
        <p style="font-weight: bold;" id="headlineName"></p>
        <div id="supermapping" class="freegrid grid_mapping"></div>
    </div>

    <script src="../js/jQuery/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../Scripts/_Libs/jQuery/jquery-ui-1.9.1.min.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.json-1.3.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/_Libs/FanexGrid/Fanex.Grid.js?v=<%= AccCommon.Version() %>"></script>
    <script type="text/javascript">
        window.rootUrl = '<%=Page.ResolveUrl("~/")%>';
        var $confirmAddUsername = $("#confirmAddUsername").dialog({
            title: "Confirm Auto Adding Super To External Group",
            resizable: true,
            autoOpen: false,
            modal: true,
            width: '950px',
            height: 'auto',
            buttons: [
                {
                    text: "Submit",
                    click: function () {
                        $(this).dialog("close");
                        ExecuteCreditTerm('1');
                    }
                }
            ]
        });

        function checkall() {
            $("input[name=chkuser]").attr("checked", $("#chkcheckall").is(":checked"));
        }

        function ShowConfirmBeforeAdd() {
            if (!ValidateUsername()) {
                return;
            }

            try { parent.jQuery.BlockUI(); } catch (e) { }
            var usernames = $('#txtuser').val();
            var groupId = $('#groupid').val();
            $.ajax({
                type: "GET",
                url: window.rootUrl + "HeadlineExternalGroupMapping/GetExternalGroupHeadlineMappingSuper?usernames=" + usernames + "&groupId=" + groupId,
                contentType: "application/json; charset=utf-8",
                success: function (result) {

                    if (result.Supers !== null && result.Supers.length > 0) {
                        $confirmAddUsername.removeClass("hidden").dialog("open");
                        $("#headlineName").html("Headline: " + $("#headline").val());
                        RenderMapping(result.Supers);
                    } else {
                        ExecuteCreditTerm('1');
                    }
                },
                error: function () {
                },
                complete: function () {
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            });
        }

        function RenderMapping(data) {
            var bodyRows = [];
            for (var i = 0; i < data.length; i++) {
                var row = { Columns: [] };
                row.Columns.push({ Name: 'Super', Value: data[i].Username });
                row.Columns.push({ Name: 'Site', Value: data[i].Site });
                row.Columns.push({ Name: 'Currency', Value: data[i].Currency });
                row.Columns.push({ Name: 'NormalGroup', Value: data[i].NormalGroup });
                row.Columns.push({ Name: 'ExtraPTGroup', Value: data[i].ExtraPTGroup });
                row.Columns.push({ Name: 'PrimaryExtraPTGroup', Value: data[i].PrimaryExtraPTGroup });
                bodyRows.push(row);
            }

            var gridOptions =
            {
                columns: [
                    { HeaderText: "Super", Width: 100, Name: "Super", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Site", Width: 100, Name: "Site", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Currency", Width: 100, Name: "Currency", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "NormalGroup", Width: 150, Name: "NormalGroup", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "ExtraPTGroup", Width: 250, Name: "ExtraPTGroup", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "PrimaryExtraPTGroup", Width: 150, Name: "PrimaryExtraPTGroup", HeaderAlign: "center", CellAlign: "left" }
                ],
                bodyRows: bodyRows,
                gridExpandHeight: 200,
                showDistinctRow: false
            };

            $('#supermapping').fanexGrid(gridOptions);
        }

        function ValidateUsername() {
            var usernamesValue = $('#txtuser').val();
            if (usernamesValue.trim().length == 0) {
                showAlertMessage("errMsg", "Please enter Username.", 1);
                return false;
            }

            // Check usernames contains '\'.
            if (usernamesValue.indexOf("\n") >= 0
                || usernamesValue.indexOf("\r") >= 0
                || usernamesValue.indexOf("\\") >= 0) {
                showAlertMessage("errMsg", "You are not allowed to input [Enter] and backslash '\\' into the Usernames field.", 1);
                return false;
            }

            var usernamesArray = usernamesValue.split(",");
            if (usernamesArray.length > 500) {
                showAlertMessage("errMsg", "The function can only create up to 500 accounts per request.", 1);
                return false;
            }

            return true;
        }

        function ExecuteCreditTerm(type, userid, isshowConfirm) {
            $("#errMsg,#errMsg1").html('');
            var listuser = "";
            switch (type) {
                case '1':
                    if (!ValidateUsername()) {
                        return;
                    }
                    break;
                case '2':
                    listuser = userid;
                    break;
                case '3':
                    $('input[name=chkuser]:checked').each(
                        function () {
                            listuser += $(this).val() + ",";
                        });
                    if (listuser == "") {
                        showAlertMessage("errMsg", "Please select user.", 1);
                        return;
                        break;
                    }
            }
            if (type == 3 && isshowConfirm && !confirm('The group has only 1 member (is Headline). Are you sure you want to remove this group?')) {
                return;
            }
            else {
                try { parent.BlockUI(); } catch (e) { }

                var param = '';
                param += '&mode=9';
                param += '&type=' + type;
                param += '&username=' + $('#txtuser').val();
                param += '&lstlistname=' + listuser;
                $.ajax(
                    {
                        type: "POST",
                        url: "../Report/GetData.ashx",
                        data: param,
                        success: GetJSONResult
                    }
                );
            }
        }

        function GetJSONResult(resContent) {
            var reportInfo = $.evalJSON(resContent);
            var sData = reportInfo.CreditTerm;
            $("#sContent").html(sData);
            try {
                setTimeout(function () { parent.jQuery.unblockUI(); }, 2000);
            } catch (e) { }

        }

        function RedirectToGroupList() {
            setTimeout(function () {
                window.location.href = "../CreditTerm/CreditTermGroupView.aspx";
            }, 3000)
        }

        function ExportToExcel(groupid) {
            $('#IsExport').val('1');
            $('#groupid').val(groupid);
            $('#form').submit();
        }
    </script>
</body>
</html>

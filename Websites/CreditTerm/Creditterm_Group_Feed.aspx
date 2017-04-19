<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Creditterm_Group_Feed.aspx.cs"
    Inherits="CreditTerm_Creditterm_Group_Feed" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/image.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/CreditTerm/creditterm.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.json-1.3.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>

</head>
<body>
    <div class="main" style="width: 908px">
        <div class="maincontent">
             <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL"></td>
                    <td class="maincontent_headerC">Credit Term - Edit Group</td>
                    <td class="maincontent_headerR"></td>
                    <td>
                        <a id="goBack" href="CreditTermGroupView.aspx" class="" title="Credit Term List"><img alt="Credit Term List" src="../App_Themes/Images/go_back.png" border="0" height="20" /></a>
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                <div id="errMsg"></div>
                <div id="clear2"></div>
                <%=strfilter%>
            </div>
            <div class="bg_table">
                <div style="width: 900px">
                    <div id="sContent">
                        <%=strContent%>
                    </div>
                </div>
                <script type="text/javascript">
                    
                    function checkall() {
                        $("input[name=chkuser]").attr("checked", $("#chkcheckall").attr("checked"));
                    }
                    function ExecuteCreditTerm(type, userid) {
                        var listuser="";
                        switch (type) {
                            case '1':
                                if ($('#txtuser').val().length == 0) {
                                    showAlertMessage("errMsg","Please enter Username.",1);
                                    return;
                                }
                                break;
                            case '2':
                                listuser = userid;
                                break;
                            case '3':
                                $('input[name=chkuser]:checked').each(
                                function() {
                                listuser += $(this).val() + ",";
                                });

                                if (listuser == "") {
                                    showAlertMessage("errMsg", "Please select user.",1);
                                    return;
                                }
                                break;
                        }

                        try{parent.BlockUI();}catch(e){}

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

                    function GetJSONResult(resContent) {
                        var reportInfo = $.evalJSON(resContent);
                        var sData = reportInfo.CreditTerm;

                        $("#sContent").html(sData);
                        try{parent.jQuery.unblockUI();}catch(e){}

                    }

                    function ExportToExcel(groupid) {
                    	$('#IsExport').val('1');
                    	$('#groupid').val(groupid);
                    	$('#form').submit();
                    }
                </script>

            </div>
        </div>
    </div>
	<form id="form" method="post" action="Creditterm_Group_Feed.aspx">
        <input type="hidden" id="IsExport" name="IsExport" value="0" />
		<input type="hidden" id="groupid" name="groupid" value="0" />
    </form>
</body>
</html>

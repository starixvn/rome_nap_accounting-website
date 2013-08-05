<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreditTermGroupView.aspx.cs"
    Inherits="CreditTermGroupView" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../js/jQuery/Template/jquery.tmpl.js" type="text/javascript"></script>
    <script src="../js/Common/Functions.js" type="text/javascript"></script>
    <script src="js/CreditTerm_V2.js?vs=134" type="text/javascript"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            CreditTerm.Group.getAllView();
        });
    </script>
</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        Credit Term - View All Group
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box" style="width: 800px;">
                <div id="credit_content" style="">
                    <div class="panel_search">
                        <span id="search"><b>Headline Filtering</b>&nbsp;
                            <input id="txt_search" type="text" class="text" value="" /><input id="btn_search"
                                type="button" class="btn_search" value="&nbsp;" title="Search" /><input id="btn_clear"
                                    type="button" class="btn_clear" value="&nbsp;" title="Clear search" />
                        </span>
						<div class="btngroup">
                            <input type="button" title="Export to Excel" class="btnExcel" id="btnExport" name="btnExport" value="  " onclick="ExportToExcel();" />
						</div>
					</div>
                    <div id="group_header" class="header">
                        <div class="index">
                            #</div>
                        <div class="group sorter" sort_name="group">
                            GroupID</div>
                        <div class="uname sorter" sort_name="uname">
                            Username of Headline</div>
                        <div class="currency">
                            Currency</div>
                        <div class="members">
                            Member(s)</div>
                        <div class="checker">
                            Checker</div>
                        <div class="consolidationreport">
                            Consolidated Group</div>
                        <div class="edit">
                            Edit Group</div>
                        <div id="refresh" class="refresh end">
                            <img title="Refresh" alt="Refresh" src="../App_Themes/images/refresh_icon.png" border="0" /></div>
                    </div>
                    <div id="group_content" class="content">
                    </div>
                </div>
            </div>
        </div>
    </div>
	<form id="form" method="post" action="CreditTermGroupView.aspx">
        <input type="hidden" id="IsExport" name="IsExport" value="0" />
    </form>
	<script type="text/javascript">
		function ExportToExcel() {
			$('#IsExport').val('1');
			$('#form').submit();
		}
    </script>
</body>
</html>

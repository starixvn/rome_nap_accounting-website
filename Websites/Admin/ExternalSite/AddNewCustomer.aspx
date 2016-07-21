<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddNewCustomer.aspx.cs" Inherits="Admin_ExternalSite_AddNewCustomer" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>External Site</title>
    <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../js/jQuery/Pager/pagination.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link href="style/AddCustomer.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table cellspacing="0px" cellpadding="0px" border="0" class="maincontent_header">
                <tbody>
                    <tr>
                        <td class="maincontent_headerL"></td>
                        <td class="maincontent_headerC">Manage External Accounts</td>
                        <td class="maincontent_headerR"></td>
                    </tr>
                </tbody>
            </table>
            <div class="maincontent_box" style="width: 1057px">
                <div class="hidden" id="trMsg">
                    <div id="dvMsg"></div>
                </div>
                <table>
                    <tr>
                        <td>Username</td>
                        <td colspan="2">
                            <input type="text" class="text" id="txt_search" style="width: 97%" />
                        </td>
                    </tr>
                    <tr>
                        <td>System</td>
                        <td>
                            <select id="groupName" style="width: 102px; border: 1px solid Black"></select>
                        </td>
                        <td>
                            <button type="button" class="btn" id="btnSearch">Submit</button>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="width: 1063px" class="hidden" id="nodata">
                <div id="err_nodata" class="hidden"></div>
                <div class="content-toolbox bgtitle">
                    <div class="btngroup">
                        <input type="button" class="btnAdd" title="Add Existing Super" value="  " />
                        <input type="button" class="btnRefresh" title="Refresh" value="  " />
                    </div>
                </div>
            </div>
            <div id="gridExAccControl" class="content-warper hidden">
                <div class="content-toolbox bgtitle">
                    <div class="btngroup">
                        <input type="button" class="btnAdd" title="Add Existing Super" value="  " />
                        <input type="button" class="btnRefresh" title="Refresh" value="  " />
                    </div>
                </div>
                <div id="nodata_search" class="hidden"></div>
                <div class="content-header clearfix">
                    <div id="headerscrollbettype" class="header-scroll">
                        <table class="tbl-header-scroll">
                            <tr>
                                <td style="border-bottom: none 0px;">
                                    <div class="w40 c b">No</div>
                                </td>
                                <td style="border-bottom: none 0px;">
                                    <div class="w180 c">Username</div>
                                </td>
                                <td style="border-bottom: none 0px;">
                                    <div class="w160 c">
                                        <%=Resources.Language.FirstName %>
                                    </div>
                                </td>
                                <td style="border-bottom: none 0px;">
                                    <div class="w160 c">
                                        <%=Resources.Language.LastName %>
                                    </div>
                                </td>
                                <td style="border-bottom: none 0px;">
                                    <div class="w160 c">
                                        <%=Resources.Language.NormalGroup %>
                                    </div>
                                </td>
                                <td style="border-bottom: none 0px;">
                                    <div class="w160 c">
                                        <%=Resources.Language.ExtraPTGroup %>
                                    </div>
                                </td>
                                <td style="border-bottom: none 0px;">
                                    <div class="w120 c">
                                        <%=Resources.Language.iplogin%>
                                    </div>
                                </td>
                                <td style="border-bottom: none 0px;">
                                    <div class="w40 c"></div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="content-body clearfix">
                    <div class="body-scroll">
                        <table id="tblexternalsite" class="tbl-body-scroll"></table>
                    </div>
                </div>
                <div class="content-pagination bgtitle clearfix">
                    <div class="pagination-scroll">
                        <table style="width: 100%; padding-top: 2px" cellspacing="0" cellpadding="0">
                            <tr>
                                <td>
                                    <div id="pagination"></div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="addSuperToGroupDialog" class="hidden">
        <span id="errMsg"></span>
        <table>
            <tr>
                <td>
                    <div class="w60 c">Username</div>
                </td>
                <td>
                    <div class="w150">
                        <input type="text" id="txtSuper" style="width: 140px" />
                    </div>
                </td>
                <td>
                    <button type="button" class="btn" id="addSuperToGroup">Add</button>
                </td>
            </tr>
        </table>
    </div>
    <div id="changePrimaryGroup" class="hidden">
        <%--<span id="errMsg"></span>--%>
        <table>
            <tr>
                <td>
                    <div class="w60 c">Username:</div>
                </td>
                <td>
                    <div class="w150">
                        AD
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="w60 c">Effective Date:</div>
                </td>
                <td>
                    <div class="w150">
                        AD
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/gridscroller.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.json-2.2.min.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="../../js/jQuery/Pager/jpager.js" type="text/javascript"></script>
    <script src="../../js/showhidepopup.js" type="text/javascript"></script>
    <script src="../../js/Common/ArrayUtils.js" type="text/javascript"></script>
    <script src="js/ExternalSite.js" type="text/javascript"></script>
</body>
</html>

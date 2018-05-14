<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddNewCustomer.aspx.cs" Inherits="Admin_ExternalSite_AddNewCustomer" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>External Site</title>
    <link rel="stylesheet" href="../../App_Themes/Public/common.css" />
    <link rel="stylesheet" href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" />
    <link rel="stylesheet" href="../../App_Themes/Public/input.css" />
    <link rel="stylesheet" href="../../App_Themes/Public/Message.css" />
    <link rel="stylesheet" href="../../js/jQuery/Pager/pagination.css" />
    <link rel="stylesheet" href="../../App_Themes/Report/reportgrid.css" />
    <link rel="stylesheet" href="../../js/select2/select2.min.css" />
    <link rel="stylesheet" href="style/AddCustomer.css?v=<%=AccCommon.Version()%>" />
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
                <div id="trMsg"></div>
                <table width="100%">
                    <tr>
                        <td>Username</td>
                        <td colspan="2">
                            <input type="text" class="text" id="txt_search" style="width: 97%" />
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="w50">System</td>
                        <td class="w105">
                            <select id="groupName" style="width: 102px; border: 1px solid Black"></select>
                        </td>
                        <td class="w40">
                            <button type="button" class="btn" id="btnSearch">Submit</button>
                        </td>
                        <td>
                            <button type="button" class="btn" id="btnAdd">Add Existing Super</button>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="width: 1063px" class="hidden" id="nodata">
                <div id="err_nodata"></div>
                <div class="content-toolbox bgtitle">
                    <div class="btngroup">
                        <input type="button" class="btnRefresh" title="Refresh" value="  " />
                    </div>
                </div>
            </div>
            <div id="gridExAccControl" class="content-warper hidden">
                <div class="content-toolbox bgtitle">
                    <div class="btngroup">
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
        <div id="errMsg"></div>
        <table width="100%">
            <tr>
                <td class="w60 r">Group</td>
                <td>
                    <select id="dropDownGroupName" multiple="multiple" style="width: 100%;"></select>
                </td>
            </tr>
            <tr>
                <td class="r">Username</td>
                <td>
                    <textarea id="txtSuper" cols="50" rows="5"></textarea>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>Enter usernames separated by ','</td>
            </tr>
            <tr>
                <td></td>
                <td class="r">
                    <button type="button" class="btn w50" id="addSuperToGroup">Add</button>
                </td>
            </tr>
        </table>
    </div>
    <div id="changeGroupDialog" class="hidden">
        <span id="changeGroupErrorMsg"></span>
        <table>
            <tr>
                <td>
                    <div class="w120">Username:</div>
                </td>
                <td>
                    <span id="spanSuperName"></span>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="w120">New Effective Date:</div>
                </td>
                <td>
                    <input type="text" class="txtdate" id="txtEffectiveDate" readonly="readonly" />
                </td>
            </tr>
        </table>
        <div id="changeGroupGrid" class="content-warper">
            <div class="content-header clearfix">
                <div class="header-scroll">
                    <table class="tbl-header-scroll">
                        <tr>
                            <td style="border-bottom: none 0px;">
                                <div class="w40 c">#</div>
                            </td>
                            <td style="border-bottom: none 0px;">
                                <div class="w180 c">Group</div>
                            </td>
                            <td style="border-bottom: none 0px;">
                                <div class="w120 c">Is Primary</div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="content-body clearfix">
                <div class="body-scroll">
                    <table id="changeGroupTable" class="tbl-body-scroll"></table>
                </div>
            </div>
        </div>
        <div class="clearfix" style="margin-top: 10px; width: 341px">
            <div style="float: right">
                <button type="button" class="btn" id="changeGroupSubmit">Submit</button>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var today = "<%=DateTime.Today.ToString("MM/dd/yyyy")%>";
        var tomorrow = "<%=DateTime.Today.AddDays(1).ToString("MM/dd/yyyy")%>";
    </script>
    <script type="text/javascript" src="../../Scripts/_Libs/jQuery/jquery-1.8.2.min.js"></script>
    <script type="text/javascript" src="../../Scripts/_Libs/jQuery/jquery-ui-1.9.1.min.js"></script>
    <script type="text/javascript" src="../../js/jQuery/gridscroller.js"></script>
    <script type="text/javascript" src="../../js/jQuery/jquery.json-2.2.min.js"></script>
    <script type="text/javascript" src="../../js/common.js"></script>
    <script type="text/javascript" src="../../js/jQuery/Pager/jpager.js"></script>
    <script type="text/javascript" src="../../js/showhidepopup.js"></script>
    <script type="text/javascript" src="../../js/Common/ArrayUtils.js"></script>
    <script type="text/javascript" src="../../js/select2/select2.min.js"></script>
    <script type="text/javascript" src="js/ExternalSite.js?v=<%=AccCommon.Version()%>"></script>
</body>
</html>

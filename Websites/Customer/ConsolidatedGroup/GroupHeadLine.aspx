<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GroupHeadLine.aspx.cs" Inherits="Customer_AgencyGroup_GroupHeadLine" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Group headline</title>
    <link href="../../App_Themes/Public/table.css"rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../js/Controls/StyledCheckbox/form[1].css" rel="stylesheet" type="text/css" />
    <link type="text/css" href="css/jquery.msg.css" rel="stylesheet" />
    <link type="text/css" href="css/grid.css" rel="stylesheet" />
    
    <script type="text/javascript" src="../../js/jQuery/jquery-latest.js"></script>
    <script src="../../js/Controls/StyledCheckbox/custom-form-elements.js" type="text/javascript"></script>
    
    <script src="../../js/jQuery/Bubblepopup/jquery.bubblepopup.v2.3.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../js/jQuery/message/jquery.center.min.js"></script>
    <script type="text/javascript" src="../../js/jQuery/message/jquery.msg.js"></script>
    
    <script type="text/javascript" src="../../js/BlockUI.js"></script>
    <script type="text/javascript" src="../../js/jQuery/jquery.blockUI.1.33.js"></script>
    
    <script type="text/javascript" src="../../js/jQuery/Template/jquery.tmpl.js"></script>
    <script type="text/javascript" src="../../js/jQuery/knockout-1.2.0.js"></script>
    <script type="text/javascript" src="../../js/jQuery/jquery.maxlength.js"></script>
    
    <script type="text/javascript" src="../../js/jQuery/Jlinq/jlinq.js"></script>
    <script type="text/javascript" src="../../js/jQuery/Jlinq/jlinq.jquery.js"></script>
    
    <script type="text/javascript" src="../../js/jQuery/highlight/jquery.highlight-3.js"></script>
    
    <script type="text/javascript" src="js/ConsolidatedGroup.js?ver=<%=WebSettings.Version%>"></script>
    
    <script type="text/javascript" src="../../js/json2.js"></script>
    
    <script type="text/javascript">
    $(window).load(function() {
        ConsolidatedGroup.init();
    });
    ConsolidatedGroup.dataTableCustomerGroup = [
        <%= strRows %>
    ];
    ConsolidatedGroup.dataTableCustomerGroupBackup = [
        <%= strRows %>
    ];
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
                        <div id="divTitle">View All Groups</div>
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div id="divMessage" style="display:none"></div>
            <div id="divAssignGroupHeader" class="maincontent_box" style="display:none">
                <div class="pad_tb_4">
                    <span id="btnViewAllGroups" style="cursor:pointer; color:#3399FF">[ View all groups ]</span>
                </div>
            </div>
            <div id="divAssignGroup" class="bg_table" style="display:none">
                <div style="width:500px">
                    <table id="tblData" class="tbllayout" cellpadding="1" style="width:500px">
                        <tr class="title">
                            <td colspan="2">
                                <div>New Group</div>
                            </td>
                        </tr>
                        <tr>
                            <td class="l b" style="width:140px">
                                <div>Customer:</div>
                            </td>
                            <td class="l">
                                <div>
                                    <input class="txt" type="text" id="tbxCustomer" style="float:left" />
                                    <span style="color:red; display:none; float:left; margin-left:4px" id="spanErrorCustomer">Please input customer name!</span>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="l b" valign="top">
                                <div>Remark:</div>
                            </td>
                            <td class="l">
                                <div>
                                    <textarea id="textRemark" class="txt" cols="50" rows="10" style="width:320px" maxlength="500"></textarea>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div>
                                    <input id="btnSubmit" type="button" class="btn" value="Submit" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            
            <div id="divViewAllGroupHeader" class="maincontent_box">
                <table border="0" cellpadding="0" cellspacing="0" style="border-collapse:collapse">
                    <tr>
                        <td>
                            <div class="pad_tb_4" style="width:454px">
                                <table border="0" cellpadding="0" cellspacing="0" style="border-collapse:collapse">
                                    <tr>
                                        <td>
                                            <div style="font-weight:bold">Search:</div>
                                        </td>
                                        <td>
                                            <div style="width:165px;  padding-left:4px; padding-right:4px; position:relative">
                                                <input type="text" class="txt" id="tbxSearch" value="By username of headline" style="font-style:italic; color:Gray; width:100%" />
                                                <img id="btnSearch" title="Search" 
                                                    alt="Search" src="img/search.png" 
                                                    style="cursor:pointer; position:absolute; float:right; top:1px; right:20px" />
                                                <img id="btnClearText" title="Clear search text" 
                                                    alt="Clear search text" src="img/Close_up.png"
                                                    style="cursor:pointer; position:absolute; float:right; top:1px; right:2px" />
                                            </div>
                                        </td>
                                        <td>
                                            <div style="width:86px; margin-left:8px">
                                                <div id="divAddNewGroup" class="buttonhover" style="border:1px solid #7C98A4; cursor:pointer; background:url('img/bg_btn.jpg') repeat-x scroll left center transparent">
                                                    <table cellspacing="0" cellpadding="0" border="0" style="border-collapse:collapse">
                                                        <tbody><tr>
                                                            <td>
                                                                <div style="width:12px">
                                                                    <img src="img/plus.png" alt="">
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div style="font-weight:bold; width:70px; text-align:center; padding-top:1px; padding-bottom:1px">
                                                                    New Group
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </tbody></table>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <td>
                            <div>
                                <span id="spanTotal" style="color:black; font-weight:bold">Total: <span data-bind="text: dataTableCustomerGroup.length"></span><span data-bind="text: dataTableCustomerGroup.length <= 1 ? ' group' : ' groups'"> groups</span></span>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divViewAllGroup" class="bg_table">
                <div style="width:546px">
                    <div>
                        <table border="0" cellpadding="0" cellspacing="0" style="border-collapse:collapse">
                            <thead>
                                <tr class="gridtitle">
                                    <th>
                                        <div id="divGroupIDHeader" style="width:80px; cursor:pointer;">
                                            <table border="0" cellpadding="0" cellspacing="0" style="border-collapse:collapse; margin-left:auto; margin-right:auto">
                                                <tr>
                                                    <td>
                                                        <span>GroupID</span>
                                                    </td>
                                                    <td>
                                                        <div>
                                                            <table border="0" cellpadding="1" cellspacing="0" style="border-collapse:collapse">
                                                                <tr>
                                                                    <td>
                                                                        <div id="btnGroupIDUp" style="margin-left:-1px; width:21px; height:4px; background:url('img/sorting_asc.gif') no-repeat;"></div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <div id="btnGroupIDDown" style="width:21px; height:4px; background:url('img/sorting_desc.gif') no-repeat;"></div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </th>
                                    <th>
                                        <div id="divUserNameHeader" style="width:200px; cursor:pointer">
                                            <table border="0" cellpadding="0" cellspacing="0" style="border-collapse:collapse; margin-left:auto; margin-right:auto">
                                                <tr>
                                                    <td>
                                                        <span>Username of Headline</span>
                                                    </td>
                                                    <td>
                                                        <div>
                                                            <table border="0" cellpadding="1" cellspacing="0" style="border-collapse:collapse">
                                                                <tr>
                                                                    <td>
                                                                        <div id="btnUsernameUp" style="margin-left:-1px; width:21px; height:4px; background:url('img/sorting_asc.gif') no-repeat;"></div>
                                                                    </td>
                                                                </tr>
                                                                <tr style="display:none">
                                                                    <td>
                                                                        <div id="btnUsernameDown" style="width:21px; height:4px; background:url('img/sorting_desc.gif') no-repeat;"></div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </th>
                                    <th>
                                        <div style="width:100px">
                                            <span>Total Member(s)</span>
                                        </div>
                                    </th>
                                    <th>
                                        <div id="divCurrencyHeader" style="width:80px; cursor:pointer">
                                            <span>Currency</span>
                                            <img alt="Filter" src="img/Filter.png" style="width:10px; height:10px" />
                                        </div>
                                        <div id="divListCurrency" style="display:none">
                                            <div style="position:absolute; width:80px; display:none">
                                                <select id="selectListCurrency"></select>
                                            </div>
                                            <div style="position:absolute; width:78px; height:80px; overflow-y:auto; overflow-x:hidden;
                                                border:1px solid gray; background-color:White">
                                                <table border="0" cellpadding="1" cellspacing="0" style="border-collapse:collapse">
                                                    <tbody id="tbodyCurrencyList">
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </th>
                                    <th>
                                        <div style="width:80px;">Edit Group</div>
                                    </th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                    <div style="text-align:center; 
                        border-bottom:1px solid #7C98A4; 
                        border-left:1px solid #7C98A4; 
                        border-right:1px solid #7C98A4;
                        padding-top:4px; padding-bottom:4px; background-color:white; display:none"
                        data-bind="visible: dataTableCustomerGroup.length <= 0">There is no information</div>
                    <div id="divBodyAllGroup" data-bind="style:{height: dataTableCustomerGroup.length > 20 ? '500px' : 'auto'}, visible: dataTableCustomerGroup.length > 0" 
                        style="overflow-y:auto;border-right:1px solid #7C98A4; border-bottom:1px solid #7C98A4; overflow-x:hidden; display:none">
                        <table border="0" cellpadding="0" cellspacing="0" style="border-collapse:collapse">
                            <tbody id="tbodyAllGroup" class="gridrow">
                            </tbody>
                        </table>
                        <script id="scriptAllGroup" type="text/html">
                            <tr><td>
                                    <div style="width:80px">${GroupID}</div>
                                </td>
                                <td>
                                    <div style="width:200px">
                                        ${UserName}
                                    </div>
                                </td>
                                <td>
                                    <div style="width:100px">
                                        ${TotalMember}
                                    </div>
                                </td>
                                <td>
                                    <div style="width:80px">
                                        ${CurrencyName}
                                    </div>
                                </td>
                                <td>
                                    <div style="width:80px" endcol="true">
                                        <img alt="edit" src="img/page_white_edit.png" style="cursor:pointer" onclick="javascript:ConsolidatedGroup.AllGroup.editGroup(${GroupID});" />
                                    </div>
                                </td>
                            </tr>
                        </script>
                    </div>
                </div>
            </div>
            
            <div id="divEditGroupHeader" class="maincontent_box" style="display:none">
                <div>
                    <table border="0" cellpadding="4" cellspacing="0" style="border-collapse:collapse">
                        <tr>
                            <td>
                                <div style="font-weight:bold">GroupID:</div>
                            </td>
                            <td>
                                <div id="divGroupID" style="width:30px; text-align:left; border-right:1px solid gray; color:black"></div>
                                <input type="hidden" id="hiddenEditGroupGroupID" />
                            </td>
                            <td>
                                <div style="font-weight:bold; margin-left:10px">Username:</div>
                            </td>
                            <td>
                                <input type="text" class="txt" id="tbxUsernameEdit" />
                            </td>
                            <td>
                                <div id="btnAddToGroup" style="border:1px solid #7C98A4; cursor:pointer; background:url('img/bg_btn.jpg') repeat-x scroll left center transparent" class="buttonhover">
                                    <table border="0" cellpadding="0" cellspacing="0" style="border-collapse:collapse">
                                        <tr>
                                            <td>
                                                <div style="width:12px">
                                                    <img alt="" src="img/plus.png" />
                                                </div>
                                            </td>
                                            <td>
                                                <div style="font-weight:bold; width:80px; text-align:center; padding-top:1px; padding-bottom:1px">
                                                    Add to group
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                            <td>
                                <div style="width:16px; margin-left:4px">
                                    <img id="btnBackFromEditGroup" alt="Back" src="img/back_icon.png" style="cursor:pointer" />
                                </div>
                            </td>
                            <td>
                                <div style="width: 135px; text-align:right">
                                    <span style="font-weight:bold; color:black">Total: <span data-bind="text: consolidatedMemberTable.length"></span><span data-bind="text: consolidatedMemberTable.length <= 1 ? ' member' : ' members'">members</span>
                                    </span>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="divEditGroup" class="bg_table" style="display:none">
                <div style="width:716px">
                    <div>
                        <table border="0" cellpadding="0" cellspacing="0" style="border-collapse:collapse">
                                <thead>
                                    <tr class="gridtitle">
                                        <th>
                                            <div id="divEditGroupUserNameHeader" style="width:200px; cursor:pointer">
                                                <table border="0" cellpadding="0" cellspacing="0" style="border-collapse:collapse; margin-left:auto; margin-right:auto">
                                                    <tr>
                                                        <td>
                                                            <span>Username</span>
                                                        </td>
                                                        <td>
                                                            <div>
                                                                <table border="0" cellpadding="1" cellspacing="0" style="border-collapse:collapse">
                                                                    <tr>
                                                                        <td>
                                                                            <div id="btnEditGroupUserNameSortUp" style="margin-left:-2px; width:21px; height:4px; background:url('img/sorting_asc.gif') no-repeat;"></div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <div id="btnEditGroupUserNameSortDown" style="margin-left:-1px; width:21px; height:4px; background:url('img/sorting_desc.gif') no-repeat;"></div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </th>
                                        <th>
                                            <div style="width:140px">
                                                <span>Currency</span>
                                            </div>
                                        </th>
                                        <th>
                                            <div id="divSetHeadlineHeader" style="width:280px">
                                                <span>Set Headline</span>
                                            </div>
                                        </th>
                                        <th>
                                            <div style="width:73px; cursor:pointer">
                                                <input type="checkbox" class="styled" id="cbxEditGroupHeader" />
                                            </div>
                                        </th>
                                        <th>
                                            <div style="width:18px" id="divEditGroupScrollBar"></div>
                                        </th>
                                    </tr>
                                </thead>
                            </table>
                    </div>
                    <div style="text-align:center; 
                            border-bottom:1px solid #7C98A4; 
                            border-left:1px solid #7C98A4; 
                            border-right:1px solid #7C98A4;
                            padding-top:4px; padding-bottom:4px; background-color:white;"
                            data-bind="visible: consolidatedMemberTable.length <= 0">There is no information</div>
                    <div id="divBodyEditGroup" data-bind="style:{height: consolidatedMemberTable.length > 23 ? '500px' : 'auto'}, visible: consolidatedMemberTable.length > 0"
                        style="overflow-y:scroll;border-right:1px solid #7C98A4; border-bottom:1px solid #7C98A4; overflow-x:hidden;">
                        <table border="0" cellpadding="0" cellspacing="0" style="border-collapse:collapse">
                            <tbody id="tbodyEditGroup" class="gridrow">
                            </tbody>
                        </table>
                        <script id="scriptEditGroup" type="text/html">
                            <tr><td>
                                    <div style="width:200px">
                                        ${UserName}
                                    </div>
                                </td>
                                <td>
                                    <div style="width:140px">
                                        ${CurrencyName}
                                    </div>
                                </td>
                                <td>
                                    <div style="width:280px">
                                        {{if IsHeader == true}}
                                            <img alt="" src="img/headlineH.gif" imgheadline="true" style="cursor:pointer" />
                                        {{else}}
                                            <img alt="" src="img/headline.gif" style="cursor:pointer" setheadline="true" />
                                        {{/if}}
                                        <input type="hidden" value="${UserName}" username="true" />
                                        <input type="hidden" value="${CustID}" custid="true" />
                                        <input type="hidden" value="${IsHeader}" isheader="true" />
                                    </div>
                                </td>
                                <td>
                                    <div style="width:73px" endcol="true">
                                        <input type="checkbox" 
                                        {{if IsHeader == false}}
                                            class="styled" 
                                        {{else}}
                                            style="display:none"
                                        {{/if}}
                                        />
                                        <input type="hidden" value="${UserName}" username="true" />
                                        <input type="hidden" value="${CustID}" custid="true" />
                                        <input type="hidden" value="${IsHeader}" isheader="true" />
                                    </div>
                                </td>
                            </tr>
                        </script>
                    </div>
                    <div id="divFooterEditGroup" style="margin-top:4px">
                        <div id="btnRemoveUser" style="border:1px solid #7C98A4; cursor:pointer; width:100px; float:right; background:url('img/bg_btn.jpg') repeat-x scroll left center transparent" class="buttonhover">
                            <table border="0" cellpadding="0" cellspacing="0" style="border-collapse:collapse">
                                <tr>
                                    <td>
                                        <div style="width:12px">
                                            <img alt="" src="img/cleartext.png" />
                                        </div>
                                    </td>
                                    <td>
                                        <div style="font-weight:bold; width:80px; text-align:center; padding-top:1px; padding-bottom:1px; margin-left:2px">
                                            Remove user
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>

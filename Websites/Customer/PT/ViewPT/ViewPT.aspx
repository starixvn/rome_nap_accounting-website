<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewPT.aspx.cs" Inherits="Customer_PT_ViewPT_ViewPT" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View PT</title>
    <link href="../../../js/Controls/StyledCheckbox/form[1].css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/Public/Common.css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/PT.css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="Styles/ViewPT.css" />
</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        <%=TileText%><%--<asp:Literal ID="sTitle" runat="server"></asp:Literal>--%>
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                <div>
                    <table cellpadding="2">
                        <tr>
                            <td class="l b">
                                PT:
                            </td>
                            <td class="l">
                                <input type="radio" class="styled" id="rdoPT" name="chkType" value="PT" checked="checked"
                                    onclick="ChangeType(this);" />
                            </td>
                            <td class="l b">
                                Comm:
                            </td>
                            <td class="l">
                                <input type="radio" class="styled" id="rdoComm" name="chkType" value="COMM" onclick="ChangeType(this);" />
                            </td>
                            <td width="20">
                                &nbsp;
                            </td>
                            <td>
                                <table id="tblPT" cellpadding="0px" cellspacing="0px" border="0px">
                                    <tr>
                                        <td class="b">
                                            Sport Type:&nbsp;
                                        </td>
                                        <td>
                                            <select id="ddlSporttype" style="width: 120px;">
                                            </select>
                                        </td>
                                        <td width="10">
                                            &nbsp;
                                        </td>
                                        <td class="b">
                                            Bet Type:&nbsp;
                                        </td>
                                        <td id="tdBettype">
                                            <select id="ddlBettype" style="width: 150px;">
                                                <option value="0">..:: ALL ::..</option>
                                            </select>
                                        </td>
                                        <td width="10">
                                            &nbsp;
                                        </td>
                                        <td class="b">
                                            Is Live:&nbsp;
                                        </td>
                                        <td id="showChkLive">
                                            <input type="checkbox" id="chkLive" value="1" class="styled" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td id="products">
                                <table id="tblProducts" style="display: none;" cellpadding="0px" cellspacing="0px" border="0px">
                                    <tr>
                                        <td class="b">
                                            Products:&nbsp;
                                        </td>
                                        <td>
                                            <select id="ddlProduct" style="width: 120px;">
                                            </select>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="b">
                                Username:
                            </td>
                            <td>
                                <input type="text" id="txtUsername" onkeypress="return KP(event);" class="txt" />
                            </td>
                            <td>
                                <input type="button" id="btnSubmit" value="Submit" onclick="ViewPT.viewData();" class="btn"
                                    disabled="disabled" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divHeader">
                </div>
            </div>
            <div class="bg_table" id="divcontainer">
             <iframe id="frmSportsbook2" name="frmSportsbook2" src="" marginwidth="0" scrolling="auto"
                    marginheight="0" frameborder="0" vspace="0" hspace="0" style="width: 99%; height: 825px;">
                </iframe>
                <div id="sContent" style="overflow: auto; height: auto; max-height: 700px">
                </div>
                <div id="err">
                </div>
            </div>
        </div>
    </div>
  
    <script src="../../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../../js/jQuery/jquery.json-2.2.min.js" type="text/javascript"></script>
    <script src="../../../js/jQuery/jquery.tmpl.js" type="text/javascript"></script>
    <script src="../../../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="../../../js/BlockUI.js" type="text/javascript"></script>
    <script src="../../../js/common.js" type="text/javascript"></script>
    <script src="../../../js/Controls/StyledCheckbox/custom-form-elements.js" type="text/javascript"></script>
    <script type="text/javascript" src="JS/ViewPT_Bingo.js?ver=<%=WebSettings.Version%>"></script>
    <script type="text/javascript" src="JS/ViewPT.js?ver=<%=WebSettings.Version%>"></script>
</body>
</html>
<%-- HI; HI01; HI0112; HI0112111 --%>
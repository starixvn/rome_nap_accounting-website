<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NumberGame.aspx.cs" Inherits="SpecialDelete_NumberGame" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />

    <script type="text/javascript" src="../../js/common.js"></script>

    <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/SpecialDelete/SpecialDelete.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../js/jQuery/json2.js" type="text/javascript"></script>
    <script src="../../js/jQuery/gridscroller.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../../js/stdlib.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="../../js/showhidepopup.js" type="text/javascript"></script>
    <script src="js/gridrender.js?ver=2" type="text/javascript"></script>
    <script src="js/SpecialDelete.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    

    <style type="text/css">
        input[type=checkbox]
        {
            cursor: pointer;
        }
    </style>
</head>
<body>
    <div class="maincontent">
        <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
            <tr>
                <td class="maincontent_headerL">
                </td>
                <td class="maincontent_headerC">
                    Special Delete for Number Game
                </td>
                <td class="maincontent_headerR">
                </td>
            </tr>
        </table>
        <div class="maincontent_box" style="width:1185px;">
            <div id="ErrMsg" class="errMsg">
            </div>
        </div>
        <div class="bg_table" style="width:1185px;">
            <asp:Literal ID="HtmlHolder" runat="server"></asp:Literal>
            <div id="dvHeader"><%=html %></div>
            <div id="mainreport" class="hidden" style="width:1185px;">
                    <div id="gridspecialdelete" class="content-warper">
                        <div class="content-header clearfix">
                            <div class="header-scroll">
                                <table class="tbl-header-scroll" >
                                    <tr>
                                        <td  style="border-bottom:none 0px;" >
                                            <div class="col40 c">NO</div>
                                        </td>
                                        <td  style="border-bottom:none 0px;">
                                            <div class="col120 c">ACCOUNT</div>
                                        </td>
                                        <td  style="border-bottom:none 0px;">
                                            <div class="col80 c">REFNO</div>
                                        </td>
                                        <td style="border-bottom:none 0px;">
                                            <div class="col160 c">BET TYPE</div>
                                        </td>
                                        <td  style="border-bottom:none 0px;">
                                            <div class="col40 c">BET</div>
                                        </td>
                                        <td   style="border-bottom:none 0px;">
                                            <div class="col60 c">ODDS</div>
                                        </td>
                                        <td  style="border-bottom:none 0px;">
                                            <div class="col60 c">AMOUNT</div>
                                        </td>
                                         <td  style="border-bottom:none 0px;">
                                            <div class="col60 c">CURRENCY</div>
                                        </td>
                                         <td  style="border-bottom:none 0px;">
                                            <div class="col40 c">HDP</div>
                                        </td>
                                        <td  style="border-bottom:none 0px;">
                                            <div class="col120 c">TRANS.TIME</div>
                                        </td>
                                       <%-- <td colspan="2" style="border-bottom:none 0px;">
                                            <div class="c" style="width:83px;">SCORE</div>
                                        </td>--%>
                                         <td  style="border-bottom:none 0px;">
                                            <div class="col120 c">CREATOR</div>
                                        </td>
                                         <td  style="border-bottom:none 0px;">
                                            <div class="col60 c">STATUS</div>
                                        </td>
                                         <td  style="border-bottom:none 0px;">
                                            <div class="col80 c">OPTION</div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="content-body clearfix">
                            <div class="body-scroll">
                                <table id="tblspecialdelete" class="tbl-body-scroll" style="">
                                </table>
                            </div>
                        </div>
                    </div>
                    <br />
                     <div id="dvContent"></div>
                </div>
            
            <input type="button" id="btnDeleteBets" name="btnDeleteBets" class="btn" style="display:none" value="Delete Bets" onclick="ShowPopupDeleteBets();" />
        </div>
    </div>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="SpecialDelete_Index"
    Title="Search Transaction" Theme="" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Special Delete</title>
      <script type="text/javascript">
          window.location.href = "V2/Default.aspx";
    </script>
    <script type="text/javascript" src="../js/common.js"></script>
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/SpecialDelete/SpecialDelete.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <script src="../js/jQuery/jquery.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../js/jQuery/json2.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../js/jQuery/gridscroller.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
  
    <script src="../js/common.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="js/gridrender.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
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
                    Special Delete
                </td>
                <td class="maincontent_headerR">
                </td>
            </tr>
        </table>
        <div class="maincontent_box">
            <div id="ErrMsg" class="errMsg">
            </div>
        </div>
        <div class="bg_table">
            <asp:Literal ID="HtmlHolder" runat="server"></asp:Literal>
            <div id="dvHeader">
                <%=html %></div>
            <div id="mainreport" class="hidden">
                    <table style="width:600px;" class="tblcollapse">
                        <tbody>
                            <tr class="title c">
                                <td nowrap="">CODE</td><td>MATCH EVENT</td><td width="150">EVENT DATE</td>
                            </tr>
                            <tr class="h3 c">
                                <td id="code"></td><td nowrap="" id="teams"></td><td id="transtime"></td>
                            </tr>
                        </tbody>
                    </table>
                    <br />
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
                                        <td colspan="2" style="border-bottom:none 0px;">
                                            <div class="c" style="width:83px;">SCORE</div>
                                        </td>
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
                                <table id="tblspecialdelete" class="tbl-body-scroll">
                                </table>
                            </div>
                        </div>
                    </div>
                    <br />
                     <div id="dvContent"></div>
                </div>
          
            <br />
            <script type="text/javascript">
                on_changeBetype(1);
            </script>
            <input type="hidden" id="userId" name="userId" value="<%=AppSession.AccId %>" />
        </div>
    </div>
</body>
</html>

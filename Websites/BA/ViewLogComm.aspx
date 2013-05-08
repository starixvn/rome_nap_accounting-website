<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewLogComm.aspx.cs" Inherits="BA_ViewLogComm" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<%@ Import Namespace="Sunplus.Accounting.DataAccess.Report" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server"> 
    <title id="ReportTitle" runat="server"></title>
 
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../js/jQuery/Pager/pagination.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link href="Style/Commission.css" rel="stylesheet" type="text/css" />

    <script src="Script/websocket/modernizr-1.7.min.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../js/jQuery/Tpl/jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="../js/jQuery/knockout-1.2.0.js" type="text/javascript"></script>
    <script src="../js/jQuery/json2.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../js/common.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../js/jQuery/gridscroller.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="Script/ConfigCommLog.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
</head>
<body >
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        View log
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box" style="width:960px;">
                <div id="header_main">
                  <table>
                    <tr>
                        <td>Licensee:</td>
                        <td>
                            <select id="dllSite" style="width:120px" >
                                <option value="0">All</option>
                            </select>                            
                        </td>
                        <td>
                            <input id="btnReport" name="btnReport" type="button" value="View" class="btn" onclick="ConfigCommLog.viewGrid()" />
                        </td>
                    </tr>
                </table>
                 <div id="ErrMsg"></div>
            </div>
          </div>
            <div id="mainreport" style="margin-top:5px;">
                <div id="commConfigLog" class="content-warper hidden">
                        <div class="content-header clearfix">
                            <div class="header-scroll">
                                <table class="tbl-header-scroll" >
                                     <tr>
                                        <td style="border-bottom: none 0px;">
                                            <div class="col200 c">
                                                Asian HDP, OU, OE Comm</div>
                                        </td>
                                        <td style="border-bottom: none 0px;">
                                            <div class="w60 c">
                                                1x2 Comm</div>
                                        </td>
                                        <td style="border-bottom: none 0px;">
                                            <div class="w60 c">
                                                Other Bettypes Comm</div>
                                        </td>
                                        <td style="border-bottom: none 0px;">
                                            <div class="w60 c">
                                                Group</div>
                                        </td>
                                        <td style="border-bottom: none 0px;">
                                            <div class="w60 c">
                                                Allow BA</div>
                                        </td>
                                         <td style="border-bottom: none 0px;">
                                            <div class="w100 c">
                                            Modified By
                                            </div>
                                        </td>
                                         <td style="border-bottom: none 0px;">
                                            <div class="col100 c">
                                                IP</div>
                                        </td>
                                        <td style="border-bottom: none 0px;">
                                            <div class="w120 c">
                                                 Modified Date</div>
                                        </td> 
                                        <td style="border-bottom: none 0px;">
                                            <div class="w160 c">
                                                 Description</div>
                                        </td>   
                                                                          
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="content-body clearfix">
                            <div class="body-scroll" id="latestbetcontent">
                                <table id="tblcommconfiglog" class="tbl-body-scroll">
                                </table>
                            </div>
                        </div>
                    </div>
            </div>
        </div>
    </div>
    <script language="javascript">
        var licenseeId=<%=licenseeId%>;
        var iplocation='<% =ConfigurationManager.AppSettings["iplocation"].ToString() %>';
    </script>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BetListLiveCasinoResult.aspx.cs" Inherits="LiveCasino_Betlist_ViewResultLiveCasino" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../js/jQuery/Pager/pagination.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />

    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>  
    <script src="../../js/jQuery/json2.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="../../js/BlockUI.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script> 
    <script src="../../js/Datetime.js" type="text/javascript"></script>
    <script src="../../js/jQuery/gridscroller.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../js/BetlistLiveCasinoResult.js?ver=1" type="text/javascript"></script>
</head>
<body>
    <div id="LiveCasinoDetails" class="content-warper hidden" style="margin-top:6px;">
                        <div class="content-toolbox bgtitle">
                            <div id="gridtitle" class="gridtitle"></div>
                        </div>
                        <div class="content-header clearfix">
                            <div class="header-scroll">
                                <table class="tbl-header-scroll" >
                                     <tr>
                                        <td style="border-bottom:none 0px;">
                                            <div class="col140 c">KICK OFF.TIME</div>
                                        </td>
                                         <td style="border-bottom:none 0px;">
                                            <div class="col200 c">GAME TYPE - TABLE NUMBER</div>
                                        </td>  
                                         <td style="border-bottom:none 0px;">
                                            <div class="col100 c">FINAL SCORE</div>
                                        </td>       
                                         <td style="border-bottom:none 0px;">
                                            <div class="col180 c">RESULTS</div>
                                        </td>   
                                         <td style="border-bottom:none 0px;">
                                            <div class="col80 c">STATUS</div>
                                        </td>                                                
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="content-body clearfix">
                            <div class="body-scroll">
                                <table id="tblLiveCasino" class="tbl-body-scroll">
                                </table>
                            </div>
                        </div>
                    </div>
    
    <iframe id="frm" width="0" height="0"  frameborder="0" style="display:none"></iframe>
    <script type="text/javascript">
     var matchId = '<%=matchId %>';
     var bettype = '<%=bettype %>';
     var league = '<%=league %>';
    </script>
</body>
</html>

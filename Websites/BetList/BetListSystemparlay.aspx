<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BetListSystemparlay.aspx.cs" Inherits="BetList_SystemParlayDetail" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/BetList/BetList.css" />
    <link href="../js/IBCWnd/IBCWnd.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/ui.jqgrid.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
   
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Pagination.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/LeftPanel/left_panel.css" rel="stylesheet" type="text/css" />
   <style type="text/css">
        .box_header {
            background: none;
            height: 26px;
            margin-left: 2px;
            width: 251px;
        }
    </style>
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../js/IBCWnd/IBCWnd.js" type="text/javascript"></script>    
    <script type="text/javascript" src="../js/BlockUI.js"></script>
    <script src="../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="../js/jQuery/gridscroller.js" type="text/javascript"></script>
    <script src="../js/pager_v2.js" type="text/javascript"></script>

    <script type="text/javascript" src="../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js"></script>

    <script type="text/javascript" src="../js/dialogbox.js"></script>
    
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/BetList.js" type="text/javascript"></script>
    <script src="JS/SystemParlayDetail.js?v=<%=WebSettings.Version %>" type="text/javascript"></script>
    
    
</head>
<body>
    <div class="main">
        <div class="maincontent">
            <div id="boxheader" class="box_header">
                <div class="left_tab">
                    <ul>
                        <li class="current" id="Li1" onclick="viewBetListTab();" ><a href="javascript:;" ><span>Bet list</span></a></li>
                        <li class="" id="Li2" onclick="viewSystemParlayTab();"><a href="javascript:;"><span>System parlay</span></a></li>                        
                   </ul>
                </div>
            </div>
            
            
            <%--mainconten_betlist --%>
            <div class="maincontent_box" id="maincontent_betlist">
                <div id="errMsg">
                </div>
                <div id="clear2">
                </div>
                <asp:Literal ID="reportHeader" runat="server">
                </asp:Literal>
            </div>
            <%--maincontent_systemparlay --%>
            <div class="maincontent_box hidden" id="maincontent_systemparlay">                               
                <table>
                    <tr>
                        <td style="font-weight:bold;">By Ref No </td>
                        <td><input type="text" id="txtRefNo" value="" onkeypress="return systemParlayDetail.preSubmit(event)" /> </td>
                        <td><input type="button" id="btnSubmit" value="Submit" class="btn" onclick="systemParlayDetail.submit();" /></td>
                    </tr>
                </table>
                <div id="errMsg2"></div> 
            </div>
            <%--Result container Bet list--%>
            <div id="divcontainer" class="bg_table allhidden">
                <%--betlist result --%>
                <div id="sContent">
                </div>
                
                
            </div>
            
             <%--Result container  system paraly --%>
            <div id="divcontainer_systemparlay"  class="bg_table hidden">
                <%--system parlay result --%>
                <div id="systemparlay" class="content-warper hidden">

                         <div class="content-header clearfix">
                            <div class="header-scroll">
                                <table class="tbl-header-scroll" >
                                    <tr>
                                        <td style="border-bottom:none 0px;" >
                                            <div class="col40 c">No</div>
                                        </td>
                                        <td style="border-bottom:none 0px;" >
                                            <div class="col130 c">Trans.Time</div>
                                        </td>
                                        <td style="border-bottom:none 0px;" >
                                            <div class="col280 c">Choice</div>
                                        </td>
                                        <td style="border-bottom:none 0px;" >
                                            <div class="col50 c">Odds</div>
                                        </td>
                                        <td style="border-bottom:none 0px;" >
                                            <div class="col60 c">Stake</div>
                                        </td>
                                        <td style="border-bottom:none 0px;" >
                                            <div class="col70 c">Win Loss</div>
                                        </td>
                                        <td style="border-bottom:none 0px;" >
                                            <div class="col90 c">Status</div>
                                        </td>
                                        <td style="border-bottom:none 0px;" >
                                            <div class="col70 c">Super PT/Comm</div>
                                        </td>
                                        <td style="border-bottom:none 0px;" >
                                            <div class="col70 c">Master PT/Comm</div>
                                        </td>
                                        <td style="border-bottom:none 0px;" >
                                            <div class="col70 c">Agent PT/Comm</div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>

                        <div class="content-body clearfix">
                            <div class="body-scroll">
                                <table id="tblsystemparlay" class="tbl-body-scroll">
                                </table>
                            </div>
                        </div>
                    </div>
                    <br/>
                    
                <div id="systemparalydetail" class="content-warper hidden">
                        <div class="content-toolbox bgtitle">
                            <div id="systemparalydetailtitle" class="gridtitle">Combination details</div>
                            <div class="btngroup"></div>                            
                        </div>

                         <div class="content-header clearfix">
                            <div class="header-scroll">
                                <table  class="tbl-header-scroll" >
                                    <tr>
                                        <td style="border-bottom:none 0px;" >
                                            <div class="col80 c">Date</div>
                                        </td>
                                        <td style="border-bottom:none 0px;" >
                                            <div class="col200 c">Event</div>
                                        </td>
                                        <td style="border-bottom:none 0px;" >
                                            <div class="col50 c">Odds</div>
                                        </td>
                                        <td style="border-bottom:none 0px;" >
                                            <div class="col60 c">Stake</div>
                                        </td>
                                        <td style="border-bottom:none 0px;" >
                                            <div class="col70 c">Win Loss</div>
                                        </td>
                                        <td style="border-bottom:none 0px;" >
                                            <div class="col70 c">Status</div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>

                        <div class="content-body clearfix">
                            <div class="body-scroll">
                                <table id="tblsystemparlaydetail" class="tbl-body-scroll">
                                </table>
                            </div>
                        </div>
                    </div>
            
            </div>
        </div>
    </div>
    <script language="javascript" type="text/javascript">
        var custId=<%=CustId %>;                
        var transId='<%=transId %>'; 
       
        $(document).ready(function () {
             if(transId != ''){
                var CustId = <%=custId %>;
               GetBetListFromCheckBet(transId, CustId);
             }
            $.unblockUI();

        });
        
    </script>
    
    
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DailyWinLossNEW.aspx.cs" Inherits="Report_External_WinLoss_DailyWinLossNEW" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../../App_Themes/Public/input.css" />
    <link href="../../../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link href="styles/dailywinloss.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        Win/Loss Daily
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box" style="width: 906px;">
                <table>
                    <tr>
                        <td>Date:</td>
                        <td><%=strDate%></td>                        
                        <td>
                            <input type="checkbox" checked="checked" id="isExcludeMargin" title=""/>WinLoss with Margin
                            <input type="button" onclick="getExternalDailyWL(1)" class="btn" value="REPORT" name="btnReport" id="btnReport" />
                            <input type="button" onclick="getExternalDailyWL(2)" class="btn" value="<%=DateTime.Now.ToShortDateString() %>" name="btnReport" id="btnCurrentDateReport" />
                            
                            <input type="button" onclick="getExternalDailyWL(3)" class="btn" value="<%=DateTime.Now.AddDays(-1).ToShortDateString() %>" name="btnReport" id="btnCurrentYesterdayReport" />
                        </td>
                    </tr>
                </table>
                <div id="err">
                </div>
            </div>

            <div id="bg_table" style="margin-top:2px;">            
                <div id="superWLGrid" class="content-warper hidden">
                    <div class="content-toolbox bgtitle">
                        <div id="superWLGridTitle" class="gridtitle">
                        </div>
                        <div class="btngroup">                          
                            <input type="button" onclick="superDailyWinLoss.exportExcel();" value="  " id="btnSuperExport" class="btnExcel" title="Export to Excel" /></div>
                    </div>
                    <div class="content-header clearfix">
                        <div id="header-frozen" class="header-frozen">
                            <table class="tbl-header-frozen">
                                 <tr>                                    
                                    <td style="border-bottom:none 0px; " rowspan="2">
                                        <div class="col20 c">
                                            No.</div>
                                    </td>
                                    <td style="border-bottom:none 0px;" rowspan="2">
                                        <div class="col100  c">
                                            Username</div>                                    
                                    </td>
                                    <td style="border-bottom:none 0px;" rowspan="2">
                                        <div class="col100 c">
                                           Total T/O</div>
                                    </td>  
                                    </tr>
                            </table>
                        </div>
                        <div id="header-scroll" class="header-scroll">
                            <table class="tbl-header-scroll">
                                <tr>
                                    <td colspan="8">
                                        <div class="c">Turnover & Position Taking </div>
                                    </td>
                                    <td colspan="8">
                                        <div class="c">Win/Loss & Margin </div>
                                    </td>
                                </tr>
                                <tr>    
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Company</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Super</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Master</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Agent</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Company</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Super</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Master</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Agent</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="content-body clearfix">
                        <div id="body-frozen" class="body-frozen">
                            <table id="tblSuperWLfrozen" class="tbl-body-frozen">
                            </table>
                        </div>
                        <div id="body-scroll" class="body-scroll">
                            <table id="tblSuperWL" class="tbl-body-scroll">                               
                            </table>
                        </div>
                    </div>
                    <div class="content-footer">
                        <div id="footer-frozen" class="footer-frozen">
                            <table id="tblfooterSuperWLfrozen" class="tbl-footer-frozen">
                            </table>
                        </div>
                        <div id="footer-scroll" class="footer-scroll">
                            <table class="tbl-footer-scroll" id="tblfooterSuperWL">
                            </table>
                        </div>
                    </div>
                </div>                                

                <div id="masterWLGrid" class="content-warper hidden">
                    <div class="content-toolbox bgtitle">
                        <div id="masterWLGridTitle" class="gridtitle">
                        </div>
                        <div class="btngroup">                          
                            <input type="button" onclick="masterDailyWinLoss.goback();" value="  " id="btnGoBackSuper" class="btnBack" title="Back To Super" />
                            <input type="button" onclick="masterDailyWinLoss.exportExcel();" value="  " id="btnMasterExport" class="btnExcel" title="Export to Excel" />
                            
                        </div>
                    </div>
                    <div class="content-header clearfix">
                        <div class="header-frozen">
                            <table class="tbl-header-frozen">
                                 <tr>                                    
                                    <td style="border-bottom:none 0px; " rowspan="2">
                                        <div class="col20 c">
                                            No.</div>
                                    </td>
                                    <td style="border-bottom:none 0px;" rowspan="2">
                                        <div class="col100  c">
                                            Username</div>                                    
                                    </td>
                                    <td style="border-bottom:none 0px;" rowspan="2">
                                        <div class="col100 c">
                                           Total T/O</div>
                                    </td>  
                                    </tr>
                            </table>
                        </div>
                        <div id="Div2" class="header-scroll">
                            <table class="tbl-header-scroll">
                                <tr>
                                    <td colspan="8">
                                        <div class="c">Turnover & Position Taking </div>
                                    </td>
                                    <td colspan="8">
                                        <div class="c">Win/Loss & Margin </div>
                                    </td>
                                </tr>
                                <tr>    
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Company</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Super</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Master</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Agent</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Company</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Super</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Master</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Agent</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="content-body clearfix">
                        <div class="body-frozen">
                            <table id="tblMasterWLfrozen" class="tbl-body-frozen">
                            </table>
                        </div>
                        <div id="Div4" class="body-scroll">
                            <table id="tblMasterWL" class="tbl-body-scroll">                               
                            </table>
                        </div>
                    </div>
                    <div class="content-footer">
                        <div class="footer-frozen">
                            <table id="tblfooterMasterWLfrozen" class="tbl-footer-frozen">
                            </table>
                        </div>
                        <div id="Div6" class="footer-scroll">
                            <table class="tbl-footer-scroll" id="tblfooterMasterWL">
                            </table>
                        </div>
                    </div>
                </div>

                <div id="agentWLGrid" class="content-warper hidden">
                    <div class="content-toolbox bgtitle">
                        <div id="agentWLGridTitle" class="gridtitle">
                        </div>
                        <div class="btngroup">                          
                            <input type="button" onclick="" value="  " id="btnGoBackMaster" class="btnBack" title="Back To" />
                            <a href="#" onclick="masterDailyWinLoss.goback();">Super</a>|
                            <a href="#" onclick="agentDailyWinLoss.goback();">Master</a>
                            <input type="button" onclick="agentDailyWinLoss.exportExcel();" value="  " id="btnAgentExport" class="btnExcel" title="Export to Excel" />                            
                        </div>
                    </div>

                    <div class="content-header clearfix">
                        <div class="header-frozen">
                            <table class="tbl-header-frozen">
                                 <tr>                                    
                                    <td style="border-bottom:none 0px; " rowspan="2">
                                        <div class="col20 c">
                                            No.</div>
                                    </td>
                                    <td style="border-bottom:none 0px;" rowspan="2">
                                        <div class="col100  c">
                                            Username</div>                                    
                                    </td>
                                    <td style="border-bottom:none 0px;" rowspan="2">
                                        <div class="col100 c">
                                           Total T/O</div>
                                    </td>  
                                    </tr>
                            </table>
                        </div>
                        <div class="header-scroll">
                            <table class="tbl-header-scroll">
                                <tr>
                                    <td colspan="8">
                                        <div class="c">Turnover & Position Taking </div>
                                    </td>
                                    <td colspan="8">
                                        <div class="c">Win/Loss & Margin </div>
                                    </td>
                                </tr>
                                <tr>    
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Company</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Super</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Master</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Agent</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Company</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Super</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Master</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Agent</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="content-body clearfix">
                        <div class="body-frozen">
                            <table id="tblAgentWLfrozen" class="tbl-body-frozen">
                            </table>
                        </div>
                        <div class="body-scroll">
                            <table id="tblAgentWL" class="tbl-body-scroll">                               
                            </table>
                        </div>
                    </div>
                    <div class="content-footer">
                        <div class="footer-frozen">
                            <table id="tblfooterAgentWLfrozen" class="tbl-footer-frozen">
                            </table>
                        </div>
                        <div class="footer-scroll">
                            <table class="tbl-footer-scroll" id="tblfooterAgentWL">
                            </table>
                        </div>
                    </div>
                
                </div>

               <div id="memberWLGrid" class="content-warper hidden">
                    <div class="content-toolbox bgtitle">
                        <div id="memberWLGridTitle" class="gridtitle">
                        </div>
                        <div class="btngroup">                          
                            <input type="button" onclick="" value="  " id="btnGoBackAgent" class="btnBack" title="Back To" />
                            <a href="#" onclick="masterDailyWinLoss.goback();">Super</a>|
                            <a href="#" onclick="agentDailyWinLoss.goback();">Master</a>|
                            <a href="#" onclick="memberDailyWinLoss.goback();">Agent</a>
                            <input type="button" onclick="memberDailyWinLoss.exportExcel();" value="  " id="btnMemberExport" class="btnExcel" title="Export to Excel" />                            
                        </div>
                    </div>

                    <div class="content-header clearfix">
                        <div class="header-frozen">
                            <table class="tbl-header-frozen">
                                 <tr>                                    
                                    <td style="border-bottom:none 0px; " rowspan="2">
                                        <div class="col20 c">
                                            No.</div>
                                    </td>
                                    <td style="border-bottom:none 0px;" rowspan="2">
                                        <div class="col100  c">
                                            Username</div>                                    
                                    </td>
                                    <td style="border-bottom:none 0px;" rowspan="2">
                                        <div class="col100 c">
                                           Total T/O</div>
                                    </td>  
                                    </tr>
                            </table>
                        </div>
                        <div class="header-scroll">
                            <table class="tbl-header-scroll">
                                <tr>
                                    <td colspan="8">
                                        <div class="c">Turnover & Position Taking </div>
                                    </td>
                                    <td colspan="8">
                                        <div class="c">Win/Loss & Margin </div>
                                    </td>
                                </tr>
                                <tr>    
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Company</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Super</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Master</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Agent</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Company</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Super</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Master</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Agent</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="content-body clearfix">
                        <div class="body-frozen">
                            <table id="tblMemberWLfrozen" class="tbl-body-frozen">
                            </table>
                        </div>
                        <div class="body-scroll">
                            <table id="tblMemberWL" class="tbl-body-scroll">                               
                            </table>
                        </div>
                    </div>
                    <div class="content-footer">
                        <div class="footer-frozen">
                            <table id="tblfooterMemberWLfrozen" class="tbl-footer-frozen">
                            </table>
                        </div>
                        <div class="footer-scroll">
                            <table class="tbl-footer-scroll" id="tblfooterMemberWL">
                            </table>
                        </div>
                    </div>
                
                </div>

                <!--remove margin-->
                <div id="superWLNotMargin" class="content-warper hidden">
                    <div class="content-toolbox bgtitle">
                        <div id="superWLNotMarginTitle" class="gridtitle">
                        </div>
                        <div class="btngroup">                          
                            <input type="button" onclick="superDailyWinLossNotMargin.exportExcel();" value="  " id="btnSuperExportNotMargin" class="btnExcel" title="Export to Excel" /></div>
                    </div>
                    <div class="content-header clearfix">
                        <div class="header-frozen">
                            <table class="tbl-header-frozen">
                                 <tr>                                    
                                    <td style="border-bottom:none 0px; " rowspan="2">
                                        <div class="col20 c">
                                            No.</div>
                                    </td>
                                    <td style="border-bottom:none 0px;" rowspan="2">
                                        <div class="col100  c">
                                            Username</div>                                    
                                    </td>
                                    <td style="border-bottom:none 0px;" rowspan="2">
                                        <div class="col100 c">
                                           Total T/O</div>
                                    </td>  
                                    </tr>
                            </table>
                        </div>
                        <div class="header-scroll">
                            <table class="tbl-header-scroll">
                                <tr>
                                    <td colspan="8">
                                        <div class="c">Turnover & Position Taking </div>
                                    </td>
                                    <td colspan="4">
                                        <div class="c">Win/Loss</div>
                                    </td>
                                </tr>
                                <tr>    
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Company</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Super</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Master</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Agent</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Company</div>
                                    </td> 
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Super</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Master</div>
                                    </td>  
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Agent</div>
                                    </td>  
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="content-body clearfix">
                        <div class="body-frozen">
                            <table id="tblSuperWLNotMarginFrozen" class="tbl-body-frozen">
                            </table>
                        </div>
                        <div class="body-scroll">
                            <table id="tblSuperWLNotMarginScroll" class="tbl-body-scroll">
                            </table>
                        </div>
                    </div>
                    <div class="content-footer">
                        <div class="footer-frozen">
                            <table id="tblSuperWLNotMarginFrozenFooter" class="tbl-footer-frozen">
                            </table>
                        </div>
                        <div class="footer-scroll">
                            <table class="tbl-footer-scroll" id="tblSuperWLNotMarginScrollFooter">
                            </table>
                        </div>
                    </div>
                </div>

                <div id="masterWLNotMargin" class="content-warper hidden">
                    <div class="content-toolbox bgtitle">
                        <div id="masterWLNotMarginTitle" class="gridtitle">
                        </div>
                        <div class="btngroup">                          
                            <input type="button" onclick="masterDailyWinLossNotMargin.goback();" value="  " id="btnMasterGobackNotMargin" class="btnBack" title="Back To Super" />
                            <input type="button" onclick="masterDailyWinLossNotMargin.exportExcel();" value="  " id="btnMasterExportNotMargin" class="btnExcel" title="Export to Excel" />
                            
                        </div>
                    </div>
                    <div class="content-header clearfix">
                        <div class="header-frozen">
                            <table class="tbl-header-frozen">
                                 <tr>                                    
                                    <td style="border-bottom:none 0px; " rowspan="2">
                                        <div class="col20 c">
                                            No.</div>
                                    </td>
                                    <td style="border-bottom:none 0px;" rowspan="2">
                                        <div class="col100  c">
                                            Username</div>                                    
                                    </td>
                                    <td style="border-bottom:none 0px;" rowspan="2">
                                        <div class="col100 c">
                                           Total T/O</div>
                                    </td>  
                                    </tr>
                            </table>
                        </div>
                        <div class="header-scroll">
                            <table class="tbl-header-scroll">
                                <tr>
                                    <td colspan="8">
                                        <div class="c">Turnover & Position Taking </div>
                                    </td>
                                    <td colspan="4">
                                        <div class="c">Win/Loss</div>
                                    </td>
                                </tr>
                                <tr>    
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Company</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Super</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Master</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Agent</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Company</div>
                                    </td>  
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Super</div>
                                    </td>  
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Master</div>
                                    </td>  
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Agent</div>
                                    </td>  
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="content-body clearfix">
                        <div class="body-frozen">
                            <table id="tblMasterWLNotMarginFrozeen" class="tbl-body-frozen">
                            </table>
                        </div>
                        <div class="body-scroll">
                            <table id="tblMasterWLNotMarginScroll" class="tbl-body-scroll">                               
                            </table>
                        </div>
                    </div>
                    <div class="content-footer">
                        <div class="footer-frozen">
                            <table id="tblMasterWLNotMarginFrozeenFooter" class="tbl-footer-frozen">
                            </table>
                        </div>
                        <div class="footer-scroll">
                            <table class="tbl-footer-scroll" id="tblMasterWLNotMarginScrollFooter">
                            </table>
                        </div>
                    </div>
                </div>

                <div id="agentWLNotMargin" class="content-warper hidden">
                    <div class="content-toolbox bgtitle">
                        <div id="agentWLNotMarginTitle" class="gridtitle">
                        </div>
                        <div class="btngroup">                          
                            <input type="button" onclick="" value="  " id="btnAgentGobackNotMargin" class="btnBack" title="Back To" />
                            <a href="#" onclick="masterDailyWinLossNotMargin.goback();">Super</a>|
                            <a href="#" onclick="agentDailyWinLossNotMargin.goback();">Master</a>
                            <input type="button" onclick="agentDailyWinLossNotMargin.exportExcel();" value="  " id="btnExportAgentNotMargin" class="btnExcel" title="Export to Excel" />                            
                        </div>
                    </div>

                    <div class="content-header clearfix">
                        <div class="header-frozen">
                            <table class="tbl-header-frozen">
                                 <tr>                                    
                                    <td style="border-bottom:none 0px; " rowspan="2">
                                        <div class="col20 c">
                                            No.</div>
                                    </td>
                                    <td style="border-bottom:none 0px;" rowspan="2">
                                        <div class="col100  c">
                                            Username</div>                                    
                                    </td>
                                    <td style="border-bottom:none 0px;" rowspan="2">
                                        <div class="col100 c">
                                           Total T/O</div>
                                    </td>  
                                    </tr>
                            </table>
                        </div>
                        <div class="header-scroll">
                            <table class="tbl-header-scroll">
                                <tr>
                                    <td colspan="8">
                                        <div class="c">Turnover & Position Taking </div>
                                    </td>
                                    <td colspan="4">
                                        <div class="c">Win/Loss</div>
                                    </td>
                                </tr>
                                <tr>    
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Company</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Super</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Master</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Agent</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Company</div>
                                    </td>  
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Super</div>
                                    </td>  
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Master</div>
                                    </td>  
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Agent</div>
                                    </td> 
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="content-body clearfix">
                        <div class="body-frozen">
                            <table id="tblagentWLNotMarginFroozen" class="tbl-body-frozen">
                            </table>
                        </div>
                        <div class="body-scroll">
                            <table id="tblagentWLNotMarginScroll" class="tbl-body-scroll">                               
                            </table>
                        </div>
                    </div>
                    <div class="content-footer">
                        <div class="footer-frozen">
                            <table id="tblagentWLNotMarginFroozenFooter" class="tbl-footer-frozen">
                            </table>
                        </div>
                        <div class="footer-scroll">
                            <table class="tbl-footer-scroll" id="tblagentWLNotMarginScrollFooter">
                            </table>
                        </div>
                    </div>
                
                </div>

                <div id="memberWLNotMargin" class="content-warper hidden">
                    <div class="content-toolbox bgtitle">
                        <div id="memberWLNotMarginTitle" class="gridtitle">
                        </div>
                        <div class="btngroup">                          
                            <input type="button" onclick="" value="  " id="btnGobackMemberNotMargin" class="btnBack" title="Back To" />
                            <a href="#" onclick="masterDailyWinLossNotMargin.goback();">Super</a>|
                            <a href="#" onclick="agentDailyWinLossNotMargin.goback();">Master</a>|
                            <a href="#" onclick="memberDailyWinLossNotMargin.goback();">Agent</a>
                            <input type="button" onclick="memberDailyWinLossNotMargin.exportExcel();" value="  " id="btnExportMemberNotMargin" class="btnExcel" title="Export to Excel" />                            
                        </div>
                    </div>

                    <div class="content-header clearfix">
                        <div class="header-frozen">
                            <table class="tbl-header-frozen">
                                 <tr>                                    
                                    <td style="border-bottom:none 0px; " rowspan="2">
                                        <div class="col20 c">
                                            No.</div>
                                    </td>
                                    <td style="border-bottom:none 0px;" rowspan="2">
                                        <div class="col100  c">
                                            Username</div>                                    
                                    </td>
                                    <td style="border-bottom:none 0px;" rowspan="2">
                                        <div class="col100 c">
                                           Total T/O</div>
                                    </td>  
                                    </tr>
                            </table>
                        </div>
                        <div class="header-scroll">
                            <table class="tbl-header-scroll">
                                <tr>
                                    <td colspan="8">
                                        <div class="c">Turnover & Position Taking </div>
                                    </td>
                                    <td colspan="4">
                                        <div class="c">Win/Loss</div>
                                    </td>
                                </tr>
                                <tr>    
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Company</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Super</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Master</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Agent</div>
                                    </td>  
                                     <td style="border-bottom:none 0px;">
                                        <div class="col60 c">
                                            %</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Company</div>
                                    </td>  
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Super</div>
                                    </td>  
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Master</div>
                                    </td>  
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">
                                            Agent</div>
                                    </td>  
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="content-body clearfix">
                        <div class="body-frozen">
                            <table id="tblmemberWLNotMarginFrozen" class="tbl-body-frozen">
                            </table>
                        </div>
                        <div class="body-scroll">
                            <table id="tblmemberWLNotMarginScroll" class="tbl-body-scroll">                               
                            </table>
                        </div>
                    </div>
                    <div class="content-footer">
                        <div class="footer-frozen">
                            <table id="tblmemberWLNotMarginFrozenFooter" class="tbl-footer-frozen">
                            </table>
                        </div>
                        <div class="footer-scroll">
                            <table class="tbl-footer-scroll" id="tblmemberWLNotMarginScrollFooter">
                            </table>
                        </div>
                    </div>
                
                </div>
           </div>
        </div>
    </div>

    <iframe id="frm" width="0" height="0" frameborder="0" style="display: none"></iframe>
</body>

    <script src="../../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../../js/jQuery/jquery.json-2.2.min.js" type="text/javascript"></script>
    <script src="../../../js/jQuery/json2.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../../../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="../../../js/BlockUI.js" type="text/javascript"></script>
    <script src="../../../js/common.js" type="text/javascript"></script>
    <script src="../../../js/jQuery/gridscroller.js" type="text/javascript"></script>
    <script src="js/ExternalDailyWinLoss.js?v=<%=WebSettings.Version %>" type="text/javascript"></script>
</html>
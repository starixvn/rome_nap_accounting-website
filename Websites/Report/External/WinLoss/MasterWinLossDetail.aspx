<%@ Page Title="" Language="C#" MasterPageFile="~/Report/External/WinLoss/default.master"
    AutoEventWireup="true" CodeFile="MasterWinLossDetail.aspx.cs" Inherits="Report_WinLoss_MasterWinLossDetail" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <%--<div id="reportMessage">
        <b>
            <%=ReportTitle%>
        </b>(From
        <%=FromDate.ToString("MM/dd/yyyy")%>
        to
        <%=ToDate.ToString("MM/dd/yyyy")%>
        )
    </div>--%>
    <%--<div id="rptContent">--%>
    <%--/header--%>
    <asp:Repeater ID="rptWinLoss" runat="server" EnableViewState="false" EnableTheming="false">
        <HeaderTemplate>
          <div class="bg_table">
            <table class="tbllayout" cellspacing="1" id="rptGrid">
                <%--header--%>
                <thead>
                    <tr class="title">
                        <th rowspan="2">
                            Username
                        </th>
                        <th rowspan="2">
                            Turnover
                        </th>
                        <!--Limit_Grosscomm-->
                        <th rowspan="2">
                            GrossComm
                        </th>
                        <!--/Limit_Grosscomm-->
                        <!--Limit_Member-->
                        <th colspan="3">
                            Member
                        </th>
                        <!--/Limit_Member-->
                        <!--Limit_Agent-->
                        <th colspan="3">
                            Agent
                        </th>
                        <!--/Limit_Agent-->
                        <!--Limit_Master-->
                        <th colspan="3">
                            Master
                        </th>
                        <!--/Limit_Master-->
                        <th rowspan="2">
                            Company
                        </th>
                    </tr>
                    <tr class="title" id="rptHeader">
                        <!--Limit_Member-->
                        <td>
                            Win/Loss
                        </td>
                        <td>
                            Comm
                        </td>
                        <td>
                            Total
                        </td>
                        <!--/Limit_Member-->
                        <!--Limit_Agent-->
                        <td>
                            Win/Loss
                        </td>
                        <td>
                            Comm
                        </td>
                        <td>
                            Total
                        </td>
                        <!--/Limit_Agent-->
                        <!--Limit_Master-->
                        <td>
                            Win/Loss
                        </td>
                        <td>
                            Comm
                        </td>
                        <td>
                            Total
                        </td>
                        <!--/Limit_Master-->
                        <!--Limit_Company-->
                        <!--/Limit_Company-->
                    </tr>
                </thead>
                <tbody>
        </HeaderTemplate>
         <ItemTemplate>
            <tr class="item">
                <%#RenderRowItem(Container.DataItem)%>
            </tr>
        </ItemTemplate>
     
        <FooterTemplate>
            </tbody>
            <tfoot>
                <tr id="rptFooter">
                    <%#RenderFooter(lastItem)%>
                </tr>
            </tfoot>
            </table>
            </div>
        </FooterTemplate>
    </asp:Repeater>
    <%--info not available row--%>
   <div class="bg_table" runat="server" visible="false" id="noInfoPanel" enableviewstate="false">
      
        <table class="tbllayout" cellspacing="1" id="rptGrid">
            <%--header--%>
            <thead>
            <tr class="title">
                <th rowspan="2">
                    Username
                </th>
                <th rowspan="2">
                    Turnover
                </th>
                <!--Limit_Grosscomm-->
                <th rowspan="2">
                    GrossComm
                </th>
                <!--/Limit_Grosscomm-->
                <!--Limit_Member-->
                <th colspan="3">
                    Member
                </th>
                <!--/Limit_Member-->
                <!--Limit_Agent-->
                <th colspan="3">
                    Agent
                </th>
                <!--/Limit_Agent-->
                <!--Limit_Master-->
                <th colspan="3">
                    Master
                </th>
                <!--/Limit_Master-->
                <th rowspan="2">
                    Company
                </th>
            </tr>
            <tr class="title">
                <!--Limit_Member-->
                <td>
                    Win/Loss
                </td>
                <td>
                    Comm
                </td>
                <td>
                    Total
                </td>
                <!--/Limit_Member-->
                <!--Limit_Agent-->
                <td>
                    Win/Loss
                </td>
                <td>
                    Comm
                </td>
                <td>
                    Total
                </td>
                <!--/Limit_Agent-->
                <!--Limit_Master-->
                <td>
                    Win/Loss
                </td>
                <td>
                    Comm
                </td>
                <td>
                    Total
                </td>
                <!--/Limit_Master-->
                <!--Limit_Company-->
                <!--/Limit_Company-->
            </tr>
            </thead>
            <tbody>
            <tr>
                <td colspan="16" style="border: 0;">
                   No information is available
                </td>
                <%--/info not available row--%>
            </tr>
            </tbody>
        </table>
       
    </div>
</asp:Content>

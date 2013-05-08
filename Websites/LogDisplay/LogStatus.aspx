<%@ Page Language="C#" MasterPageFile="~/LogDisplay/ViewLog.master" AutoEventWireup="true"
    CodeFile="LogStatus.aspx.cs" Inherits="_ViewLog_LogStatus" Title="View Log Status" Theme="" Async="true" %>


<asp:Content ID="Content1" ContentPlaceHolderID="contentData" runat="Server">
    <div id="content" runat="server">
        <table width="100%" cellpadding="0" cellspacing="0" class="tblcollapse">
            <tr class="title">
                <th>
                    <asp:Literal ID="Literal1" Text="Log ID" EnableViewState="false" runat="server"></asp:Literal>
                </th>
                <th>
                    <asp:Literal ID="Literal2" Text="Log Date" EnableViewState="false" runat="server"></asp:Literal>
                </th>
                <th>
                    <asp:Literal ID="Literal3" Text="Edited By" EnableViewState="false" runat="server"></asp:Literal>
                </th>
                 <th>
                    <asp:Literal ID="Literal6" Text="Username" EnableViewState="false" runat="server"></asp:Literal>
                </th>
               <th>
                    <asp:Literal ID="Literal4" Text="Action" EnableViewState="false" runat="server"></asp:Literal>
                </th>
               
                <th>
                    <asp:Literal ID="Literal5" Text="Detail" EnableViewState="false" runat="server"></asp:Literal>
                </th>
            </tr>
            <asp:Repeater ID="rptDataLog" runat="server" EnableViewState="false">
                <ItemTemplate>
                    <tr>
                        <td>
                            <%#Eval("LogID")%>
                        </td>
                        <td>
                            <%#Eval("LogDate","{0:d}<br/>{0:hh:mm:ss tt}")%>
                        </td>
                        <td>
                            <%#Eval("CustId")%>
                            <br>
                            (<%#Eval("UserID")%>)
                        </td>
                        <td>
                            <%#Eval("CustName")%>
                            <br>
                            (<%#Eval("CustID")%>)
                        </td>
                        <td class="ActionName">
                            <%#Eval("ActionName")%>
                            <br>
                            (<%#Eval("SP")%>)
                        </td>
                        <td>
                           <%#GetContentHtml(Eval("TrackingValue"), Eval("ActionID"), Eval("LogID"))%>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr>
                        <td>
                            <%#Eval("LogID")%>
                        </td>
                        <td>
                            <%#Eval("LogDate","{0:d}<br/>{0:hh:mm:ss tt}")%>
                        </td>
                        <td>
                            <%#Eval("CustId")%>
                            <br>
                            (<%#Eval("UserID")%>)
                        </td>
                        <td>
                            <%#Eval("CustName")%>
                            <br>
                            (<%#Eval("CustID")%>)
                        </td>
                        <td class="ActionName">
                            <%#Eval("ActionName")%>
                            <br>
                            (<%#Eval("SP")%>)
                        </td>
                        
                        <td>
                            <%#GetContentHtml(Eval("TrackingValue"), Eval("ActionID"), Eval("LogID"))%>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:Repeater>
            <tr class="noInfo" id="ltrNoInfo" runat="server">
                <td colspan="6">
                    There is no information available.
                </td>
            </tr>
        </table>
    </div>
    <%--pager--%>
    <%=Pager%>
    <%=GoTopage%>
    <%--/pager--%>
</asp:Content>

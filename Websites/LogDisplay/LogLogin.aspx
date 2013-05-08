<%@ Page Language="C#" MasterPageFile="~/LogDisplay/ViewLog.master" AutoEventWireup="true"
    CodeFile="LogLogin.aspx.cs" Inherits="_ViewLog_LogLogin" Title="View Log Login" Theme="" Async="true"
    %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentData" runat="Server">
    <div id="content" runat="server">
        <table width="100%" cellpadding="0" cellspacing="1" class="tblcollapse">
            <tr class="title">
                <th>
                    <asp:Literal ID="Literal1" Text="Log ID" EnableViewState="false" runat="server"/>
                </th>
                <th>
                    <asp:Literal ID="Literal2" Text="Log Date" EnableViewState="false" runat="server"/>
                </th>
                <th>
                    <asp:Literal ID="Literal3" Text="Username" EnableViewState="false" runat="server"/>
                </th>
                <th>
                    <asp:Literal ID="Literal4" Text="Action" EnableViewState="false" runat="server"/>
                </th>
                <th>
                    <asp:Literal ID="Literal5" Text="Description" EnableViewState="false" runat="server"/>
                </th>
                <th>
                    <asp:Literal ID="Literal7" Text="Domain" EnableViewState="false" runat="server"/>
                </th>
                <th>
                    <asp:Literal ID="Literal8" Text="Browser" EnableViewState="false" runat="server"/>
                </th>
                <th>
                    <asp:Literal ID="Literal6" Text="IP" EnableViewState="false" runat="server"/>
                </th>
            </tr>
            <asp:Repeater ID="rptDataLog" runat="server" EnableViewState="false">
                <ItemTemplate>
                    <tr class="evenrow">
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
                        <td class="ActionName">
                            <%#Eval("ActionName")%>
                            <br>
                            (<%#Eval("SP")%>)
                        </td>
                        <td class="Doing">
                            <%#Eval("Doing")%>                            
                        </td>
                        <td class="Domain">
                            <%#Eval("Domain")%>                            
                        </td>
                        <td class="Browser">
                            <%#Eval("Browser")%>                            
                        </td>
                        <td class="Ip">
                            <%#ShowIp(Eval("IP"))%>
                            
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="oddrow">
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
                        <td class="ActionName">
                            <%#Eval("ActionName")%>
                            <br>
                            (<%#Eval("SP")%>)
                        </td>
                        <td class="Doing">
                            <%#Eval("Doing")%>                            
                        </td>
                        <td class="Domain">
                            <%#Eval("Domain")%>                            
                        </td>
                        <td class="Browser">
                            <%#Eval("Browser")%>                            
                        </td>
                        <td class="Ip">
                            <%#ShowIp(Eval("IP"))%>
                            
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

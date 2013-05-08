<%@ Page Language="C#" MasterPageFile="~/LogDisplay/ViewLog.master" AutoEventWireup="true"
    CodeFile="LogCredit.aspx.cs" Inherits="_ViewLog_LogCredit" Title="View Log Credit" Theme="" Async="true" %>

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
                    <asp:Literal ID="Literal9" Text="Username" EnableViewState="false" runat="server"></asp:Literal>
                </th>
                <th>
                    <asp:Literal ID="Literal4" Text="Action" EnableViewState="false" runat="server"></asp:Literal>
                </th>
                <th>
                    <asp:Literal ID="Literal7" Text="Old Value" EnableViewState="false" runat="server"></asp:Literal>
                </th>
                <th>
                    <asp:Literal ID="Literal8" Text="New Value" EnableViewState="false" runat="server"></asp:Literal>
                </th>
                <th>
                    <asp:Literal ID="Literal6" Text="IP" EnableViewState="false" runat="server"></asp:Literal>
                </th>
                
                <th>
                    <asp:Literal ID="Literal5" Text="Detail" EnableViewState="false" runat="server"></asp:Literal>
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
                            <br/>
                            (<%#Eval("UserID")%>)
                        </td>
                        <td>
                            <%#Eval("CustName")%>
                            <br/>
                            (<%#Eval("CustID")%>)
                        </td>
                        <td class="ActionName">
                            <%#Eval("ActionName")%>
                            <br>
                            (<%#Eval("SP")%>)
                        </td>
                        <td class="Value">
                            <%#Eval("OldValue", "{0:#,##0.00}")%>
                        </td>
                        <td class="Value">
                            <%#Eval("NewValue", "{0:#,##0.00}")%>
                        </td>
                        <td class="Ip">
                           <%#ShowIp(Eval("IP"))%>
                        </td>
                        
                        <td class="Doing">
                            <%#Eval("Doing")%>
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
                            <br/>
                            (<%#Eval("UserID")%>)
                        </td>
                        <td>
                            <%#Eval("CustName")%>
                            <br/>
                            (<%#Eval("CustID")%>)
                        </td>
                        <td class="ActionName">
                            <%#Eval("ActionName")%>
                            <br>
                            (<%#Eval("SP")%>)
                        </td>
                        <td class="Value">
                            <%#Eval("OldValue", "{0:#,##0.00}")%>
                        </td>
                        <td class="Value">
                            <%#Eval("NewValue", "{0:#,##0.00}")%>
                        </td>
                        <td class="Ip">
                            <%#ShowIp(Eval("IP"))%>
                            
                        </td>
                        
                        <td class="Doing">
                            <%#Eval("Doing")%>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:Repeater>
            <tr class="noInfo" id="ltrNoInfo" runat="server">
                <td colspan="9">
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

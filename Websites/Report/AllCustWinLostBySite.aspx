<%@ Page Language="C#" MasterPageFile="~/App_Templates/Default/Report.master" AutoEventWireup="true" CodeFile="AllCustWinLostBySite.aspx.cs" Inherits="Report_AllCustWinLostBySite" Title="Untitled Page" %>
<asp:Content ID="CustWinLostBySiteHeader" ContentPlaceHolderID="ReportTitle" Runat="Server">    
</asp:Content>
<asp:Content ID="CustWinLostBySiteContent" ContentPlaceHolderID="ReportContent" Runat="Server">
    <script type="text/javascript" src="../js/common.js"></script>
    <script type="text/javascript">
        function LoadData()
        {
            var siteName="";
            var betFrom="";   
            for(var i=0;i<document.fSelectSite.rdoSite.length;i++)
            {
                if(document.fSelectSite.rdoSite[i].checked)
                {
                    siteName=document.fSelectSite.rdoSite[i].value;
                }
            }
            for(var j=0;j<document.fSelectSite.rdoBetFrom.length;j++)
            {
                if(document.fSelectSite.rdoBetFrom[j].checked)
                {
                    betFrom=document.fSelectSite.rdoBetFrom[j].value;
                }
            }
            var url='AllCustWinLostBySite.aspx?fdate=<%=Request["fdate"] %>&tdate=<%=Request["tdate"]%>&rdoSite='+siteName + '&rdoBetFrom='+betFrom;
            document.fGetData.action=url;            
            document.fGetData.submit();
        }
    </script>    
    <asp:Literal ID="MenuSite" runat="server"></asp:Literal>
    <asp:Literal ID="CustWinLostBySiteHeaderStr" runat="server"></asp:Literal>
    <asp:Literal ID="CustWinLostBySiteContentHtml" runat="server"></asp:Literal>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ReportFooter" Runat="Server">
</asp:Content>


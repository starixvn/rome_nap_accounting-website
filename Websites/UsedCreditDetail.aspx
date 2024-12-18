<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UsedCreditDetail.aspx.cs" Inherits="UsedCreditDetail" Title="Untitled Page" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="App_Themes/Public/table.css" rel="stylesheet" type="text/css"/>
    <link href="App_Themes/Public/input.css" rel="stylesheet" type="text/css"/>
    <link href="App_Themes/Public/common.css" rel="stylesheet" type="text/css"/>
    <link href="App_Themes/Public/Message.css" rel="stylesheet" type="text/css"/>
    <link href="App_Themes/LeftPanel/left_panel.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
         function resetClosedConfirm() {
             return confirm("Are you sure you want to reset all closed account credit?");
         }
         function resetAllConfirm() {
             return confirm("Are you sure you want to reset all account credit?");
         }
           function initTabsNavigator(numberOfTabs) {
               for (let i = 0; i < numberOfTabs; i++) {
                   const tab = document.getElementById(`left_tab1_${i+1}`);
                   const content = document.getElementById(`left_tab1_${i+1}_content`);
       
                   if (tab) {
                       tab.addEventListener('click', function () {
                           for (let j = 0; j < numberOfTabs; j++) {
                               const otherTab = document.getElementById(`left_tab1_${j+1}`);
                               const otherContent = document.getElementById(`left_tab1_${j+1}_content`);
       
                               if (otherTab) otherTab.classList.remove('current');
                               if (otherContent) otherContent.style.display = 'none';
                           }

                           tab.classList.add('current');
                           if (content) content.style.display = 'block';
                       });
                   }
               }
           }
       
           document.addEventListener('DOMContentLoaded', function () {
               initTabsNavigator(2);
           });
    </script>
</head>
<body>
<div class="main">
    <div class="maincontent">
        <asp:Literal ID="sHeader" runat="server"></asp:Literal>
        <div class="bg_table" id="left_tab1_1_content">
            <asp:Literal ID="sBody" runat="server"></asp:Literal>
        </div>
        <div class="bg_table" id="left_tab1_2_content">
            <asp:Literal ID="sBody2" runat="server"></asp:Literal>
        </div>
    </div>
</div>
</body>
</html>
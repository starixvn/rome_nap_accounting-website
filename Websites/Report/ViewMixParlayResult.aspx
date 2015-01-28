<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewMixParlayResult.aspx.cs" Inherits="Report_ViewMixParlayResult" Theme="" Async="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <title>Mix Parlay Result</title>
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css"/>
    <link rel="stylesheet" type="text/css" href="../Content/Fanex.Result/fanex.result.css"/>
    <link rel="stylesheet" type="text/css" href="../Content/Fanex.Result/fanex.result-customized.css"/>
    <script type="text/javascript">
        window.onload = function() {
        ibcWnd = parent.ibcWnd;
        if (typeof ibcWnd == 'undefined') {
            return;
        }
        
        if (ibcWnd.loaded) {
            var width = 684; //750
            var height = 80; //250           
            if ((typeof numOfLeague != 'undefined') && (typeof numOfMatch != 'undefined'))
                height = height + (numOfLeague*42) + numOfMatch * 43;            
            else
                height = 100;
            ibcWnd.setRect(ibcWnd.left, ibcWnd.top, width, height);
        }
    }
    </script>
</head>
<body>
     <div id = "aspResult">
        <asp:Literal ID="lMPResult" runat="server"></asp:Literal>  
    </div>
    <div id = "parlayDiv">
   </div>
</body>
</html>

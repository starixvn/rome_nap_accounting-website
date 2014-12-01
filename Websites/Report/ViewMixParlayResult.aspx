<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewMixParlayResult.aspx.cs" Inherits="Report_ViewMixParlayResult" Theme="" Async="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <title>Mix Parlay Result</title>
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link rel="Stylesheet" type ="text/css" href ="../App_Themes/BetList/Betlist.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css"/>
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
   <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
  <script type="text/javascript">
      $.ajax({
          type: "POST",
          url: rootUrl + "BetList/GetResultParlay",
          data: 'refno=' + refno,
          success: function (result) {
              $("#parlayDiv").append(result);
              if ((typeof numOfLeague != 'undefined') && numOfLeague - _numberLeageOfTennis == 0) {                
                  $("#aspResult").empty();
              }

              if (_numberLeageOfTennis == 0) {
                  $("#parlayDiv").empty();
              }
          },
          error: function () {
              try { parent.jQuery.unblockUI(); } catch (e) { }
          },
          complete: function () {
              try { parent.jQuery.unblockUI(); } catch (e) { }
          }
      });
    </script>
</html>

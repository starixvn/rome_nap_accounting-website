<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Resize.aspx.cs" Inherits="ExcelData1234" Async="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../js/jQuery/Table/Resize/jquery.dimensions.pack.js" type="text/javascript"></script>
    <script src="../js/jQuery/Table/Resize/jquery.cookies.pack.js" type="text/javascript"></script>
     <script src="../js/jQuery/Table/Resize/jquery.iutil.pack.js" type="text/javascript"></script>
    
    <script src="../js/jQuery/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/jQuery/Table/Resize/jquery.grid.columnSizing.js" type="text/javascript"></script>

    <script type="text/javascript" src="../js/jQuery/jquery.js"></script>

    <script type="text/javascript">

    /*
        $(document).ready(function () {
            var data = {
                'winlossdate': '2011-03-29'                
            };
            $.ajax({
                type: "Post",
                url: "../Services/WCF/Report/ServiceReportDailyWL.svc/GetWinlossRacingReport",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(data),
                success: function (rs) {
                    alert(rs);
                }

            });

           /*
            var data = {
                'sporttype': '151',
                'winlossdate': '2011-03-29'
            };
            $.ajax({
                type: "Post",
                url: "../Services/WCF/Report/ServiceReportDailyWL.svc/DailyWLDirectRacingBySport",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(data),
                success: function (rs) {
                    alert(rs);
                }

            });*/

           
            var data = {                
                'winlossdate': '2011-03-29',
                'bettype': '31'
            };

            $.ajax({
                type: "Post",
                url: "../Services/WCF/Report/ServiceReportDailyWL.svc/DailyWLDirectRacingBySportDetails",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(data),
                success: function (rs) {
                    alert(rs);
                }

            });
        
    </script>
    
    <link href="../js/jQuery/qunit/qunit.css" rel="stylesheet" type="text/css" />
    <script src="../js/jQuery/qunit/qunit.js" type="text/javascript"></script>
    
   
    <style type="text/css">
        table.jquery_columnSizing
        {
            table-layout: fixed;
            width: 1250px;
        }
        table.jquery_columnSizing tr td
        {
            overflow: hidden;
        }
        table{
			border-collapse:collapse;
			font : normal 7.5pt verdana;
			}
		table thead,
		table tbody tr.first {
			background-color:#def;
		}
		table tr td{
			border : 1px solid #bbb;
			vertical-align:top;
			}
		
		table col.noSelectable{
			background-color: #f0f0f0;
			color:#aaa;
		}
    </style>
</head>
<body>
     <script type="text/javascript">

         $(document).ready(function () {
             module("Basic Unit Test");
             test("Sample test", function () {
                 expect(1);
                 equals(divide(4, 2), 2, 'Expected 2 as the result, result was: ' + divide(4, 2));
             });
         });
         function divide(a, b) {
             return a / b;
         }  
    </script>
    <form id="form1" runat="server">
    
    </form>

     
    <h1 id="qunit-header">QUnit example</h1>  
    <h2 id="qunit-banner"></h2>  
    <h2 id="qunit-userAgent"></h2>  
    <ol id="qunit-tests">  
    </ol>  
   
</body>
</html>

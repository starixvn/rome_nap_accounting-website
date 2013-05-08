<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script type="text/javascript" src="../js/common.js"></script>
    <script type="text/javascript" src="../js/prototype.js"></script>
    
    <style type="text/css">
        .ag { background-color:#d8dde2;}
        .ibc { background-color:#d0e8fb;}
        .phu { background-color:#d8dde2;}
        .race { background-color:#99ccff;}
        
    </style>
    
    <script type="text/javascript">
        function DoExit()
        {
            
            parent.document.getElementById('divMatch').style.display = 'none';

        }

        //leagueid|raceNo|currencyID|currencyName|wldate|db|WL
        function ViewBetList(leagueid, raceNo, currencyID, currencyName, wldate, db, WL) 
        {
            var url = 'HRBL.aspx?leagueid=' + leagueid;
            url += '&raceNo=' + raceNo;
            url += '&currencyID=' + currencyID;
            url += '&currencyName=' + currencyName;
            url += '&wldate=' + wldate;
            url += '&db=' + db;
            url += '&WL=' + WL;
                     
            var w = 1100;
            var h = 800;
            PopUpWindow(url, w, h);
        }

        
    </script>
    
   
    
</head>
<body>
    <center> 
     </center>   
     
     <script type="text/javascript">
         function ExportToExcel() {
             var wldate = getPramJs('wldate');
             var bettype = getPramJs('bettype');
             var WL = getPramJs('WL');
             var db = getPramJs('db');

             var url = '?wldate=' + wldate;
             url += '&bettype=' + bettype;
             url += '&WL=' + WL;
             url += '&db=' + db;
             url += '&IsExport=1';

             //alert(url);

             window.location.href = url;
         }
     </script>
</body>
</html>

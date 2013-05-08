<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MiniGamesDetail.aspx.cs" Inherits="Report_MiniGamesWLDetails" Theme="" %>
<%@ Import Namespace="Sunplus.Accounting.DataAccess.Report" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%=Resources.Language.lblAccountingSystem%></title>    
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>

    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css"/> 
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css"/>               
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css"/>      
    <link href="../App_Themes/Report/Report.css" rel="stylesheet" type="text/css" />
    <script type ="text/javascript">
        function resize_iframe()
        {     
            var fheight = $('#container').height() - 10;
            $('#mainfr').height(fheight + "px");
        }
        // this will resize the iframe every
        // time you change the size of the window.
        window.onresize=resize_iframe; 
        
        function obj(id)
        {
            return document.getElementById(id);
        }
        
        function showProgressBar(show)
        {		
	        if(show)
		         obj("inProgressBar").style.display = "";
	        else
		        obj("inProgressBar").style.display = "none";		
        }

        function ViewReport() 
        {
            var db  = (obj('rdMain').checked) ? 'main' : 'rep';            
            showProgressBar(true);
            var fdate = obj('fdate').value;            
            var frm = obj('mainfr');
            var url = 'MiniGamesDetail_1.aspx?type=Direct&click=1&WL=WL&wldate=' + fdate + '&db=' + db;            

            var movebetsday = new Date(obj('movebetsday').value);
            if (movebetsday >= new Date(fdate))
                obj('rdRep').checked = 'checked';
            
            frm.src = url;
        }

        function ExportToExcel() {
            var db = (obj('rdMain').checked) ? 'main' : 'rep';
            var fdate = obj('fdate').value;
            var frm = obj('mainfr');
            var url = 'MiniGamesDetail_1.aspx?type=Direct&click=1&WL=WL&wldate=' + fdate + '&db=' + db + '&IsExport=1';

            var movebetsday = new Date(obj('movebetsday').value);
            if (movebetsday >= new Date(fdate))
                obj('rdRep').checked = 'checked';

            frm.src = url;
        }
       
    </script>    
</head>
    <body>  
        <div class="main">
            <div class="maincontent">
                 <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                    <tr>
                        <td class="maincontent_headerL">
                        </td>
                        <td class="maincontent_headerC"> Mini Game Win Lost 
                        </td>
                        <td class="maincontent_headerR">
                        </td>
                    </tr>
                </table>             
                <div class="maincontent_box">
                    <asp:Literal ID="headerDatetime" runat="server"></asp:Literal>                        
                </div>
                <div class="bg_table">
                    <div id="container" style="height:655px;">
                        <iframe src="MiniGamesDetail_1.aspx?<%=param %>"  frameborder="0" name="mainfr" id="mainfr" style="width:100%; height:90%;"  onload="resize_iframe();" ></iframe>
                    </div> 
                    <input type="hidden" id="movebetsday" name="movebetsday" value="<%=ReportUtils.BalanceUp.MoveBetsDay %>"   
                </div>
            </div>
        </div>        
    </body>
</html>


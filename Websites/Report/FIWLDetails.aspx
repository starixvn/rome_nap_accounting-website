<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FIWLDetails.aspx.cs" Inherits="Report_FIWLDetails" Theme=""  %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title><%=Resources.Language.lblAccountingSystem%></title>
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css"/> 
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css"/>               
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css"/>
    <link href="../App_Themes/Report/Report.css" rel="stylesheet" type="text/css" />
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script type ="text/javascript">
        function resize_iframe() {
            var height = window.innerWidth; //Firefox
            if (document.body.clientHeight) height = document.body.clientHeight; //IE
            if (navigator.appName.indexOf('Microsoft') == 0)//IE   
                document.getElementById("mainfr").style.height = parseInt(height - document.getElementById("mainfr").offsetTop - 5) + "px";
            else //firefox or other browser
            {
                height = window.innerHeight;
                document.getElementById("mainfr").style.height = parseInt(height - document.getElementById("mainfr").offsetTop - 10) + "px";
                if (document.getElementById("mainfr").style.height > 781) document.getElementById("mainfr").style.height = '781px';
            }
            //resize the iframe according to the size of the
            //window (all these should be on the same line)
        }
        // this will resize the iframe every
        // time you change the size of the window.
        window.onresize = resize_iframe;

        function obj(id) {
            return document.getElementById(id);
        }

        function showProgressBar(show) {
            if (show)
                obj("inProgressBar").style.display = "";
            else
                obj("inProgressBar").style.display = "none";
        }

        function ViewReport() {
            showProgressBar(true);
            var fdate = obj('fdate').value;
            var tdate = obj('tdate').value;
            var frm = obj('mainfr');
            var url = 'FIWLDetails_1.aspx?type=Direct&click=1&bettype=Finance&fdate=' + fdate + '&tdate=' + tdate;
            frm.src = url;
        }

        function ExportToExcel() {
            var fdate = obj('fdate').value;
            var tdate = obj('tdate').value;
            var frm = obj('mainfr');
            var url = 'FIWLDetails_1.aspx?type=Direct&click=1&bettype=Finance&fdate=' + fdate + '&tdate=' + tdate + '&IsExport=1';
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
                    <td class="maincontent_headerC">Finance  Win Lost
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
                <div class="maincontent_box">
                    <asp:Literal ID="headerDatetime" runat="server"></asp:Literal>                        
                </div>
                <div class="bg_table">
                    <div id="container" style="height:690px;">
                        <iframe src="FIWLDetails_1.aspx?<%=param %>"  frameborder="0" name="mainfr" id="mainfr" style="width:100%;height:90%;"  onload="resize_iframe();" ></iframe>
                    </div>
                </div>
            </div>
        </div>
    </body>
</html>


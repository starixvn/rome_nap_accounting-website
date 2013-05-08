<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadBatchMsg.aspx.cs" Inherits="Report_UploadBatchMsg_UploadBatchMsg" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css"></link>
    <link href="../../App_Themes/Public/Public.css" rel="stylesheet" type="text/css"></link>
    <link href="../../App_Themes/Public/table.css" rel="stylesheet" type="text/css"></link>
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css"></link>
    <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css"></link>
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css"></link>
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    
    <style>
        textarea
        {
          font-family:Tahoma;
          font-size:8pt; 
        }
    </style>
        
    
    
    <script type="text/javascript">
        function UpLoad() {           
            if (confirm('Are you sure you want to Upload Batch MSG?')) {
                document.getElementById('txt').value = 'upload';
                document.getElementById('frm').submit();
            }
        }
    </script>
</head> 
<body>
    <form method="post" action="UploadBatchMsg.aspx" id="frm" name="frm" runat="server">        
        <table>
            <tr>
                <td>En</td>
                <td><textarea id="txtEn" name="txtEn" style="width:800px"></textarea></td>
            </tr>
             <tr>
                <td>Ch</td>
                <td><textarea id="txtCh" name="txtCh" style="width:800px"></textarea></td>
            </tr>
            <tr>
                <td>Cs</td>
                <td><textarea id="txtCs" name="txtCs" style="width:800px"></textarea></td>
            </tr>
        </table>
        <input type="button" id="upload" name="upload" value="Upload Batch MSG" onclick="UpLoad();" class="btn" />
        <input type="hidden" value="" id="txt" name="txt" />
        
        <%=msg%>
    </form>

</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadVirtualRate.aspx.cs"
    Inherits="UploadVirtualRate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css"></link>
  
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" /> 

    
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>    
    <script src="../js/jQuery/ajaxfileupload.js" type="text/javascript"></script>

    
    <script type="text/javascript" src="../js/common.js"></script> 
    <script type="text/javascript">
        function obj(id) {
            return document.getElementById(id);
        }


        function IsValidFileExtension() {
            var filename = obj('txtfile').value;
            obj('FileName').value = filename;            
            var isOK = false;
            if (filename != "") {
                var ext = filename.substring(filename.lastIndexOf('.') + 1, filename.length).toLowerCase();
                //isOK = (ext == "xls" || ext == "xlsx" || ext == "ods");
                isOK = (ext == "xls");
                if (!isOK) {
                    obj('msgError').style.display = '';
                    $('#divcontainer').removeClass('allhidden', '');
                    showAlertMessage("msgError", "Sorry, You choose invalid file extension. Please choose an excel file.", 1);
                }
                else {
                    obj('msgError').innerHTML = "";
                    isOK = true;
                }
                obj('content').innerHTML = "";
                obj('btnSubmit').disabled = true;
            }
            return isOK;
        }

        function UploadFile() {
            if (IsValidFileExtension()) {
                try{parent.BlockUI();}catch(e){}
                $.ajaxFileUpload
                (
                    {
                        url: '../Handlers/UploadVirtualRate.ashx',
                        secureuri: false,
                        fileElementId: 'txtfile',
                        dataType: 'html',
                        success: function(data, status) {
                            if (!data) data = 'false|logout|false';
                            status = data.split('|')[0];
                            var strdata = data.split('|')[1];
                            var IsForceLogout = data.split('|')[2];

                            if (IsForceLogout == 'true') {
                                alert('Another session has been logged in or your session is expired. Forced to logout');
                                top.location.href = '../PreLogIn/LogOut.ashx';
                                return;
                            }

                            obj('msgError').style.display = 'none';
                            obj('content').innerHTML = strdata;
                            if (status == 'true')
                                obj('btnSubmit').disabled = false;
                            else // Fail
                                obj('btnSubmit').disabled = true;
                            obj('bolDelDataTmp').value = 1;
                            $("#divcontainer").removeClass("allhidden");
                            parent.jQuery.unblockUI();
                        }
                    }
                )
            }
        }

        function UpdateData() {
            var allowUpd = confirm('Are you sure you want to update virtual rate for multiple customers?');
            if (allowUpd) {
                try{parent.BlockUI();}catch(e){}
                $.ajax({
                    url: '../Handlers/UpdateVirtualRateMain.ashx?bolUnLoadPage=0&FileName=' + obj('FileName').value + '&FileSize=' + obj('FileSize').value + '&Index=' + obj('Index').value,
                    dataType: 'html',
                    success: function(data) {
                        $("#divcontainer").removeClass("allhidden");
                        parent.jQuery.unblockUI();
                        obj('msgError').style.display = 'block';
                        obj('msgError').innerHTML = data;
                        obj('btnSubmit').disabled = true;
                        obj('bolDelDataTmp').value = 0;
                    }
                });
            }
        }

        window.onunload = DelDataTmp;
        function DelDataTmp() {
            if (obj('bolDelDataTmp').value == 1)
                obj("fDeleteTmp").src = '../Handlers/UpdateVirtualRateMain.ashx?bolUnLoadPage=1';
        }
    </script>

</head>
<body>
    <div class="maincontent">
         <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL"></td>
                    <td class="maincontent_headerC">Upload Virtual Rate</td>
                    <td class="maincontent_headerR"></td>
                </tr>
            </table>
        <div class="maincontent_box ">
            <div class="b mT5">
                Choose an Excel file to update:
            </div>
            <input type="file" style="font-size: 11px; vertical-align: middle" onchange="return IsValidFileExtension()"
                size="40" name="txtfile" id="txtfile" />&nbsp;&nbsp;&nbsp;
            <input class="btn" id="btnUpload" type="submit" onclick="UploadFile()" value="Open File" />&nbsp;&nbsp;&nbsp;
           <input class='btn' id='btnSubmit' onclick='UpdateData()' type='submit' value='Update Now!'
                disabled="disabled" />
            <div class="defaulttext">
                File Size limit 1 MB</div>
        </div>
        <div id="divcontainer" class="bg_table allhidden">
            <div id="msgError" class="errMsg" style="display: block">
                &nbsp</div>
            <div id="content">
            </div>
            <input type="hidden" id="bolDelDataTmp" value="0" />
            <input type="hidden" id="FileName" value="0" />
            <iframe id="fDeleteTmp" name="fDeleteTmp" style="display: none"></iframe>
        </div>
    </div>
</body>
</html>

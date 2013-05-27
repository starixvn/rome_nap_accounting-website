<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AutomateTransfer.aspx.cs"
    Inherits="Finance_AutomateTransfer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Customer/customerlist.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/LeftPanel/left_panel.css" rel="stylesheet" type="text/css" />
    <link href="../UserGuide/css/userguide.css" rel="stylesheet" type="text/css" />
    <link href="style/AutomateTransfer.css" rel="stylesheet" type="text/css" />
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>

    <script src="../js/jQuery/ajaxfileupload.js" type="text/javascript"></script>

    <script type="text/javascript" src="../js/common.js"></script>

    <script type="text/javascript" src="js/automateTransfer.js"></script>
    
    <script type="text/javascript" src="../js/BlockUI.js"></script>

    <script src="../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>

    <script type="text/javascript" src="../UserGuide/js/userGuide.js"></script>
    <script type="text/javascript" src="../UserGuide/js/centreIt-1.1.5.js"></script>

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
                    $('#msgError').show();
                    showAlertMessage("msgError", "Sorry, You choose invalid file extension. Please choose an Excel file (.xls)", 1);
                    $("#txtfile").val("");
                }
                else if (autoTran.getFileSize("#txtfile") > 1024000) {
                    $('#msgError').show();
                    showAlertMessage("msgError", "The size of file is larger than 1MB", 1);
                    $("#txtfile").val("");
                }
                else {
                    $('#msgError').hide().html("");
                    isOK = true;
                }
                $('#content').html("");
                $('#btnSubmit').attr('disabled', 'disabled');
            }
            return isOK;
        }
        function UploadFile() {
            autoTran.hideError();
            if (IsValidFileExtension()) {
                BlockUI();
                $.ajaxFileUpload({
                    url: 'UploadFinanceTrans.ashx',
                    secureuri: false,
                    fileElementId: 'txtfile',
                    dataType: 'html',
                    success: function(data, status) {
                        $.unblockUI();
                        if (typeof data == "undefined" || data == null) {
                            return;
                        }
                    
                        //--------------------------------------
                        var arr = data.split('|');
                        var msg = arr[0];
                        var html = arr[1];

                        if (msg == '') {
                            $('#content').html(html);
                            $('#btnSubmit').removeAttr('disabled');
                            $("#divcontainer").show();
                            $("#divcontainer").get(0).isShowing = true;
                        }
                        else // Fail
                        {
                            $('#btnSubmit').attr('disabled', 'disabled');
                            $('#msgError').show();
                            autoTran.showError(msg);
                        }
                        $('#bolDelDataTmp').val(1);
                    },
                    error: function(data, status, e) {
                        $.unblockUI();
                        alert(data);
                    }
                });
            }
        }

        function UpdateData() {
            var allowUpd = confirm('Are you sure you want to update Finance Transfer Transactions for these customers?');
            if (allowUpd) {
                try{parent.BlockUI();}catch(e){}
                $.ajax({
                    url: 'UpdateFinanceTransferMul.ashx?bolUnLoadPage=0&FileName=' + obj('FileName').value + '&FileSize=' + obj('FileSize').value, // + '&Index=' + obj('Index').value,
                    dataType: 'html',
                    cache: false,
                    success: function(data) {
                        $("#divcontainer").removeClass("allhidden");
                        parent.jQuery.unblockUI();
                        $('#content').html(data);
                        $('#btnSubmit').attr('disabled', 'disabled');
                        $('#bolDelDataTmp').val("0");
                    }
                });
            }
        }

        window.onunload = DelDataTmp;
        function DelDataTmp() {
            if (obj('bolDelDataTmp').value == 1)
                obj("fDeleteTmp").src = 'UpdateFinanceTransferMul.ashx?bolUnLoadPage=1';
        }
    </script>

</head>
<body>
    <div class="main">
        <div class="maincontent">
            <div style="border: 1px solid #7C98A4; width: 700px; padding: 0px" class="maincontent_box">
                <div class="box_header" style="width: 100%; margin: 0px">
                    <div id="left_tab1" class="left_tab" style="width: 100%">
                        <ul>
                            <li id="left_tab1_1" class="current"><a href="javascript:;"><span>Automate Transfer</span></a></li>
                            <li id="left_tab1_2" style="position:relative">
                                <a href="javascript:;"><span style="padding-right:22px">Cross-Currency Transfer</span> </a>
                                <span style="background-image:url('../UserGuide/image/icon_help.png'); cursor:pointer;
                                        width:14px; height:14px; position:absolute; right:8px; top:3px"
                                        onclick="javascript:userGuide.show('MultiCrossCurrencyTransfer', 'Multiple Cross-Currency Finance Transfer', 700, 500);"></span>
                            </li>
                        </ul>
                    </div>
                </div>
                <div id="msgError" class="errMsg" style="display: none">
                </div>
                <div style="padding-bottom: 10px">
                    <div id="content_tab1" style="margin-top: 10px; margin-left: 10px;">
                        <div>
                            <div class="b mT5">
                                Choose an Excel file to update:
                            </div>
                            <input type="file" style="font-size: 11px; vertical-align: middle" onchange="javascript:return IsValidFileExtension();"
                                size="40" name="txtfile" id="txtfile" />
                            <input class="btn" id="btnUpload" type="submit" onclick="UploadFile()" value="Open File" style="margin-left: 9px" />
                            <input class='btn' id='btnSubmit' onclick='UpdateData()' type='button' value='Update Now!'
                                disabled="disabled" style="margin-left: 9px" />
                            <div class="defaulttext">
                                <span>File Size limit 1 MB</span> <a href="Automate Finance.xls" style="font-weight: bold;
                                    margin-left: 10px">Click here to download template</a>
                            </div>
                        </div>
                    </div>
                    <div id="content_tab2" style="margin-top: 10px; margin-left: 10px; display: none">
                        <div>
                            <div class="b mT5" style="position:relative">
                                <div style="width:180px">Choose an Excel file to update:</div>
                            </div>
                            <input type="file" style="font-size: 11px; vertical-align: middle" onchange="javascript:return autoTran.isValidFileExtension();"
                                size="40" name="fileCrossCur" id="fileCrossCur" />
                            <input class="btn" id="btnUploadFile" type="submit" onclick="javascript:autoTran.UploadFile();"
                                style="margin-left: 9px" value="Open File" />
                            <div class="defaulttext">
                                <span>File Size limit 1 MB</span> <a href="CrossCurrencyTemplate.xls" style="font-weight: bold;
                                    margin-left: 10px">Click here to download template</a>
                            </div>
                        </div>
                        <div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="divcontainer" class="bg_table" style="width:696px; display:none">
                <div id="content">
                </div>
                <input type="hidden" id="bolDelDataTmp" value="0" />
                <input type="hidden" id="FileName" value="0" />
                <iframe id="fDeleteTmp" name="fDeleteTmp" style="display: none"></iframe>
            </div>
            <div id="divCrossCurTran" class="bg_table" style="width:696px; display:none">
                <div id="divHTML"></div>
                <div id="divUploadCCT" style="margin-top:10px">
                    <table border="0" cellpadding="0" cellspacing="0" style="border-collapse: collapse">
                        <tr>
                            <td align="left">
                                <div align="left" style="width: 100px; text-align: left; font-weight:bold">
                                    Forex account:
                                </div>
                            </td>
                            <td align="left">
                                <div align="left" style="width: 140px; text-align: left">
                                    <select id="selectForexAccount" style="width: 120px">
                                    </select>
                                </div>
                            </td>
                            <td align="right">
                                <div>
                                    <input type="button" class="btn" id="uploadCrossCur" value="Update Now!" onclick="autoTran.submit(false);" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
</body>
</html>

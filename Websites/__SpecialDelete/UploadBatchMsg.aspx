<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadBatchMsg.aspx.cs" Inherits="Report_UploadBatchMsg_UploadBatchMsg" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css"></link>
    <link href="../App_Themes/Public/Public.css" rel="stylesheet" type="text/css"></link>
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css"></link>
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css"></link>
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css"></link>
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css"></link>
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>

    <script src="../js/common.js" type="text/javascript"></script>

    <script src="../js/Message/message.js" type="text/javascript"></script>

    <style>
        textarea
        {
          font-family:Tahoma;
          font-size:8pt; 
        }
    </style>
        
    
    
    <script type="text/javascript">

        function CheckDecNumber(e) {
            var code = "";
            if (e.which)
                code = e.which;
            else if (e.keyCode)
                code = e.keyCode;
            //alert(code);

            if (code == 44 || code == 37 || code == 39 || code == 36 || code == 35 || code == 118) 
                return true;

            if ((code < 48 || code > 57) && code != 8 && code != 13 && code != 0 && code != 46 && code != 9) {
                return false;
            }
            
            return true;
        }

        function UpLoad() {
            $('#errMsg').html('');

            var TransIds = $('#txtTransId').val();
            var txtEn = $('#txtEn').val();
            var txtCh = $('#txtCh').val();
            var txtCs = $('#txtCs').val();
            var txtTh = $('#txtTh').val();
            var txtIt = $('#txtIt').val();
            var txtJp = $('#txtJp').val();
            var txtEs = $('#txtEs').val();
            var txtKo = $('#txtKo').val();

            if (TransIds == '') {
                showAlertMessage("errMsg", "Please fill in the TransIds.", 1);
                $('#txtTransId').focus();
                return;
            }

            if (txtEn == '') {
                showAlertMessage("errMsg", "Please fill in the En content.", 1);
                $('#txtEn').focus();
                return;
            }

            if (txtCh == '') {
                showAlertMessage("errMsg", "Please fill in the Ch content.", 1);
                $('#txtCh').focus();
                return;
            }

            if (confirm('Are you sure you want to Upload Batch MSG?')) {
                parent.BlockUI();

                var params = '';

                params = {
                    pmsg: {
                        Content_en: txtEn,
                        Content_cs: txtCs,
                        Content_ch: txtCh,
                        Content_it: txtIt,
                        Content_jp: txtJp,
                        Content_th: txtTh,
                        Content_es: txtEs,
                        Content_ko: txtKo

                    },
                    TransIds: TransIds
                };

                $.ajax({
                    type: "POST",
                    url: "Services/SpecialDeleteService.svc/UploadMsg",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(params),
                    success: function (rs) {
                        showEnrichMessage('errMsg', rs);
                    },
                    error: function () { },
                    complete: function () { try { parent.jQuery.unblockUI(); } catch (e) { } }
                });
                
            }
        }
    </script>
</head> 
<body>

<div class="maincontent">
        <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
            <tr>
                <td class="maincontent_headerL">
                </td>
                <td class="maincontent_headerC">
                    Update Batch Message
                </td>
                <td class="maincontent_headerR">
                </td>
            </tr>
        </table>
        <div class="maincontent_box" style="width:986px; padding:4px 0px!important">
            <div id="errMsg" name="errMsg"></div>
        </div>
        <div class="bg_table" style="width:982px;">
            <table>
                <tr>
                    <td>Transid<span class="error-message">*</span></td>
                    <td>
                        <textarea onkeypress="return CheckDecNumber(event);" id="txtTransId" name="txtTransId" style="width:800px"></textarea>
                        <div>Ex: 12345678,12345679,12345680</div>
                    </td>
                </tr>
                <tr>
                    <td>En<span class="error-message">*</span></td>
                    <td><textarea id="txtEn" name="txtEn" style="width:800px"></textarea></td>
                </tr>
                 <tr>
                    <td>Ch<span class="error-message">*</span></td>
                    <td><textarea id="txtCh" name="txtCh" style="width:800px"></textarea></td>
                </tr>
                <tr>
                    <td>Cs</td>
                    <td><textarea id="txtCs" name="txtCs" style="width:800px"></textarea></td>
                </tr>
                <tr>
                    <td>Th</td>
                    <td><textarea id="txtTh" name="txtTh" style="width:800px"></textarea></td>
                </tr>
                <tr>
                    <td>It</td>
                    <td><textarea id="txtIt" name="txtIt" style="width:800px"></textarea></td>
                </tr>
                <tr>
                    <td>Jp</td>
                    <td><textarea id="txtJp" name="txtJp" style="width:800px"></textarea></td>
                </tr>
                <tr>
                    <td>Es</td>
                    <td><textarea id="txtEs" name="txtEs" style="width:800px"></textarea></td>
                </tr>
                <tr>
                    <td>Ko</td>
                    <td><textarea id="txtKo" name="txtKo" style="width:800px"></textarea></td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <input type="button" id="upload" name="upload" value="Upload Batch MSG" onclick="UpLoad();" class="btn" />
                    </td>
                </tr>
            </table>
        </div>
</div>

        
        
        <input type="hidden" value="" id="txt" name="txt" />
        

</body>
</html>

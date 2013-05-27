var AutomateTransfer = (function ($) {
    return {
        init: function () {
            alert('hi');
        },

        UploadFile: function () {
            autoTran.hideError();
            if (IsValidFileExtension()) {
                BlockUI();
                $.ajaxFileUpload({
                    url: 'UploadFinanceTrans.ashx',
                    secureuri: false,
                    fileElementId: 'txtfile',
                    dataType: 'html',
                    success: function (data, status) {
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
                    error: function (data, status, e) {
                        $.unblockUI();
                        alert(data);
                    }
                });
            }
        },

        IsValidFileExtension: function () {
            var filename = $('#txtfile').val();
            $('#FileName').val(filename);
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
    };
} (jQuery));
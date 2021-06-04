var form;
$(document).ready(function () {
    form = $('#mainForm');
    form.submit(function (e) {
        e.preventDefault();
        var action = "";
        switch (cachedTabId) {
            case "tab_1":
                action = 'InsertProduct';
                break;
            case "tab_2":
                action = 'InsertSport';
                break;
            case "tab_3":
                action = 'InsertBettype';
                break;
            case "tab_4":
                action = 'InsertSportBettypeMatrix';
                break;
            case "tab_5":
                action = 'InsertCSSportBettypeMatrix';
                break;
            case "tab_6":
                action = 'InsertRefResource';
                break;
            case "tab_7":
                action = 'InsertCanSee';
                break;
            case "tab_8":
                action = GetInsertResetWinningAction();
                break;
            case "tab_10":
                action = window.rootUrl + 'ManageVendorGlobalSetting/Insert';
                break;
            case "tab_11":
                action = window.rootUrl + 'ManageProduct3RD/Insert';
                break;
        }
        var formData = new FormData(form[0]);
        $.ajax({
            url: action,
            type: 'POST',
            datatype: 'json',
            contentType: false,
            processData: false,
            enctype: 'multipart/form-data',
            data: formData,
            success: function (result) {
                showAlertMessage('errMsg', result.ErrorCode === 0 ? 'Updated successfully.' : result.ErrorMessage, result.ErrorCode === 0 ? 0 : 1);
                if (result.ErrorCode === 0) {
                    setTimeout(function () {
                        $('#insert-setting').dialog('close');
                        Search();
                    }, 3000);
                }
            },
            error: function (xhr) {
                showAlertMessage('errMsg', 'Internal Server Error.', 1);
            },
            complete: function () {
                parent.jQuery.unblockUI();
            }
        });
    });
});

var InsertSetting = function () {
    if (!ValidateUploadFile()) {
        return;
    }

    parent.BlockUI();
    form.submit();
};

function ValidateUploadFile() {
    var filename = $('#uploadFile').val();
    if (filename === '' || filename == undefined) {
        showAlertMessage('errMsg', 'Please select file to upload.', 1);
        return false;
    }

    var extfilename = filename.substring(filename.lastIndexOf('.') + 1, filename.length).toLowerCase();
    if (extfilename !== 'xls' && extfilename !== 'xlsx') {
        showAlertMessage('errMsg', 'Sorry, You choose invalid file extension. Please choose an Excel file (.xls or .xlsx)', 1);
        return false;
    }

    if (GetFileSize('#uploadFile') > 1024000) {
        showAlertMessage('errMsg', "Your uploaded file's size can't be larger than 1 MB", 1);
        return false;
    }

    if (!hasHostPermission) {
        showAlertMessage('errMsg', "You're not authorized on this function", 1);
        return false;
    }

    HideErrMsg();
    return true;
}

function ajax_form($form, on_complete) {
    var iframe;

    if (!$form.attr('target')) {
        //create a unique iframe for the form
        iframe = $("<iframe></iframe>").attr('name', 'ajax_form_' + Math.floor(Math.random() * 999999)).hide().appendTo($('body'));
        $form.attr('target', iframe.attr('name'));
    }

    if (on_complete) {
        iframe = iframe || $('iframe[name=" ' + $form.attr('target') + ' "]');
        iframe.load(function () {
            //get the server response
            var response = $.parseJSON(iframe.contents().find('body').text());
            on_complete(response);
        });
    }
}

function GetFileSize(eName) {
    if ($.browser.msie) {
        return 1;//IE is not support File API, so no way for get file size.
    }
    else {
        return parseFloat($(eName).get(0).files[0].size);
    }
}

function HideErrMsg() {
    $('#errMsg').html('');
}
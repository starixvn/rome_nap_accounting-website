"use strict";
/* jshint unused: vars,
camelcase: false */
/* global showAlertMessage,
viewResources,
<<<<<<< HEAD
hasHostPermission,
userId */
/* exported importDirectAccountManager */
var importDirectAccountManager;
$(document).ready(function () {
    importDirectAccountManager.initPage();
});

var ImportDirectAccountManager = function () {
    var self = this;
    self.cachedData = {
        successShowTime: 3000,
        gridOptsFailedRecords: {
            tblContentId: 'failed-records-table',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: false,
            gridWidth: 855,
            gridHeight: 300,
            freezeWidth: 0,
            numberFreezeCol: 0
        },
        gridOptsSuccessRecords: {
            tblContentId: 'success-records-table',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridWidth: 295,
            gridHeight: 400,
            freezeWidth: 0,
            numberFreezeCol: 0
        }
    };

    self.Params = {
    };

    self.blockUI = function () {
        try {
            var blocker = parent.parent.BlockUI ? parent.parent : parent;
            blocker.BlockUI();
        }
        catch (e) {
        }
    };
    self.unblockUI = function () {
        try {
            var blocker = parent.parent.$ && parent.parent.$.unblockUI ?
             parent.parent.$ :
             parent.$;
            blocker.unblockUI();
        }
        catch (e) {
        }
    };
    self.CallAjax = function (reportHandler,
        url,
        requestParams,
        successFunction,
        errorFunction,
        completeFunction) {
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(requestParams),
            beforeSend: function () {
            },
            success: function (rs) {
                if (successFunction && typeof (successFunction) === 'function') {
                    successFunction.call(reportHandler, rs);
                }
            },
            error: function (jqXHR, event, ajaxSettings) {
                if (errorFunction && typeof (errorFunction) === 'function') {
                    errorFunction.call(reportHandler);
                }
            },
            complete: function () {
                if (completeFunction && typeof (completeFunction) === 'function') {
                    completeFunction();
                }
            }
        });
    };
    self.downloadFile = function (url, data, method) {
        //url and data options required
        if (url && data) {
            //data can be string of parameters or array/object
            data = typeof data === 'string' ? data : $.param(data);
            if (url.indexOf('ExportTopPunter') > 0) {
                $.fileDownload(url, { httpMethod: "POST", data: data });
            }
            else {
                var downloadUrl = url + "?" + data;
                var iframe = $('body iframe');
                if (iframe.length === 0) {
                    iframe = $('<iframe width="0" height="0" class="display-none"></iframe>');
                    iframe.appendTo('body');
                }

                iframe.attr('src', downloadUrl);
            }
        }
    };

    self.initPage = function () {
        self.blockUI();
        self.cachedDOMElements();
        if (!hasHostPermission) {
            showAlertMessage("result-message", viewResources.Error_NoPermission, 1);
            self.cachedData.resultMessage.removeClass("display-none");
            self.unblockUI();
            return;
        }

        $(".uploadFileContainer").removeClass("display-none");
        $(".check-import-result-box ").removeClass("display-none");
        self.bindingEvent();

        self.cachedData.failedRecordsTableContainer.initGrid({
            isShowPager: false
        });
        self.cachedData.successRecordsTableContainer.initGrid({
            isShowPager: false
        });

        self.unblockUI();
    };

    self.cachedDOMElements = function () {
        self.cachedData.fileUpload = $('#file-upload-control');
        self.cachedData.buttonOpenFile = $('#button-open-file');
        self.cachedData.resultMessage = $('#result-message');
        self.cachedData.failedRecordsTableContainer = $(".failed-records-table-container");
        self.cachedData.successRecordsTableContainer = $(".success-records-table-container");
    };
    self.bindingEvent = function () {
        $(".link-download-excel-template").click(function () {
            self.downloadTemplate();
        });
        self.cachedData.buttonOpenFile.unbind('click');
        self.cachedData.buttonOpenFile.click(function () {
            self.uploadImportDirectAccountFile();
        });

        var buttonUpdate = $("#button-update");
        buttonUpdate.unbind('click');
        buttonUpdate.click(function () {
            self.updateImportDirectAccount();
        });
    };
    self.downloadTemplate = function () {
        self.downloadFile(window.rootUrl + "ConfigureDirectAccount/DownloadImportDirectAccountTemplate", {});
    };
    self.resetUploadFileControl = function () {
        self.cachedData.fileUpload.val("");
        self.cachedData.fileUpload.val("");
        var fileUploader = self.cachedData.fileUpload;
        fileUploader.after('<input id="file-upload-control" name="file-upload-control" size="85" type="file" />').remove();
        self.cachedData.fileUpload = $('#file_UploadFile');
    };
    self.uploadImportDirectAccountFile = function () {
        self.blockUI();
        $("#AccId_UploadFile").val(userId);
        $('#file-upload-form').ajaxSubmit({
            success: function (rs) {
                var prefix = "<pre>";
                var suffix = "</pre>";
                if (rs.toLowerCase().indexOf(prefix) === 0) {
                    rs = rs.substring(5, rs.length);
                }

                if (rs.toLowerCase().indexOf(suffix) === rs.length - suffix.length) {
                    rs = rs.substring(0, rs.length - suffix.length);
                }

                if (rs && rs !== "") {
                    var objRs = JSON.parse(rs);
                    if (objRs && objRs.ErrorCode > 0) {
                        showAlertMessage("result-message", objRs.ErrorMessage, 1);
                        $("#button-update").attr("disabled", "disabled");
                        self.cachedData.resultMessage.removeClass("display-none");
                    } else {
                        self.cachedData.resultMessage.empty().addClass("display-none");
                        self.renderCheckImportDirectAccountResult($.parseJSON(objRs.ActionResult));
                    }
                }

                self.unblockUI();
            }
        });
    };
    self.renderCheckImportDirectAccountResult = function (importResult) {
        if (importResult == null ||
            (importResult.FailedInfoList.length === 0 &&
            importResult.SucceededInfoList.length === 0)) {
            $("#button-update").attr("disabled", "disabled");
            showAlertMessage("result-message", viewResources.Error_SystemError, 1);
            self.cachedData.resultMessage.removeClass("display-none");
            self.cachedData.failedRecordsTableContainer.addClass("display-none");
            self.cachedData.successRecordsTableContainer.addClass("display-none");
            return;
        }

        $("#button-update").attr("disabled", null);
        self.renderFailedRecords(importResult);
        self.renderSuccessRecords(importResult);
    };
    self.renderFailedRecords = function (importResult) {
        if (importResult == null) {
            return;
        }

        var failedLength = importResult && importResult.FailedInfoList ?
            importResult.FailedInfoList.length :
            0;
        var htmlContent = '';
        if (failedLength) {
            for (var i = 0; i < failedLength; i++) {
                var importInfo = importResult.FailedInfoList[i];
                htmlContent += '<tr>\
                        <td class="c">' + importInfo.UsernameIndex + '</td>\
                        <td class="l">' + importInfo.CustomerInfo.UserName + '</td>\
                        <td class="c">' + importInfo.CustomerInfo.RoleName + '</td>\
                        <td class="l">' + importInfo.ImportResultMessage + '</td>\
                    </tr>';
            }
        }

        $(".failed-records-table tbody").empty().append(htmlContent);
        self.cachedData.failedRecordsTableContainer.removeClass("display-none").gridScroller(self.cachedData.gridOptsFailedRecords);
    };
    self.renderSuccessRecords = function (importResult) {
        if (importResult == null) {
            return;
        }

        var successLength = importResult && importResult.SucceededInfoList ?
            importResult.SucceededInfoList.length :
            0;
        var htmlContent = '';
        if (successLength) {
            for (var i = 0; i < successLength; i++) {
                var importInfo = importResult.SucceededInfoList[i];
                htmlContent += '<tr>\
                        <td class="c">' + importInfo.UsernameIndex + '</td>\
                        <td class="l">' + importInfo.CustomerInfo.UserName + '</td>\
                        <td class="c">' + importInfo.CustomerInfo.RoleName + '</td>\
                    </tr>';
            }
        }

        $(".success-records-table tbody").empty().append(htmlContent);
        self.cachedData.successRecordsTableContainer.removeClass("display-none").gridScroller(self.cachedData.gridOptsSuccessRecords);
    };
    self.updateImportDirectAccount = function () {
        if (confirm(viewResources.ConfigureDirectAccount_Import_UpdateConfirm)) {
            self.blockUI();
            self.CallAjax(self,
                window.rootUrl + "ConfigureDirectAccount/UpdateImportDirectAccount",
                {},
                self.renderUpdateResult,
                self.renderUpdateResult,
                self.unblockUI);
        }
    };
    self.renderUpdateResult = function (updateResult) {
        if (updateResult == null) {
            showAlertMessage("result-message", viewResources.Error_SystemError, 1);
            self.cachedData.resultMessage.removeClass("display-none");
            return;
        }

        if (updateResult.ErrorCode > 0) {
            showAlertMessage("result-message", updateResult.ErrorMessage, 1);
            self.cachedData.resultMessage.removeClass("display-none");
            return;
        }

        $("#button-update").attr("disabled", "disabled");
        self.cachedData.failedRecordsTableContainer.addClass("display-none");
        self.cachedData.successRecordsTableContainer.addClass("display-none");
        showAlertMessage("result-message", updateResult.ErrorMessage, 0);
        self.cachedData.resultMessage.removeClass("display-none");

        setTimeout(function () {
            self.cachedData.resultMessage.empty().addClass("display-none");
        }, self.cachedData.successShowTime);
    };
};

importDirectAccountManager = new ImportDirectAccountManager();
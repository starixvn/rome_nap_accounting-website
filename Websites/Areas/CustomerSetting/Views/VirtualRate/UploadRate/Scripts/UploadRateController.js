var uploadRateController;
$(document).ready(function () {
    uploadRateController.initPage();
});

var UploadRateController = function () {
    var self = this;
    self.cachedData = {
        successShowTime: 3000,
        resultMessage: null,
        importResult: null
    };

    self.blockUI = function () {
        try {
            parent.BlockUI();
        } catch (e) {
        }
    };
    self.unblockUI = function () {
        parent.$.unblockUI();
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
        self.cachedDOMElements();

        self.bindEvents();
        if (hasHostPermission) {
            $(".success-records-table-container").initGrid({
                isShowPager: false
            });

            $(".failed-records-table-container").initGrid({
                isShowPager: false
            });

            $(".updatefailed-records-table-container").initGrid({
                isShowPager: false
            });
        }
    };
    self.cachedDOMElements = function () {
        self.cachedData.fileUpload = $('#file-upload-control');
        self.cachedData.buttonOpenFile = $('#button-open-file');
        self.cachedData.resultMessage = $('#result-message');
        self.cachedData.failedRecordsTableContainer = $(".failed-records-table-container");
        self.cachedData.successRecordsTableContainer = $(".success-records-table-container");
    };
    self.bindEvents = function () {
        $(".link-download-excel-template").click(function () {
            self.downloadTemplate();
        });

        var checkImportResultBox = $(".check-import-result-box");
        var buttonOpenFile = $("#button-open-file");
        var buttonUpdate = $("#button-update");
        if (!hasHostPermission) {
            $("#file-upload-control").attr('disabled', 'disabled');
            buttonOpenFile.attr('disabled', 'disabled');
            showAlertMessage("result-message", viewResources.Error_NoPermission, 1);
            self.cachedData.resultMessage.removeClass("display-none");
            checkImportResultBox.removeClass("display-none");
            self.unblockUI();
            return;
        }

        checkImportResultBox.addClass("display-none");

        buttonOpenFile.unbind('click');
        buttonOpenFile.click(function () {
            $(".updatefailed-records-table-container").addClass("display-none");
            self.uploadFile();
        });

        buttonUpdate.unbind('click');
        buttonUpdate.click(function () {
            self.updateImporRate();
        });

        window.onunload = self.deleteTempRate;
    };
    self.uploadFile = function () {
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

                    var checkImportResultBox = $(".check-import-result-box");
                    var resultNote = $("#result-message-note");
                    if (objRs && objRs.ErrorCode > 0) {
                        checkImportResultBox.removeClass("display-none");
                        if (objRs.ResultNote) {
                            resultNote.empty().html(viewResources.Notes + ': ' + objRs.ResultNote).removeClass("display-none");
                        } else {
                            resultNote.addClass("display-none");
                        }

                        showAlertMessage("result-message", objRs.ErrorMessage, 1);
                        $("#btnSubmit").attr("disabled", "disabled");
                        self.cachedData.resultMessage.removeClass("display-none");
                    } else {
                        checkImportResultBox.addClass("display-none");
                        self.cachedData.resultMessage.addClass("display-none");
                        resultNote.addClass("display-none");

                        self.renderUploadResult($.parseJSON(objRs.ActionResult));
                    }
                }

                self.unblockUI();
            }
        });
    };
    self.renderUploadResult = function (importResult) {
        self.cachedData.importResult = importResult;
        var successRecordsTableContainer = $(".success-records-table-container");
        var failedRecordsTableContainer = $(".failed-records-table-container");
        if (importResult == null ||
            (importResult.FailedInfoList.length === 0 &&
            importResult.SucceededInfoList.length === 0)) {
            $("#button-update").attr("disabled", "disabled");
            showAlertMessage("result-message", viewResources.Error_SystemError, 1);
            self.cachedData.resultMessage.removeClass("display-none");
            failedRecordsTableContainer.addClass("display-none");
            successRecordsTableContainer.addClass("display-none");
            return;
        }

        $("#button-update").attr("disabled", null);
        var checkImportResultBox = $(".check-import-result-box");
        checkImportResultBox.removeClass("display-none");
        self.renderSuccessRecords(importResult);
        self.renderFailedRecords(importResult);
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
                var group = importResult.SucceededInfoList[i];
                var groupLength = group.ImportVirtualRateResultList.length;
                for (var j = 0; j < groupLength; j++) {
                    var importInfo = group.ImportVirtualRateResultList[j];
                    htmlContent += '<tr>';
                    if (j == 0) {
                        htmlContent +=
                            '<td class="c" rowspan="' + groupLength + '"><div class="column-body-no">' + (i + 1) + '</div></td>\
                            <td class="c" rowspan="' + groupLength + '"><div class="column-body-username">' +
                                (importInfo.Username ? importInfo.Username : '&nbsp;') +
                            '</div></td>';
                    }

                    htmlContent +=
                            '<td class="c"><div class="column-body-sport">' +
                                (importInfo.SportName ? importInfo.SportName : '&nbsp;') +
                            '</div></td>\
                            <td class="c"><div class="column-body-bettype">' +
                                (importInfo.BetTypeName ? importInfo.BetTypeName : '&nbsp;') +
                            '</div></td>\
                            <td class="c"><div class="column-body-livetype">' +
                                (importInfo.LiveTypeName ? importInfo.LiveTypeName : '&nbsp;') +
                            '</div></td>\
                            <td class="c"><div class="column-body-virtualrate">' + importInfo.VirtualRate * 100 + '</div></td>\
                        </tr>';
                }
            }
        }
        else {
            htmlContent += '<tr>\
                                <td colspan="' + numberColumnSuccess + '" class="c">' + viewResources.Message_NoInfo + '</td>\
                            </tr>';
        }

        var successRecordsTableContainer = $(".success-records-table-container");
        successRecordsTableContainer.find(".success-label").html(
            viewResources.VirtualRate_UploadFile_ValidAlert +
            ' <span class="noted-number">(' + successLength + ')</span>');

        $(".success-records-table tbody").empty().append(htmlContent);
        successRecordsTableContainer.removeClass("display-none").gridScroller({
            tblContentId: 'success-records-table',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: false,
            gridWidth: 0,
            gridHeight: $(parent.window).height() - 200,
            freezeWidth: 0,
            numberFreezeCol: 0
        });
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
                var group = importResult.FailedInfoList[i];
                var groupLength = group.ImportVirtualRateResultList.length;
                for (var j = 0; j < groupLength; j++) {
                    var importInfo = group.ImportVirtualRateResultList[j];
                    htmlContent += '<tr>';
                    if (j == 0) {
                        htmlContent +=
                            '<td class="c" rowspan="' + groupLength + '"><div class="column-body-no">' + (i + 1) + '</div></td>\
                            <td class="c" rowspan="' + groupLength + '"><div class="column-body-username">' +
                                (importInfo.Username ? importInfo.Username : '&nbsp;') +
                            '</div></td>';
                    }

                    htmlContent +=
                            '<td class="c"><div class="column-body-sport">' +
                                (importInfo.SportName ? importInfo.SportName : '&nbsp;') +
                            '</div></td>\
                            <td class="c"><div class="column-body-bettype">' +
                                (importInfo.BetTypeName ? importInfo.BetTypeName : '&nbsp;') +
                            '</div></td>\
                            <td class="c"><div class="column-body-livetype">' +
                                (importInfo.LiveTypeName ? importInfo.LiveTypeName : '&nbsp;') +
                            '</div></td>\
                            <td class="c"><div class="column-body-virtualrate">' + (importInfo.VirtualRateString ? importInfo.VirtualRateString : '&nbsp;') + '</div></td>\
                            <td class="c"><div class="column-body-reason">' + importInfo.ResultMessage + '</div></td>\
                        </tr>';
                }
            }
        }
        else {
            htmlContent += '<tr>\
                                <td colspan="' + numberColumnFailed + '" class="c">' + viewResources.Message_NoInfo + '</td>\
                            </tr>';
        }

        var failedRecordsTableContainer = $(".failed-records-table-container");
        failedRecordsTableContainer.find(".error-label").html(
            viewResources.VirtualRate_UploadFile_InvalidAlert +
            ' <span class="noted-number">(' + failedLength + ')</span>');

        $(".failed-records-table tbody").empty().append(htmlContent);
        failedRecordsTableContainer.removeClass("display-none").gridScroller({
            tblContentId: 'failed-records-table',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: false,
            gridWidth: 0,
            gridHeight: $(parent.window).height() - 200,
            freezeWidth: 0,
            numberFreezeCol: 0
        });
    };
    self.downloadTemplate = function () {
        self.downloadFile(window.rootUrl + "VirtualRate/DownloadImportRateTemplate", {});
    };
    self.updateImporRate = function () {
        if (confirm(viewResources.VirtualRate_UploadFile_UpdateConfirm)) {
            self.blockUI();
            self.CallAjax(self,
                window.rootUrl + "VirtualRate/UpdateImportRate",
                {
                    FileName: self.cachedData.importResult.FileName,
                    FileSize: self.cachedData.importResult.FileSize,
                    TotalSuccessRow: self.cachedData.importResult.TotalSuccessRow,
                    SuccessRecords: self.cachedData.importResult.SuccessRecords
                },
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

        if (updateResult.ResultCode > 0 || updateResult.ErrorCode > 0) {
            var message = updateResult.ResultMessage || updateResult.ErrorMessage;
            if (updateResult.FailedInfoList &&
                updateResult.FailedInfoList.length > 0) {
                message += ' (' + updateResult.FailedInfoList.length + ')';
                self.renderUpdateFailedResult(updateResult.FailedInfoList);
            } else {
                $(".updatefailed-records-table-container").addClass("display-none");
            }

            showAlertMessage("result-message", message, 1);
            self.cachedData.resultMessage.removeClass("display-none");
            return;
        }

        $("#button-update").attr("disabled", "disabled");
        $(".updatefailed-records-table-container").addClass("display-none");
        $(".success-records-table-container").addClass("display-none");
        $(".failed-records-table-container").addClass("display-none");

        showAlertMessage("result-message", updateResult.ResultMessage, 0);
        self.cachedData.resultMessage.removeClass("display-none");
        setTimeout(function () {
            $(".check-import-result-box").addClass("display-none");
            self.cachedData.resultMessage.empty().addClass("display-none");
        }, self.cachedData.successShowTime);
    };
    self.renderUpdateFailedResult = function (failedList) {
        if (failedList == null) {
            return;
        }

        var listLength = failedList.length;
        var htmlContent = '';
        if (listLength) {
            for (var i = 0; i < listLength; i++) {
                var group = failedList[i];
                var groupLength = group.ImportVirtualRateResultList.length;
                for (var j = 0; j < groupLength; j++) {
                    var importInfo = group.ImportVirtualRateResultList[j];
                    htmlContent += '<tr>';
                    if (j == 0) {
                        htmlContent +=
                            '<td class="c" rowspan="' + groupLength + '"><div class="column-body-no">' + (i + 1) + '</div></td>\
                            <td class="c" rowspan="' + groupLength + '"><div class="column-body-username">' +
                                (importInfo.Username ? importInfo.Username : '&nbsp;') +
                            '</div></td>';
                    }

                    htmlContent +=
                            '<td class="c"><div class="column-body-sport">' +
                                (importInfo.SportName ? importInfo.SportName : '&nbsp;') +
                            '</div></td>\
                            <td class="c"><div class="column-body-bettype">' +
                                (importInfo.BetTypeName ? importInfo.BetTypeName : '&nbsp;') +
                            '</div></td>\
                            <td class="c"><div class="column-body-livetype">' +
                                (importInfo.LiveTypeName ? importInfo.LiveTypeName : '&nbsp;') +
                            '</div></td>\
                            <td class="c"><div class="column-body-virtualrate">' + importInfo.VirtualRate * 100 + '</div></td>\
                        </tr>';
                }
            }
        }
        else {
            htmlContent += '<tr>\
                                <td colspan="' + numberColumnSuccess + '" class="c">' + viewResources.Message_NoInfo + '</td>\
                            </tr>';
        }

        var failedRecordsTableContainer = $(".updatefailed-records-table-container");
        $(".updatefailed-records-table tbody").empty().append(htmlContent);
        failedRecordsTableContainer.removeClass("display-none").gridScroller({
            tblContentId: 'updatefailed-records-table',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: false,
            gridWidth: 0,
            gridHeight: $(parent.window).height() - 200,
            freezeWidth: 0,
            numberFreezeCol: 0
        });
    };
    self.deleteTempRate = function () {
        self.CallAjax(self,
            window.rootUrl + "VirtualRate/DeleteTempRate",
            {},
            null,
            null,
            null);
    };
};

uploadRateController = new UploadRateController();
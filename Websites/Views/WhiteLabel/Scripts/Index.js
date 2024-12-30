var WhiteLabel = function () {
    var self = this;
    self.cachedDOMElement = {};

    self.Search = function () {
        $.ajax({
            type: "GET",
            url: window.rootUrl + "WhiteLabel/Search?username=" + self.cachedDOMElement.txtWhiteLabelUsername.val() + "&custId=" + self.cachedDOMElement.selWhiteLabel.val(),
            contentType: "application/json; charset=utf-8",
            beforeSend: function () {
                try { parent.BlockUI(); } catch (e) { }
            },
            success: function (data) {
                if (data.indexOf('Access Denied') > 0) {
                    self.showErrMsg($('#result-message'), 'You are not authorized on this function');
                }
                else {
                    self.RenderSearchGrid(data);
                }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    }

    self.RenderSearchGrid = function (data) {
        var bodyRows = [];
        for (var i = 0; i < data.length; i++) {
            var dataRow = data[i];
            var rowHL = { Columns: [], CssClass: "" };
            rowHL.Columns.push({ Name: '#', Value: i + 1, MergeRowNumber: 3 });
            rowHL.Columns.push({ Name: 'WhiteLabel', Value: dataRow.WhiteLabel, MergeRowNumber: 3 });
            rowHL.Columns.push({ Name: 'WhiteLabelPercent', Value: dataRow.AgentRate + '%', MergeRowNumber: 3 });
            rowHL.Columns.push({ Name: 'UserName', Value: dataRow.UserName, MergeRowNumber: 3 });
            rowHL.Columns.push({ Name: 'LaunchedDate', Value: self.toDateTime(dataRow.LaunchedDate), MergeRowNumber: 3 });
            rowHL.Columns.push({ Name: 'TerminatedDate', Value: self.toDateTime(dataRow.TerminatedDate), MergeRowNumber: 3 });

            rowHL.Columns.push({ Name: 'Level', Value: "Headline" });
            rowHL.Columns.push({ Name: 'MiddlemanUsername', Value: dataRow.Headline });
            rowHL.Columns.push({ Name: 'PercentEarning', Value: dataRow.HeadlineRate + '%' });

            rowHL.Columns.push({ Name: 'Remark', Value: dataRow.Remark, MergeRowNumber: 3 });
            rowHL.Columns.push({
                Name: 'Action',
                Value: '<img alt="Edit" title="Edit" src="' + window.rootUrl + 'App_Themes/Images/icn_edit.png" style="cursor: pointer" onclick="whiteLabel.EditWhiteLabel(\'' + dataRow.UserName + '\')">'
                , MergeRowNumber: 3
            });

            bodyRows.push(rowHL);

            var rowSuper = { Columns: [], CssClass: "" };
            rowSuper.Columns.push({ Name: 'Level', Value: "Super" });
            rowSuper.Columns.push({ Name: 'MiddlemanUsername', Value: dataRow.Super });
            rowSuper.Columns.push({ Name: 'PercentEarning', Value: dataRow.SuperRate + '%' });
            bodyRows.push(rowSuper);

            var rowMaster = { Columns: [], CssClass: "" };
            rowMaster.Columns.push({ Name: 'Level', Value: "Master" });
            rowMaster.Columns.push({ Name: 'MiddlemanUsername', Value: dataRow.Master });
            rowMaster.Columns.push({ Name: 'PercentEarning', Value: dataRow.MasterRate + '%' });
            bodyRows.push(rowMaster);
        }

        var gridOptions =
        {
            columns: [
                { HeaderText: "#", Width: 20, Name: "#", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "White Label", Width: 150, Name: "WhiteLabel", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "White Label %", Width: 100, Name: "WhiteLabelPercent", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Username", Width: 150, Name: "UserName", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Launched Date", Width: 100, Name: "LaunchedDate", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "YYYY-MM-DD" },
                { HeaderText: "Terminated Date", Width: 100, Name: "TerminatedDate", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "YYYY-MM-DD" },
                { HeaderText: "Level", Width: 70, Name: "Level", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: "Middleman Username", Width: 150, Name: "MiddlemanUsername", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: "% Earning", Width: 70, Name: "PercentEarning", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: "Remark", Width: 200, Name: "Remark", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true },
                { HeaderText: "Action", Width: 70, Name: "Action", HeaderAlign: "center", CellAlign: "center" }
            ],
            bodyRows: bodyRows,
            gridExpandHeight: 50,
            showDistinctRow: false
        };

        $('#gridId').fanexGrid(gridOptions);
    }

    self.AddWhiteLabel = function () {
        var dialog = $("#add-white-label-dialog").dialog({
            title: "Add White Label Setting",
            autoOpen: false,
            height: 300,
            width: 600,
            modal: true,
            open: function (event, ui) {
                self.cachedDOMElement.txtLaunchedDate_AddNew.datepicker("enable");
            },
            close: function (event, ui) {
                self.cachedDOMElement.txtLaunchedDate_AddNew.datepicker("disable");
            }
        });
        dialog.removeClass("hide").dialog("open");
        self.ClearForm(dialog);
        $('#txtUsername', dialog).focusout(function () {
            $.get(window.rootUrl + "WhiteLabel/GetAgentInfoForAddNew?agentName=" + $(this).val(), function (data) {
                parent.BlockUI();
                $('#hidCustId', dialog).val(0);
                if (data.ErrorCode == 999) {
                    self.showErrMsg(dialog, data.ErrorMessage);
                }
                else if (data.ErrCode !== undefined) {
                    self.showErrMsg(dialog, data.ErrMsg);
                }
                else {
                    self.hideMsg(dialog);
                    $('#hidCustId', dialog).val(data.CustId);
                    $('#txtSite', dialog).text(data.Site);
                    $('#txtWhiteLabel', dialog).val('');
                    $('#txtAgentName', dialog).text(data.Agent);
                    self.RenderDropdowPercent($('#selAgentRate', dialog), -1);
                    $('#txtMasterName', dialog).text(data.Master);
                    self.RenderDropdowPercent($('#selMasterRate', dialog), -1);
                    $('#txtSuperName', dialog).text(data.Super);
                    self.RenderDropdowPercent($('#selSuperRate', dialog), -1);
                    $('#txtHeadlineName', dialog).text(data.Headline);
                    self.RenderDropdowPercent($('#selHeadlineRate', dialog), -1);
                    $('#txtRemark', dialog).val('');
                    $('#btnSubmit', dialog).off().click(function () {
                        parent.BlockUI();
                        if ($('#selAgentRate', dialog).val() == '' || $('#selMasterRate', dialog).val() == '' || $('#selSuperRate', dialog).val() == '' || $('#selHeadlineRate', dialog).val() == '') {
                            self.showErrMsg(dialog, "Please select rates for middleman.");
                            parent.jQuery.unblockUI();
                            return;
                        }
                        var params = {
                            CustId: $('#hidCustId', dialog).val(),
                            WhiteLabel: $('#txtWhiteLabel', dialog).val(),
                            AgentRate: parseInt($('#selAgentRate', dialog).val()),
                            HeadlineRate: parseInt($('#selHeadlineRate', dialog).val()),
                            SuperRate: parseInt($('#selSuperRate', dialog).val()),
                            MasterRate: parseInt($('#selMasterRate', dialog).val()),
                            LaunchedDate: $('#txtLaunchedDate_AddNew', dialog).val(),
                            Remark: $('#txtRemark', dialog).val()
                        };

                        if (params.CustId == undefined || params.CustId == '' || params.CustId == 0) {
                            self.showErrMsg(dialog, "Agent is required.");
                            parent.jQuery.unblockUI();
                            return;
                        }

                        if (params.WhiteLabel == undefined || params.WhiteLabel == '') {
                            self.showErrMsg(dialog, "WhiteLabel is required.");
                            parent.jQuery.unblockUI();
                            return;
                        }

                        if (params.HeadlineRate + params.AgentRate + params.SuperRate + params.MasterRate > 91) {
                            self.showErrMsg(dialog, "The Total Earning Rate must not exceed 91%.");
                            parent.jQuery.unblockUI();
                            return;
                        }

                        $.post(window.rootUrl + "WhiteLabel/Add", params, function (data) {
                            parent.jQuery.unblockUI();
                            if (data.ErrorCode == 999) {
                                self.showErrMsg(dialog, data.ErrorMessage);
                            }
                            else if (data.ErrCode == 0) {
                                self.showSuccessMsg(dialog, 'Add White Label successfully');
                                setTimeout(function () {
                                    dialog.dialog('close');
                                    self.Search();
                                }, 3000);
                            }
                            else {
                                self.showErrMsg(dialog, data.ErrMsg);
                            }
                        })
                    });
                }

                parent.jQuery.unblockUI();
            });
        });
    }

    self.EditWhiteLabel = function (username) {
        parent.BlockUI();
        $.get(window.rootUrl + "WhiteLabel/GetWhiteLabelSettingForEdit?agentName=" + username, function (data) {
            var dialog = $("#update-white-label-dialog").dialog({
                title: "Update White Label Setting",
                autoOpen: false,
                height: 300,
                width: 600,
                modal: true,
                open: function (event, ui) {
                    self.cachedDOMElement.txtLaunchedDate_Update.datepicker("enable");
                },
                close: function (event, ui) {
                    self.cachedDOMElement.txtLaunchedDate_Update.datepicker("disable");
                }
            });

            self.ClearForm(dialog);
            $('#hidCustId', dialog).val(data.CustId);
            $('#txtUsername', dialog).text(data.UserName);
            $('#txtSite', dialog).text(data.Site);
            $('#txtWhiteLabel', dialog).text(data.WhiteLabel);
            $('#txtAgentName', dialog).text(data.Agent);
            self.RenderDropdowPercent($('#selAgentRate', dialog), data.AgentRate);
            $('#txtMasterName', dialog).text(data.Master);
            self.RenderDropdowPercent($('#selMasterRate', dialog), data.MasterRate);
            $('#txtSuperName', dialog).text(data.Super);
            self.RenderDropdowPercent($('#selSuperRate', dialog), data.SuperRate);
            $('#txtHeadlineName', dialog).text(data.Headline);
            self.RenderDropdowPercent($('#selHeadlineRate', dialog), data.HeadlineRate);
            $('#txtLaunchedDate_Update', dialog).datepicker('setDate', self.toDateTime(data.LaunchedDate)).datepicker('enable');
            $('#txtRemark', dialog).val(data.Remark);
            $('#btnSubmit', dialog).off().click(function () {
                parent.BlockUI();
                var params = {
                    CustId: $('#hidCustId', dialog).val(),
                    AgentRate: parseInt($('#selAgentRate', dialog).val()),
                    HeadlineRate: parseInt($('#selHeadlineRate', dialog).val()),
                    SuperRate: parseInt($('#selSuperRate', dialog).val()),
                    MasterRate: parseInt($('#selMasterRate', dialog).val()),
                    LaunchedDate: $('#txtLaunchedDate_Update', dialog).val(),
                    Remark: $('#txtRemark', dialog).val()
                };

                if (params.HeadlineRate + params.AgentRate + params.SuperRate + params.MasterRate > 91) {
                    self.showErrMsg(dialog, "The Total Earning Rate must not exceed 91%.");
                    parent.jQuery.unblockUI();
                    return;
                }

                $.post(window.rootUrl + "WhiteLabel/Update", params, function (data) {
                    parent.jQuery.unblockUI();
                    if (data.ErrorCode == 999) {
                        self.showErrMsg(dialog, data.ErrorMessage);
                    }
                    else if (data.ErrCode == 0) {
                        self.showSuccessMsg(dialog, 'Update White Label successfully');
                        setTimeout(function () {
                            dialog.dialog('close');
                            self.Search();
                        }, 3000);
                    }
                    else {
                        self.showErrMsg(dialog, data.ErrMsg);
                    }
                })
            });
            dialog.removeClass("hide").dialog("open");
            parent.jQuery.unblockUI();
        });
    }

    self.ClearForm = function (dialog) {
        $('#txtUsername', dialog).val('');
        $('#txtSite', dialog).text('');
        $('#txtWhiteLabel', dialog).val('');
        $('#txtAgentName', dialog).empty();
        $('#selAgentRate', dialog).empty();
        $('#txtMasterName', dialog).empty();
        $('#selMasterRate', dialog).empty();
        $('#txtSuperName', dialog).empty();
        $('#selSuperRate', dialog).empty();
        $('#txtHeadlineName', dialog).empty();
        $('#selHeadlineRate', dialog).empty();
        $('#txtLaunchedDate_AddNew').datepicker('setDate', new Date()).datepicker("enable");
        $('#txtLaunchedDate_Update').datepicker('setDate', new Date()).datepicker("enable");
        $('#txtRemark', dialog).val('');
        self.hideMsg(dialog);
    }

    self.showSuccessMsg = function ($ele, errMsg) {
        $('#error-messages', $ele).html('<div class="alertboxscc"><div>' + errMsg + '</div></div>').removeClass('hide');
    }

    self.showErrMsg = function ($ele, errMsg) {
        $('#error-messages', $ele).html('<div class="alertboxerr"><div>' + errMsg + '</div></div>').removeClass('hide');
    }

    self.hideMsg = function ($ele) {
        $('#error-messages', $ele).addClass('hide').html('');
    }

    self.RenderDropdowPercent = function (ele, selectedVal) {
        ele.empty();
        if (selectedVal < 0) {
            ele.append($('<option>', {
                value: "",
                text: "Please select",
                selected: true,
                disabled: true
            }));
        }
        for (let i = 99; i >= 0; i--) {
            ele.append($('<option>', {
                value: i,
                text: i.toString(),
                selected: i == selectedVal
            }));
        }

        if (selectedVal >= 0) {
            ele.val(selectedVal);
        }
        
    }

    self.initialize = function () {
        self.cachedDOMElement = {
            selWhiteLabel: $('#selWhiteLabel'),
            txtWhiteLabelUsername: $('#txtWhiteLabelUsername'),
            btnSubmit: $('#btnSubmit'),
            txtLaunchedDate_AddNew: null,
            txtLaunchedDate_Update: null
        };

        self.cachedDOMElement.btnSubmit.click(function () { self.Search(); });
        var dateOption = {
            showOtherMonths: true,
            selectOtherMonths: true,
            //minDate: new Date()
        };

        self.cachedDOMElement.txtLaunchedDate_AddNew = $("#txtLaunchedDate_AddNew")
            .datepicker(dateOption);
        self.cachedDOMElement.txtLaunchedDate_Update = $("#txtLaunchedDate_Update")
            .datepicker(dateOption);
    }

    self.toDateTime = function (ISODate) {
        return ISODate == undefined ? '' : new Date(parseInt(ISODate.substr(6)));
    }
};

var whiteLabel = new WhiteLabel();

$(document).ready(whiteLabel.initialize());
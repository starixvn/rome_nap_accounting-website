let Chatbot = {
    height: 720,
    width: 742,
    chatbotId: -1,
    invalidCommand: 1000000,
    currentLanguage: '',
    currentFunctionIds: [],

    assignFeatures: () => {
        let feature;
        let multiselectData = [];
        let isCheckAll = true;

        for (let i = 0, totalFeature = jsonFeatures.length; i < totalFeature; i++) {
            feature = jsonFeatures[i];

            multiselectData.push({
                Id: "ddlFeature" + status.Value,
                Value: feature.Value,
                Text: feature.Text,
                Checked: feature.Selected,
                Enable: true,
                CssClass: "",
                Items: []
            });

            if (feature.Selected == false) {
                isCheckAll = false;
            }
        }

        $("#ddlFeature").multiSelect({
            minWidth: 180,
            maxHeight: 450,
            shadow: true,
            showHeader: true,
            isCheckAll: isCheckAll,
            showFooter: false,
            checkAllText: "All",
            listData: multiselectData,
            noItemText: "No feature found."
        });
    },

    renderCalendar: (ids, opts) => {
        var defaults = {
            changeMonth: true,
            changeYear: true,
            showOtherMonths: true,
            selectOtherMonths: true
        };
        var new_opts = $.extend(true, defaults, opts);
        $("#" + ids).datepicker(new_opts);
        $("#" + ids).datepicker("setDate", new Date(today));
    },

    setupCalendar: (id, toDate) => {
        var limitDate = {
            maxDate: toDate,
            minDate: mindate,
        };
        Chatbot.renderCalendar(id, limitDate);
        $('#' + id).click(function () {
            $('#' + id).datepicker("show");
        });
    },

    renderMessageLogs: (messages) => {
        var html = [];
        if (messages && messages.length > 0) {
            for (var i = 0; i < messages.length; i++) {
                var message = messages[i];

                if (message) {
                    var sender = message.Sender === Chatbot.chatbotId ? "Chatbot" : message.MessengerUsername;
                    var css = message.Sender !== Chatbot.chatbotId ? "user-row" : "";
                    var messageCss = message.Sender !== Chatbot.chatbotId && message.FunctionId === Chatbot.invalidCommand ? ' invalid-command' : '';

                    var row = '<tr class="' + css + '">' +
                        '<td><div class="col100 r">' + sender + '</div></td>' +
                        '<td><div style="width:513px;" class="l' + messageCss + '">' + message.Content + '</div></td>' +
                        '<td><div class="col120 c">' + message.SentDateString + '</div></td>' +
                        '</tr>';

                    html.push(row);
                }
            }
        }
        else {
            var emptyrow = '<tr><td colspan="3" style="border-right:0px none"><div class="c b" style="width:' + Chatbot.width + 'px">There is no information available</div></td></tr>';
            html.push(emptyrow)
        }

        $('#chatHistoryContentBody').empty().append(html.join(''));
        $('#chatHistory').removeClass('hidden');
        $('#chatHistory').reRenderScroller(Chatbot.width, Chatbot.height, true);
    },

    ConvertToDate: (date) => {
        var arr = date.split('/');

        date = new Date();
        date.setYear(arr[2]);
        date.setMonth(arr[0] - 1);
        date.setDate(arr[1]);

        return date;
    },

    getMessageLogs: () => {
        var userId = $('#UserId').val();
        var fromDate = $('#txtfromdate').val();
        var toDate = $('#txttodate').val();
        var functionId = $('#ddlFeatureFilter').val();

        if (Chatbot.ConvertToDate(fromDate) > Chatbot.ConvertToDate(toDate)) {
            Chatbot.showErr("'To date' must be greater than or equal 'from date'!");
            return;
        }

        var data = { userId: userId, fromDate: fromDate, toDate: toDate, functionId: functionId };

        Chatbot.blockUI();

        $.ajax({
            type: "GET",
            url: window.rootUrl + "Chatbot/GetMessageLogs",
            contentType: "application/json; charset=utf-8",
            data: data,
            dataType: "json",
            success: Chatbot.renderMessageLogs,
            complete: () => {
                Chatbot.unBlockUI();
            }
        });
    },

    updateLanguage: () => {
        var userId = $('#UserId').val();
        var language = $('#ddlLanguage').val();

        if (language === Chatbot.currentLanguage) {
            return;
        }

        var data = { userId: userId, language: language };

        Chatbot.blockUI();

        $.ajax({
            type: "POST",
            url: window.rootUrl + "Chatbot/UpdateLanguage",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(data),
            dataType: "json",
            success: (result) => {
                if (result.IsOk) {
                    Chatbot.currentLanguage = language;
                    Chatbot.showSuccess("Update language successfully!")
                } else {
                    Chatbot.showErr("Update language failed.")
                }
            },
            complete: () => {
                Chatbot.unBlockUI();
            }
        });
    },

    compareArrays: (array1, array2) => {
        return $(array1).not(array2).length == 0 && $(array2).not(array1).length == 0;
    },

    updateFeatures: () => {
        var userId = $('#UserId').val();
        var functionIds = $('#ddlFeature').multiSelect("getCheckedItems");

        if (Chatbot.compareArrays(functionIds, Chatbot.currentFunctionIds)) {
            return;
        }

        var data = { userId: userId, functionIds: functionIds };

        Chatbot.blockUI();

        $.ajax({
            type: "POST",
            url: window.rootUrl + "Chatbot/UpdateFunctions",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(data),
            dataType: "json",
            success: (result) => {
                if (result.IsOk) {
                    Chatbot.currentFunctionIds = functionIds;
                    Chatbot.showSuccess("Update feature successfully!")
                } else {
                    Chatbot.showErr("Update feature failed.")
                }
            },
            complete: () => {
                Chatbot.unBlockUI();
            }
        });
    },

    unlink: () => {
        if (confirm("Are you sure to unlink Chatbot for this Letstalk account?")) {
            var userId = $('#UserId').val();
            var letstalkId = $('#LetstalkId').val();

            var data = { userId: userId, letstalkId: letstalkId };

            Chatbot.blockUI();

            $.ajax({
                type: "POST",
                url: window.rootUrl + "Chatbot/Unlink",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(data),
                dataType: "json",
                success: () => {
                    parent.$('#centerFr').attr("src", "about:blank");

                    var leftPanel = parent.$('#leftPanel');
                    leftPanel.attr('src', leftPanel.attr('src'));
                },
                complete: () => {
                    Chatbot.unBlockUI();
                }
            });
        }
    },

    showErr: (msg) => {
        showAlertMessage("err", msg, 1);
        $("#err").show();
        var timerId = setTimeout($.proxy(function () {
            $("#err").hide();
            clearTimeout(timerId);
        }, this), 2000);
    },

    showSuccess: (msg) => {
        showAlertMessage("err", msg, 0);
        $("#err").show();
        var timerId = setTimeout($.proxy(function () {
            $("#err").hide();
            clearTimeout(timerId);
        }, this), 2000);
    },

    blockUI: () => {
        if (parent.parent.BlockUI) {
            try { parent.parent.BlockUI(); } catch (e) { }
        }
        else {
            try { parent.BlockUI(); } catch (e) { }
        }
    },

    unBlockUI: () => {
        if (parent.parent.$ && parent.parent.$.unblockUI) {
            try { parent.parent.$.unblockUI(); } catch (e) { }
        }
        else {
            try { parent.$.unblockUI(); } catch (e) { }
        }
    },

    initialize: () => {
        Chatbot.currentLanguage = $('#ddlLanguage').val();

        Chatbot.assignFeatures();
        Chatbot.currentFunctionIds = $('#ddlFeature').multiSelect("getCheckedItems");

        Chatbot.setupCalendar("txtfromdate", today);
        Chatbot.setupCalendar("txttodate", today);

        $('#btnSubmit').click(Chatbot.getMessageLogs)
        $('#btnSaveLanguage').click(Chatbot.updateLanguage);
        $('#btnSaveFeature').click(Chatbot.updateFeatures);
        $('#btnUnlink').click(Chatbot.unlink);
    },
};

$(function () {
    Chatbot.initialize();
});
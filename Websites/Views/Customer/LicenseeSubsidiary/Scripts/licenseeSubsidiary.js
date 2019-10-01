"use strict";
/* forceNumber()
 * ==========
 * Allow input Number only.
 *
 * @usage: $(selector).forceNumber();
 *
 */
$.fn.forceNumber = function (keyupCallback) {
    var number_0 = 48,
        number_9 = 57,
        numpad_0 = 96,
        numpad_9 = 105,
        end = 35,
        home = 36,
        leftArrow = 37,
        rightArrow = 39,
        backspace = 8,
        tab = 9,
        enter = 13,
        escape = 27,
        del = 46,
        a = 65,
        c = 67,
        v = 86,
        x = 88;

    function isNumeric(text) {
        var regExp = /^-?(\d+|\d+\.\d*|\d*\.\d+)$/;
        return text.match(regExp);
    }

    function keydownHandler(e) {
        var code = e.which || e.keyCode;
        if (number_0 <= code && code <= number_9 ||
            numpad_0 <= code && code <= numpad_9 ||
            code === end ||
            code === home ||
            code === leftArrow ||
            code === rightArrow ||
            code === backspace ||
            code === tab ||
            code === enter ||
            code === escape ||
            code === del) {
            return true;
        }
        var ctrlKey = e.ctrlKey;
        var metaKey = e.metaKey;
        return (code === a || code === c || code === v || code === x) && (ctrlKey || metaKey);
    }

    this.keydown(keydownHandler);
};

(function ($, rootUrl, resources) {
    var maxNumber = 500,
        textboxAgent = $("#txtAgent"),
        textboxNumber = $("#txtNumber"),
        textareaAgentUsernames = $("#txtAgentUsernames"),
        tablePreview = $("#tablePreview > tbody"),
        buttonGenerate = $("#btnGenerate"),
        buttonSubmit = $("#btnSubmit"),
        buttonCreate = $("#btnCreate");
    var isProcessing = false;
    var globalCopiedAccounts = [];

    function showMessage(type, message) {
        var html = ['<div class="', type === 0 ? "alertboxscc" : "alertboxerr", '"><div>', message, "</div></div>"].join("");
        var messageDiv = $("#sectionMessage");
        return messageDiv.html(html).removeClass("hide");
    }

    function hideMessage() {
        var messageDiv = $("#sectionMessage");
        return messageDiv.html("").addClass("hide");
    }

    function showErrorMessage(message) {
        showMessage(1, message);
    }

    function getGenerateFormData() {
        var agent = $.trim(textboxAgent.val());
        if (agent.length === 0) {
            showErrorMessage(resources.sourceAgentRequiredError);
            textboxAgent.focus();
            return;
        }

        var numberValue = $.trim(textboxNumber.val());
        if (numberValue.length === 0) {
            showErrorMessage(resources.numberRequiredError);
            textboxNumber.focus();
            return;
        }

        var number = parseInt(numberValue);
        if (number > maxNumber) {
            showErrorMessage(resources.overMaxNumberError);
            textboxNumber.focus();
            return;
        }

        hideMessage();
        return { username: agent, number: number };
    }

    function generateAgentUsernames(formData) {
        BlockUI();
        $.post(
            rootUrl + "LicenseeSubsidiary/Generate",
            formData,
            function (data) {
                if (data && data.length > 0) {
                    textareaAgentUsernames.val(data.join(","));
                } else {
                    showErrorMessage(resources.sourceAgentNotFoundError.replace("{0}", formData.username));
                    textareaAgentUsernames.val("");
                }
            },
            "json")
        .fail(function (jqXHR) {
            if (jqXHR.status === 401 || jqXHR.status === 403) {
                var denyObject = $.parseJSON(jqXHR.responseText);
                showMessage(jqXHR.status, denyObject.ErrorMessage);
            }
        })
        .always($.unblockUI);
    }

    function validateSourceAgentAndRenderPreviewTable() {
        var agent = $.trim(textboxAgent.val());
        if (agent.length === 0) {
            showErrorMessage(resources.sourceAgentRequiredError);
            textboxAgent.focus();
            return;
        }

        BlockUI();
        $.post(
            rootUrl + "LicenseeSubsidiary/Generate",
            { username: agent, number: 1 },
            function (data) {
                if (data && data.length > 0) {
                    globalCopiedAccounts = getAgentUsernames();
                    renderPreviewTable(globalCopiedAccounts);
                } else {
                    showErrorMessage(resources.sourceAgentNotFoundError.replace("{0}", agent));
                }
            },
            "json")
        .fail(function (jqXHR) {
            if (jqXHR.status === 401 || jqXHR.status === 403) {
                var denyObject = $.parseJSON(jqXHR.responseText);
                showMessage(jqXHR.status, denyObject.ErrorMessage);
            }
        })
        .always($.unblockUI);
    }

    function getAgentUsernames() {
        var agentUsernames = $.trim(textareaAgentUsernames.val());
        if (agentUsernames.length === 0) {
            showErrorMessage(resources.agentUsernamesRequiredError);
            textareaAgentUsernames.focus();
            return [];
        }

        var agentUsernamesArray = agentUsernames.split(",");
        if (agentUsernamesArray.length > maxNumber) {
            showErrorMessage(resources.overMaxNumberError);
            textareaAgentUsernames.focus();
            return [];
        }

        // Check duplicate.
        var trimAgentUsernames = agentUsernamesArray
            .map(function (item) { return $.trim(item); })
            .filter(function (item) { return item.length > 0; });

        var duplicatedAgentUsernames = trimAgentUsernames.filter(function (item, index) {
            return trimAgentUsernames.indexOf(item) !== index;
        });

        if (duplicatedAgentUsernames.length > 0) {
            showErrorMessage(resources.duplicatedAgentUsernamesError.replace("{0}", duplicatedAgentUsernames.join(", ")));
            textareaAgentUsernames.focus();
            return [];
        }

        hideMessage();
        return trimAgentUsernames;
    }

    function renderPreviewTable(agentUsernames) {
        var html = [];
        for (var i = 0, max = agentUsernames.length; i < max; i++) {
            var username = agentUsernames[i];
            html.push('<tr><td>');
            html.push(i + 1);
            html.push('</td>');
            html.push('<td class="l">');
            html.push(username);
            html.push('</td>');
            html.push('<td>X</td>');
            html.push('<td class="l" id="row');
            html.push(username);
            html.push('">');
            html.push(resources.notCreated);
            html.push('</td></tr>');
        }

        tablePreview.html(html.join(""));
    }

    function emptyPreviewTable() {
        tablePreview.html("");
    }

    function copyMultipleAccounts(existedAgent, agentUsernames) {
        isProcessing = agentUsernames.length > 0;
        disableFormControls(isProcessing);
        if (isProcessing) {
            disableFormControls(true);

            var username = agentUsernames.shift();
            var rowAccount = $("#row" + username);
            rowAccount.addClass("b").html(resources.inprogress);

            $.post(
                rootUrl + "LicenseeSubsidiary/Create",
                { username: username, existedUsername: existedAgent },
                function (data) {
                    if (data.ErrorCode === 0) {
                        rowAccount.addClass("text-success").html(resources.created);
                    } else {
                        rowAccount.addClass("text-red").html(data.ErrorMessage);
                    }
                },
                "json")
            .fail(function (jqXHR) {
                if (jqXHR.status === 401 || jqXHR.status === 403) {
                    var denyObject = $.parseJSON(jqXHR.responseText);
                    rowAccount.addClass("text-red").html(denyObject.ErrorMessage);
                }
            })
            .always(function () {
                copyMultipleAccounts(existedAgent, agentUsernames);
            });
        }
    }

    function disableFormControls(value) {
        textboxAgent.prop("disabled", value);
        textboxNumber.prop("disabled", value);
        textareaAgentUsernames.prop("disabled", value);
        buttonGenerate.prop("disabled", value);
        buttonSubmit.prop("disabled", value);
        buttonCreate.prop("disabled", value);
    }

    function initialize() {
        textboxNumber.forceNumber();

        $("#formGenerate").submit(function (e) {
            e.preventDefault();

            emptyPreviewTable();
            var param = getGenerateFormData();
            param && generateAgentUsernames(param);
        });

        buttonSubmit = $("#btnSubmit").click(validateSourceAgentAndRenderPreviewTable);

        $("#formCreate").submit(function (e) {
            e.preventDefault();

            var agent = $.trim(textboxAgent.val());
            if (agent.length === 0) {
                showErrorMessage(resources.sourceAgentRequiredError);
                textboxAgent.focus();
                return;
            }

            copyMultipleAccounts(agent, globalCopiedAccounts);
        });

        $(window).on("beforeunload", function (e) {
            if (isProcessing) {
                var confirmationMessage =
                    "There are one or many accounts which have not been created yet."
                    + " Closing this browser tab will cancel their creation process."
                    + "\nAre you sure you want to close the tab?";
                e.returnValue = confirmationMessage;
                return confirmationMessage;
            }
        });
    }

    initialize();
})(jQuery, rootUrl, resources);
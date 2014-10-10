Namespace.Register("Fanex.Accounting");

Fanex.Accounting.AddNewAgent = function () {
    var me = this;

    me.exportTemplate = function () {
        var url = window.rootUrl + 'AutoTransferAll/ExportTemplateExcelFile';
        jQuery('#frm').attr('src', url);
    };

    me.checkValidAgents = function (btnClicked) {
        try { parent.BlockUI(); } catch (e) { }
        var $form = $(btnClicked).parents('form');
        $form.attr('action', window.rootUrl + "AutoTransferAll/CheckValidAgentsFromFile");

        $('#uploadForm').ajaxForm(function (result) {
            $("#validAgentDiv").html(result);
            if (_validCount > 0) {
                $("#btnSubmit").removeAttr("disabled");
            }
            parent.jQuery.unblockUI();
        })
    };

    me.updatedAgentFromFile = function (btnClicked) {
        try { parent.BlockUI(); } catch (e) { }
        if (_validCount > 0) {
            var $form = $(btnClicked).parents('form');
            $form.attr('action', window.rootUrl + "AutoTransferAll/InsertAgentsFromFile");

            $('#uploadForm').ajaxForm(function (result) {
                if (typeof result.ErrorCode != 'undefined' && result.ErrorCode != 0 ) {
                    $("#validAgentDiv").empty().html("<div class='alerterror'>" + result.ErrorMessage + "</div>");
                }
                else {
                    $("#validAgentDiv").empty().html(result);
                    $("#btnSubmit").attr('disabled', 'disabled');
                }
                parent.jQuery.unblockUI();
            });
        }
    };
};


var AddNewAgent = new Fanex.Accounting.AddNewAgent();

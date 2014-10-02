Namespace.Register("Fanex.Accounting");

Fanex.Accounting.AddNewAgent = function () {
    var me = this;

    me.exportTemplate = function () {
        var url = window.rootUrl + 'AutoTransferAll/ExportTemplateExcelFile';
        jQuery('#frm').attr('src', url);
    };

    me.checkValidAgents = function (btnClicked) {
        var $form = $(btnClicked).parents('form');
        $form.attr('action', window.rootUrl + "AutoTransferAll/CheckValidAgentsFromFile");

        $('#uploadForm').ajaxForm(function (result) {
            $("#validAgentDiv").html(result);
            if (_validCount > 0) {
                $("#btnSubmit").removeAttr("disabled");
            }
        });
    };

    me.updatedAgentFromFile = function (btnClicked) {
        if (_validCount > 0) {
            var $form = $(btnClicked).parents('form');
            $form.attr('action', window.rootUrl + "AutoTransferAll/InsertAgentsFromFile");

            $('#uploadForm').ajaxForm(function (result) {
                $("#validAgentDiv").empty().html(result);
                $("#btnSubmit").attr('disabled', 'disabled');
            });
        }
    };
};


var AddNewAgent = new Fanex.Accounting.AddNewAgent();

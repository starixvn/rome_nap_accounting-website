$(document).ready(function () {
    var isonauto = false;
    var DOMElements = {
        hidSuperId: $('#hidSuperId'),
        txtNewHeadLineName: $('#txtNewHeadLineName'),
        txtSearchSuperUserName: $('#txtSearchSuperUserName'),
        lblOldHeadLineName: $('#lblOldHeadLineName'),
        txtNewHeadLineName: $('#txtNewHeadLineName'),
        lblOldSportsbookPossess: $('#lblOldSportsbookPossess'),
        lblOldNonSportPossess: $('#lblOldNonSportPossess'),
        ddlNewSportsbookPossess: $('#ddlNewSportsbookPossess'),
        ddlNewNonSportPossess: $('#ddlNewNonSportPossess'),
        lblEffectiveDate: $('#lblEffectiveDate'),
        errorMsg: $('#errorMsg'),
        successMsg: $('#successMsg'),
        btnSubmit: $('#btnSubmit'),
        btnSearch: $('#btnSearch')
    };

    var clearMessage = function () {
        DOMElements.errorMsg.hide();
        DOMElements.successMsg.hide();
    };

    var showErrorMessage = function (msg) {
        DOMElements.errorMsg.html(msg);
        DOMElements.errorMsg.show();
    };

    var showSuccessMessage = function (msg) {
        DOMElements.successMsg.show();
    };

    var submitSearchSuper = function () {
        parent.BlockUI();
        clearMessage();
        if (DOMElements.txtSearchSuperUserName.val().trim().length == 0) {
            showErrorMessage(languages.InvalidSuper);
            parent.$.unblockUI();
            return;
        }

        $.post("SearchSuper",
            { userName: DOMElements.txtSearchSuperUserName.val().trim() },
            function (result) {
                if (result == undefined) {
                    showErrorMessage(languages.InvalidSuper);
                }
                else {
                    DOMElements.hidSuperId.val(result.SuperId);
                    DOMElements.lblOldHeadLineName.text(result.CurrentHeadlineUsername);
                    DOMElements.txtNewHeadLineName.val(result.NextHeadlineUsername);
                    DOMElements.lblOldSportsbookPossess.text(result.CurrentSportsbookPossess + '%');
                    DOMElements.ddlNewSportsbookPossess.val(!result.NextSportsbookPossess ? result.CurrentSportsbookPossess : result.NextSportsbookPossess);
                    DOMElements.lblOldNonSportPossess.text(result.CurrentNonSportPossess + '%');
                    DOMElements.ddlNewNonSportPossess.val(!result.NextNonSportPossess ? result.CurrentNonSportPossess : result.NextNonSportPossess);
                    DOMElements.lblEffectiveDate.text(result.EffectiveDate);
                }
            })
            .always(function () {
                parent.$.unblockUI();
            });
    };

    DOMElements.txtNewHeadLineName.autocomplete({ serviceUrl: 'GetHeadlineUserNameSuggestion', minChars: 2 });
    $('.autocomplete').mouseover(function () {
        isonauto = true;
    }).mouseout(function () {
        isonauto = false;
    });

    DOMElements.txtNewHeadLineName.blur(function () {
        if (!isonauto) {
            $('#Autocomplete_1').css('display', 'none');
        }
        else {
            txtusername.focus();
        }
    });

    DOMElements.txtSearchSuperUserName.keypress(function (event) {
        parent.KeyPressEnterCallback(event, submitSearchSuper);
    });

    DOMElements.btnSearch.click(submitSearchSuper);

    DOMElements.btnSubmit.click(function () {
        if (confirm(languages.ConfirmSubmit)) {
            parent.BlockUI();
            clearMessage();
            if (DOMElements.txtNewHeadLineName.val().trim().length == 0) {
                showErrorMessage(languages.InvalidHeadline);
                parent.$.unblockUI();
                return;
            }

            $.post("MoveSuper",
                {
                    SuperId: DOMElements.hidSuperId.val(),
                    HeadlineUsername: DOMElements.txtNewHeadLineName.val().trim(),
                    SportsbookPossess: DOMElements.ddlNewSportsbookPossess.val(),
                    NonSportPossess: DOMElements.ddlNewNonSportPossess.val(),
                    EffectiveDate: DOMElements.lblEffectiveDate.text()
                },
                function (result) {
                    if (result.ErrorCode != 0) {
                        showErrorMessage(result.ErrorMessage);
                    }
                    else {
                        showSuccessMessage(languages.MoveSuperSuccess);
                    }

                    if (result.ErrorCode == 105 || result.ErrorCode == 106) {
                        location.reload(true);
                    }
                })
                .always(function () {
                    parent.$.unblockUI();
                });
        }
    });
});


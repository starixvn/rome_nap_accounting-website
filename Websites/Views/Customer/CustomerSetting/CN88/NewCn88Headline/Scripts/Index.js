$(document).ready(function () {
    var DOMElements = {
        txtUsername: $('#txtUsername'),
        txtPwd: $('#txtPwd'),
        txtFirstName: $('#txtFirstName'),
        txtLastName: $('#txtLastName'),
        txtPhone: $('#txtPhone'),
        txtMobile: $('#txtMobile'),
        txtFax: $('#txtFax'),
        selCurrency: $('#selCurrency'),
        selSportbooksPossess: $('#selSportbooksPossess'),
        selNonSportsPossess: $('#selNonSportsPossess'),
        btnAdd: $('#btnAdd'),
        divShowInfo: $('#divShowInfo'),
        successMessage: $('#successMessage'),
        code: $('#code')
    };

    DOMElements.btnAdd.click(function () {
        clearMessage();
        DOMElements.btnAdd.attr("disabled", true);
        if (DOMElements.txtUsername.val() == undefined || DOMElements.txtUsername.val() == '') {
            showErrorMessage('Username cannot be empty');
            return;
        }

        if (DOMElements.txtPwd.val() == undefined || DOMElements.txtPwd.val() == '') {
            showErrorMessage('Password cannot be empty');
            return;
        }

        if (DOMElements.selCurrency.val() == '0') {
            showErrorMessage('Please choose Currency');
            return;
        }

        try { parent.BlockUI(); } catch (e) { }
        
        var params = {
            userName: DOMElements.txtUsername.val(),
            password: DOMElements.txtPwd.val().encrypt(DOMElements.code.val()),
            firstName: DOMElements.txtFirstName.val(),
            lastName: DOMElements.txtLastName.val(),
            phone: DOMElements.txtPhone.val(),
            mobilePhone: DOMElements.txtMobile.val(),
            fax: DOMElements.txtFax.val(),
            currency: DOMElements.selCurrency.val(),
            sportbooksPossess: DOMElements.selSportbooksPossess.val(),
            nonSportsPossess: DOMElements.selNonSportsPossess.val(),
            code: DOMElements.code.val()
        };

        $.ajax({
            type: "POST",
            url: "AddNewCN88Headline",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(params),
            success: function (result) {
                if (result.ErrorCode == 0) {
                    showSuccessMessage(languages.successMessage);
                }
                else {
                    showErrorMessage(result.ErrorMessage);
                }

                DOMElements.btnAdd.removeAttr('disabled');
            },
            error: function () {
                DOMElements.btnAdd.removeAttr('disabled');
                parent.$.unblockUI();
            },
            complete: function () {
                parent.$.unblockUI();
            }
        });
    });

    var clearMessage = function () {
        DOMElements.divShowInfo.empty();
    }

    var showErrorMessage = function (msg) {
        DOMElements.divShowInfo.append('<div class="alertboxerr"><div>' + msg + '</div></div>');
    }

    var showSuccessMessage = function (msg) {
        DOMElements.divShowInfo.append('<div class="alertboxscc"><div>' + msg + '</div></div>');
    }

});


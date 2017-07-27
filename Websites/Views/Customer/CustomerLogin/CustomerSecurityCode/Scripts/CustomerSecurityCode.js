$(document).ready(function () {
    customerSecurityCode.InitPage();
});

CustomerSecurityCode = function () {
    var self = this;
    self.Enum = {
        Role: {
            Member: 1
        }
    };
    self.Tables = {
        customerName: null,
        txtSecurityCode: null,
        txtConfirmSecurityCode: null,
        chbForceChange: null,
        btnSubmit: null,
        btnReset: null,
        errorMessage: null,
        tdForceChange: null
    };

    self.Params = {
        reportParams: {
            CustomerName: "",
            Custid: null,
            Userid: null,
            IsForcedChangeSecurityCode: false,
            Securitycode: null,
            ConfirmSecurityCode: null,
            Language: null,
            IP: null,
            RoleId: null
        }
    };
    self.CacheData = {
    };
    self.InitPage = function () {
        self.CachedDOMElements();
        self.BindingEvent();
        self.Tables.customerName.html(customerName);
        if (roleId == self.Enum.Role.Member) {
            self.Tables.tdForceChange.addClass("hidden");
        } else {
            self.Tables.tdForceChange.removeClass("hidden");
        }
    };
    self.CachedDOMElements = function () {
        self.Tables.customerName = $(".customerName");
        self.Tables.txtSecurityCode = $("#txtSecurityCode");
        self.Tables.txtConfirmSecurityCode = $("#txtConfirmSecurityCode");
        self.Tables.chbForceChange = $("#chbForceChange");
        self.Tables.btnSubmit = $("#btnSubmit");
        self.Tables.btnReset = $("#btnReset");
        self.Tables.errorMessage = $("#errorMessage");
        self.Tables.tdForceChange = $(".tdForceChange");
    };
    self.BindingEvent = function () {
        self.Tables.btnSubmit.click(function () {
            self.ResetSecurityCode();
        });

        self.Tables.btnReset.click(function () {
            self.ResetPage();
        });
    };
    self.ResetPage = function () {
        self.Tables.txtSecurityCode.val('');
        self.Tables.txtConfirmSecurityCode.val('');
        self.Tables.chbForceChange.attr("checked", "checked");
    };
    self.GetParams = function () {
        self.Params.reportParams.Securitycode = self.Tables.txtSecurityCode.val();
        self.Params.reportParams.ConfirmSecurityCode = self.Tables.txtConfirmSecurityCode.val();
        self.Params.reportParams.IsForcedChangeSecurityCode = self.Tables.chbForceChange.attr("checked") ? true : false;
        self.Params.reportParams.Custid = customerId;
        self.Params.reportParams.RoleId = roleId;
        self.Params.reportParams.Code = $('#code').val();
    };
    self.ResetSecurityCode = function () {
        try { parent.BlockUI(); } catch (e) { }
        self.GetParams();
        if (self.ValidateParams()) {
            self.Tables.errorMessage.empty();
            self.Tables.errorMessage.addClass("hidden");

            var code = document.getElementById('code').value;
            self.Params.reportParams.Securitycode = self.Tables.txtSecurityCode.val().ec(code).hc();
            self.Params.reportParams.ConfirmSecurityCode = self.Tables.txtConfirmSecurityCode.val().ec(code).hc();

            $.ajax({
                type: "POST",
                url: window.rootUrl + "CustomerSecurityCode/ResetSecurityCode",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(self.Params.reportParams),
                success: function (rs) {
                    if (rs && rs.ErrorCode > 0) {
                        showAlertMessage("errorMessage", rs.ErrorMessage, 1);
                    } else {
                        showAlertMessage("errorMessage", "Reset security code successfully!", 0);
                    }

                    self.Tables.errorMessage.removeClass("hidden");
                },
                error: function () {
                },
                complete: function () {
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            });
        } else {
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    };
    self.ValidateParams = function () {
        if (self.Params.reportParams.Securitycode == "") {
            showAlertMessage("errorMessage", "Sorry, security code can't be empty.", 1);
            self.Tables.errorMessage.removeClass("hidden");
            return false;
        }

        if (self.Params.reportParams.Securitycode != self.Params.reportParams.ConfirmSecurityCode) {
            showAlertMessage("errorMessage", "Sorry, confirm security code doesn't match with security code.", 1);
            self.Tables.errorMessage.removeClass("hidden");
            return false;
        }

        if (self.IsSecurityCode(self.Params.reportParams.Securitycode) == false) {
            showAlertMessage("errorMessage", "Sorry, security code is incorrect format.", 1);
            self.Tables.errorMessage.removeClass("hidden");
            return false;
        }

        return true;
    };
    self.IsSecurityCode = function (secCode) {
        var secCodeLength = secCode.length;
        if (secCodeLength != 6) { // check length
            return false;
        }

        var regex = /^([0-9])(?!\1+$)[0-9]+$/; // check digits and all of them are not the same
        var result = secCode.search(regex) != -1 ? true : false;
        if (result) { // check 'Forward Run' and 'Backward Run'
            var isForwardRun = true;
            var isBackwardRun = true;
            var forward = 0;
            var backward = 0;
            var forward = parseInt(secCode.charAt(0));
            var backward = parseInt(secCode.charAt(0));
            for (var i = 1; i < secCodeLength; i++) {
                var current = parseInt(secCode.charAt(i));
                if (++forward != current) {
                    isForwardRun = false;
                }

                if (--backward != current) {
                    isBackwardRun = false;
                }
            }

            result = !(isForwardRun || isBackwardRun);
        }

        return result;
    };
};

var customerSecurityCode = new CustomerSecurityCode();
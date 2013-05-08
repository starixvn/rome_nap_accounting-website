//load function
$(function () {
    edm.init();
});

var customer;
var context;
var ptGrid = null;

var edm = {
    ptGrid: null,
    init: function () {
        edm.initContext();
    },
    initContext: function () {//get context of form
        try { parent.BlockUI(); } catch (e) { parent.parent.BlockUI(); }
        $.ajax({
            type: "POST",
            url: "GetContextObject",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"targetId": ' + JSON.stringify(targetId) + '}',
            success: function (stringContext) {
                context = JSON.parse(stringContext);
                edm.initCustomer();
            }
            , error: function (e) {
                showAlertMessage("err", "Init Context Error", 1);
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    initCustomer: function () {//get customer's info
        $.ajax({
            type: "POST",
            url: "GetCustomerObject",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"requestContext":' + JSON.stringify(context) + '}',
            success: function (stringCustomer) {
                customer = JSON.parse(stringCustomer);
                edm.initPtGrid();
                edm.initForKnockout();
                storeOldPTSetting = customer.ptSetting.clone();
            },
            error: function (e) {
                showAlertMessage("err", "Init Customer Error", 1);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    initForKnockout: function () {//biding for knockout
        var CustomerModel = function (customer) {
            this.saveCustomer = function () {
            };
        };
        ko.applyBindings(new CustomerModel(customer));
    },
    initPtGrid: function () {//init PT Object Grid        
        Fanex.PTEngine.CURRENT_CONTEXT = context;
        ptGrid = new Fanex.PTEngine.PTGrid(customer.ptSetting);
        var divPtGrid = document.getElementById('divPtGrid');
        ptGrid.Render(divPtGrid);

        ptGrid.ShowCloneHint = function () { showCloneHint(this); }
    },
    editSuper: function () {//do -> edit super's Min PT
        if (ptGrid) {
            customer.ptSetting = ptGrid.Save();
            customer.bettingLimit = null;
            customer.statusSetting = null;
            customer.commissionSetting = null;
            customer.commissionSetting2 = null;
            try { parent.BlockUI(); } catch (e) { parent.parent.BlockUI(); }
            $.ajax({
                type: "POST",
                url: "DoEditSuper",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: '{"stringcustomer":' + JSON.stringify(JSON.stringify(customer)) + ', "currentContext":' + JSON.stringify(context) + '}',
                success: function (result) {
                    if (result && result.length > 0) {
                        var totalErrors = result.length;
                        for (var i = 0; i < totalErrors; i++) {
                            var errContent = result[i];
                            if (errContent.ErrorCode == 0) {
                                showAlertMessage("err", "Updated successfully!", 0);

                                //redirect to current page
                                setTimeout(function () { location.href = location.href; }, 3000);
                                try {
                                    parent.afterUpdate(false, 2000);
                                } catch (e) { };
                            }
                            else {
                                showAlertMessage("err", errContent.ErrorMessage, 1);
                            }
                        }
                    }
                }, error: function (e) {
                    showAlertMessage("err", "Request Error", 1);
                },
                complete: function () {
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            });
        }
    }
};

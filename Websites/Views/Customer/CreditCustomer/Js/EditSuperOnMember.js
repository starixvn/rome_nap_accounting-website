//load function
$(function () {
    edm.init();
    $('#chkClosed').on('click', function () {
        if (!confirm("Are you sure you want to change Close status for this customer?")) {
            $('#chkClosed').prop('checked', !($('#chkClosed').prop('checked')));
        }
        else {
            $('#chkClosed').prop('checked', ($('#chkClosed').prop('checked')));
        }
    });
});

var customer;
var context;
var ptGrid = null;
var bettingLimitGrid = null;
var isInsterTwice = false;

var edm = {
    ptGrid: null
};
//init
edm.init = function () {
    edm.initContext();
};
//biding for knockout
edm.initForKnockout = function () {
    var CustomerModel = function (customer) {
        this.saveCustomer = function () {
        };
    };
    ko.applyBindings(new CustomerModel(customer));
};
//get context of form
edm.initContext = function () {
    $.ajax({
        type: "POST",
        url: "GetContextObject",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"agentId":' + JSON.stringify(agentId) + ',"memberId":' + JSON.stringify(memberId) + '}',
        success: function (stringContext) {
            context = JSON.parse(stringContext);
            edm.initCustomer();
        }
        , error: function (e) {
            showAlertMessage("err", "Init Context Error", 1);
        }
    });
};
//get default super value
edm.initCustomer = function () {
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
        }
        , error: function (e) {
            showAlertMessage("err", "Init Customer Error", 1);
        }
    });
};
//init PT Object Grid
edm.initPtGrid = function () {
    Fanex.PTEngine.CURRENT_CONTEXT = context;
    ptGrid = new Fanex.PTEngine.PTGrid(customer.ptSetting);
    var divPtGrid = document.getElementById('divPtGrid');
    ptGrid.Render(divPtGrid);

    bettingLimitGrid = new Fanex.PTEngine.BettingLimit(customer.bettingLimit);
    var bettingLimitContainer = document.getElementById('divBettingLimit');
    bettingLimitContainer.innerHTML = "";
    bettingLimitGrid.Render(bettingLimitContainer, true);

};

//render Transfer Option
// check valid user input
edm.checkValidInput = function () {
    // get user group
    //customer.group = $('#ddlUGruop').val();
    //get pt setting
    customer.ptSetting = ptGrid.Save();
    customer.bettingLimit = null;
    return true;
};
// do edit member pt setting
edm.editMember = function () {
    if (edm.checkValidInput()) {
        try { parent.parent.BlockUI(); } catch (e) { parent.BlockUI();}
        $.ajax({
            type: "POST",
            url: "DoEditMember",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"stringCustomer":' + JSON.stringify(JSON.stringify(customer)) + ', "currentContext":' + JSON.stringify(context) + '}',
            success: function (result) {
                $('#divShowInfo').show();
                if (typeof result != 'undefined' && result != null) {
                    if (result.ErrorCode > 0) {
                        alert_error(result.ErrorMessage, 'err');
                        parent.scroll(0, 0);
                        window.scroll(0, 0);
                    }
                    else {
                        showAlertMessage("err", "Edited Member Successfully!", 0);
                        //redirect to current page
                        setTimeout(function () { location.href = location.href; }, 3000);
                        try {
                            parent.afterUpdate(false, 2000);
                        } catch (e) { };
                    }
                }
                else {
                    showAlertMessage("err", "Edit Member Unsuccessfully!", 1);
                }
                try { parent.parent.jQuery.unblockUI(); } catch (e) { }
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
            , error: function (e) {
                $('#divShowInfo').show();
                if (e.status == '404' || e.statusText == 'Not Found') {
                    if (isInsterTwice == false) {
                        edm.editMember();
                        isInsterTwice = true;
                    }
                    else {
                        showAlertMessage("err", "Request Error", 1);
                    }
                }
                else {
                    showAlertMessage("err", "Request Error", 1);
                }
                try { parent.parent.jQuery.unblockUI(); } catch (e) { parent.jQuery.unblockUI();}
            }
        });
    }
    else {
        $('#divShowInfo').show();
    }
};


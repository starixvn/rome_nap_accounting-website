/// <reference path="../../../../js/dialogbox.js" />
/// <reference path="../../../../js/Popup/confirmbox.js" />
function Search() {
    parent.BlockUI();
    document.location.href = resolveClientUrl('ExtraPt/Index?productId=' + document.getElementById('product').value);
    
    parent.$.unblockUI();
}

function GetEditForm(groupId, productId, effDate) {
    url = resolveClientUrl('ExtraPt/Edit?groupId=' + groupId + '&productId=' + productId + '&effectiveDate=' + effDate);
    parent.OpenNextPopup('edit', url, 'Edit Extra PT Group', 400, 450, true, false, this, 2000, null, function () {
        location.reload(true);
    });
}

function Update() {
    if (parseFloat($('#extrapt').val()) <= 0 || $('#extrapt').val() == '') {
        showAlertMessage("errMsg", "Please input %Extra PT.", 1);
        $("#errMsg").show();

        return;
    }
    else if ($('#effdate').val() > $('#enddate').val()) {
        showAlertMessage("errMsg", "Effective Date is greater than End Date.", 1);
        $("#errMsg").show();

        return;
    }

    parent.BlockUI();
    $.ajax({
        dataType: 'application/json',
        data: {
            data: JSON.stringify({
                newEffectiveDate: $('#effdate').val(),
                newEndDate: $('#enddate').val(),
                newExtraPt: $('#extrapt').val(),
                oldEffectiveDate: $('#oldEffdate').val(),
                oldEndDate: $('#oldEnddate').val(),
                oldExtraPt: $('#oldExtrapt').val(),
                group: {
                    groupId: $('#group').val()
                },
                product: {
                    productId: $('#product').val()
                }
            })
        },
        type: 'post',
        url: resolveClientUrl('ExtraPt/Update')
    }).complete(function (result) {
        result = $.parseJSON(result.responseText);
        showAlertMessage("errMsg", result.ErrorMessage, result.IsSuccess ? 0 : 1);
        $('#errMsg').show();
        parent.$.unblockUI();
    });
}

function Delete(groupId, productId, extraPt, effDate, endDate) {
    parent.BlockUI();
    $.ajax({
        dataType: 'application/json',
        data: {
            data: JSON.stringify({
                effectiveDate: effDate,
                endDate: endDate,
                extraPt: extraPt,
                group: {
                    groupId: groupId
                },
                product: {
                    productId: productId
                }
            })
        },
        type: 'post',
        url: resolveClientUrl('ExtraPt/Delete')
    }).complete(function (result) {
        result = $.parseJSON(result.responseText);
        showAlertMessage("errMsg", result.ErrorMessage, result.IsSuccess ? 0 : 1);
        $('#errMsg').show();
        parent.$.unblockUI();
    });
}

function GetAddForm() {
    url = resolveClientUrl('ExtraPt/Add');
    parent.OpenNextPopup('edit', url, 'Add Extra PT Group', 400, 450, true, false, this, 2000, null, function () {
        location.reload(true);
    });
}

function Insert() {
    if ($('#group').val() == '0') {
        showAlertMessage("errMsg", "Please select Group.", 1);
        $("#errMsg").show();
    }
    else if ($('#product').val() == '0') {
        showAlertMessage("errMsg", "Please select Product.", 1);
        $("#errMsg").show();
    }
    else if (parseFloat($('#extrapt').val()) <= 0 || $('#extrapt').val() == '') {
        showAlertMessage("errMsg", "Please input %Extra PT.", 1);
        $("#errMsg").show();
    }
    else if ($('#effdate').val() > $('#enddate').val()) {
        showAlertMessage("errMsg", "Effective Date is greater than End Date.", 1);
        $("#errMsg").show();
    }
    else {
        $("#errMsg").html('').hide();
        if ($('#effdate').val() == $('#enddate').val()) {
            __confirm("Confirm",
                "Adding a new Extra PT setting which take effect from today might have impact on the reset amounts in the Reset Winning report. Are you sure you want to change?",
                350, 150,
                'confirm',
                function () { DoInsert(); },
                function () { __close(false, 300, 'confirm'); },
                function () { },
                false);
        }
        else {
            DoInsert();
        }
    }
}

function DoInsert() {
    parent.BlockUI();
    $.ajax({
        dataType: 'application/json',
        data: {
            data: JSON.stringify({
                effectiveDate: $('#effdate').val(),
                endDate: $('#enddate').val(),
                extraPt: $('#extrapt').val(),
                group: {
                    groupId: $('#group').val()
                },
                product: {
                    productId: $('#product').val()
                }
            })
        },
        type: 'post',
        url: resolveClientUrl('ExtraPt/Insert')
    }).complete(function (result) {
        result = $.parseJSON(result.responseText);
        showAlertMessage("errMsg", result.ErrorMessage, result.IsSuccess ? 0 : 1);
        $('#errMsg').show();
        parent.$.unblockUI();
    });
}

function ConfirmDelete(groupId, productId, extraPt, effDate, endDate) {    
    __confirm("Delete", "Are you sure you want to delete the selected Extra PT setting?", 300, 150, 'del', function () {
        Delete(groupId, productId, extraPt, effDate, endDate);
        __close(true, 3000, 'del', null);
    }, function () { __close(false, 300, 'del', null); }, function () { __close(false, 300, 'del', null); }, null, false);
}
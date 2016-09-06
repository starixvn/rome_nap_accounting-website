function syncCurrency() {
    try {
        if (validateCurrency()) {
            parent.BlockUI();

            $.ajax({
                type: "POST",
                url: "Add",
                data: { currencyCode: document.getElementById("currency").value },
                success: function (rs) {
                    showAlertMessage("errMsg", rs.IsSuccess ? "Sync currency successfully. Please contact to DBA to update synchronized currency list" : rs.ErrorMessage, rs.ErrorCode);
                },
                error: function (rs) {
                    showAlertMessage("errMsg", rs.responseText + "!", 1);
                },
                complete: function () {
                    parent.jQuery.unblockUI();
                }
            });
        }
        else {
            showAlertMessage("errMsg", "Please enter a currency name!", 1);
        }        
    } catch (e) {

    }    
}

function validateCurrency() {
    var currency = document.getElementById("currency");
    if (currency == null || currency.value == "" || $.trim(currency.value) == "") {
        return false;
    }

    return true;
}
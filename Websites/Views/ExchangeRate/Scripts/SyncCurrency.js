function syncCurrency() {
    try {
        if (validateCurrency()) {
            parent.BlockUI();

            $.ajax({
                type: "POST",
                url: "Add",
                data: { currencyCode: document.getElementById("currency").value },
                success: function (rs) {
                    showAlertMessage("errMsg", rs.IsSuccess ? "Update base rate successfully" : rs.ErrorMessage, (rs.IsSuccess ? 0 : 1));
                },
                error: function (rs) {
                    if (rs.status == 0) {
                        document.location.reload(true);
                    }

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
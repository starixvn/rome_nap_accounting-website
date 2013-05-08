var addTransaction = {
    custName: '',
    custId: 0,
    transDate: '',
    amount: 0,
    lang: 'E',
    transDesc: '',
    addNewTrans: function () {
        addTransaction.valueAddValid();
        $('#errmsg').html('');

        parent.BlockUI();

        addTransaction.custId = gup("CustID");
        addTransaction.custName = gup("CustName");
        addTransaction.transDate = $("#txtDate").val();
        addTransaction.amount = $("#txtamount").val();
        addTransaction.transDesc = $("#txtDesc").val();

        $.ajax({
            type: "Post",
            url: "../Services/WCF/ServiceDailyTran.svc/AddTransaction",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"custId":' + addTransaction.custId + ',"transDate":"' + addTransaction.transDate + '","amount":' + addTransaction.amount + ', "transDesc":"' + addTransaction.transDesc + '", "lang": "' + addTransaction.lang + '"}',
            success: function (rs) {
                if (rs != null) {
                    if (rs.ErrorCode == "0") {
                        var url = "Statement/CustomerStatement.aspx?repno=3&CustName=" + addTransaction.custName;
                        parent.gotoURL('centerFr', url);

                    }
                    else {
                        showAlertMessage("errMsg", rs.ErrorMessage, 1);

                    }
                }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    valueAddValid: function () {
        if (isEmpty("txtDate")) {

            showAlertMessage("errMsg", "Please select date.", 1);
            obj("txtDate").focus();
            return false;
        }
        if (isEmpty("txtamount") || isString("txtamount")) {

            showAlertMessage("errMsg", "Invalid Amount.", 1);
            obj("txtamount").focus();
            return false;
        }
        return true;
    }
};
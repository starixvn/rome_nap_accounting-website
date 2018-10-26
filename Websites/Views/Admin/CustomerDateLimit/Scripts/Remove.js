var CustomerDataRangeRemove = {

    init: function () {
        CustomerDataRangeRemove.clearForm();

        $("#btn-clear").click(CustomerDataRangeRemove.clearForm);

        $("#btn-submit").click(CustomerDataRangeRemove.updateDataRange);

        $("#userNames").keydown(function (e) {
            var key = e.charCode || e.keyCode || 0;

            var isAccept = key != 13 //enter

            return isAccept;
        });

    },
    clearForm: function () {
        $("#userNames").val("");
    },
    updateDataRange: function () {
        $("#ErrorSection").html('');
        $("#SuccessSection").html('');
        $("#InfoSection").html('');
        if (CustomerDataRangeRemove.getUserNames() == "") {
            showMessage("ErrorSection", "Please enter username.", 1);
            return;
        }
        
        if (CustomerDataRangeRemove.getUserNames().split(',').length > 500) {
            showMessage("ErrorSection", "The function can only support up to 500 accounts per request.", 1);
            return;
        }

        if (CustomerDataRangeRemove.getUserNames().indexOf("\\") >= 0 ||
            CustomerDataRangeRemove.getUserNames().indexOf("\n") >= 0 ||
            CustomerDataRangeRemove.getUserNames().indexOf("\r") >= 0) {
            showMessage("ErrorSection", "You are not allowed to input [Enter] and backslash '\\' into the Username field.", 1);
            return;
        }

        $.ajax({
            url: resolveClientUrl("CustomerDateLimit/Remove"),
            dataType: 'json',
            type: 'post',
            data: { userNames: CustomerDataRangeRemove.getUserNames() },
            beforeSend: function () {
                try { BlockUI(); } catch (e) { }
            },
        }).success(function (result) {
            if (result.ErrorMessage != "") {
                showMessage("ErrorSection", result.ErrorMessage, 1);
            }           
            

            if (result.NoChangeUsers != "") {
                showMessage("InfoSection", result.NoChangeUsers + ": There is no change to be updated.", 2);
            }

            if (result.EffectedRow > 0) {
                showMessage("SuccessSection", "Successfully updated: " + result.EffectedRow + " super(s)", 0);
            }

        }).error(function (xhr, status, err) {
            showMessage("ErrorSection", err, 1);

        }).complete(function () {
            try { jQuery.unblockUI(); } catch (e) { }
        });
    },
    getUserNames: function () {
        return $("#userNames").val();
    }
};

$(document).ready(CustomerDataRangeRemove.init);
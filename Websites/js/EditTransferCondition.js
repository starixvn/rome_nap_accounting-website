
function confirmUpdateTransferCondition() {
    var message = lblConfirm;

    if (!isDailyCondition) {
        message += "\n" +
            (autoTransferCondition == 0
                ? document.getElementById("MessageConfirmUpdate_NoteOn").value
                : document.getElementById("MessageConfirmUpdate_NoteOff").value);
    }

    var strConfirm = confirm(message);
    if (!strConfirm) {
        return false;
    }

    var chkCount = 0;
    var isSelect = false;
    if (obj("rdWeekly").checked) {
        var elements = obj("alldays").getElementsByTagName("input");
        for (var i = 1; i < elements.length; i++) {
            if (elements[i].checked) {
                isSelect = true;
                chkCount++;
            }
        }

        if (!isSelect) {
            showAlertMessage("err", lblChooseOneDay, 1);
            scroll(0, 0);
            return false;
        }
    }

    return true;
}

function toggleConditionNote(isShow) {
    var rowNote = document.getElementsByClassName("row-note")[0];
    if (isShow) {
        isDailyCondition = false;
        rowNote.className = "row-note";
    } else {
        isDailyCondition = true;
        rowNote.className = "row-note hidden";
    }
}

function updateAutoTransferCondition(condition) {
    autoTransferCondition = condition;
}
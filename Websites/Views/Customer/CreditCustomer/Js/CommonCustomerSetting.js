function GetTransferOption() {
    var str = "00000000";
    var transferConditionId = document.getElementsByName("transferCondition");

    if (transferConditionId != 'undefined' && transferConditionId.length > 0) {
        if (transferConditionId[1].checked) {
            str = "";
            var arrWeekly = new Array("chkSun", "chkMon", "chkTue", "chkWed", "chkThurs", "chkFri", "chkSat");
            for (var i = 0; i < arrWeekly.length; i++) {
                if (document.getElementById(arrWeekly[i]).checked) {
                    str += "1";
                }
                else {
                    str += "0";
                }
            }
            str += "1";
        }
    }
    return str;
};
//this function is used to check case editing transfer time
//When Weekly radio is selected and don't have any checkbox of days is checked
function CheckTimeForTransferOption() {
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
            $('#divShowInfo').show();
            showAlertMessage("err", lblChooseOneDay, 1);
            scroll(0, 0);
            return false;
        }
    }
    return true;
}

//this function is used to check case editing transfer time
//When Weekly radio is selected and all checkbox of 7 days can not be checked
function ChooseDateTransfer() {
    var chkCount = 0;
    if (obj("rdWeekly").checked) {
        var elements = obj("alldays").getElementsByTagName("input");
        if (obj("err") != null && obj("err").innerHTML != "") {
            obj("err").innerHTML = "";
        }
        for (var i = 1; i < elements.length; i++) {
            if (elements[i].checked) {
                isSelect = true;
                chkCount++;
                if (chkCount == 7) {
                    $('#divShowInfo').show();
                    showAlertMessage("err", lblChooseDaily, 2);
                    return false;
                }
            }
        }
    }
    return true;
};

function ValidateUserName(userName) {
    var validateExpr = /^[A-Za-z0-9_-]{0,40}$/;
    return validateExpr.test(userName);
};

function CheckShowTransferOption(transferOption) {
    if (transferOption == "00000000") {
        return true;
    }
    else {
        return false;
    }
};

// Success=0, Error=1, Warning=2
function AlertValidationMessage(containerid, msg, type, controlValidatedId) {
    switch (type) {
        case 0:
            document.getElementById(containerid).innerHTML = "<div class='alertboxscc'><div>" + msg + "</div></div>";
            break;
        case 1:
            document.getElementById(containerid).innerHTML = "<div class='alertboxerr'><div>" + msg + "</div></div>";
            break;
        case 2:
            document.getElementById(containerid).innerHTML = "<div class='alertboxwrn'><div>" + msg + "</div></div>"; ;
            break;
        default:
            break
    }
    Validators.animate(document.getElementById(controlValidatedId));
    document.getElementById(controlValidatedId).focus();
    window.scroll(0, 0);
    parent.scroll(0, 0);
}

// Check Constrain for user name and password
function CheckConstrainForUsernameAndPassword() {
    if (isEmpty("txtUsername")) {
        AlertValidationMessage("err", alertUserName, 1, 'txtUsername');
        return false;
    }
    if ($('#txtUsername').val().length > 30) {
        AlertValidationMessage("err", "Sorry, Username cannot exceed 30 characters.", 1, 'txtUsername');
        return false;
    }
    if (isEmpty("txtPwd")) {
        AlertValidationMessage("err", "Sorry, password cannot be empty!!!", 1, 'txtPwd');
        return false;
    }
    if ($('#txtPwd').val().length > 30) {
        $('#txtPwd').val("");
        AlertValidationMessage("err", "Sorry, Password cannot exceed 30 characters.", 1, 'txtPwd');
        return false;
    }
    // Save Customer Info
    customer.firstName = $('#txtFirstname').val();
    customer.lastName = $('#txtLastname').val();
    customer.phone = $('#txtPhone').val();
    customer.mobilePhone = $('#txtMobile').val();
    customer.fax = $('#txtFax').val();
    return true;
}

function showCloneHint(ptGrid) {
    
    if (ptGrid.copiedItem == null) {
        return;
    }
    var copiedElements = [0, ptGrid.items[0].items[0].items[0].items[0].element,
                                        ptGrid.items[1].items[0].items[0].items[0].element]; // Handicap of Soccer and Basketball
    var hint = Language.COPY_ALL_HINT.Format('<b>' + ptGrid.activeCopiedTitle + '</b>');
    var otherBetTypesOf = Language.OTHER_BET_TYPE_OF.Format(ptGrid.items[ptGrid.copiedItem.sportId - 1].name);
    hint = hint + '<div class="ul-position"><ul><li>' + otherBetTypesOf + '</li>';
    // Find all copied items to generate tooltip
    var count = ptGrid.items.length;
    var top = 0;
    for (var i = ptGrid.copiedItem.sportId; i < count; i++) {
        if (ptGrid.items[i].copiedItem != null && ptGrid.items[i].copiedItem.sportId == ptGrid.copiedItem.sportId) {
            hint += '<li>' + ptGrid.items[i].name + '</li>';
            top += 18;
        }
    }
    hint = hint + '</ul></div>';
    top = top + 40;
    // Show tooltip
    var yPosition = 70;
    if (context.roleId == 4) {
        yPosition = 122;
    }
    show(copiedElements[ptGrid.copiedItem.sportId], hint, 0, 10, yPosition, -10, top, 30000);
}

function show(elementInput, message, xPos, pointerXPos, yPos, pointerYPos, height, timeout) {
    // <span class="hint"><span>This is the message.<span><span class="hint-pointer">&nbsp;</span></span>
    var hint = document.getElementById('fHint');
    var hintPointer = null;
    if (hint == null) {
        var hint = document.createElement("SPAN");
        hint.className = 'hint';
        var hintMsg = document.createElement("SPAN");
        hint.appendChild(hintMsg);
        hintPointer = document.createElement("SPAN");
        hintPointer.className = 'hint-pointerDown';
        hint.appendChild(hintPointer);
        hint.id = 'fHint';
        hint.style.top = '10px';
        hint.style.left = '10px';
        hint.onclick = function () { // Hide onclick
            hint.style.display = 'none';
        };
        document.body.appendChild(hint);
    };
    hintPointer = hint.lastChild;
    hint.firstChild.innerHTML = message;
    if (this.timeOutHideTooltip) {
        clearTimeout(this.timeOutHideTooltip);
        this.timeOutHideTooltip = null;
    };
    if (typeof (timeout) == 'undefined') {
        timeout = 10000;
    }
    this.timeOutHideTooltip = setTimeout(function () {
        if (hint == null) {
            return;
        }
        hint.style.display = 'none';
    }, timeout); // Auto hide in 10s
    if (elementInput != null) {
        if (typeof xPos == 'undefined') {
            xPos = 0;
        }
        if (typeof pointerXPos == 'undefined') {
            pointerXPos = 10;
        }
        if (typeof yPos == 'undefined') {
            yPos = -45;
        }
        if (typeof (pointerYPos) == 'undefined') {
            pointerYPos = 38;
        }
        if (typeof (height) == 'undefined') {
            height = 18;
        }
        hint.style.height = height + 'px';
        hintPointer.style.left = (pointerXPos + 'px');
        hintPointer.style.top = (pointerYPos + 'px');
        var pos = Validators.utils.findPos(elementInput);
        hint.style.left = (pos[0] + xPos) + 'px';
        hint.style.top = (pos[1] + yPos) + 'px';
        elementInput.onchange = function () {
            hint.style.display = 'none';
        };
        elementInput.focus();
    };
    hint.style.display = 'block';
}
/*--Global variable*/
/*--End global variable declaration--*/


/*--Global function--*/
function AllowNumber(e) {
    var code = "";
    if (e.which)
        code = e.which;
    else if (e.keyCode)
        code = e.keyCode;
    if (code == 37 || code == 39)
        return true;
    if ((code < 48 || code > 57) && code != 8 && code != 13 && code != 0 && code != 46 && code != 9) {
        return false;
    }
    if (code == 13) {
       BtnOnClick();
    }
    return true;
}
function roundNumber(num, dec) {
    var result = Math.round(Math.round(num * Math.pow(10, dec + 1)) / Math.pow(10, 1)) / Math.pow(10, dec);
    return result;
}
function toString(obj) {
    if (obj == null)
        return "";
     else return obj.toString();
}
function obj(id) {
    if (document.getElementById(id))
        return document.getElementById(id);
    else return null
}
function gup(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null)
        return "";
    else
        return results[1];
}
var t1, t2 = null;
function clearMessage(containerid) {
    document.getElementById(containerid).innerHTML = "";
    clearTimeout(t1);
    clearTimeout(t2);
}
function clearSuccessMessages() {
    $("#err .msgcorrect").remove();
    clearTimeout(t2);
}
function showAlertMessage(containerid, msg, type) {
    // Success=0, Error=1, Warning=2
    if (document.getElementById(containerid)!=null)
    {
        switch (type) {
            case 0:
                document.getElementById(containerid).innerHTML = "<div class='alertboxscc'><div>" + msg + "</div></div>";
                var t = setTimeout("clearMessage('" + containerid + "')", 6000);
                break;
            case 1:
                document.getElementById(containerid).innerHTML = "<div class='alertboxerr'><div>" + msg + "</div></div>";
                break;
            case 2:
                document.getElementById(containerid).innerHTML = "<div class='alertboxwrn'><div>" + msg + "</div></div>";
                break;
            default:
                break
        }
        window.scroll(0, 0);
        parent.scroll(0, 0);
        
    }
}
function formatAlertMessage(msg, type) {
    // Success=0, Error=1, Warning=2
    switch (type) {
        case 0:
            return "<div class='msgcorrect'>" + msg + "</div>";
            break;
        case 1:
            return "<div class='msgerror'>" + msg + "</div>";
            break;
        case 2:
            return "<div class='msgwarning'>" + msg + "</div>";
            break;
        default:
            return "";
            break;
    }
}
function showEnrichMessage(containerid, ErrorHandler) {
    var isSuccess = true;
    var result = '';
    if (ErrorHandler != null) {
        if (ErrorHandler.length > 0) {
            $.each(ErrorHandler, function (index, value) {
                if (value.Key > 0) {
                    result += formatAlertMessage(value.Value, 1);
                    isSuccess = false;
                }
                else {
                    result += formatAlertMessage(value.Value, 0);
                }
            });
        }
        else {
            if (ErrorHandler.ErrorCode <= 0) {
                result = '<div class="boxscc">' + result + '</div>';
                t1 = setTimeout("clearMessage('" + containerid + "')", 8000);
            }
            else {
                result = '<div class="boxerr">' + ErrorHandler.ErrorMessage + '</div>';
                t2 = setTimeout("clearSuccessMessages()", 8000);
            }
        }
     }
          
     $("#".concat(containerid)).html(result);
     window.scroll(0, 0);
     parent.scroll(0, 0);
     return isSuccess;
}

/*--End Global Funtions--*/



//Create option for select control
////obj: jquery object 
function CreateOption(obj, curr, max, min, step, round) {
    obj.children().remove().end();
    var index = 0;
    var i = max;
    if (curr > max) obj.append('<option selected value="' + curr + '">' + curr + '</option>');
    while (i >= min) {
        obj.append('<option value="' + i + '">' + i + '</option>');
        if (i == curr) {
            obj.get(0).selectedIndex = index;
        }
        i = roundNumber(i - step, round);
        index++;
    }
    if (curr < min) obj.append('<option selected value="' + curr + '">' + curr + '</option>');
}

function AssignCommission() {
    jQuery.each($(".select-comm"), function() {
        if ($("#" + $(this).attr("id") + " option").size() <= 1) {
            var curr = $(this).val();
            var max = roundNumber($(this).attr("max"), 4);
            CreateOption($(this), curr, max, 0, 0.0005, 4);
        }
    });
}

function AssignPT() {
    jQuery.each($(".pt"), function() {
            if ($("#" + $(this).attr("id") + " option").size() <= 1) {
                var curr = $(this).val();
                var max = $(this).attr("max");
                var min = $(this).attr("min");
                CreateOption($(this), curr, max, min, 0.1, 2);
            }
    });
}

function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}

function getRoleName(id) {
    name = "";
    switch (parseInt(id)) {
        case 4:
        case -3:
            name = "Super";
            break;
        case 3:
        case -4:
            name = "Master";
            break;
        case 2:
        case -5:
            name = "Agent";
            break;
        case 1:
        case -1:
        case -2:
            name = "Member";
            break;
        default:
            name = "Member";
            break;
    }
    return name;
}

function PTBlockUI() {

    if (parent.BlockUI) {
        try { parent.BlockUI(); } catch (e) { }
    }
    //Called inside a frame set (#centerFr)
    else {
        if (parent.parent.BlockUI) {
            parent.parent.BlockUI();
        }
    }
    //Called from popup
    if (opener) {
        if (opener.BlockUI) {
            opener.BlockUI();
        }
        opener.parent.BlockUI();
    }
}
function PTUnBlockUI() {
    
    if (parent.jQuery && parent.jQuery.unblockUI) {
        parent.jQuery.unblockUI();
    }
    //Called inside a frame set (#centerFr)
    if (parent.parent.jQuery && parent.parent.jQuery.unblockUI) {
       
        parent.parent.jQuery.unblockUI();
    }
    //Called from popup

    if (opener) {
        if (opener.parent.jQuery.unblockUI) {
            opener.parent.jQuery.unblockUI();
        }
        //parent.jQuery.unblockUI();
    }
}
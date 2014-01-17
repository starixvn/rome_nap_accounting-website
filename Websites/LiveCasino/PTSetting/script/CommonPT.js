/*--Require jquery--*/
/*--Global variable*/
/*--End global variable declaration--*/

/*--Global function--*/
function AllowNumber(e) {
   
    var code;
    if (e.which)
        code = e.which;
    else if (e.keyCode)
        code = e.keyCode;
    if (code == 37 || code == 39 || (code>=48 && code<=57) || code==9 || code==13 || code==8)
        return true;
    return false;
}
function allowInt(event)
{ 
   var code=parseInt(event.which); 
   var isCtrl = false;
   if(event.ctrlKey) isCtrl = true;
   if((code == 99 || code == 118) && isCtrl) {
            return true;
        }
   if (code==0 || code == 37 || code == 39 || (code>=48 && code<=57) || code==9 || code==13 || code==8)
   {}
   else
   {
        event.preventDefault();
   }
}
function autoFormatNumber(number)
{
    var _number=$.number_format(parseInt(number), {precision: 0});
    return  _number=="0,00"? "0":_number;   
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

function isInt(value){
  if((parseFloat(value) == parseInt(value)) && !isNaN(value)){
      return true;
  } else {
      return false;
  }
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

function clearMessage(containerid) {
    
    $("#" + containerid).html('');
}

function showAlertMessage(containerid, msg, type) {
    // Success=0, Error=1, Warning=2
    if (document.getElementById(containerid)!=null)
    {
        switch (type) {
            case 0:
                $("#" + containerid).html("<div class='alertboxscc'><div>" + msg + "</div></div>");
                var t = setTimeout("clearMessage('" + containerid + "')", 6000);
                break;
            case 1:
                $("#" + containerid).html("<div class='alertboxerr'><div>" + msg + "</div></div>");
                break;
            case 2:
                 $("#" + containerid).html( "<div class='alertboxwrn'><div>" + msg + "</div></div>");
                break;
            default:
                break;
        }
        window.scroll(0, 0);
        parent.scroll(0, 0);
        
    }
}
/*--End Global Funtions--*/

function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
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
    }
}

function getPtValues(min, max, current) {
    //alert(min + ',' + max + ',' + current);
    if (max <= current)
        current = max;
    var step = 0.01;
    var arr = [];
    var isValid=false;
    for (var i = parseFloat(max); i >= roundNumber(parseFloat(min),2) ; i = roundNumber(i - step , 2)) {
        var item=roundNumber(i,2);
        if (item==current)
        {
            isValid=true;
        }
        arr.push(item);
    }
    if (!isValid)
    {
        arr.unshift(current);
    }
    return arr;
}

function getPtValuesEdited(min, max, current) {
    //alert(min + ',' + max + ',' + current);
//        if (max <= current)
//            current = max;
    var step = 0.01;
    var arr = [];
    var isValid = false;
    for (var i = parseFloat(max); i >= roundNumber(parseFloat(min), 2); i = roundNumber(i - step, 2)) {
        var item = roundNumber(i, 2);
        if (item == current) {
            isValid = true;
        }
        arr.push(item);
    }
    if (!isValid) {
        arr.unshift(current);
    }
    return arr;
}

function getCommissionValues(min, max, current) {
    var step = 0.0005;
    var arr = [];
    var isValid = false;
    for (var i = parseFloat(max); i >= roundNumber(parseFloat(min), 4); i = roundNumber(i - step, 4)) {
        var item = roundNumber(i, 4);
        if (item == current) {
            isValid = true;
        }
        arr.push(item);
    }
    if (!isValid) {
        arr.unshift(current);
    }
    return arr;
}

function bindingCommInfo (min, max, current) {
    var step = 0.01;
    var arr = [];
    
    for (var i = parseFloat(max); i >= roundNumber(parseFloat(min), 2); i = roundNumber(i - step, 2)) {
        var item = roundNumber(i, 2);
        if(current == item)
            arr.push('<option value="' + item + '" selected="selected">' + item + '</option>');
        else
            arr.push('<option value="' + item + '">' + item + '</option>');
    }
    return arr;
}

function getRoleName(id) {
    name = "";
    switch (parseInt(id)) {
        case 4:
            name = "Super";
            break;
        case 3:
            name = "Master";
            break;
        case 2:
            name = "Agent";
            break;
        case 1:
            name = "Member";
            break;
        default:
            name = "Member";
            break;
    }
    return name;
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
     var isError500=true;
     var isSuccess = true;
     var result = '';
     if (ErrorHandler != null && ErrorHandler.length > 0) {
         $.each(ErrorHandler, function (index, value) {
             if (value.Key > 0) {
                 if (isError500)
                 {
                    result += formatAlertMessage(value.Value, 1);
                 }
                 isSuccess = false;
             }
             else {
                 result += formatAlertMessage(value.Value, 0);
             }
             if (value.Key==500) isError500=false;
         });
     }
     
     if (isSuccess) {
         result = '<div class="boxscc">' + result + '</div>';
         t1 = setTimeout("clearMessage('" + containerid + "')", 8000);
     }
     else {
         result = '<div class="boxerr">' + result + '</div>';
         t2 = setTimeout("clearSuccessMessages()", 8000);
     }
     
     $("#".concat(containerid)).html(result);
  
     window.scroll(0, 0);
     parent.scroll(0, 0);
     return isSuccess;
}

function clearSuccessMessages() {
    
    $("#err .msgcorrect").remove();
    clearTimeout(t2);
}

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
Array.prototype.clean = function (deleteValue) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == deleteValue) {
            this.splice(i, 1);
            i--;
        }
    }
    return this;
};
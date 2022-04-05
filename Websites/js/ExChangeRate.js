function ResetValue() {
    $('.tbllayout input[name=txtEx]').val('');
    $("#tbxDescription").val('');
    $("#tbxMultipliedBy").val('1');
    //$("#selectStandardCurrency").val('');
    $("#tbxStandardCurrency").val('');
}

function InitFixedHeaderEx() {

    var widthmainpage = 700;
    if ($('#trFooter').length == 0) return;

    $('#trFooter').show();
    var topHeader = $('#trHeader').position().top;
    var topFooter = $('#trFooter').position().top;
    $('#trFooter').hide();

    var innerHeight = window.innerHeight;
    var delta = 0;
    if (!innerHeight) {
        innerHeight = document.documentElement.clientHeight;
        delta = 0;
    }

    var RealHeight = $(window).height() - topHeader - 20;

    if ((topFooter - topHeader) > $(window).height()) {
        jQuery('#table').Scrollable(RealHeight, widthmainpage);
    }
    else {
        $('#table .rspace').removeClass('rspace');
    }

}

function Validator() {

    document.getElementById('errMsg').innerHTML = '';
    var arrIDs = ['txtCurrency', 'txtBaseRate', 'txtMinbet', 'txtMinbetLive', 'tbxMultipliedBy'];
    var arrNames = ['Currency', 'Base Rate', 'Min Bet', 'Min Bet Live', 'MultipliedBy'];
    for (i = 0; i < arrIDs.length; i++) {
        if ($('#' + arrIDs[i]).val().length == 0) {
            showAlertMessage('errMsg', 'Please input ' + arrNames[i] + '.', 1);
            $('#' + arrIDs[i]).focus();
            return false;
        }
        else {

            if ($('#' + arrIDs[i]).val().toLowerCase().indexOf("e-") != -1)
                return true;
            if (!isFloat($('#' + arrIDs[i]).val()) && i > 0) {
                showAlertMessage('errMsg', 'Invalid data.', 1);
                $('#' + arrIDs[i]).focus();
                return false;
            }
        }
    }

    return true;
}


function InsertExChangeRate() {
    var f = Validator();
    if (f) {
        var param = 'type=insert';
        param += '&BaseRate=' + $('#txtBaseRate').val();
        param += '&Currency=' + $('#txtCurrency').val();
        param += '&Minbet=' + $('#txtMinbet').val();
        param += '&MinbetLive=' + $('#txtMinbetLive').val();
        param += '&Code=' + $('#tbxStandardCurrency').val();
        param += '&Description=' + $('#tbxDescription').val();
        param += '&MultipliedBy=' + $('#tbxMultipliedBy').val();
        param += '&OfferSite=' + $('#selOfferSite').val();

        try { parent.BlockUI(); } catch (e) { }

        $.ajax({
            type: 'POST',
            url: '../Handlers/UpdateExRate.ashx',
            async: true,
            data: param,
            success: function (rs) {
                if (rs.length > 0) {
                    if (rs.split('|')[0] == 'no')
                        showAlertMessage('errMsg', rs.split('|')[1], 1)
                    //document.getElementById('errMsg').innerHTML = rs.split('|')[1];
                    else {
                        parent.jQuery.unblockUI();
                        showAlertMessage('errMsg', rs.split('|')[1], 0);
                        afterUpdate(false, 0);
                        ReloadExchangeRateList();
                    }
                }
                InitFixedHeaderEx();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    }
}

function UpdateEditExChangeRate() {
    var f = Validator();
    if (f) {
        var param = 'type=update';
        param += '&exrateId=' + gup('exrateId');
        param += '&BaseRate=' + $('#txtBaseRate').val();
        param += '&Currency=' + $('#txtCurrency').val();
        param += '&Minbet=' + $('#txtMinbet').val();
        param += '&MinbetLive=' + $('#txtMinbetLive').val();
        param += '&Code=' + $('#tbxStandardCurrency').val();
        param += '&Description=' + $('#tbxDescription').val();
        param += '&MultipliedBy=' + $('#tbxMultipliedBy').val();
        param += '&OfferSite=' + $('#selOfferSite').val();
        param += '&IsActive=' + $('#ckbIsActive')[0].checked;

        try { parent.BlockUI(); } catch (e) { }

        $.ajax({
            type: 'POST',
            url: '../Handlers/UpdateExRate.ashx',
            async: true,
            data: param,
            success: function (rs) {
                if (rs.length > 0) {
                    if (rs.split('|')[0] == 'no') {
                        showAlertMessage("errMsg", rs.split('|')[1], 1);
                    }
                    else if (rs.split('|')[0] == '500') {
                        showAlertMessage("errMsg", rs.split('|')[1], 1);
                        afterUpdate(false, 0);
                    }
                    else {
                        showAlertMessage("errMsg", rs.split('|')[1], 0);
                        afterUpdate(false, 0);
                        ReloadExchangeRateList();
                    }
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            }
        });
    }
}

function UpdateExChangeRate(exrateId) {
    var url = 'ExRate/EditRate.aspx?exrateId=' + exrateId;

    parent.OpenPopup(url, "Edit Exchange Rate", 870, 220, true, true, window, 2000, null, null);
}

function CheckDecNumber_ExRate(e) {

    var code = "";
    if (e.which)
        code = e.which;
    else if (e.keyCode)
        code = e.keyCode;

    if (code == 69 || code == 101 || code == 37 || code == 39 || code == 36 || code == 35) //e or E or "-"
        return true;
    if ((code < 48 || code > 57) && code != 8 && code != 13 && code != 0 && code != 46 && code != 9)
        return false;
    return true;
}

var gup = function (name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null)
        return "";
    else
        return results[1];
};

function ResetValueWhenEdit() {
    try { parent.BlockUI(); } catch (e) { }
    var param = '';
    param = 'type=reset';
    param += '&exrateId=' + gup('exrateId');
    $.ajax({
        type: 'POST',
        url: '../Handlers/UpdateExRate.ashx',
        async: true,
        data: param,
        success: function (rs) {
            if (rs.length > 0) {
                var BaseRate = 0, Currency = 1, Minbet = 2, MinbetLive = 3, Description = 9, MultipliedBy = 10, Code = 11;
                var arr = rs.split('|');
                $('#txtBaseRate').val(arr[BaseRate]);
                $('#txtCurrency').val(arr[Currency]);
                $('#txtMinbet').val(arr[Minbet]);
                $('#txtMinbetLive').val(arr[MinbetLive]);
                $('#tbxDescription').val(arr[Description]);
                $('#tbxMultipliedBy').val(arr[MultipliedBy]);
                //$('#tbxStandardCurrency').val(arr[Code]);
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
            else {
                ResetValue();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }

        }
    });
}

function ResultInsertExrate(rs) {
    if (rs.responseText == "Success.") {
        DelayReloadPage1("ExchangeRate.aspx?result=1", 2000, "", "");
    }
    else {
        showAlertMessage("ErrMsg", rs.responseText, 1);
    }
}


window.onload = loadwindow;
function loadwindow() {
    if (gup("result") != null && gup("result") == "1") {
        showAlertMessage("ErrMsg", "The process to add new currency is not complete, please contact IT-Programmer for the further assistance. Thanks!", 2);
    }
}


function AddNewExChangeRate() {
    var url = 'ExRate/AddNewExRate.aspx';
    //showPopup('Add New Exchange Rate', url, 0, 0, 860, 220, true, 2000, null);
    parent.OpenPopup(url, "Add New Exchange Rate", 870, 220, true, true, window, 2000, null, null);
}

function EditBaseRate() {
    var url = 'ExRate/EditBaseRate.aspx';
    parent.OpenPopup(url, "Edit Base Rate", 400, 610, true, true, window, 2000, null, null);
}

function ResetEditBaseRate() {
    document.location.reload();
}

function ReloadExchangeRateList() {
    window.top.frames['content'].window.frames['centerFr'].location.reload()
}

function UpdateBaseRate() {
    document.getElementById('errMsg').innerHTML = '';
    document.getElementById('errMsg1').innerHTML = '';
    parent.BlockUI();
    var param = 'type=editbaserate';
    param += '&updateParams=';
    var $newBaseRateList = $('.new-base-rate').filter(function () {
        return $(this).val() !== undefined && $(this).val() !== '';
    });

    if ($newBaseRateList.length === 0) {
        showAlertMessage('errMsg', 'There is nothing to update.', 1);
        parent.jQuery.unblockUI();
        return;
    }

    var $newBaseRateIsZeroOrNegativeList = $('.new-base-rate').filter(function () {
        return parseFloat($(this).val()) <= 0;
    });

    if ($newBaseRateIsZeroOrNegativeList.length !== 0) {
        var listZeroBaseRateCurrencies = [];
        $newBaseRateIsZeroOrNegativeList.each(function () {
            listZeroBaseRateCurrencies.push($(this).attr('data-currency'));
        });
        showAlertMessage('errMsg', 'The following currencies must be greater than 0: ' + listZeroBaseRateCurrencies.join(', '), 1);
        parent.jQuery.unblockUI();
        return;
    }

    $newBaseRateList.each(function () {
        var $this = $(this);
        param += $this.attr('data-exrateId') +
            ',' + $this.val() +
            ',' + $this.attr('data-currency') +
            ',' + $this.attr('data-currency-code') +
            ',' + $this.attr('data-old-rate') +
            ',' + $this.attr('data-minbet') +
            ',' + $this.attr('data-minbetlive') + 
            ';';
    });

    $.ajax({
        type: 'POST',
        url: '../Handlers/UpdateExRate.ashx',
        async: true,
        data: param,
        success: function (rs) {
            if (rs.length > 0) {
                if (rs.split('|')[0] === 'no') {
                    showAlertMessage("errMsg", rs.split('|')[1], 1);
                }
                else if (rs.split('|')[0] === '2') // Success udpate MBC but failed sync to CS
                {
                    showAlertMessage("errMsg", "The base rates have been updated successfully.", 0);
                    showAlertMessage("errMsg1", rs.split('|')[1], 1);
                }
                else if (rs.split('|')[0] === '500') {
                    showAlertMessage("errMsg", rs.split('|')[1], 1);
                    afterUpdate(false, 0);
                }
                else {
                    showAlertMessage("errMsg", rs.split('|')[1], 0);
                    afterUpdate(false, 0);
                    ReloadExchangeRateList();
                    setTimeout(function () {
                        parent.ClosePopup();
                    }, 3000);
                }

                parent.jQuery.unblockUI();
            }
        }
    });
}
var _pageIndex = 1;
var _pageSize = 50;
var _totalItem = 0;
var _DeepLevel = 0;

function ResetValue() {
    _pageIndex = 1;
    _pageSize = 50;
    _totalItem = 0;
}

var CurrencyOption = '<option value="${currencyId}_${custId}">${currency}</option>';

var DepositRow = '<tr>\
                    {{if level == 1}}\
                        <td class="{{if index%2 == 0}} bgeven {{/if}}"><div class="col100 c">${ReportType}</div></td>\
                        <td class="{{if index%2 == 0}} bgeven {{/if}}"><div class="col80 c link" onclick="ResetValue();SelReportDepositBySite(2, ${currencyId}, \'${ReportType}\')">${currency}</div></td>\
                    {{else}}\
                        {{if level == 2 && _DeepLevel == 6}}\
                            <td class="{{if index%2 == 0}} bgeven {{/if}}"><div class="col100 c link" onclick="ResetValue();SelReportDepositBySite(3, \'${currencyId}\', \'${Username}\', false, ${Custid} )">${Username}</div></td>\
                            <td class="{{if index%2 == 0}} bgeven {{/if}}"><div class="col80 c ">${currency}</div></td>\
                        {{else}}\
                            <td class="{{if index%2 == 0}} bgeven {{/if}}"><div class="col100 c link" onclick="getBL(${Custid}, \'${Username}\');" >${ReportType}</div></td>\
                            <td class="{{if index%2 == 0}} bgeven {{/if}}"><div class="col80 c ">${currency}</div></td>\
                        {{/if}}\
                    {{/if}}\
                    <td class="{{if index%2 == 0}} bgeven {{/if}}"><div class="col80 r">${Turnover}</div></td>\
                    <td class="{{if index%2 == 0}} bgeven {{/if}}"><div class="col80 r">${GrossComm}</div></td>\
                    <td class="{{if index%2 == 0}} bgeven {{/if}}"><div class="col80 r">${Winlost}</div></td>\
                    <td class="{{if index%2 == 0}} bgeven {{/if}}"><div class="col80 r">${Comm}</div></td>\
                    <td class="{{if index%2 == 0}} bgeven {{/if}}"><div class="col80 r">${MTotal}</div></td>\
                    <td class="{{if index%2 == 0}} bgeven {{/if}}"><div class="col80 r">${AWinlost}</div></td>\
                    <td class="{{if index%2 == 0}} bgeven {{/if}}"><div class="col80 r">${AComm}</div></td>\
                    <td class="{{if index%2 == 0}} bgeven {{/if}}"><div class="col80 r">${ATotal}</div></td>\
                    <td class="{{if index%2 == 0}} bgeven {{/if}}"><div class="col80 r">${Company}</div></td>\
                </tr>';

var DepositTotalCurrency = '<tr>\
                    <td colspan="2" class="total"><div class="c b" style=""width:180px>TOTAL ${currency}</div></td>\
                    <td class="total"><div class="col80 r b">${Turnover}</div></td>\
                    <td class="total"><div class="col80 r b">${GrossComm}</div></td>\
                    <td class="total"><div class="col80 r b">${Winlost}</div></td>\
                    <td class="total"><div class="col80 r b">${Comm}</div></td>\
                    <td class="total"><div class="col80 r b">${MTotal}</div></td>\
                    <td class="total"><div class="col80 r b">${AWinlost}</div></td>\
                    <td class="total"><div class="col80 r b">${AComm}</div></td>\
                    <td class="total"><div class="col80 r b">${ATotal}</div></td>\
                    <td class="total"><div class="col80 r b">${Company}</div></td>\
                </tr>';

var DepositGrandTotalCurrency = '<tr>\
                    <td colspan="2" class="grandtotal"><div class="c b" style="width:185px">TOTAL ${currency}</div></td>\
                    <td class="grandtotal"><div class="col80 r b">${Turnover}</div></td>\
                    <td class="grandtotal"><div class="col80 r b">${GrossComm}</div></td>\
                    <td class="grandtotal"><div class="col80 r b">${Winlost}</div></td>\
                    <td class="grandtotal"><div class="col80 r b">${Comm}</div></td>\
                    <td class="grandtotal"><div class="col80 r b">${MTotal}</div></td>\
                    <td class="grandtotal"><div class="col80 r b">${AWinlost}</div></td>\
                    <td class="grandtotal"><div class="col80 r b">${AComm}</div></td>\
                    <td class="grandtotal"><div class="col80 r b">${ATotal}</div></td>\
                    <td class="grandtotal"><div class="col80 r b">${Company}</div></td>\
                </tr>';

var tmplNoinfo = '<tr><td colspan="11"> <div class="c" style="width:933px">There is no information available.</div></td></tr>';

var tmplCurrency = '<option value="0">Please Choose One !</option>\
                    <option value="-1">Base Currency-USD</option>\
                    <option value="-2">Base Currency-RM</option>';


function GetLicenseeList() {
    
    $.ajax({
        type: "POST",
        url: "../Service/ReportService.svc/GetLicenseeList",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: "",
        success: function (rs) {
            var dlSite = $('#dlSite');
            for (i = 0; i < rs.length; i++) {
                dlSite.append("<option value='" + rs[i].Name + "'>" + rs[i].Name + "</option>")
            }
            
        }
    });
}


function SelCurrency() {    
    var data = {
        site: $('#dlSite').val()
    };
    
    if ($('#dlSite').val() == '0')
        return;
    try { BlockUI(); } catch (e) { }
    $.ajax({
        type: "POST",
        url: "../Service/ReportService.svc/SelCurrency",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data),
        success: function (rs) {
            $('#dlCurrency').empty();
            $('#dlCurrency').append(tmplCurrency);
            $.tmpl(CurrencyOption, rs.currency).appendTo("#dlCurrency");

            jQuery.unblockUI();
        }
    });
}

function GetDateTime(vl) {
    if (vl != 8) {
        var data = {
            dateOption: vl
        };

        try { BlockUI(); } catch (e) { }

        $.ajax({
            type: "POST",
            url: "../Service/ReportService.svc/GetDatetimeOption",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                $('#fdate').val(rs.fdate);
                $('#tdate').val(rs.tdate);
                jQuery.unblockUI();
            }
        });
    }
}

function DoReset() {
    obj('dlDate').selectedIndex = 0;
    obj('dlSport').selectedIndex = 0;
    obj('dlCurrency').selectedIndex = 0;
    GetDateTime(0);
}

var m_level, m_currencyId, m_wldate, m_isCache, m_custid;
function SelReportDepositBySite(level, currencyId, wldate, isCache, _custid) {

    //alert(level);
       

    $('#level').val(level);
    $('#ErrMsg').html('');
    $('#currency').val(currencyId);

    isCache = (!isCache) ? false : isCache;

    var site = $('#dlSite').val();
    if (site == '0') {
        showAlertMessage("ErrMsg", "Please choose a site.", 1);
        return;
    }

    if ($('#dlDate').val() == '0')
        $('#dlDate').val(1);

     //Get ShowSB, ShowHR
     var showSB, showHR;
    switch ($('#dlSport').val())
    {
        case "0": //Sport Book
            showSB = true;
            showHR = false;
            break;
        case "151": //Horse Racing
            showSB = false;
            showHR = true;
            break;
        default:
            showSB = true;
            showHR = true;
            break;
    }

    //Get Report Type (Daily/Monthly/Sport)
    var reportType;
    reportType = $("input[name='RdReportBy']:checked").val();

    //Get Currency
    var custid, baseCurrency, currency = '';

    var dlCurrency = obj('dlCurrency');
    var count = dlCurrency.options.length;
    var arr;
    

    switch ($('#dlCurrency').val()) {
        case "0": //default
            baseCurrency = 0;
            custid = 0;
            break;
        case "-1":
            baseCurrency = 3; //USD
            custid = 0;
            break;
        case "-2":
            baseCurrency = 2; //RM
            custid = 0;
            break;
        default:
            baseCurrency = 0;
            custid = $('#dlCurrency').val().split('_')[1];
            currency = $('#dlCurrency').val().split('_')[0];
            break;
    }

    if (baseCurrency == 0) {
        for (var i = 0; i < count; i++) {
            arr = dlCurrency.options[i].value.split('_');
            if (currencyId == arr[0]) {
                dlCurrency.options[i].selected = true;
                currency = arr[0];
                custid = arr[1]
                break;
            }
        }
    }

    site = $('#dlSite').val();
    var fdate = $('#fdate').val();
    var tdate = $('#tdate').val();

    if (level == 2) {

        if(reportType != 1){
            wldate = $('#fdate').val();
        }

        fdate = wldate.toString();
        tdate = wldate.toString();
        $('#fdate').val(wldate);
        $('#tdate').val(wldate);
        $('#dlDate').val(8);
        $('#dvDate_UserName').html('UserName');
    }
    else if (level == 3)
        custid = _custid;
    else {
        if(reportType == 3)
            $('#dvDate_UserName').html('Sport');
        else
            $('#dvDate_UserName').html('Date');
    }

    $("#gridtitle").html("Deposit report for " + site + " from " + fdate + " to " + tdate);

    var data = {
        site: site,
        custid: custid,
        level: level,
        fdate: fdate,
        tdate: tdate,
        showSB: showSB,
        showHR: showHR,
        ReportType: reportType,
        basecurrency: baseCurrency,
        balanceUpday: $('#balanceUpday').val(),
        currency: currency,
        pageIndex: _pageIndex,
        pageSize: _pageSize,
        isCache: isCache
    };

    try { BlockUI(); } catch (e) { }
    var url = "../Service/ReportService.svc/SelReportDepositBySite";

    if (level == 2 || level == 3)
        url = "../Service/ReportService.svc/SelReportDepositDetailsBySite";

    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data),
        success: function (rs) {
            $('#mixparlaywon').show();
            var tbl = $("#tblmixparlay");
            var mixparlaywon = $("#mixparlaywon");
            tbl.empty();
            $('#tblFooter').empty();
            _totalItem = rs.total;
            _DeepLevel = rs.DeepLevel;
            if (rs.rows.length > 0) {
                $.tmpl(DepositRow, rs.rows).appendTo("#tblmixparlay");

                $.tmpl(DepositTotalCurrency, rs.totalcurrency).appendTo("#tblmixparlay");

                $.tmpl(DepositGrandTotalCurrency, rs.grandtotalcurrency).appendTo("#tblFooter");
            }
            else {

                $("#tblmixparlay").append(tmplNoinfo);
            }

            var gridwidth = tbl.width() + 17; //17:scrollbar width
            var headerscroll = mixparlaywon.find("div.header-scroll");
            var footerscroll = mixparlaywon.find("div.footer-scroll");
            var gridheight = 26 + headerscroll.height() + footerscroll.height() + 6 + 500 + 26; //26 + 4 + 587 + 26:tool box + border+div scroll height+ pagination
            mixparlaywon.css({ width: gridwidth + 'px', height: gridheight + 'px' });
            $('.maincontent_box').css('width', gridwidth - 3 + 'px');
            $('#ErrMsg').css('width', gridwidth + 2 + 'px');

            $('#tblFooter tr:first-child').css("border-bottom", "solid 1px #7C98A4");
            if (!isCache)
                initPager(_totalItem, 50, 0);
            jQuery.unblockUI();

            m_level = level;
            m_currencyId = currencyId;
            m_wldate = wldate;
            m_isCache = isCache;
            m_custid = _custid;
        }
    });
}

function initPager(total_item, items_per_page, current_page) {
    var opt = { callback: pageselectCallback };
    // Collect options from the text fields - the fields are named like their option counterparts
    opt["items_per_page"] = items_per_page;
    opt["current_page"] = current_page;
    $("#pagination").pagination(total_item, opt);
}

function pageselectCallback(page_index, _items_per_page, jq) {
    var max_page = Math.ceil(_totalItem / _items_per_page);
    page_index = page_index < max_page ? page_index : max_page - 1;

    _pageIndex = page_index + 1;
    _pageSize = _items_per_page;


    SelReportDepositBySite(m_level, m_currencyId, m_wldate, true, m_custid);
    
    initPager(_totalItem, _items_per_page, page_index);

    return false;
}

function getBL(custid, username) {
    var showSB = 1;
    var showHR = 1;
    var currencyOption = $('#dlCurrency').val();
    var site = $('#dlSite').val();
    if (site == '0') {
        showAlertMessage("ErrMsg", "Please choose a site.", 1);
        return;
    }

    //Get ShowSB, ShowHR
    var showSB, showHR;
    switch ($('#dlSport').val()) {
        case "0": //Sport Book
            showSB = true;
            showHR = false;
            break;
        case "151": //Horse Racing
            showSB = false;
            showHR = true;
            break;
        default:
            showSB = true;
            showHR = true;
            break;
    }

    var baseCurrency = $('#dlCurrency').val();
    if (currencyOption == '-1') //base usd
        baseCurrency = 'usd';
    else if (currencyOption == '-2') //base rm
        baseCurrency = 'rm';
    else
        baseCurrency = 'none';

    var url = '../../BetList/BetListDepositReport.aspx?custid=' + custid + '&username=' + username + '&fdate=' + $('#fdate').val() + '&tdate=' + $('#tdate').val() + '&showSB=' + showSB + '&showHR=' + showHR + '&baseCurrency=' + baseCurrency + '&site=' + site;

    var NewWindow = window.open(url, 'BetList', ' scrollbars=yes, toolbar=no, menubar=no,location=no,status=no,resizable=yes');
    if (NewWindow) NewWindow.focus();
}

function ExportToExcel() {
    try { BlockUI(); } catch (e) { }

    var reportType, baseCurrency;
    switch ($('#dlCurrency').val()) {
        case "0": //default
            baseCurrency = 0;
            break;
        case "-1":
            baseCurrency = 3; //USD
            break;
        case "-2":
            baseCurrency = 2; //RM
            break;
        default:
            baseCurrency = 0;
            break;
    }

    reportType = $("input[name='RdReportBy']:checked").val();
    $("#frm").attr('src', 'Deposit.aspx?IsExport=1&site=' + $('#dlSite').val() + '&level=' + $('#level').val() + '&currency=' + $('#currency').val() + '&baseCurrency=' + baseCurrency
        + '&fdate=' + $('#fdate').val() + '&tdate=' + $('#tdate').val() + '&reportType=' + reportType);

    jQuery.unblockUI();
}
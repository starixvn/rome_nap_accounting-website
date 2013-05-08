
$(document).ready(function() {

    $(document).bind('keypress', function(e) {
        var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
        if (key === 13) {
            e.preventDefault();
        }
    });

   


    //frezzeheader
    InitFixedHeader("#rptGrid", "#rptHeader", "#rptFooter");

    //hover on row
    $("tbody > tr.item").hover(function() {
        $(this).addClass("hovcolor");
    }, function() {
        $(this).removeClass("hovcolor");
    });


});


function SetupCalendar(inputField, trigger, selection, maxDate) {
    Calendar.setup
    ({
        animation: false,
        showTime: false,
        bottomBar: true,
        trigger: trigger,
        inputField: inputField,
        dateFormat: "%m/%d/%Y",
        maxDate: maxDate,
        selection: [selection],
        onSelect: function() {
            this.hide();            
        }
    });
}

//get number of date beetween fromdate and todate
function GetDayDistance(toDate, fromDate) {
    var days = Math.round((toDate - fromDate) / (1000 * 60 * 60 * 24));

    return days;        
}

function getQuerystring(key, defaultValue) {
    if (defaultValue == null)
        defaultValue = "";

    key = key.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regex = new RegExp("[\\?&]" + key + "=([^&#]*)");
    var qs = regex.exec(window.location.href);

    if (qs == null) {
        return defaultValue;
    }
    else {
        return qs[1];
    }
}

//get datetime from querystring
function getDateFromQueryString(key, defaultDate) {

    var date = getQuerystring(key, defaultDate);

    if (date.match("/^\d\d?\/\d\d?\/\d\d\d\d$/")) {
        return date;
    }
    else {
        if (defaultDate.toString().length == 0) {
            return GetFormatedDateStringNow("mm/dd/yyyy");
        }
        else {
            if (defaultDate.match("/^\d\d?\/\d\d?\/\d\d\d\d$/"))
                return defaultDate;
            else
                return GetFormatedDateStringNow("mm/dd/yyyy");
        }
    }
}

//get current client date and return string with input format
function GetFormatedDateStringNow(format) {
    var now = new Date();
    return now.format(format);
}

//User 7 digitals to [0, 1] to indicate
//what filter checkbox checked
//1111110 (number game is setted false)
//0111100
//1111101 (bingo is setted false)
//0111100
//1111100 (bingo & number game are setted false)
//1111111 (they are setted true)
//0111101 (bingo is not checked)
//0111110 (number game is not checked)
//0111100 (they are not checked)
function GetSearchOptionFromGUI() {
    var option = "";    
    var index = 1;
   
    var isBingo = $('#chkItems_4').is(':checked') ? 1 : 0;
    var isNumberGame = $('#chkItems_5').is(':checked') ? 1 : 0; 
        
    if ($("#chkAll").attr('checked')) {
        option += "11111";  //checkall        
    }
    else {
        option = "0"; //set check all is false        
        $("#chkItems input:checkbox").each(function() {
            
            if(index < 5){
                option += (this.checked) ? "1" : "0";               
            }
            index++;
        });
    }  
    option += isBingo;
    option += isNumberGame;  
        
    return option;
}

function CheckSelectAll() {
    var flag = true;
    $("#chkItems input:checkbox").each(function() {
        if (this.checked == false)
            flag = false;
    });
    $("#chkAll").attr('checked', flag);
}

function GenerateQueryParamsForDetailReport(page, uplineid, fromDate, toDate, option, isExport) {
    var url = page + "?fdate=" + fromDate + "&tdate=" + toDate + "&option=" + option + "&export=" + isExport
    if (uplineid > 0)
        url = url + "&uplineid=" + uplineid.toString();
    return url;
}


//view detail winloss report
function ViewReport(page, uplineid, fromDate, toDate) {
   
    var option = GetSearchOptionFromGUI();
    var url = GenerateQueryParamsForDetailReport(page, uplineid, fromDate, toDate, option, 0);
    parent.BlockUI();
    window.location.href = url;
}
//view daily winloss report
function ViewDailyWLReport(uplineid, date, reportLevel) {

    var url = GenerateQueryParamsForDailyReport("DailyWinLoss.aspx", uplineid, date, reportLevel);
    
    parent.BlockUI();
    window.location.href = url;
}

function GenerateQueryParamsForDailyReport(page, uplineid, date, reportLevel) {
    
    var url = page + "?fdate=" + date
    if (uplineid > 0)
        url = url + "&uplineid=" + uplineid.toString() + "&level=" + reportLevel;
    return url;
}

function ExportExcel() {
    parent.BlockUI();
}

//all id must be prefix by # (jquery style)
function InitFixedHeader(tableId, headerId, footerId) {

    try {
        if ($(headerId).length == 0) return;

        var ishide = $(footerId).is(':visible');

        if (!ishide) $(footerId).show();

        var topHeader = $(headerId).position().top;
        var topFooter = $(footerId).position().top;

        if (!ishide) $(footerId).hide();

        var innerHeight = window.innerHeight;
        var delta = 0;
        if (!innerHeight) {
            innerHeight = document.documentElement.clientHeight;
            delta = 0;
        }

        var realHeight = $(window).height() - topHeader;
        var gridWidth = $(tableId).width();

        if ((topFooter - topHeader) > $(window).height()) {

            jQuery(tableId).Scrollable(realHeight, gridWidth + 20);
        }
    } catch (err)
    { }
}

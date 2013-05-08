var msgBox;
var headerBar;
var currentTheme;
var fullviewer;
var authInfo;
var _monthNames = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
var DATE_FORMAT = "dd M yy";
var Core = Core || {};
var ajaxRequest;

var intervalID = window.setInterval("EveryFiveSeconds()", 5000);
var timerID = null;
var exportSuccess = "False";
var operationId = null;
var minHeight = 0;
var minGridHeight = 0;
var percentageContent = 0.71615; // 53%
var percentageGrid = 0.36458; // 53%

/// Scripts run on page load complete
$(document).ready(function () {
    setScreenResolution();
    Core.maxDate = new Date($("#maxDate").val());
    Core.minDate = new Date($("#minDate").val());
    Core.ajaxTimeout = 3600000;
    //   registerWeekPicker();
    // init common values and constants
    initCoreValues();
    authInfo.hide();
    logoSmall.show();
    welkomSmall.show();
    $.cookie('fullscreen', 'hide', { path: '/' });

    $.ajaxSetup({//Setup for export excel button
        type: "POST",
        enctype: 'multipart/form-data',
        timeout: Core.ajaxTimeout,
        beforeSend: Core.ajaxBeforeSend,
        success: Core.ajaxSuccess,
        error: Core.ajaxError,
        complete: Core.ajaxCompleted
    });


    //    fullviewer.show();

    fullviewer.click(function () {
        if ($(authInfo).is(':hidden')) {
            authInfo.show('fast');
            logoSmall.hide('fast');
            welkomSmall.hide('fast');
            $.cookie('fullscreen', 'show', { path: '/' });
        }
        else {
            authInfo.hide('fast');
            logoSmall.show('fast');
            welkomSmall.show('fast');
            $.cookie('fullscreen', 'hide', { path: '/' });
        }
    });

    if ($('.excel-download-link').length == 1) {
        $('.excel-download-link')[0].click();
    }

    InitPlaceholderProperty();

    currentTheme = $('#currentTheme').val();

    // re-position msgBox if it exists then display it (the msgBox won't be
    // displayed if it contains no inner text)
    if ($('#msgWrp').length > 0) {
        headerBar.append($('#msgWrp').html());
        $('#msgWrp').remove();
        msgBox = new MessageBox('#msg', '#msg-overlay');
        msgBox.showMsg();
    }

    if ($('.datepicker').length > 0) {
        var mindate = $('.datepicker').attr('mindate');
        var maxdate = $('.datepicker').attr('maxdate');
        $('.datepicker').datepicker({
            dateFormat: DATE_FORMAT,
            showOn: "button",
            firstDay: 1, // monday
            changeYear: true,
            changeMonth: true,
            buttonImageOnly: true,
            buttonText: "",
            buttonImage: $("#currentDomain").val() + "/App_Themes/" + currentTheme + "/Images/calendar.gif"
        });
        if (mindate != undefined && mindate != 'null') {
            mindate = mindate.split("/");
            $('.datepicker').datepicker("option", "minDate", new Date(mindate[0], mindate[1] - 1, mindate[2]));
        }
        if (maxdate != undefined && maxdate != 'null') {
            maxdate = maxdate.split("/");
            $('.datepicker').datepicker("option", "maxDate", new Date(maxdate[0], maxdate[1] - 1, maxdate[2]));
        }

    }
});

function InitPlaceholderProperty() {
    if (!Modernizr.input.placeholder) {
        //Assign to those input elements that have 'placeholder' attribute
        $('input[placeholder]').each(function () {

            var input = $(this);
            if ($(input).val() == input.attr('placeholder') || $.trim($(input).val()) == "") {
                $(input).val(input.attr('placeholder'));
                input.attr('style', 'color: silver');
            }

            $(input).focus(function () {
                if (input.val() == input.attr('placeholder')) {
                    input.val('');
                    input.attr('style', 'color: #000000')
                }
            });

            $(input).blur(function () {
                if (input.val() == '' || input.val() == input.attr('placeholder')) {
                    input.val(input.attr('placeholder'));
                    input.attr('style', 'color: silver')
                }
            });
        });
    }
}

function EveryFiveSeconds() {
     $.ajax({
        url: $("#currentDomain").val() + "/Common/Rechecking",
        beforeSend: function (jqXHR, settings) { },
        success: function (data, textStatus, jqXHR) {  },
        complete: function (jqXHR, textStatus) { }
    });
}

function initCoreValues() {
    tooltip = $('#tooltip');
    headerBar = $('#mainPage .header.showcase .content').eq(0);
    fullviewer = $('.body .header .fullviewer').eq(0);

    authInfo = $('#authInfo');
    logoSmall = $('#mainPage .menu a.logo').eq(0);
    welkomSmall = $('.nav.welkom').eq(0);
    fullviewer = $('.body .header .fullviewer').eq(0);
}

Core.registerWeekPicker = function () {
    // week picker plugin

    var configs = {};

    $.fn.weekpicker = function (options, func) {

        var $this = $(this);
        $this.css("margin", "2px");
        var $calendar = $this.nextAll("div:first");

        var id = $this.attr("id");
        if (!configs[id])
            configs[id] = { date: Core.maxDate };
        var defaults = configs[id];

        var displayYear = defaults.date.getFullYear();
        var selectCurrentWeek = function () {
            window.setTimeout(function () {
                $calendar.find('.ui-datepicker-current-day a').addClass('ui-state-active')
            }, 1);
        };

        var selectWeek = function (date, hideOnSelect) {

            if (!date) {
                date = $calendar.datepicker('getDate');
                if (!date)
                    date = Core.maxDate;
            }
            var dat = date.getDate();
            var day = date.getDay();

            defaults.startDate = new Date(date.getFullYear(), date.getMonth(), 1);
            if (day == 0)
                day = 7;
            defaults.startDate.setDate(dat - day + 1);
            defaults.endDate = new Date(date.getFullYear(), date.getMonth(), 1);
            defaults.endDate.setDate(dat - day + 7);

            if (date) {
                var week = date.getWeek();
                $this.val(week + " (" + defaults.startDate.format() + ")");
            }

            if (defaults.selectHandlers) {
                for (var i in defaults.selectHandlers)
                    defaults.selectHandlers[i](defaults.startDate, defaults.endDate, $this[0]);
            }

            $calendar.datepicker('setDate', defaults.startDate);
            if (hideOnSelect)
                $this.weekpicker("hide");

            selectCurrentWeek();

            displayYear = defaults.startDate.getFullYear();
        };

        if (typeof options == "string") {

            if (options == "hide") {
                $calendar.fadeOut("fast");
            }
            else if (options == "show") {
                if ($calendar.is(":visible"))
                    return;
                $calendar.show();
            }
            else if (options == "onSelect") {
                if (!defaults.selectHandlers)
                    defaults.selectHandlers = [];
                defaults.selectHandlers.push(func);
            }
            else if (options == "getDates") {
                return { start: defaults.startDate, end: defaults.endDate };
            }
            else if (options == "setDate") {
                selectWeek(func);
            }
        }
        else {

            $.extend(defaults, options);

            $this.addClass("datepicker hasDatepicker");
            $this.after("<div class='weekcalendar'></div><img style='position: relative;margin-top: 1px' class='ui-datepicker-trigger' src='" + $("#currentDomain").val() + "/App_Themes/" + currentTheme + "/Images/calendar.gif' alt='' title=''>");
            $calendar = $this.next();

            $calendar.css({ "position": "absolute", "z-index": 1000, "margin-top": "22px", "float": "left" });
            var ishover = false;
            $.extend(defaults, {
                dateFormat: DATE_FORMAT,
                showOn: null,
                buttonImageOnly: true,
                buttonText: "",
                buttonImage: $("#currentDomain").val() + "/App_Themes/" + currentTheme + "/Images/calendar.gif",
                firstDay: 1,
                showOtherMonths: true,
                selectOtherMonths: true,
                showWeek: true,
         //       minDate: new Date(2010,11,11),// defaults.minDate,
         //       maxDate: new Date(2012,11,11), //defaults.maxDate,
                changeYear: true,
                changeMonth: true,
                calculateWeek: function (date) {
                    var week = date.getWeek();
                    var dat = date.getDate();
                    var day = date.getDay();
                    if (day == 0)
                        day = 7;
                    var startD = new Date(date.getFullYear(), date.getMonth(), 1);
                    startD.setDate(dat - day + 1);

                    if (displayYear != startD.getFullYear()) {
                        var endD = new Date(date.getFullYear(), date.getMonth(), 1);
                        endD.setDate(dat - day + 7);
                        if (startD.getFullYear() != endD.getFullYear())
                            week = 1;
                    }

                    return week;
                },
                onSelect: function (dateText, inst) {
                    selectWeek(null, true);
                },
                beforeShowDay: function (date) {
                    var cssClass = '';
                    if (date >= defaults.startDate && date <= defaults.endDate)
                        cssClass = 'ui-datepicker-current-day';
                    return [true, cssClass];
                },
                onChangeMonthYear: function (year, month, inst) {
                    selectCurrentWeek();
                    displayYear = year;
                }
            });



            $calendar.datepicker(defaults);
           
      
            var show = function () {
                ishover = true;
                $this.weekpicker("show");
            }

            $calendar.hide();

            $this.width(144).mousedown(show);

            function enter() {
                ishover = true;
            }
            function leave() {
                ishover = false;
            }

            $calendar.mouseenter(enter).mouseleave(leave);
            $this.mouseenter(enter).mouseleave(leave);

            $("html").bind("click", function (e) {
                if (!ishover) {
                    $this.weekpicker("hide");
                }
            });
            $this.nextAll("img.ui-datepicker-trigger:first").click(function () {
                if ($calendar.is(":visible"))
                    $this.weekpicker("hide");
                else
                    show();
            });

            // set initial value    
            selectWeek(defaults.date);
        }
    }

    if ($('.weekpicker').length > 0) {
        $('.weekpicker').each(function () {
            $(this).weekpicker({
                date: new Date($(this).attr('data-value')),
                minDate: new Date($(this).attr('data-mindate')),
                maxDate: new Date($(this).attr('data-maxdate'))
            });
        });
    }
    $('.weekcalendar table.ui-datepicker-calendar tr').die().live({
        mousemove: function () { $(this).find('td a').addClass('ui-state-hover'); },
        mouseleave: function () { $(this).find('td a').removeClass('ui-state-hover'); }
    });
}
Date.prototype.format = function(){
    return this.getDate() + " " + _monthNames[this.getMonth()] + " " + this.getFullYear();
}

Date.prototype.getWeek = function () {
    // Copy date so don't modify original
    d = new Date(this);
    d.setHours(0, 0, 0);
    // Set to nearest Thursday: current date + 4 - current day number
    // Make Sunday's day number 7
    d.setDate(d.getDate() + 4 - (d.getDay() || 7));
    // Get first day of year
    var yearStart = new Date(d.getFullYear(), 0, 1);
    // Calculate full weeks to nearest Thursday
    var weekNo = Math.ceil((((d - yearStart) / 86400000) + 1) / 7) + 1;
    // Return array of year and week number
    return weekNo;
}

/// Message Box {
function MessageBox(msgBoxId, msgOverlay) {

    this.messageBox = $(msgBoxId);
    this.messageOverlay = $(msgOverlay);
    this.messageType = this.messageBox.attr('class');
    this.autohideSpeed = parseInt(this.messageBox.attr('autohide'));

    this.showInfoMsg = function (msg, autohide_speed, isModal, isShowCloseButton) {
        this.showMsg(msg, 'info', autohide_speed, isModal, isShowCloseButton);
    }

    this.showWarnMsg = function (msg, autohide_speed, isModal, isShowCloseButton) {
        this.showMsg(msg, 'warn', autohide_speed, isModal, isShowCloseButton);
    }

    this.showErrMsg = function (msg, autohide_speed, isModal, isShowCloseButton) {
        this.showMsg(msg, 'err', autohide_speed, isModal, isShowCloseButton);
    }

    this.showLoading = function () {
        this.showWarnMsg('Loading...', 0, true);
    }

    this.hideLoading = function () {
        this.hideMsg();
    }

    this.showMsg = function (msg, msgType, autohide_speed, isModal, isShowCloseButton) {
        if (msg != undefined)
            this.messageBox.html(msg);
        if (msgType != undefined)
            this.messageBox.attr('class', msgType);
        if (autohide_speed != undefined)
            this.autohideSpeed = autohide_speed;

        this.messageBox.hide();
        this.messageOverlay.hide();
        this.messageBox.stop(); // stop any current animation

        if (this.messageBox.html().length > 0) {

            if (isShowCloseButton)
                this.messageBox.html(this.messageBox.html() +
                    '<a class="close" href="javascript:msgBox.hideMsg();" title="Close Message"></a>');

            this.messageBox.css('left', (headerBar.width() / 2 - this.messageBox.width() / 2) + 'px');

            if (isModal) {
                this.messageOverlay.width($(document).eq(0).width());
                this.messageOverlay.height($(document).eq(0).height());
                this.messageOverlay.show();
            }

            this.messageBox.show();
        }

        if (this.autohideSpeed > 0)
            this.hideMsg(this.autohideSpeed);

        scrollToTop();
    }

    this.hideMsg = function (speed) {
        speed = speed > -1 ? speed : 0;
        this.messageBox.animate({ "opacity": "hidden" }, speed, function () {
            $('#msg-overlay').hide();
            $(this).fadeOut(100);
        });
    }
};

// Gets the page scrolled to top
function scrollToTop() {
    $('html, body').animate({ scrollTop: 0 }, 'slow');
}

function download(hostURL, listFileId) {
    var downloadFilePath = "http://" + hostURL + "/Report/DownloadFile?fileName=" + $('#' + listFileId).val();
    window.open(downloadFilePath, "_blank");
}

function convertDateToString(date) {
    var day = date.getDate();
    var month = date.getMonth() + 1;
    var year = date.getFullYear();

    var returnDate = "";
    if (day < 10)
        returnDate += "0" + day;
    else
        returnDate += day;

    if (month < 10)
        returnDate += "/0" + month + "/" + year;
    else
        returnDate += "/" + month + "/" + year;

    return returnDate;
}

function convertStringToDate(stringDate) {
    var temp = stringDate.split("/");
    if (temp.length != 3) {
        return stringDate + " is not valid format";
    }

    var day = temp[0];
    var month = temp[1];
    var year = temp[2];

    return new Date(Number(year), Number(month) - 1, Number(day));
}

function setDateTimeForModel(srcId, destId, isFullFormat) {
    var srcDate = $("#" + srcId).val();
    if (isFullFormat) {
        srcDate = srcDate.split(" ")[0];
        var temp1 = srcDate.split("/");
        srcDate = temp1[0].length > 1 ? temp1[0] : "0" + temp1[0];
        srcDate += "/" + (temp1[1].length > 1 ? temp1[1] : "0" + temp1[1]);
        srcDate += "/" + temp1[2];
    }
    var temp = srcDate.split("/");
    $("#" + destId).val(temp[1] + "/" + temp[0] + "/" + temp[2]);
}


// Date time constraints

Core.setMonthConstraint = function (monthId, yearId) {

    var $month = monthId ? $("#" + monthId) : $("#Month");
    var $year = yearId ? $("#" + yearId) : $("#Year");

    $year.change(setConstraint);

    setConstraint();
    function setConstraint() {
       
        $("option", $month).removeAttr("disabled");
        var year = $year.val();
        
        if (year == Core.maxDate.getFullYear()) {
            var month = Core.maxDate.getMonth() + 1;
            
            if ($month.val() > month)
                $month.val(month).change();
            $("option:gt(" + (month - 1) + ")", $month).attr("disabled", "disabled");
        }

        else if (year == Core.minDate.getFullYear()) {
            var month = Core.minDate.getMonth() + 1;
            if ($month.val() < month)
                $month.val(month).change();
            $("option:lt(" + (month - 1) + ")", $month).attr("disabled", "disabled");

        }
    }

}

Core.setWeekRangeConstraint = function (fromWeekId, toWeekId) {

    fromWeekId = fromWeekId || "Week";
    toWeekId = toWeekId || "ToWeek";
    var $from = $("#" + fromWeekId);
    var $to = $("#" + toWeekId);

    $from.weekpicker("onSelect", setConstraint);
    $to.weekpicker("onSelect", setConstraint);

    setConstraint();
    function setConstraint(start, end, element) {
        var from = $from.weekpicker("getDates");
        var to = $to.weekpicker("getDates");
        
        if (from.start > to.start) {
            if (element.id == fromWeekId) {
                $to.weekpicker("setDate", start);
            }
            else {
                $from.weekpicker("setDate", start);
            }
        }
    }
}

Core.setWeekRangeConstraintLimit2Months = function (fromWeekId, toWeekId) {

    fromWeekId = fromWeekId || "Week";
    toWeekId = toWeekId || "ToWeek";
    var $from = $("#" + fromWeekId);
    var $to = $("#" + toWeekId);

    $from.weekpicker("onSelect", setConstraint);
    $to.weekpicker("onSelect", setConstraint);

    setConstraint();
    function setConstraint(start, end, element) {
        if (start == undefined || end == undefined || element == undefined)
            return;

        var from = $from.weekpicker("getDates");
        var to = $to.weekpicker("getDates");
        var i = from.start.MonthSubtract(to.end);
        if (Math.abs(i) > 1 || from.start > to.start) {
            if (element.id == fromWeekId) {
                $to.weekpicker("setDate", start);
            }
            else {
                $from.weekpicker("setDate", start);
            }
        }
    }
}

Core.setWeekRangeConstraintLimit1Year = function (fromWeekId, toWeekId) {

    fromWeekId = fromWeekId || "Week";
    toWeekId = toWeekId || "ToWeek";
    var $from = $("#" + fromWeekId);
    var $to = $("#" + toWeekId);

    $from.weekpicker("onSelect", setConstraint);
    $to.weekpicker("onSelect", setConstraint);

    setConstraint();
    function setConstraint(start, end, element) {
        if (start == undefined || end == undefined || element == undefined)
            return;

        var from = $from.weekpicker("getDates");
        var to = $to.weekpicker("getDates");
        var i = from.start.MonthSubtract(to.end);
        if (Math.abs(i) > 12 || from.start > to.start) {
            if (element.id == fromWeekId) {
                $to.weekpicker("setDate", start);
            }
            else {
                $from.weekpicker("setDate", start);
            }
        }
    }
}

Core.setFromToDateConstraintLimit1Year = function (dateStartId, dateEndId, callback) {

    dateStartId = dateStartId || "DateStart";
    dateEndId = dateEndId || "DateEnd";
    var $dateStart = $("#" + dateStartId);
    var $dateEnd = $("#" + dateEndId);

    $dateStart.change(setConstraint);
    $dateEnd.change(setConstraint);

    function setConstraint() {

        var startdate = $dateStart.datepicker("getDate");
        var enddate = $dateEnd.datepicker("getDate");

        if (enddate < startdate) {

            if (this.id == dateStartId) {
                $dateEnd.datepicker("setDate", startdate);
            }
            else {

                $dateStart.datepicker("setDate", enddate);
            }
            //  return;
        }
        else if (enddate.getYear() != startdate.getYear()) {
            if (enddate.MonthSubtract(startdate) > 12) {

                if (this.id == dateStartId) {
                    $dateEnd.datepicker("setDate", startdate);
                }
                else {
                    $dateStart.datepicker("setDate", enddate);
                }
            }
        }

        if (callback)
            callback();
    }

}

// from/to date constraint
Core.setFromToDateConstraint = function (dateStartId, dateEndId, callback) {

    dateStartId = dateStartId || "DateStart";
    dateEndId = dateEndId || "DateEnd";
    var $dateStart = $("#" + dateStartId);
    var $dateEnd = $("#" + dateEndId);

    $dateStart.change(setConstraint);
    $dateEnd.change(setConstraint);

    function setConstraint() {

        var startdate = $dateStart.datepicker("getDate");
        var enddate = $dateEnd.datepicker("getDate");

        if (enddate < startdate) {

            if (this.id == dateStartId) {
                $dateEnd.datepicker("setDate", startdate);
            }
            else {

                $dateStart.datepicker("setDate", enddate);
            }
            //  return;
        }
        else if ((enddate.getYear() != startdate.getYear()) ||
                (enddate.getMonth() != startdate.getMonth())) {

            if (enddate.getMonth() - 1 != startdate.getMonth() || enddate.getYear() != startdate.getYear()) {

                if (this.id == dateStartId) {
                    var month = startdate.getMonth() + 1;
                    if (month > 11)
                        month = 0;

                    var end = new Date(startdate);
                    end.setMonth(end.getMonth() + 1);

                    if (end.getMonth() > month ||
                    (enddate.getMonth() != end.getMonth() || enddate.getYear() != end.getYear())) {

                        if (end.getMonth() > month)
                            end.setDate(0);
                        if (enddate - end > 0)
                            enddate = end;

                        $dateEnd.datepicker("setDate", enddate);
                    }
                }
                else {
                    var month = enddate.getMonth() - 1;
                    if (month < 0)
                        month = 12;

                    var start = new Date(enddate);
                    start.setMonth(start.getMonth() - 1);

                    if (start.getMonth() > month ||
                        (startdate.getMonth() != start.getMonth() || startdate.getYear() != start.getYear())) {
                        if (start.getMonth() > month)
                            start.setDate(0);
                        if (startdate - start < 0)
                            startdate = start;
                        $dateStart.datepicker("setDate", startdate);
                    }
                }
            }
        }

        if (callback)
            callback();
    }

}

// remove placeholder text of input controls in IE
Core.removePlaceholderText = function () {
    var ret = false;
    if (!Modernizr.input.placeholder) {
        $('input[placeholder]').each(function () {
            var $ctl = $(this);
            if ($ctl.val() == $ctl.attr("placeholder")) {
                $ctl.val("");
                ret = true;
            }
        });
    }
    return ret;
}

Core.addPlaceholderText = function () {
    if (!Modernizr.input.placeholder) {
        $('input[placeholder]').each(function () {
            var $ctl = $(this);
            if ($ctl.val() == "")
                $ctl.val($ctl.attr("placeholder"));
        });
    }
}

Core.doSubmit = function (submitFunction) {
    var valid = 1;

    $("input:text[placeholder]").each(function () {
        var $this = $(this);

        $this.val($.trim($this.val()));
        var ret = Core.validators.injection($this.val());
        if (ret.ErrCode != 0) {
            alert(ret.ErrMsg)
            $this.focus();
            valid = 0;
            return;
        }
    });
    if (!valid)
        return;

    Core.removePlaceholderText();

    $loading = $(".loading");
    if ($loading.length == 0) {
        if (submitFunction)
            submitFunction();
    } else {
        $(".loading").show();
        
        $(".loading").parent().focus();
        $('input:button').attr('disabled', true);
        if (submitFunction)
            submitFunction();
    }
    Core.addPlaceholderText();
    // $("#loading").hide();
}
Core.onAjaxCompleted = function (obj) {
    $(".loading").hide();
    $('input:button').attr('disabled', false);

    if (obj.status == 200 && obj.responseText != "") {
        window.location = obj.responseText;        
    }
}
Core.onAjaxFailure = function (data) {
    if (data.status == 401)
        window.location = data.responseText;
    else {
        var obj = $.parseJSON(data.responseText);
        if (!$.isEmptyObject(obj))
            alert(obj.errormsg);
    }
}

Core.validators = (function () {

    // return 0: email is invalid
    // return 1: email is valid
    function ValidateEmail(email) {
        var emailReg = /^([\w|\-|\.]+@([\w]+.)+[\w]{2,4})?$/;
        if (!emailReg.test($.trim(email)))
            return 0;
        return 1;
    }

    function ValidateIsRequired(value) {

        if (!value || value == 'undefined')
            return 0;
        if ($.trim(value) == '')
            return 0;
        return 1;
    }

    function ValidateHTMLAndSQLInjection(inputVal) {
        var message = { ErrCode: 0, ErrMsg: '' };

        if (/((\%3D)|(=)|(\%27)|(\')|(\-\-)|(\%3B)|(;))()/i.test(inputVal) === true) {
            message.ErrMsg = 'Please do not use SQL meta-characters.';
            message.ErrCode = 1
            return message;
        }

        //Cross site script attack
        if (/((\%3C)|<)*[a-z0-9\%]+((\%3E)|>)/i.test(inputVal) === true) {
            message.ErrMsg = 'Please do not use html syntax.';
            message.ErrCode = 2
            return message;
        }

        //SQL Injection 
        if (/\w*((\s)|(\%32))((\%6F)|o|(\%4F))((\%72)|r|(\%52))((\s)|(\%32))/i.test(inputVal) === true) {
            message.ErrMsg = 'Please do not use word "or".';
            message.ErrCode = 3
            return message;
        }

        // valid characters
        if (/[^\w\s\-\*\(\)\&\~\/\!\.\+\<\>\%\$\\]/.test(inputVal) === true) {
            message.ErrMsg = 'Please do not use special characters.';
            message.ErrCode = 4
            return message;
        }

        return message;
    }

    return {
        email: ValidateEmail,
        isRequired: ValidateIsRequired,
        injection: ValidateHTMLAndSQLInjection
    }
} ());

Core.ajaxBeforeSend = function (jqXHR, settings) {
    var valid = 1;
    Core.removePlaceholderText();
     $("input:text[placeholder]").each(function () {
        var $this = $(this);

        $this.val($.trim($this.val()));
        var ret = Core.validators.injection($this.val());
        if (ret.ErrCode != 0) {
            alert(ret.ErrMsg)
            $this.focus();
            valid = 0;
            return;
        }
    });
    

    if (!valid)
        ajaxRequest.abort();

    Core.removePlaceholderText();
    $(".loading").parent().focus();
    $(".loading").show();
    $('input:button').attr('disabled', true);
    Core.addPlaceholderText();
    //alert("test");
}
Core.ajaxSuccess = function (data) {
    window.location = data;
}
Core.ajaxError = function (xhr, ajaxOptions, thrownError) {
    if (xhr.status == 0) {
        alert('Not connect.\n Verify Network.');
    } else if (xhr.status == 404) {
        alert('Requested page not found. [404]');
    } else if (xhr.status == 500) {
        alert('Internal Server Error [500].');
    } else if (thrownError == 'parsererror') {
        alert('Requested JSON parse failed.');
    } else if (thrownError == 'timeout') {
        alert('Time out error.');
    } else if (thrownError == 'abort') {
        alert('Ajax request aborted.');
    } else {
        var obj = $.parseJSON(xhr.responseText);
        if (!$.isEmptyObject(obj))
            alert(obj.errormsg);
    }
}
Core.ajaxCompleted = function (xhr, textStatus) {
    $(".loading").hide();
    $('input:button').attr('disabled', false);
}

function GetFullDomainName()
{
    var protocol = window.location.protocol;
    var host = window.location.host;
    return protocol + "//" + host;
}

Date.prototype.MonthSubtract = function (d) {
    if (this.getFullYear() == d.getFullYear()) {
        return this.getMonth() - d.getMonth();
    } else  {
        return this.getMonth() - d.getMonth() + 12 * (this.getFullYear() - d.getFullYear());
    }

    return 1;
};

function checkURL(link) {
    //get timespan
    var timespan = 0;
    $.ajax({
        url: $("#currentDomain").val() + "/Common/GetExportExcelTimeOut",
        success: function (data, textStatus, jqXHR) {
            timespan = data / 5; //link is checked at every 5 seconds            
        },
        error: {},
        complete: {}
    });

    var currentDate = getCurrentFormatDate();
    var count = -1;
    exportSuccess = "False";
    if (timerID == null) { // to avoid multiple registration
        timerID = setInterval(function () {
            $.ajax({
                url: $("#currentDomain").val() + "/Common/IsExistURL",
                data: "link=" + link + "&clickDate=" + currentDate,
                success: function (data, textStatus, jqXHR) {
                    exportSuccess = data;
                    if (exportSuccess == "True") {
                        if (timerID != intervalID) {
                            clearInterval(timerID);
                            timerID = null;
                        }
                    }
                    else {
                        count = count + 1;
                        if (count >= timespan) {// cancel or email
                            if (timerID != intervalID) {
                                clearInterval(timerID);
                                timerID = null;
                            }

                            //wait for response of user 
                            $.ajax({
                                url: $("#currentDomain").val() + "/Common/Wait",
                                data: 'operationId=' + operationId,
                                success: function (data, textStatus, jqXHR) {
                                    $(".loading").hide();
                                    $('input:button').attr('disabled', false);
                                },
                                error: function (xhr, ajaxOptions, thrownError) {
                                    alert("Export Service is not started.");
                                    $(".loading").hide();
                                    $('input:button').attr('disabled', false);
                                },
                                complete: function (jqXHR, textStatus) {
                                    if (textStatus == "success") {
                                        var retVal = confirm("The server is processing (busy). Do you want to Email (Ok) or Cancel?");
                                        if (retVal == true) {//email                                                
                                            $.ajax({
                                                url: $("#currentDomain").val() + "/Common/Email",
                                                data: 'operationId=' + operationId,
                                                success: function (data, textStatus, jqXHR) {
                                                    $(".loading").hide();
                                                    $('input:button').attr('disabled', false);
                                                },
                                                error: {},
                                                complete: {}
                                            });
                                        }
                                        else {//cancel
                                            $.ajax({
                                                url: $("#currentDomain").val() + "/Common/Cancel",
                                                data: 'operationId=' + operationId,
                                                success: function (data, textStatus, jqXHR) {
                                                    $(".loading").hide();
                                                    $('input:button').attr('disabled', false);
                                                },
                                                error: {},
                                                complete: {}
                                            });
                                        }
                                    }
                                }
                            });
                        }
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                },
                complete: function (jqXHR, textStatus) {
                    if (exportSuccess == "True") {
                        window.location = link;
                        $(".loading").hide();
                        $('input:button').attr('disabled', false);
                    }
                }
            });
        }, 5000);
    }
    return exportSuccess;
};

function uuid() {
    var chars = '0123456789abcdef'.split('');

    var uuid = [], rnd = Math.random, r;
    uuid[8] = uuid[13] = uuid[18] = uuid[23] = '-';
    uuid[14] = '4'; // version 4

    for (var i = 0; i < 36; i++) {
        if (!uuid[i]) {
            r = 0 | rnd() * 16;

            uuid[i] = chars[(i == 19) ? (r & 0x3) | 0x8 : r & 0xf];
        }
    }

    return uuid.join('');
};

function pause(millis) {
    var date = new Date();
    var curDate = null;
    do { curDate = new Date(); }
    while (curDate - date < millis)
};

//function UrlExists(url) {
//    var http = new XMLHttpRequest();
//    http.open('HEAD', url, false);
//    http.send();
//    alert(http.status);
//    return http.status != 404;
//};

function getCurrentFormatDate() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();
    var hours = today.getHours();
    var minutes = today.getMinutes();
    var seconds = today.getSeconds();
    if (dd < 10) { dd = '0' + dd }
    if (mm < 10) { mm = '0' + mm }
    d = mm + '/' + dd + '/' + yyyy + ' ' + hours + ':' + minutes + ':' + seconds

    return d;
}

function setScreenResolution() {

//    minHeight = screen.height * percentageContent;
//    minGridHeight = screen.height * percentageGrid;
    if (screen.width == 1024 && screen.height == 768) {
        minHeight = 550;
        minGridHeight = 280;
    } 
    else if (screen.width == 1152 && screen.height == 864) {
        minHeight = 650;
        minGridHeight = 380;
    }
    else if (screen.width == 1280 && screen.height == 1024) {
        minHeight = 800;
        minGridHeight = 550;
    }
    else {
        minHeight = 550;
        minGridHeight = 280;
    }

    $('#mainPage .body > .content').css('min-height', minHeight + 'px');
}
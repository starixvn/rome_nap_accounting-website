var searchCopyController = angular.module('SearchCopy', [searchCopyService, searchCopyDirective, searchCopyFilter]);

// A Controller for your app
searchCopyController.controller('SearchCopyController', ['$scope', 'message', 'validate', function ($scope, message, validate) {
    /* define variables*/
    var user = {
        userid: -1,
        username: '',
        password: '',
        firstname: '',
        lastname: '',
        phone: '',
        mobilephone: '',
        fax: '',
        maxCredit: '',
        currencylist: [
            { currency: '' }
        ],
        internaluser: false
    };

    $scope.existedUser = { userid: -1, username: '', selectedSite: {}, sites: [], selectedCurrencyId: 0, selectedCurrency: '', maxCredit: '' };
    $scope.newUser = {};

    $scope.sports = [
            { sportname: '-- choose sport --', sportvalue: 'choosesport' },
            { sportname: 'Sportsbook', sportvalue: 'tabSportsbook' },
            { sportname: 'Sportsbook Min PT', sportvalue: 'tabSportsbookMinPT' },
            { sportname: 'Racing', sportvalue: 'tabRacing' },
            { sportname: 'Racing Min PT', sportvalue: 'tabRacingMinPT' },
            { sportname: 'Casino', sportvalue: 'tabCasino' },
            { sportname: 'Virtual Sports', sportvalue: 'tabVirtualSports' },
            { sportname: 'Bingo', sportvalue: 'tabBingo' },
            { sportname: 'Live Casino', sportvalue: 'tabLiveCasino' }
    ];
    $scope.selectedSport = $scope.sports[0];

    /* initialization */
    $scope.init = function () {
        $scope.newUser = angular.copy(user);
        $("#step2").hide();
        $scope.addEvents();
        $("#maxCredit").ForceDecimalOnly().ForceCurrencyOnly();
    };

    $scope.resize = function (frameId) {
        var windowHeight = $(window).height();
        var height = windowHeight - 100;
        $(frameId).height(height + "px");
    };

    /* define functions*/
    $scope.addEvents = function () {
        $("#existedUserName").focus(function () {
            message.hideAlertMessage("errorMsg");
            $("#checkExisted").hide();
        });

        $("#existedUserName").focusout(function () {
            $("#checkExisted").show();
            $(".tabsports div").show();
            $scope.searchExistingUser(this.value);
        });
    };

    $scope.step1Next = function () {
        if ($scope.existedUser.username == undefined || $scope.existedUser.username == "") {
            validate.setRequired("#existedUserName");
            message.showAlertMessage("errorMsg", "Please Enter User Name.", 1);
            $("#errorMsg").width("550");
        }
        else {
            if ($scope.existedUser.userid != "-1") {
                message.hideAlertMessage("errorMsg");

                $("#step1").hide();
                $("#step2").show();
                $(".settingInfo div").hide();
                $("#divGeneralInformation").show();

                $("#step2 .tab-active").attr("class", "tab-inactive");
                $("#tabGenealInformation").attr("class", "tab-active");

                $("#spanstep1").removeClass("step1-active");
                $("#spanstep1").addClass("step1-inactive");

                $("#spanstep2").removeClass("step2-inactive");
                $("#spanstep2").addClass("step2-active");

                $scope.selectedSport = $scope.sports[0];
                $scope.resetFrameSource();
            }
        }
    };

    $scope.step2Back = function () {
        message.hideAlertMessage("errorMsg");

        $("#step2").hide();
        $("#step1").show();

        $("#spanstep1").removeClass("step1-inactive");
        $("#spanstep1").addClass("step1-active");

        $("#spanstep2").removeClass("step2-active");
        $("#spanstep2").addClass("step2-inactive");
    };

    $scope.step2Submit = function () {
        if ($scope.validateData() == true) {
            $scope.copyExistingUser();
        }
    };

    $scope.validateData = function () {
        if ($scope.newUser.username == undefined || $scope.newUser.username == "") {
            message.showAlertMessage("errorMsg", "The username is required !", 1);
            $("#errorMsg").width("700");
            validate.setRequired("#username");
            return false;
        }
        if ($scope.newUser.password == undefined || $scope.newUser.password == "") {
            message.showAlertMessage("errorMsg", "The password is required !", 1);
            $("#errorMsg").width("700");
            validate.setRequired("#password");
            return false;
        }

        return true;
    };

    $scope.resetFrameSource = function () {
        $("#frmSportbook").attr('src', "");
        $("#frmSportbookMinPT").attr('src', "");
        $("#frmRacing").attr('src', "");
        $("#frmRacingMinPT").attr('src', "");
        $("#frmCasino").attr('src', "");
        $("#frmVirtualSports").attr('src', "");
        $("#frmBingo").attr('src', "");
        $("#frmLiveCasino").attr('src', "");
    };

    $scope.showSetting = function (tabId) {
        if (tabId == "choosesport") {
            return;
        }

        $(".sport").hide();
        $(".settingInfo div").hide();
        $("#step2 .tab-active").attr("class", "tab-inactive");

        if (tabId == undefined || tabId == "" || tabId == "tabSports") {
            tabId = $scope.selectedSport.sportvalue;
            $("#tabSports").attr("class", "tab-active");
        }
        else {
            $('#' + tabId).attr("class", "tab-active");
        }

        var url = $("#currentURL").val();
        switch (tabId) {
            case 'tabSportsbook':
                url += "/EditSuper/EditSuperPage?custId=" + $scope.existedUser.userid + "&uname=" + $scope.existedUser.username;
                $scope.loadData("#frmSportbook", url);
                $scope.resize("#frmSportbook");
                break;
            case 'tabSportsbookMinPT': url += "/EditSuperMinPt/EditSuperMinPtPage?&custId=" + $scope.existedUser.userid + "&uname=" + $scope.existedUser.username;
                $scope.loadData("#frmSportbookMinPT", url);
                $scope.resize("#frmSportbookMinPT");
                break;
            case 'tabRacing': url += "/EditSuperPTRacing/Index?custid=" + $scope.existedUser.userid;
                $scope.loadData("#frmRacing", url);
                $scope.resize("#frmRacing");
                break;
            case 'tabRacingMinPT': url += "/customer/pt/racing/SuperMinUpdate.aspx?custId=" + $scope.existedUser.userid;
                $scope.loadData("#frmRacingMinPT", url);
                $scope.resize("#frmRacingMinPT");
                break;
            case 'tabCasino': url += "/Customer/PositionTakingCS.aspx?custId=" + $scope.existedUser.userid;
                $scope.loadData("#frmCasino", url);
                $scope.resize("#frmCasino");
                break;
            case 'tabVirtualSports': url += "/VirtualSportSuperPT/EditSuper?custId=" + $scope.existedUser.userid + "&uname=" + $scope.existedUser.username + "&roleId=4";
                $scope.loadData("#frmVirtualSports", url);
                $scope.resize("#frmVirtualSports");
                break;
            case 'tabBingo': url += "/Customer/PT/Bingo/SuperUpdate.aspx?custId=" + $scope.existedUser.userid;
                $scope.loadData("#frmBingo", url);
                $scope.resize("#frmBingo");
                break;
            case 'tabLiveCasino': url += "/LiveCasino/PTSetting/EditSuper.aspx?custId=" + $scope.existedUser.userid + "&exchangeId=20&roleId=4";
                $scope.loadData("#frmLiveCasino", url);
                $scope.resize("#frmLiveCasino");
                break;
            case 'tabGenealInformation': $("#divGeneralInformation").show();
        }

        $("#divSport").show();
        $("#divSport div").show();
    };

    $scope.loadData = function (frameId, url) {
        if ($(frameId).attr('src') == "") {
            $(frameId).attr('src', url);
        }
        $(frameId).show();
    };

    $scope.disableSports = function (statusSports) {
        if (statusSports.HideRacing == true) {
            $("#listSports option[value='3']").remove();
            $("#listSports option[value='4']").remove();
        }
        if (statusSports.HideCasino == true) {
            $("#listSports option[value='5']").remove();
        }
        if (statusSports.HideVirtualSports == true) {
            $("#listSports option[value='6']").remove();
        }
        if (statusSports.HideBingo == true) {
            $("#listSports option[value='7']").remove();
        }
        if (statusSports.HideLiveCasino == true) {
            $("#listSports option[value='8']").remove();
        }
    };

    $scope.searchExistingUser = function (username) {
        if (username == "") {
            $("#checkExisted").removeClass('success');
            $("#checkExisted").removeClass('not_success');
            return;
        }

        $scope.existedUser.userid = -1;

        var param = {
            username: username
        };

        try { parent.parent.BlockUI(); } catch (e) { }
        try { parent.BlockUI(); } catch (e) { }
        $("body").addClass("wait");

        $.ajax({
            type: "POST",
            url: toUrl("Super/SearchExistingSuper"),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (customerSettingView) {
                try { parent.parent.jQuery.unblockUI(); } catch (e) { }
                try { parent.jQuery.unblockUI(); } catch (e) { }
                $('body').removeClass('wait');

                var custid = customerSettingView.UserId;
                var statusSports = customerSettingView.StatusSports;

                $scope.existedUser.userid = custid;
                if (custid != "-1") {
                    //step1
                    message.hideAlertMessage("errorMsg");
                    $("#checkExisted").removeClass('not_success');
                    $("#checkExisted").addClass('success');

                    //step2                    
                    $scope.disableSports(statusSports);
                    $scope.getSiteCreditAndCurrencyByUserId(custid);
                }
                else {
                    $("#checkExisted").removeClass('success');
                    $("#checkExisted").addClass('not_success');
                    message.showAlertMessage("errorMsg", "The super doesn’t exist !", 1);
                    $("#errorMsg").width("550");
                }
            }
            , error: function (xhr, ajaxOptions, thrownError) {
                message.showAlertMessage("errorMsg", "Error for searching existing user !", 1);
                $("#errorMsg").width("550");
            }
        });
    };

    $scope.getSiteCreditAndCurrencyByUserId = function (custid) {
        var param = {
            custid: custid
        };

        $.ajax({
            type: "POST",
            url: toUrl("Super/GetSiteCreditAndCurrencyByUserId"),
            contentType: "application/json; charset=utf-8",
            dataType: 'html',
            data: JSON.stringify(param),
            success: function (result) {
                message.hideAlertMessage("errorMsg");
                var site_currency = result.split(',');
                $scope.existedUser.selectedSite.SiteName = site_currency[0];
                $scope.existedUser.maxCredit = numeral(site_currency[1]).format("0,0");
                $scope.existedUser.selectedCurrencyId = site_currency[2];
                $scope.existedUser.selectedCurrency = site_currency[3];
                $scope.getAllSites();
            }
                , error: function (xhr, ajaxOptions, thrownError) {
                    message.showAlertMessage("errorMsg", "Error for getting site by user id !", 1);
                    $("#errorMsg").width("700");
                }
        });
    };

    $scope.getAllSites = function () {
        $.ajax({
            type: "POST",
            url: toUrl("Super/GetSites"),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: null,
            success: function (data) {
                message.hideAlertMessage("errorMsg");

                //get selected index.
                var index = 0;
                while (index < data.length && $scope.existedUser.selectedSite.SiteName != data[index].SiteName) {
                    index++;
                }

                $scope.$apply(function () {
                    $scope.existedUser.sites = data;
                    $scope.existedUser.selectedSite = $scope.existedUser.sites[index];
                });
            }
            , error: function (xhr, ajaxOptions, thrownError) {
                message.showAlertMessage("errorMsg", "Error for getting sites of existing user !", 1);
                $("#errorMsg").width("700");
            }
        });
    };

    $scope.copyExistingUser = function () {
        var param = {
            UserName: $scope.newUser.username,
            Userpwd: $scope.newUser.password,
            FirstName: $scope.newUser.firstname,
            LastName: $scope.newUser.lastname,
            Phone: $scope.newUser.phone,
            MobilePhone: $scope.newUser.mobilephone,
            Site: $scope.existedUser.selectedSite.SiteName,
            Fax: $scope.newUser.fax,
            Credit: $scope.existedUser.maxCredit.replace(/,/g, ''),
            IsInternal: $scope.newUser.internaluser,
            CurrencyId: parseInt($scope.existedUser.selectedCurrencyId),
            ExistingCustId: $scope.existedUser.userid,
            Creator: $("#currentUserId").val()
        };

        message.hideAlertMessage("errorMsg");

        try { parent.parent.BlockUI(); } catch (e) { }
        try { parent.BlockUI(); } catch (e) { }
        $("body").addClass("wait");

        $.ajax({
            type: "POST",
            url: toUrl("Super/CopyExistingSuper"),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (result) {

                try { parent.parent.jQuery.unblockUI(); } catch (e) { }
                try { parent.jQuery.unblockUI(); } catch (e) { }
                $('body').removeClass('wait');

                if (result != null) {
                    message.showAlertMessage("errorMsg", result.Message, 3);
                    $("#errorMsg").width("700");
                }
                else {
                    message.showAlertMessage("errorMsg", "Error for copying existing user !", 1);
                    $("#errorMsg").width("700");
                }              
            }
            , error: function (xhr, ajaxOptions, thrownError) {
                message.showAlertMessage("errorMsg", "Error for copying existing user !", 1);
                $("#errorMsg").width("700");
            }
        });
    };

    /* start */
    $scope.init();

} ]);
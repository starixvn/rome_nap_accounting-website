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

    $scope.existedUser = { userid: -1, username: '', selectedSite: {}, sites: [], selectedCurrency: '', maxCredit: '' };
    $scope.newUser = {};

    /* initialization */
    $scope.init = function () {
        $scope.newUser = angular.copy(user);
        $("#step2").hide();
        $scope.addEvents();
        $("#maxCredit").ForceDecimalOnly().ForceCurrencyOnly();
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

        $("#panelStep1").click(function () {
            $scope.step2Back();
        });

        $("#panelStep2").click(function () {
            $scope.step1Next();
        });
    };

    $scope.step1Next = function () {
        if ($scope.existedUser.username == undefined || $scope.existedUser.username == "") {
            validate.setRequired("#existedUserName");
            message.showAlertMessage("errorMsg", "Please Enter User Name.", 1);
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
            }
            else {
                message.showAlertMessage("errorMsg", "The super doesn’t exist!", 1);
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
            validate.setRequired("#username");
            return false;
        }
        if ($scope.newUser.password == undefined || $scope.newUser.password == "") {
            message.showAlertMessage("errorMsg", "The password is required !", 1);
            validate.setRequired("#password");
            return false;
        }

        return true;
    };

    $scope.showSetting = function (settingInfoId, tabId) {
        if (tabId == "tabGenealInformation") {
            return;
        }

        $(".sport").hide();
        $(".settingInfo div").hide();
        $("#step2 .tab-active").attr("class", "tab-inactive");
        $('#' + tabId).attr("class", "tab-active");

        var url = $("#currentURL").val();
        switch (tabId) {
            case 'tabSportsbook':
                url += "/EditSuper/EditSuperPage?custId=" + $scope.existedUser.userid + "&uname=" + $scope.existedUser.username;
                $scope.loadData("#frmSportbook", url);
                break;
            case 'tabSportsbookMinPT': url += "/EditSuperMinPt/EditSuperMinPtPage?&custId=" + $scope.existedUser.userid + "&uname=" + $scope.existedUser.username;
                $scope.loadData("#frmSportbookMinPT", url);
                break;
            case 'tabRacing': url += "/EditSuperPTRacing/Index?custid=" + $scope.existedUser.userid;
                $scope.loadData("#frmRacing", url);
                break;
            case 'tabRacingMinPT': url += "/customer/pt/racing/SuperMinUpdate.aspx?custId=" + $scope.existedUser.userid;
                $scope.loadData("#frmRacingMinPT", url);
                break;
            case 'tabCasino': url += "/Customer/PositionTakingCS.aspx?custId=" + $scope.existedUser.userid;
                $scope.loadData("#frmCasino", url);
                break;
            case 'tabVirtualSports': url += "/VirtualSportSuperPT/EditSuper?custId=" + $scope.existedUser.userid + "&uname=" + $scope.existedUser.username + "&roleId=4";
                $scope.loadData("#frmVirtualSports", url);
                break;
            case 'tabBingo': url += "/Customer/PT/Bingo/SuperUpdate.aspx?custId=" + $scope.existedUser.userid;
                $scope.loadData("#frmBingo", url);
                break;
            case 'tabLiveCasino': url += "/LiveCasino/PTSetting/EditSuper.aspx?custId=" + $scope.existedUser.userid + "&exchangeId=20&roleId=4";
                $scope.loadData("#frmLiveCasino", url); break;
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
        if (statusSports.DisableRacing == true) {
            $("#tabRacing").hide();
            $("#tabRacingMinPT").hide();
        }
        if (statusSports.DisableCasino == true) {
            $("#tabCasino").hide();
        }
        if (statusSports.DisableVirtualSports == true) {
            $("#tabVirtualSports").hide();
        }
        if (statusSports.DisableBingo == true) {
            $("#tabBingo").hide();
        }
        if (statusSports.DisableLiveCasino == true) {
            $("#tabLiveCasino").hide();
        }
    };

    $scope.searchExistingUser = function (username) {
        if (username == "") {
            $("#checkExisted").removeClass('success');
            $("#checkExisted").removeClass('not_success');
            return;
        }

        var param = {
            username: username
        };

        $.ajax({
            type: "POST",
            url: toUrl("Super/SearchExistingSuper"),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (customerSettingView) {
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
                }
            }
            , error: function (xhr, ajaxOptions, thrownError) {
                message.showAlertMessage("errorMsg", "Error for searching existing user !", 1);
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
                $scope.existedUser.selectedCurrency = site_currency[2];
                $scope.getAllSites();
            }
                , error: function (xhr, ajaxOptions, thrownError) {
                    message.showAlertMessage("errorMsg", "Error for getting site by user id !", 1);
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
            }
        });
    };

    $scope.copyExistingUser = function () {
        var param = {
            username: $scope.newUser.username,
            Userpwd: $scope.newUser.password,
            FirstName: $scope.newUser.firstname,
            LastName: $scope.newUser.lastname,
            Phone: $scope.newUser.phone,
            MobilePhone: $scope.newUser.mobilephone,
            Site: $scope.existedUser.selectedSite.SiteName,
            Fax: $scope.newUser.fax,
            Credit: $scope.existedUser.maxCredit.replace(/,/g, ''),
            IsInternal: $scope.newUser.internaluser,
            ExistingCustId: $scope.existedUser.userid,
            Creator: $("#currentUserId").val()
        };

        $.ajax({
            type: "POST",
            url: toUrl("Super/CopyExistingSuper"),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (result) {
                var success = result != null && result.IsSuccessful == true;
                if (success) {
                    message.hideAlertMessage("errorMsg");
                    message.showAlertMessage("errorMsg", "Created Super Successfully !", 0);
                } else {
                    if (result != null) {
                        message.showAlertMessage("errorMsg", result.Message, 1);
                    }
                    else {
                        message.showAlertMessage("errorMsg", "Created Super Un-successfully !", 1);
                    }
                }
            }
            , error: function (xhr, ajaxOptions, thrownError) {
                message.showAlertMessage("errorMsg", "Error for copying existing user !", 1);
            }
        });
    };

    /* start */
    $scope.init();

} ]);
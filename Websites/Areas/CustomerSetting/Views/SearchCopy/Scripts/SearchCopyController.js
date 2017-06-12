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

    $scope.existedUser = { userid: -1, username: '', selectedSite: {}, sites: [], selectedCurrencyId: 0, selectedCurrency: '', maxCredit: '', IsSyncCasino: 0, IsSyncBingo: 0, IsSyncLiveCasino: 0 };
    $scope.newUser = {};
    $scope.siteName = '';

    $scope.sports = parent.sports;
    $scope.selectedSport = $scope.sports[0];

    /* initialization */
    $scope.init = function () {
        $scope.newUser = angular.copy(user);
        $("#step2").hide();
        $scope.addEvents();
        //$("#maxCredit").ForceDecimalOnly().ForceCurrencyOnly();        
    };

    $scope.resize = function (frameId) {
        var windowHeight = $(window).height();
        var height = windowHeight - 115;
        $(frameId).height(height + "px");
    };

    /* define functions*/
    $scope.addEvents = function () {
        $("#existedUserName").focus(function () {
            message.hideAlertMessage("errorMsg");
            $("#checkExisted").hide();

            $scope.existedUser.userid = "-1"
        });

        $("#existedUserName").focusout(function () {
            $("#checkExisted").show();
            $(".tabsports div").show();

            if ($scope.existedUser.userid == "-1") {
                $scope.searchExistingUser(this.value, null);
            }
        });
    };

    $scope.step1Next = function () {
        if ($scope.existedUser.username == undefined || $scope.existedUser.username == "") {
            validate.setRequired("#existedUserName");
            message.showAlertMessage("errorMsg", "Please Enter User Name.", 1);
            $("#errorMsg").width("550");
        }
        else {
            var callbackFunc = function () {
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

                $scope.resetGeneralInfo();
                $scope.resetFrameSource();
            };

            if ($scope.existedUser.userid < 0) {
                $scope.searchExistingUser($scope.existedUser.username, callbackFunc);
            }
            else {
                callbackFunc();
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
        if ($scope.existedUser.maxCredit == undefined || $scope.existedUser.maxCredit == "") {
            message.showAlertMessage("errorMsg", "The max credit is required !", 1);
            $("#errorMsg").width("700");
            validate.setRequired("#maxCredit");
            return false;
        }

        var maxCredit = $scope.existedUser.maxCredit.replace(/,/g, '');
        if (isNaN(maxCredit) || parseInt(maxCredit) < 0) {
            message.showAlertMessage("errorMsg", "Please input max credit correctly !", 1);
            $("#errorMsg").width("700");
            validate.setRequired("#maxCredit");
            return false;
        }

        return true;
    };

    $scope.resetGeneralInfo = function () {
        $scope.newUser.username = "";
        $scope.newUser.password = "";
        $scope.newUser.firstname = "";
        $scope.newUser.lastname = "";
        $scope.newUser.phone = "";
        $scope.newUser.mobilephone = "";
        $scope.newUser.fax = "";

        $("input").removeClass("ng-dirty");
    };

    $scope.resetFrameSource = function () {
        $("#frmSportbook").attr('src', "");
        $("#frmSportbookMinPT").attr('src', "");
        $("#frmRacing").attr('src', "");
        $("#frmRacingMinPT").attr('src', "");
        $("#frmCasino").attr('src', "");
        $("#frmVirtualSports").attr('src', "");      
        $("#frmLiveCasino").attr('src', "");
        $("#frmGoldDleluxe").attr('src', "");
        $("#frmColossusBets").attr('src', "");
        $("#frmSportsbook2").attr('src', "");
        $("#frmCockFighting").attr('src', "");
        $("#frmCricket").attr('src', "");
        $("#frmAllbet").attr('src', "");
        $("#frmVoidbridge").attr('src', "");
        $("#frmNG2").attr('src', "");
        $("#frmFishingWorld").attr('src', "");
        $("#frmKenoLottery").attr('src', "");
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
                if (parent.level == 'super') {
                    url += "EditSuper/EditSuperPage?custId=" + $scope.existedUser.userid + "&uname=" + $scope.existedUser.username + "&siteName=" + $scope.siteName;
                }
                else {
                    url += "EditMember/EditMemberPage?custId=" + $scope.existedUser.userid + "&agentId=" + $scope.parentId + "&uname=" + $scope.existedUser.username + "&siteName=" + $scope.siteName;
                }
                $scope.loadData("#frmSportbook", url);
                $scope.resize("#frmSportbook");
                break;
            case 'tabSportsbookMinPT': url += "EditSuperMinPt/EditSuperMinPtPage?&custId=" + $scope.existedUser.userid + "&uname=" + $scope.existedUser.username;
                $scope.loadData("#frmSportbookMinPT", url);
                $scope.resize("#frmSportbookMinPT");
                break;
            case 'tabRacing': url += "EditSuperPTRacing/Index?custid=" + $scope.existedUser.userid;
                $scope.loadData("#frmRacing", url);
                $scope.resize("#frmRacing");
                break;
            case 'tabRacingMinPT': url += "customer/pt/racing/SuperMinUpdate.aspx?custId=" + $scope.existedUser.userid;
                $scope.loadData("#frmRacingMinPT", url);
                $scope.resize("#frmRacingMinPT");
                break;
            case 'tabCasino': url += "CustomerSetting/Casino/Super/EditSuper/" + $scope.existedUser.userid + "/0/true";
                $scope.loadData("#frmCasino", url);
                $scope.resize("#frmCasino");
                break;
            case 'tabVirtualSports': url += "DCS/ViewCustomerSetting?productId=5&customerLevel=4&IsReadOnly=true&customerId=" + $scope.existedUser.userid;
                $scope.loadData("#frmVirtualSports", url);
                $scope.resize("#frmVirtualSports");
                break;
            case 'tabBingo': url += "Customer/PT/Bingo/SuperUpdate.aspx?custId=" + $scope.existedUser.userid;
                $scope.loadData("#frmBingo", url);
                $scope.resize("#frmBingo");
                break;
            case 'tabLiveCasino': url += "LiveCasinoSetting/Setting/EditSetting?uplineId=0&uplineLevel=0&actorId=" + adminId + "&actorUsername=" + adminUsername + "&customerId=" + $scope.existedUser.userid + "&customerLevel=4&formType=2";
                $scope.loadData("#frmLiveCasino", url);
                $scope.resize("#frmLiveCasino");
                break;
            case 'tabGoldDeluxe': url += "GoldDeluxeSetting/GoldDeluxe/EditSetting?uplineId=0&upLineLevel=0&formType=2&actorId=" + adminId + "&custId=" + $scope.existedUser.userid + "&level=4";
                $scope.loadData("#frmGoldDleluxe", url);
                $scope.resize("#frmGoldDleluxe");
                break;
            case 'tabColossusBets': url += "customersetting/ColossusBetSetting/ColossusBetSetting/EditSetting?roleid=4&custid=" + $scope.existedUser.userid + "&UserId=" + adminId + "&ReadOnly=true";
                $scope.loadData("#frmColossusBets", url);
                $scope.resize("#frmColossusBets");
                break;
            case 'tabSportsbook2': url += "DCS/ViewCustomerSetting?productId=16&customerLevel=4&IsReadOnly=true&customerId=" + $scope.existedUser.userid;
                $scope.loadData("#frmSportsbook2", url);
                $scope.resize("#frmSportsbook2");
                break;
            case 'tabCockFighting': url += "DCS/ViewCustomerSetting?productId=19&customerLevel=4&IsReadOnly=true&customerId=" + $scope.existedUser.userid;
                $scope.loadData("#frmCockFighting", url);
                $scope.resize("#frmCockFighting");
                break;
            case 'tabAllbet': url += "DCS/ViewCustomerSetting?productId=21&customerLevel=4&IsReadOnly=true&customerId=" + $scope.existedUser.userid;
                $scope.loadData("#frmAllbet", url);
                $scope.resize("#frmAllbet");
                break;
            case 'tabVoidbridge': url += "DCS/ViewCustomerSetting?productId=22&customerLevel=4&IsReadOnly=true&customerId=" + $scope.existedUser.userid;
                $scope.loadData("#frmVoidbridge", url);
                $scope.resize("#frmVoidbridge");
                break;
            case 'tabFishingWorld': url += "DCS/ViewCustomerSetting?productId=24&customerLevel=4&IsReadOnly=true&customerId=" + $scope.existedUser.userid;
                $scope.loadData("#frmFishingWorld", url);
                $scope.resize("#frmFishingWorld");
                break;
            case 'tabKenoLottery': url += "DCS/ViewCustomerSetting?productId=28&customerLevel=4&IsReadOnly=true&customerId=" + $scope.existedUser.userid;
                $scope.loadData("#frmKenoLottery", url);
                $scope.resize("#frmKenoLottery");
                break;
            case 'tabCricket': url += "DCS/ViewCustomerSetting?productId=20&customerLevel=4&IsReadOnly=true&customerId=" + $scope.existedUser.userid;
                $scope.loadData("#frmCricket", url);
                $scope.resize("#frmCricket");
                break;
            case 'tabNG2': url += "DCS/ViewCustomerSetting?productId=23&customerLevel=4&IsReadOnly=true&customerId=" + $scope.existedUser.userid;
                $scope.loadData("#frmNG2", url);
                $scope.resize("#frmNG2");
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

    $scope.addSportsToDropdownlist = function (statusSports) {
        $scope.sports = [{ sportname: '-- choose sport --', sportvalue: 'choosesport' }];
        if (!statusSports.HideSportsbook) {
            $scope.sports.push({ sportname: 'Sportsbook', sportvalue: 'tabSportsbook' });
            $scope.sports.push({ sportname: 'Sportsbook Min PT', sportvalue: 'tabSportsbookMinPT' });
        }
        if (!statusSports.HideRacing) {
            $scope.sports.push({ sportname: 'Racing', sportvalue: 'tabRacing' });
            $scope.sports.push({ sportname: 'Racing Min PT', sportvalue: 'tabRacingMinPT' });
        }
        if (!statusSports.HideCasino) {
            $scope.sports.push({ sportname: 'Casino', sportvalue: 'tabCasino' });
        }
        if (!statusSports.HideVirtualSports) {
            $scope.sports.push({ sportname: 'Virtual Sports', sportvalue: 'tabVirtualSports' });
        }      
        if (!statusSports.HideLiveCasino) {
            $scope.sports.push({ sportname: 'Live Casino', sportvalue: 'tabLiveCasino' });
        }
        if (!statusSports.HideGoldDeluxe) {
            $scope.sports.push({ sportname: 'Gold Deluxe', sportvalue: 'tabGoldDeluxe' });
        }
        if (!statusSports.HideColossusBets) {
            $scope.sports.push({ sportname: 'Colossus Bets', sportvalue: 'tabColossusBets' });
        }
        if (!statusSports.HideSportsbook2) {
            $scope.sports.push({ sportname: 'Sportsbook 2', sportvalue: 'tabSportsbook2' });
        }
        if (!statusSports.HideCockFighting) {
            $scope.sports.push({ sportname: 'Cockfighting', sportvalue: 'tabCockFighting' });
        }
        if (!statusSports.HideCricket) {
            $scope.sports.push({ sportname: 'Cricket', sportvalue: 'tabCricket' });
        }

        $scope.selectedSport = $scope.sports[0];
    };

    $scope.searchExistingUser = function (username, callback) {
        if (username == undefined || username == "") {
            $("#checkExisted").removeClass('success');
            $("#checkExisted").removeClass('not_success');
            return;
        }

        $scope.existedUser.userid = -1;

        var param = {
            username: username,
            level: parent.level
        };

        try { parent.BlockUI(); } catch (e) { }
        try { parent.parent.BlockUI(); } catch (e) { }
        try { parent.parent.parent.BlockUI(); } catch (e) { }
        //$("body").addClass("wait");
        $.ajax({
            type: "POST",
            url: toUrl("Super/SearchExistingSuper"),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (customerSettingView) {
                if (customerSettingView.ErrorCode > 0)
                {
                    $("#checkExisted").removeClass('success');
                    $("#checkExisted").addClass('not_success');
                    message.showAlertMessage("errorMsg", customerSettingView.ErrorMessage, 1);
                    $("#errorMsg").width("550");
                }
                else
                {
                    var custid = customerSettingView.UserId;
                    //$scope.addSportsToDropdownlist(customerSettingView.StatusSports);
                    $scope.existedUser.userid = custid;
                    $scope.parentId = customerSettingView.ParentId;
                    $scope.newUser.BreadCrumbs = customerSettingView.BreadCrumbs;
                    if (custid != "-1") {
                        //step1
                        message.hideAlertMessage("errorMsg");
                        $("#checkExisted").removeClass('not_success');
                        $("#checkExisted").addClass('success');

                        $scope.getCustomerInfoByUserId(custid);
                        
                        if (callback) {
                            callback();
                        }
                    }
                    else {
                        $("#checkExisted").removeClass('success');
                        $("#checkExisted").addClass('not_success');
                        message.showAlertMessage("errorMsg", "The " + parent.level + " doesn’t exist !", 1);
                        $("#errorMsg").width("550");
                    }
                }

                try { parent.jQuery.unblockUI() } catch (e) { }
                try { parent.parent.jQuery.unblockUI(); } catch (e) { }
                try { parent.parent.parent.jQuery.unblockUI(); } catch (e) { }
                //$('body').removeClass('wait');
            }
            , error: function (xhr, ajaxOptions, thrownError) {
                message.showAlertMessage("errorMsg", "Error for searching existing user !", 1);
                $("#errorMsg").width("550");
                try { parent.jQuery.unblockUI() } catch (e) { }
                try { parent.parent.jQuery.unblockUI(); } catch (e) { }
                try { parent.parent.parent.jQuery.unblockUI(); } catch (e) { }
                $('#step1>.btnNextR').hide()
            }
        });
    };

    $scope.checkAllowedShow = function (custid) {
        $.ajax({
            type: "POST",
            url: toUrl("../../Handlers/CheckAllowedShow.ashx?custid=" + custid),
            success: function (allowedng2) {
                allowedng2 = parseInt(allowedng2);
                var existed = false;
                var index = -1;
                for (var i = 0; i < parent.sports.length; i++) {
                    if (parent.sports[i].sportvalue == 'tabNG2') {
                        existed = true;
                        index = i;
                        break;
                    }
                }

                if (allowedng2 && parent.showNG2) {                    
                    if (!existed) {
                        parent.sports.push({ sportname: parent.ng2Text, sportvalue: 'tabNG2' });                        
                    }                    
                }
                else {
                    if (existed) {
                        parent.sports.splice(index, 1);
                    }
                }

                $scope.$apply(function () {
                    $scope.sports = parent.sports;
                    $scope.selectedSport = $scope.sports[0];
                });
            }
        });
    };

    $scope.getCustomerInfoByUserId = function (custid) {
        var param = {
            custid: custid,
            level: parent.level
        };

        $.ajax({
            type: "POST",
            url: toUrl("Super/GetCustomerInfoByUserId"),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (custInfo) {
                message.hideAlertMessage("errorMsg");

                if ($scope.existedUser.selectedSite) {
                    $scope.existedUser.selectedSite.SiteName = custInfo.Site || "";
                }
                else {
                    $scope.existedUser.selectedSite = {};
                    $scope.existedUser.selectedSite.SiteName = custInfo.Site || "";
                }
                $scope.siteName = custInfo.Site || "";
                $scope.existedUser.credit = formatNumber('maxCredit', custInfo.Credit, true);
                $scope.existedUser.maxCredit = (parent.level == "super" ? formatNumber('maxCredit', custInfo.Credit, true) : formatNumber('maxCredit2', custInfo.CreditMax, true));
                $scope.existedUser.selectedCurrencyId = custInfo.CurrencyId;
                $scope.existedUser.selectedCurrency = custInfo.Currency;
                $scope.existedUser.syncCasino = custInfo.IsSyncCasino;
                $scope.existedUser.syncBingo = custInfo.IsSyncBingo;
                $scope.existedUser.syncLiveCasino = custInfo.IsSyncLiveCasino;
                $scope.existedUser.disableBingo = custInfo.DisableBingo;
                $scope.existedUser.disableCasino = custInfo.DisableCasino;
                $scope.existedUser.disableLiveCasino = custInfo.DisableLiveCasino;
                $scope.existedUser.syncNG2 = custInfo.IsSyncNG2;
                $scope.existedUser.disableNG2 = custInfo.DisableNG2;
                $scope.existedUser.isDirectCustomer = custInfo.IsDirectCustomer;

                if (custInfo.RoleId == 4) {
                    $scope.checkAllowedShow(custid);
                }

                $scope.getAllSites();

                $scope.$apply(function () {
                    $scope.sports = angular.copy(parent.sports);
                    $scope.selectedSport = $scope.sports[0];
                });

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
        var code = document.getElementById('code').value;
        var param = {
            UserName: $scope.newUser.username,
            Userpwd: $scope.newUser.password.ec(code).hc(),
            FirstName: $scope.newUser.firstname,
            LastName: $scope.newUser.lastname,
            Phone: $scope.newUser.phone,
            MobilePhone: $scope.newUser.mobilephone,
            Site: $scope.existedUser.selectedSite ? ($scope.existedUser.selectedSite.SiteName || "") : "",
            Fax: $scope.newUser.fax,
            Credit: (parent.level == 'member' ? $scope.existedUser.credit.replace(/,/g, '') : $scope.existedUser.maxCredit.replace(/,/g, '')),
            IsInternal: $scope.newUser.internaluser,
            CurrencyId: parseInt($scope.existedUser.selectedCurrencyId),
            ExistingCustId: $scope.existedUser.userid,
            Creator: $("#currentUserId").val(),
            CreatorName: adminUsername,
            IsSyncCasino: $scope.existedUser.syncCasino == "1" ? true : false,
            IsSyncBingo: $scope.existedUser.syncBingo == "1" ? true : false,
            IsSyncLiveCasino: $scope.existedUser.syncLiveCasino == "1" ? true : false,
            DisableBingo: $scope.existedUser.disableBingo,
            DisableCasino: $scope.existedUser.disableCasino,
            DisableLiveCasino: $scope.existedUser.disableLiveCasino,
            CustLevel: (parent.level == 'member' ? 1 : 4),
            UplineId: $scope.parentId,
            IsSyncNG2: $scope.existedUser.syncNG2 == 1 ? true : false,
            DisableNG2: $scope.existedUser.disableNG2 == 1 ? true : false,
            code: code
        };

        message.hideAlertMessage("errorMsg");

        try { parent.BlockUI(); } catch (e) { }
        try { parent.parent.BlockUI(); } catch (e) { }
        try { parent.parent.parent.BlockUI(); } catch (e) { }
        //$("body").addClass("wait");        

        $.ajax({
            type: "POST",
            url: toUrl("Super/CopyExistingSuper"),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (result) {
                if (result != null) {
                    var msg = result.Message != undefined ? result.Message : msg;
                    msg = result.errorMessage != undefined ? result.errorMessage : msg;
                    msg = result.ErrorMessage != undefined ? result.ErrorMessage : msg;
                    message.showAlertMessage("errorMsg", msg, 3);
                    $("#errorMsg").width("700");

                    if (result.IsSuccessful && $scope.existedUser.isDirectCustomer)
                    {
                        alert('Please fulfill the Extra Information via the Information tab on the left panel');
                    }

                }
                else {
                    message.showAlertMessage("errorMsg", "Error for copying existing user !", 1);
                    $("#errorMsg").width("700");
                }

                try { parent.jQuery.unblockUI() } catch (e) { }
                try { parent.parent.jQuery.unblockUI(); } catch (e) { }
                try { parent.parent.parent.jQuery.unblockUI(); } catch (e) { }
                //$('body').removeClass('wait');
            }
            , error: function (xhr, ajaxOptions, thrownError) {
                message.showAlertMessage("errorMsg", "Error for copying existing user !", 1);
                $("#errorMsg").width("700");
            }
        });
    };

    /* start */
    $scope.init();

}]);
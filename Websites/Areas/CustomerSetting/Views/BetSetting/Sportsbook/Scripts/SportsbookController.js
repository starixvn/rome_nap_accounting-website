var module = angular.module("sportsBookApp", []);

function SportsbookController($scope, $http) {
    $scope.model = model;
    $scope.bettingLimits = $scope.model.bettingLimitItems;

    var length = $scope.model.bettingLimitItems.length;

    $scope.$watch("model.bettingLimitItems[0].minBetFormated", function (newValue, oldValue, _model) {
        if (_model.bettingLimits[0].checked) {
            for (var i = 1; i < length - 1; i++) {
                _model.bettingLimits[i].minBetFormated = parseFloat(Fanex.CustomerSetting.Core.ReplaceAll((newValue == '' ? '0' : newValue), ',', '')).formatNumber();
                _model.bettingLimits[i].minBet = _model.bettingLimits[i].minBetFormated;
            }
        }
    });

    $scope.$watch("model.bettingLimitItems[0].maxBetFormated", function (newValue, oldValue, _model) {
        if (_model.bettingLimits[0].checked) {
            for (var i = 1; i < length - 1; i++) {
                _model.bettingLimits[i].maxBetFormated = parseFloat(Fanex.CustomerSetting.Core.ReplaceAll((newValue == '' ? '0' : newValue), ',', '')).formatNumber();
                _model.bettingLimits[i].maxBet = _model.bettingLimits[i].maxBetFormated;
            }
        }
    });

    $scope.$watch("model.bettingLimitItems[0].maxPerMatchFormated", function (newValue, oldValue, _model) {
        if (_model.bettingLimits[0].checked) {
            for (var i = 1; i < length - 1; i++) {
                _model.bettingLimits[i].maxPerMatchFormated = parseFloat(Fanex.CustomerSetting.Core.ReplaceAll((newValue == '' ? '0' : newValue), ',', '')).formatNumber();
                _model.bettingLimits[i].maxPerMatch = _model.bettingLimits[i].maxPerMatchFormated;
            }
        }
    });

    $scope.$watch("model.bettingLimitItems[1].minBetFormated", function (newValue, oldValue, _model) {
        if (_model.bettingLimits[1].checked) {
            for (var i = 2; i < length - 1; i++) {
                _model.bettingLimits[i].minBetFormated = parseFloat(Fanex.CustomerSetting.Core.ReplaceAll((newValue == '' ? '0' : newValue), ',', '')).formatNumber();
                _model.bettingLimits[i].minBet = _model.bettingLimits[i].minBetFormated;
            }
        }
    });

    $scope.$watch("model.bettingLimitItems[1].maxBetFormated", function (newValue, oldValue, _model) {
        if (_model.bettingLimits[1].checked) {
            for (var i = 2; i < length - 1; i++) {
                _model.bettingLimits[i].maxBetFormated = parseFloat(Fanex.CustomerSetting.Core.ReplaceAll((newValue == '' ? '0' : newValue), ',', '')).formatNumber();
                _model.bettingLimits[i].maxBet = _model.bettingLimits[i].maxBetFormated;
            }
        }
    });

    $scope.$watch("model.bettingLimitItems[1].maxPerMatchFormated", function (newValue, oldValue, _model) {
        if (_model.bettingLimits[1].checked) {
            for (var i = 2; i < length - 1; i++) {
                _model.bettingLimits[i].maxPerMatchFormated = parseFloat(Fanex.CustomerSetting.Core.ReplaceAll((newValue == '' ? '0' : newValue), ',', '')).formatNumber();
                _model.bettingLimits[i].maxPerMatch = _model.bettingLimits[i].maxPerMatchFormated;
            }
        }
    });

    $scope.Init = function () {
        var len = $scope.model.bettingLimitItems.length;

        for (var i = 0; i < len - 1; i++) {
            $scope.model.bettingLimitItems[i].title = $scope.model.bettingLimitItems[i].resourceKey == 'Soccer' ?
            'Copy all below bet settings from Soccer (excluding Number Game)' : $scope.model.bettingLimitItems[i].resourceKey == 'Basketball' ?
            'Copy all below bet settings from Basketball (excluding Number Game)' : '';
        }
    };

    $scope.Copy = function (scope) {
        switch (scope.betting.resourceKey) {
            case 'Soccer':
                $scope.model.bettingLimitItems[1].checked = false;
                var element = $('#_' + scope.betting.resourceKey);
                var unchecked = element.hasClass('true');
                var len = $scope.model.bettingLimitItems.length;
                var soccerBetting = $scope.model.bettingLimitItems[0];

                if (unchecked) {
                    $(element).removeClass('true');
                    $(element).addClass('copied');
                    $(element).addClass('copyActive');
                    $scope.model.bettingLimitItems[0].checked = true;
                   
                    var minBet = parseFloat(Fanex.CustomerSetting.Core.ReplaceAll((soccerBetting.minBetFormated == '' ? '0' : soccerBetting.minBetFormated), ',', ''));
                    var maxBet = parseFloat(Fanex.CustomerSetting.Core.ReplaceAll((soccerBetting.maxBetFormated == '' ? '0' : soccerBetting.maxBetFormated), ',', ''));
                    var maxPerMatch = parseFloat(Fanex.CustomerSetting.Core.ReplaceAll((soccerBetting.maxPerMatchFormated == '' ? '0' : soccerBetting.maxPerMatchFormated), ',', ''));

                    for (var i = 1; i < len - 1; i++) {
                        $scope.model.bettingLimitItems[i].bgClass = 'inactive';

                        $scope.model.bettingLimitItems[i].minBetFormated = minBet.formatNumber();
                        $scope.model.bettingLimitItems[i].maxBetFormated = maxBet.formatNumber();
                        $scope.model.bettingLimitItems[i].maxPerMatchFormated = maxPerMatch.formatNumber();

                        $scope.model.bettingLimitItems[i].minBet = minBet.formatNumber();
                        $scope.model.bettingLimitItems[i].maxBet = maxBet.formatNumber();
                        $scope.model.bettingLimitItems[i].maxPerMatch = maxPerMatch.formatNumber();
                    }
                }
                else {
                    $(element).removeClass('copied');
                    $(element).removeClass('copyActive');
                    $(element).addClass('true');
                    $scope.model.bettingLimitItems[0].checked = false;                    

                    for (var i = 1; i < len - 1; i++) {
                        $scope.model.bettingLimitItems[i].bgClass = '';

                        var minBet = $scope.model.bettingLimitItems[i].preValMinBet;
                        var maxBet = $scope.model.bettingLimitItems[i].preValMaxBet;
                        var maxPerMatch = $scope.model.bettingLimitItems[i].preValMaxPerMatch;

                        $scope.model.bettingLimitItems[i].minBetFormated = minBet;
                        $scope.model.bettingLimitItems[i].maxBetFormated = maxBet;
                        $scope.model.bettingLimitItems[i].maxPerMatchFormated = maxPerMatch;

                        $scope.model.bettingLimitItems[i].minBet = minBet;
                        $scope.model.bettingLimitItems[i].maxBet = maxBet;
                        $scope.model.bettingLimitItems[i].maxPerMatch = maxPerMatch;
                    }
                }

                var basketBallElement = $('#_Basketball');
                $(basketBallElement).removeClass('copied');
                $(basketBallElement).addClass('true');

                // Prevents model is updating value for class name
                var timeout = setTimeout(function () {
                    clearTimeout(timeout);

                    $(basketBallElement).addClass('pointer');
                }, 50);

                break;
            case 'Basketball':
                $scope.model.bettingLimitItems[0].checked = false;

                var len = $scope.model.bettingLimitItems.length;
                var soccerElement = $('#_Soccer');
                var soccerChecked = $(soccerElement).hasClass('copied');
                $(soccerElement).removeClass('copied');
                $(soccerElement).removeClass('copyActive');
                $(soccerElement).addClass('true');

                if (soccerChecked) {
                    for (var i = 1; i < len - 1; i++) {
                        $scope.model.bettingLimitItems[i].bgClass = '';

                        var minBet = $scope.model.bettingLimitItems[i].preValMinBet;
                        var maxBet = $scope.model.bettingLimitItems[i].preValMaxBet;
                        var maxPerMatch = $scope.model.bettingLimitItems[i].preValMaxPerMatch;

                        $scope.model.bettingLimitItems[i].minBetFormated = minBet;
                        $scope.model.bettingLimitItems[i].maxBetFormated = maxBet;
                        $scope.model.bettingLimitItems[i].maxPerMatchFormated = maxPerMatch;

                        $scope.model.bettingLimitItems[i].minBet = minBet;
                        $scope.model.bettingLimitItems[i].maxBet = maxBet;
                        $scope.model.bettingLimitItems[i].maxPerMatch = maxPerMatch;
                    }
                }

                var element1 = $('#_' + scope.betting.resourceKey);
                var unchecked = element1.hasClass('true');                
                var basketballBetting = $scope.model.bettingLimitItems[1];
                
                if (unchecked) {
                    $scope.model.bettingLimitItems[1].bgClass = '';
                    $scope.model.bettingLimitItems[1].checked = true;                    

                    var minBet = parseFloat(Fanex.CustomerSetting.Core.ReplaceAll((basketballBetting.minBetFormated == '' ? '0' : basketballBetting.minBetFormated), ',', ''));
                    var maxBet = parseFloat(Fanex.CustomerSetting.Core.ReplaceAll((basketballBetting.maxBetFormated == '' ? '0' : basketballBetting.maxBetFormated), ',', ''));
                    var maxPerMatch = parseFloat(Fanex.CustomerSetting.Core.ReplaceAll((basketballBetting.maxPerMatchFormated == '' ? '0' : basketballBetting.maxPerMatchFormated), ',', ''));

                    for (var i = 2; i < len - 1; i++) {
                        $scope.model.bettingLimitItems[i].bgClass = 'inactive';

                        $scope.model.bettingLimitItems[i].minBetFormated = minBet.formatNumber();
                        $scope.model.bettingLimitItems[i].maxBetFormated = maxBet.formatNumber();
                        $scope.model.bettingLimitItems[i].maxPerMatchFormated = maxPerMatch.formatNumber();

                        $scope.model.bettingLimitItems[i].minBet = minBet.formatNumber();
                        $scope.model.bettingLimitItems[i].maxBet = maxBet.formatNumber();
                        $scope.model.bettingLimitItems[i].maxPerMatch = maxPerMatch.formatNumber();
                    }

                    // Prevents model is updating value for class name
                    var timeout = setTimeout(function () {
                        clearTimeout(timeout);

                        $(element1).removeClass('true');
                        $(element1).addClass('copied');
                        $(element1).addClass('copyActive');
                    }, 50);
                }
                else {
                    $(element1).removeClass('copied');
                    $(element1).removeClass('copyActive');
                    $(element1).addClass('true');
                    $scope.model.bettingLimitItems[1].checked = false;

                    for (var i = 2; i < len - 1; i++) {
                        $scope.model.bettingLimitItems[i].bgClass = '';

                        var minBet = $scope.model.bettingLimitItems[i].preValMinBet;
                        var maxBet = $scope.model.bettingLimitItems[i].preValMaxBet;
                        var maxPerMatch = $scope.model.bettingLimitItems[i].preValMaxPerMatch;

                        $scope.model.bettingLimitItems[i].minBetFormated = minBet;
                        $scope.model.bettingLimitItems[i].maxBetFormated = maxBet;
                        $scope.model.bettingLimitItems[i].maxPerMatchFormated = maxPerMatch;

                        $scope.model.bettingLimitItems[i].minBet = minBet;
                        $scope.model.bettingLimitItems[i].maxBet = maxBet;
                        $scope.model.bettingLimitItems[i].maxPerMatch = maxPerMatch;
                    }
                }

                return;
            default:
                break;
        }
    };
};

function Update(ondone) {
    var bettings = model.bettingLimitItems;
    var length = bettings.length;

    for (var i = 0; i < length; i++) {
        var betting = bettings[i];

        var curValMinBet = parseFloat(Fanex.CustomerSetting.Core.ReplaceAll(betting.minBetFormated, ',', ''));
        var curValMaxBet = parseFloat(Fanex.CustomerSetting.Core.ReplaceAll(betting.maxBetFormated, ',', ''));

        var curValMaxPerMatch = parseFloat(Fanex.CustomerSetting.Core.ReplaceAll(betting.maxPerMatchFormated, ',', ''));
        var curValMaxPerMatchMax = betting.maxPerMatchMax;

        if (betting.maxPerBall) {
            if (curValMinBet != 0 && curValMaxBet != 0 && curValMinBet > curValMaxBet) {
                Validators.show($('#txt' + betting.resourceKey + 'Min1').get(0), betting.name + ' - Min Bet should be less than or equals to Max Bet', undefined, undefined, 3000);

                return;
            }

            if (curValMaxBet != 0 && curValMaxPerMatch != 0 && curValMaxBet > curValMaxPerMatch) {
                Validators.show($('#txt' + betting.resourceKey + 'Max1').get(0), betting.name + ' - Max Bet should be less than or equals to Max Per Match', undefined, undefined, 3000);

                return;
            }

            if (curValMaxPerMatch != 0 && curValMaxPerMatchMax != 0 && curValMaxPerMatch > curValMaxPerMatchMax) {
                Validators.show($('#txt' + betting.resourceKey + 'MaxPerMatch1').get(0), betting.name + ' - Max Per Match should be less than or equals to ' + curValMaxPerMatchMax.formatNumber(), undefined, undefined, 3000);

                return;
            }

            var curValMaxPerBall = parseFloat(Fanex.CustomerSetting.Core.ReplaceAll(betting.maxPerBallFormated, ',', ''));
            var maxPerBallMax = betting.maxPerBallMax;

            if (curValMaxPerBall != 0 && maxPerBallMax != 0 && curValMaxPerBall > maxPerBallMax) {
                Validators.show($('#txt' + betting.resourceKey + 'MaxPerBall1').get(0), betting.name + ' - Max Per Ball should be less than or equals to ' + maxPerBallMax.formatNumber(), undefined, undefined, 3000);

                return;
            }
        }
        else {
            if (curValMinBet != 0 && curValMaxBet != 0 && curValMinBet > curValMaxBet) {
                Validators.show($('#txt' + betting.resourceKey + 'Min').get(0), betting.name + ' - Min Bet should be less than or equals to Max Bet', undefined, undefined, 3000);

                return;
            }

            if (curValMaxBet != 0 && curValMaxPerMatch != 0 && curValMaxBet > curValMaxPerMatch) {
                Validators.show($('#txt' + betting.resourceKey + 'Max').get(0), betting.name + ' - Max Bet should be less than or equals to Max Per Match', undefined, undefined, 3000);

                return;
            }

            if (curValMaxPerMatch != 0 && curValMaxPerMatchMax != 0 && curValMaxPerMatch > curValMaxPerMatchMax) {
                Validators.show($('#txt' + betting.resourceKey + 'MaxPerMatch').get(0), betting.name + ' - Max Per Match should be less than or equals to ' + curValMaxPerMatchMax.formatNumber(), undefined, undefined, 3000);

                return;
            }
        }
    }

    Fanex.CustomerSetting.Core.BlockUI();
    $.ajax({ dataType: 'json', type: 'post', url: toUrl('Sportsbook/EditBetSettingPost'), data: { id: custId, uplineId: uplineId, userId: userId, txtCustomerObject: JSON.stringify(model)} }).success(function (data) {
        if (data.errorCode == "0") {
            $('#errMsg').attr('class', 'successMsg');
            $('#errMsg').html('<font color="Blue">Updated successfully</font>');
            $('#errMsg').show();
        }
        else {
            $('#errMsg').html(data.errorMessage.formatErrorMessage());
            $('#errMsg').show();
        }

        Fanex.CustomerSetting.Core.UnBlockUI();

        if (typeof ondone == 'function') {
            ondone(data);
        }
    }).error(function (data) {
        console.log(data);

        Fanex.CustomerSetting.Core.UnBlockUI();

        if (typeof ondone == 'function') {
            ondone({ errorCode: "1", errorMessage: "Error on updating..." });
        }
    });
};
var PTSetting = PTSetting || {};
PTSetting.SportType = new function () {

    var delayTime = 500;

    var RemoveButtons = function () {
        var $content = $("#frmSport").contents();
        setTimeout(function () {
            $content.find('.maincontent_header').remove();
            $content.find('table:has(td.custBtn1)').remove();
        }, delayTime);

        $content.find("#divShowInfo").remove();
    };

    var RemoveSportBookCustInfo = function () {
        var $content = $("#frmSport").contents();
        for (var i = 0; i < 6; i++) {
            $content.find(".tblcollapse tr:eq(0)").remove();
        }
        //disable edit
        setTimeout(function () {
            $content.find('.BettingLimitTextBox4').attr('readonly', 'readonly');
            $content.find('.BettingLimitTextBox5').attr('readonly', 'readonly');
            $content.find('.selectcomm').attr('disabled', 'disabled');
            $content.find('.PTSelect').attr('disabled', 'disabled');
        }, delayTime);
    };

    var RemoveSportBookMinPT = function () {
        var $content = $("#frmSport").contents();
        $content.find('.tblcollapse')[0].remove();
        //disable edit
        setTimeout(function () {
            $content.find('.PTSelect').attr('disabled', true);
            $content.find('input:button').attr('disabled', true);
        }, delayTime);

    };

    var RemoveRacingCustInfo = function () {
        var $content = $("#frmSport").contents();
        $content.find('#divCustInfo').remove();
        //disable edit
        setTimeout(function () {
            $content.find('input:checkbox').attr('disabled', true);
            $content.find('input:text').attr('disabled', true);
            $content.find('select').attr('disabled', true);
        }, delayTime);
    };

    var RemoveRacingMinPT = function () {
        var $content = $("#frmSport").contents();
        setTimeout(function () {
            $content.find('.maincontent_box').remove();
            $content = $content.find('#SettingContent');
            $content.find('.tblcollapse')[0].remove();
        }, 100);

        //disable edit
        setTimeout(function () {
            $content.find('input:checkbox').attr('disabled', true);
            $content.find('select').attr('disabled', true);
        }, delayTime);
    };

    var RemoveCasinoCustInfo = function () {
        var $content = $("#frmSport").contents();
        for (var i = 0; i < 2; i++) {
            $content.find('.tblcollapse tr:eq(0)').remove();
        }
        $content.find('.maincontent_box').remove();

        //disable edit
        setTimeout(function () {
            $content.find('input:checkbox').attr('disabled', true);
            $content.find('input:text').attr('readonly', 'readonly');
            $content.find('select').attr('disabled', true);
            $content.find('a').css('display', 'none');
        }, delayTime);
    };

    var RemoveVirtualSportsCustInfo = function () {
        var $content = $("#frmSport").contents();
        $content.find('.tblcollapse')[0].remove();
        //disable edit
        setTimeout(function () {
            $content.find('.BettingLimitTextBox4').attr('readonly', 'readonly');
            $content.find('.PTSelect').attr('disabled', true);
            $content.find('.btnUpdate').attr('disabled', true);
        }, delayTime);
    };

    var RemoveBingoCustInfo = function () {
        var $content = $("#frmSport").contents();
        for (var i = 0; i < 5; i++) {
            $content.find('.tblcollapse tr:eq(0)').remove();
        }
        $content.find('.maincontent_box').remove();
        $content.find('#tblHideUpdate').remove();

        //disable edit
        setTimeout(function () {
            $content.find('input:checkbox').attr('disabled', true);
            $content.find('input:text').attr('readonly', 'readonly');
            $content.find('select').attr('disabled', true);
            $content.find('.canchange').css('display', 'none');
        }, delayTime);
    };

    var RemoveLiveCasinoCustInfo = function () {
        var $content = $("#frmSport").contents();
        for (var i = 0; i < 2; i++) {
            $content.find('.tblcollapse tr:eq(0)').remove();
        }
        $content.find('.maincontent_box').remove();
        //disable edit
        setTimeout(function () {
            $content.find('input:checkbox').attr('disabled', true);
            $content.find('select').attr('disabled', true);
            $content.find('.btnUpdateSetting').remove();
        }, delayTime);
    };

    this.SetFrameLoaded = function () {
        var tabId = '';
        $(".tabsports div").each(function () {
            if (this.className.indexOf('tab-active') > -1) {
                tabId = this.id;
            }
        });
        //remove settings
        switch (tabId) {
            case 'tabSportsbook': RemoveSportBookCustInfo(); break;
            case 'tabSportsbookMinPT': RemoveSportBookMinPT(); break;
            case 'tabRacing': RemoveRacingCustInfo(); break;
            case 'tabRacingMinPT': RemoveRacingMinPT(); break;
            case 'tabCasino': RemoveCasinoCustInfo(); break;
            case 'tabVirtualSports': RemoveVirtualSportsCustInfo(); break;
            case 'tabBingo': RemoveBingoCustInfo(); break;
            case 'tabLiveCasino': RemoveLiveCasinoCustInfo(); break;
        }

        RemoveButtons();
    }
};
var PTSetting = PTSetting || {};
PTSetting.SportType = new function () {

    var delayTime = 500;

    var RemoveButtons = function (frameId) {
        var $content = $(frameId).contents();
        setTimeout(function () {
            $content.find('.maincontent_header').remove();
            $content.find('table:has(td.custBtn1)').remove();
        }, delayTime);

        $content.find("#divShowInfo").remove();
    };

    var RemoveSportBookCustInfo = function () {
        var $content = $("#frmSportbook").contents();
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
        var $content = $("#frmSportbookMinPT").contents();
        $content.find('.tblcollapse')[0].remove();
        //disable edit
        setTimeout(function () {
            $content.find('.PTSelect').attr('disabled', true);
            $content.find('input:button').attr('disabled', true);
        }, delayTime);

    };

    var RemoveRacingCustInfo = function () {
        var $content = $("#frmRacing").contents();
        $content.find('#divCustInfo').remove();
        //disable edit
        setTimeout(function () {
            $content.find('input:checkbox').attr('disabled', true);
            $content.find('input:text').attr('disabled', true);
            $content.find('select').attr('disabled', true);
        }, delayTime);
    };

    var RemoveRacingMinPT = function () {
        var $content = $("#frmRacingMinPT").contents();
        setTimeout(function () {
            $content.find('.maincontent_box').remove();
            $content = $content.find('#SettingContent');
            $content.find('.tblcollapse')[0].remove();
        }, delayTime);

        //disable edit
        setTimeout(function () {
            $content.find('input:checkbox').attr('disabled', true);
            $content.find('select').attr('disabled', true);
        }, delayTime);
    };

    var RemoveCasinoCustInfo = function () {
        var $content = $("#frmCasino").contents();
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
        var $content = $("#frmVirtualSports").contents();
        $content.find('.tblcollapse')[0].remove();
        //disable edit
        setTimeout(function () {
            $content.find('.BettingLimitTextBox4').attr('readonly', 'readonly');
            $content.find('.PTSelect').attr('disabled', true);
            $content.find('.btnUpdate').attr('disabled', true);
        }, delayTime);
    };

    var RemoveBingoCustInfo = function () {
        var $content = $("#frmBingo").contents();
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
        var $content = $("#frmLiveCasino").contents();
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
            case 'tabSportsbook': RemoveSportBookCustInfo(); RemoveButtons('#frmSportbook'); break;
            case 'tabSportsbookMinPT': RemoveSportBookMinPT(); RemoveButtons('#frmSportbookMinPT'); break;
            case 'tabRacing': RemoveRacingCustInfo(); RemoveButtons('#frmRacing'); break;
            case 'tabRacingMinPT': RemoveRacingMinPT(); RemoveButtons('#frmRacingMinPT'); break;
            case 'tabCasino': RemoveCasinoCustInfo(); RemoveButtons('#frmCasino'); break;
            case 'tabVirtualSports': RemoveVirtualSportsCustInfo(); RemoveButtons('#frmVirtualSports'); break;
            case 'tabBingo': RemoveBingoCustInfo(); RemoveButtons('#frmBingo'); break;
            case 'tabLiveCasino': RemoveLiveCasinoCustInfo(); RemoveButtons('#frmLiveCasino'); break;
        }
    }
};
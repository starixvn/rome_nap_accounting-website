var betDetailsManager;
$(document).ready(function () {
    betDetailsManager.initPage();
});

var BetDetailsManager = function () {
    var self = this;
    self.retryNo = 0;
    self.iframe = '';

    var iframeError;
    self.initPage = function () {
        if (memberChoice.ActionInfo.ErrCode) {
            showAlertMessage("column-memberchoice", memberChoice.ActionInfo.ErrMsg, 1);
            self.drawSeparator();
        } else {
            self.iframe = memberChoice.Iframe;
            self.retryNo = 0;

            var iframe = '<iframe src="' + self.iframe + '" frameborder="0" class="memberchoice-iframe"></iframe>';
            self.setIframe(iframe);
        }
    };
    self.retryGetIFrame = function () {
        if (self.retryNo == 5) {
            self.drawSeparator();
            self.retryNo = 0;
            return;
        }

        self.retryNo++;
        var iframe = '<iframe src="' + (self.iframe + '&version=' + self.retryNo) + '" frameborder="0" class="memberchoice-iframe"></iframe>';
        self.setIframe(iframe);
    };
    self.setIframe = function (iframe) {
        $("#column-memberchoice").html(iframe);
        var mustRetry = $(".memberchoice-iframe").contents().find("body > *").length == 0;
        if (mustRetry) {
            setTimeout(self.retryGetIFrame, 1000);
        } else {
            self.drawSeparator();
        }
    }

    self.drawSeparator = function () {
        var memberChoiceHeight = $("#column-memberchoice").height();
        console.log($("#column-memberchoice").height());

        var detailHeight = $(".column-tickerdetail").height();
        var separatorHeight = detailHeight >= memberChoiceHeight ? detailHeight : memberChoiceHeight;
        $(".column-separator").height(separatorHeight);
    };
};

betDetailsManager = new BetDetailsManager();
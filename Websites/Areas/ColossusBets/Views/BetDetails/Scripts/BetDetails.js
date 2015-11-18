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
        $(".memberchoice-betdetail").removeClass("display-none");
        $(".column-separator").height($(".column-tickerdetail").height());

        if (memberChoice.ActionInfo.ResultCode) {
            self.showAlertMessage("column-memberchoice", memberChoice.ActionInfo.ResultMessage, 1);
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
        self.drawSeparator();
    }

    self.drawSeparator = function () {
        var memberChoiceHeight = $("#column-memberchoice").height();
        var detailHeight = $(".column-tickerdetail").height();
        var separatorHeight = detailHeight >= memberChoiceHeight ? detailHeight : memberChoiceHeight;
        $(".column-separator").height(separatorHeight);
    };
    self.showAlertMessage = function (containerid, msg, type) {
        $("#" + containerid).removeClass("hidden");
        switch (type) {
            case 0:
                document.getElementById(containerid).innerHTML = "<div class='alertboxscc'><div>" + msg + "</div></div>";
                break;
            case 1:
                document.getElementById(containerid).innerHTML = "<div class='alertboxerr'><div>" + msg + "</div></div>";
                break;
            case 2:
                document.getElementById(containerid).innerHTML = "<div class='alertboxwrn'><div>" + msg + "</div></div>";
                break;
            case 3:
                document.getElementById(containerid).innerHTML = msg;
                break;
            default:
                break
        }
    };
};

betDetailsManager = new BetDetailsManager();
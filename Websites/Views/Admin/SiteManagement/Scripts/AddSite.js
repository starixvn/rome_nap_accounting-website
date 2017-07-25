function SiteManagement() {
    var self = this;

    var $rdoTypes;
    var $txtSiteName;
    var $errorMessage;

    this.init = function () {
        this.cacheElements();
        this.registerEvents();
    };
    this.cacheElements = function () {
        this.$rdoTypes = $(".rdo-type");
        this.$txtSiteName = $("#txt-site-name");
        this.$errorMessage = $("#err-message");
    };
    this.registerEvents = function () {

        $("#btn-add").click(function () {
            self.addSite();
        });
    };
    this.addSite = function () {

        var param = {
            Type: $(".rdo-type:checked").val(),
            SiteName: $.trim(this.$txtSiteName.val())
        };

        if (param.SiteName === "") {
            showAlertMessage("err-message", "Please input site name!!!", 1);
            this.$errorMessage.removeClass("hide");
            return;
        }

        parent.BlockUI();
        $.ajax({
            type: "POST",
            url: window.rootUrl + "SiteManagement/ValidateSite",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {
                if (!rs) {
                    showAlertMessage("err-message", "Site name already exists!!!", 1);
                    self.$errorMessage.removeClass("hide");
                    parent.$.unblockUI();
                }
                else {
                    self.addSiteAfterValidate(param);
                }
            }
        });
    };

    this.addSiteAfterValidate = function (param) {
        var confirmValue = confirm("Are you sure you want to add new site named: " + param.SiteName + "?");
        if (!confirmValue) {
            parent.$.unblockUI();
            return;
        }

        $.ajax({
            type: "POST",
            url: window.rootUrl + "SiteManagement/AddSite",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {
                if (rs.ErrorCode === 0) {
                    showAlertMessage("err-message", "Add new site name successfully.", 0);
                }
                else {
                    showAlertMessage("err-message", rs.ErrorMessage, 1);
                }

                self.$errorMessage.removeClass("hide");
                parent.$.unblockUI();
            }
        });
    };
};

$(document).ready(function () {
    var siteManagement = new SiteManagement();
    siteManagement.init();
});
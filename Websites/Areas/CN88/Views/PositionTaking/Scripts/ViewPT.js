var CN88ViewPTManager = function () {
    var self = this;
    this.elements = {
        btnSubmit: null,
        txtUsername: null,
        divErrorMessage: null,
        divResult: null,
        resultTbls: null,
        tblLineInfo: null,
        tblPT: null,
        divComm: null,
        tblComm: null
    };
    this.completeLoadPT = false;
    this.completeLoadComm = false;
    this.hasError = false;
    this.mustLockPage = false;

    this.init = function () {
        this.cacheElements();
        this.bindEvents();
        this.enableInput();

        var username = $.trim(this.elements.txtUsername.val());
        if (username != "") {
            this.getPTAndComm();
        }
    };
    this.cacheElements = function () {
        this.elements.txtUsername = $("#txt-username");
        this.elements.btnSubmit = $("#btn-submit");
        this.elements.divErrorMessage = $("#div-error-message");
        this.elements.divResult = $("#div-result");

        this.elements.resultTbls = $("#result-tbls");
        this.elements.tblLineInfo = $("#tbl-line-info");
        this.elements.tblPT = $("#tbl-pt");
        this.elements.divComm = $("#div-comm");
        this.elements.tblComm = $("#tbl-comm");
    };
    this.enableInput = function () {
        this.elements.txtUsername.attr("disabled", null);
        this.elements.btnSubmit.attr("disabled", null);
        parent.jQuery.unblockUI();
    };
    this.disableInput = function () {
        parent.BlockUI();
        this.elements.txtUsername.attr("disabled", "disabled");
        this.elements.btnSubmit.attr("disabled", "disabled");
    };
    this.bindEvents = function () {
        this.elements.txtUsername.bind("keyup", function () {
            self.txtUsernameKeyUp()
        });

        this.elements.btnSubmit.bind("click", function () {
            self.getPTAndComm();
        });
    };
    this.txtUsernameKeyUp = function (e) {
        e = (e) ? e : window.event;
        var key = (e.keyCode) ? e.keyCode : e.which;

        if (key != 13) {
            return;
        }

        this.getPTAndComm();
    };
    this.getPTAndComm = function () {
        var username = $.trim(this.elements.txtUsername.val());
        if (username == "") {
            showAlertMessage("div-error-message", "Please input username!", 1);
            this.elements.divResult.removeClass("display-none");
            return;
        }

        this.elements.divErrorMessage.empty();
        this.disableInput();
        this.hasError = false;
        this.mustLockPage = false;

        this.getPT(username);
        this.getComm(username);
    };
    this.getPT = function (username) {
        this.completeLoadPT = false;
        $.ajax({
            type: "POST",
            url: window.rootUrl + "CN88PT/GetPT",
            data: $.toJSON({ username: username }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (rs) {
                if (rs.ErrorCode) {
                    self.hasError = true;
                    showAlertMessage("div-error-message", rs.ErrorMessage, 1);
                    self.elements.resultTbls.addClass("display-none");
                    return;
                }

                self.renderLineInfo(rs);
                self.renderPT(rs);
            },
            error: function (xhr) {
                self.handleError(xhr);
            },
            complete: function () {
                self.completeLoadPT = true;
                self.completeLoadPTAndComm();
            }
        });
    };
    this.getComm = function (username) {
        this.completeLoadComm = false;
        $.ajax({
            type: "GET",
            url: window.rootUrl + "CN88PT/GetComm",
            data: $.param({ username: username }),
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (rs) {
                self.elements.divComm.find("#tbl-comm").remove();
                if (rs == "") {
                    self.hasError = true;
                    return;
                }

                self.elements.divComm.append(rs);
            },
            error: function (xhr) {
                self.handleError(xhr);
            },
            complete: function () {
                self.completeLoadComm = true;
                self.completeLoadPTAndComm();
            }
        });
    };
    this.completeLoadPTAndComm = function () {
        if (!this.completeLoadPT || !this.completeLoadComm) {
            return;
        }

        if (this.hasError) {
            this.elements.resultTbls.addClass("display-none");
        } else {
            this.elements.resultTbls.removeClass("display-none");
        }

        if (!this.mustLockPage) {
            this.enableInput();
        }

        this.elements.divResult.removeClass("display-none");
    };
    this.renderLineInfo = function (rs) {
        var html = "";

        for (var i = rs.Customers.length - 1; i >= 0; i--) {
            var cust = rs.Customers[i];
            html += "<tr>\
                    <td class='col-level'>" + (i == rs.Customers.length - 1 ? resources.Customer : cust.RoleName) + "</td>\
                    <td><span class='" + (i == rs.Customers.length - 1 ? "b" : "") + "'>" + cust.Username + "</span> (" + cust.CustId + ")</td>\
                </tr>";
        }

        this.elements.tblLineInfo.find("tbody").empty().append(html);
    };
    this.renderPT = function (rs) {
        var html = "";
        html += "<tr>\
                    <td class='col-level'>" + resources.Min + " - " + resources.Max + "</td>\
                    <td>" + (rs.IsSportsbookChanged ? rs.SportsbookMinPT2Value + " - " + rs.SportsbookMaxPT2Value : rs.SportsbookMinPTValue + " - " + rs.SportsbookMaxPTValue) + "</td>\
                    <td>" + (rs.IsNonSportChanged ? rs.NonsportsMinPT2Value + " - " + rs.NonsportsMaxPT2Value : rs.NonsportsMinPTValue + " - " + rs.NonsportsMaxPTValue) + "</td>\
                    <td>" + (rs.IsSportsbookChanged ? rs.SportsbookMinPTValue + " - " + rs.SportsbookMaxPTValue : "") + "</td>\
                    <td>" + (rs.IsNonSportChanged ? rs.NonsportsMinPTValue + " - " + rs.NonsportsMaxPTValue : "") + "</td>\
               </tr>";

        for (var i = 0; i < rs.CustPTs.length; i++) {
            var pt = rs.CustPTs[i];
            html += "<tr>\
                    <td class='col-level'>" + pt.RoleName + " [" + pt.RoleId + "]" + "</td>\
                    <td>" + (pt.IsSportsbookChanged ? pt.OldSportsbookPT : pt.SportsbookPT) + "</td>\
                    <td>" + (pt.IsNonSportChanged ? pt.OldNonSportPT : pt.NonSportPT) + "</td>\
                    <td>" + (pt.IsSportsbookChanged ? pt.SportsbookPT : "") + "</td>\
                    <td>" + (pt.IsNonSportChanged ? pt.NonSportPT : "") + "</td>\
                </tr>";
        }

        this.elements.tblPT.find("tbody").empty().append(html);
    };
    this.handleError = function (xhr) {
        var message = "";
        switch (xhr.status) {
            case 403:
                message = $.parseJSON(xhr.responseText).ErrorMessage;
                break;
            case 200:
                message = resources.DontHavePermission;
                break;
        }

        this.hasError = true;
        this.mustLockPage = true;
        showAlertMessage("div-error-message", message, 1);
        this.elements.resultTbls.addClass("display-none");
        this.completeLoadPT = true;
        this.completeLoadComm = true;
    };
};

$(document).ready(function () {
    var cn88ViewPTManager = new CN88ViewPTManager();
    cn88ViewPTManager.init();
});

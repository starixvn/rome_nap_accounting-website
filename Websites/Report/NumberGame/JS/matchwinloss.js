var matchwinloss = {
    company: {}
    , currency: {}
    , utils: {}
};

matchwinloss.init = function(type) {
    if (typeof type == "string" && type == "bettype") {
        this.servicePath = "../../../Services/WCF/Report/ServiceReportNumberGame.svc/";
    }
    else {
        this.servicePath = "../../Services/WCF/Report/ServiceReportNumberGame.svc/";
    }
    this.company.loadData();
    this.currency.loadData();
    $(document).ready(function() {
        matchwinloss.company.init();
    });
};

matchwinloss.company.init = function() {
    $("#selectCompany").change(function() {
        if ($(this).val() == "Licensee") {
            $(this).get(0).selectedIndex = $(this).get(0).selectedIndex + 1;
        }
        $("#hiddenCompnayFilter").val($("#selectCompany").val());
    });
    $("#selectCurrency").change(function(){
        $("#hiddenCurrencyFilter").val($("#selectCurrency").val());
    });
};

matchwinloss.company.loadData = function() {
    $.ajax({
        type: "POST",
        url: matchwinloss.servicePath + "GetSiteList",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: null,
        success: function (rs) {
            matchwinloss.company.rs = rs;
            matchwinloss.company.fillCompany();
        }
    });
};

matchwinloss.company.fillCompany = function() {
    $("#selectCompany").append("<option value='0'>All</option>");
    $("#selectCompany").append("<option value='-1'>IBC Group</option>");
    $("#selectCompany").append("<option value='Licensee' style='font-weight:bold; font-style:italic'>Licensee</option>");
    
    for (var i = 0; i < this.rs.length; ++i){
        $("#selectCompany").append("<option style='padding-left:16px' value='" + this.rs[i].Id + "'>"
        + (matchwinloss.utils.isIE() ? "&nbsp;&nbsp;&nbsp;&nbsp;" : "") + this.rs[i].Name + "</option>");
    }
    var isMatch = false;
    for (var j = 0; j < $("#selectCompany").get(0).childNodes.length; ++j) {
        if ($("#selectCompany").get(0).childNodes[j].nodeName == "OPTION") {//nodeName = "TEXT"
            if ($("#selectCompany").get(0).childNodes[j].value == $("#hiddenCompnayFilter").val()) {
                if (matchwinloss.utils.isIE() == true) {
                    $("#selectCompany").get(0).selectedIndex = j;
                }
                else {
                    $("#selectCompany").get(0).selectedIndex = j - 1;
                }
                isMatch = true;
                break;
            }
        }
    }
    if (!isMatch) {
        $("#selectCompany").get(0).selectedIndex = 0;
    }
};

matchwinloss.currency.loadData = function() {
    $.ajax({
        type: "POST",
        url: matchwinloss.servicePath + "GetCurrencyList",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: null,
        success: function (rs) {
            matchwinloss.currency.rs = rs;
            matchwinloss.currency.fillCurrency();
        }
    });
};

matchwinloss.currency.fillCurrency = function() {
    $("#selectCurrency").append("<option value='0'>All</option>");
    for (var i = 0; i < this.rs.length; ++i){
        $("#selectCurrency").append("<option value='" + this.rs[i].Value + "'>" + this.rs[i].Name + "</option>");
    }
    var isMatch = false;
    for (var j = 0; j < $("#selectCurrency").get(0).childNodes.length; ++j) {
        if ($("#selectCurrency").get(0).childNodes[j].nodeName == "OPTION") {//nodeName = "TEXT"
            if ($("#selectCurrency").get(0).childNodes[j].value == $("#hiddenCurrencyFilter").val()) {
                if (matchwinloss.utils.isIE() == true) {
                    $("#selectCurrency").get(0).selectedIndex = j;
                }
                else {
                    $("#selectCurrency").get(0).selectedIndex = j - 1;
                }
                
                isMatch = true;
                break;
            }
        }
    }
    if (!isMatch) {
        $("#selectCurrency").get(0).selectedIndex = 0;
    }
    var uusIndex = -1;
    for (var j = 0; j < $("#selectCurrency").get(0).childNodes.length; ++j) {
        if ($("#selectCurrency").get(0).childNodes[j].nodeName == "OPTION") {//nodeName = "TEXT"
            if ($("#selectCurrency").get(0).childNodes[j].innerHTML.indexOf("UUS") >= 0) {
                uusIndex = j;
                break;   
            }
        }
    }
    $("#selectCurrency").find("option[value='" + $("#selectCurrency").get(0).childNodes[uusIndex].value + "']").remove();
};

matchwinloss.utils.isIE = function () {
    if (navigator.appName == "Microsoft Internet Explorer") {
        return true;
    }
    return false;
};
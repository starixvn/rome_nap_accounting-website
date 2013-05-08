var mailboxArr = ["tbxToList", "tbxCCList", "tbxBCCList"];
var groupIDTemp = 1;
function IsValidEmail(emailAddress) {
    var emailRegex = new RegExp(/^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$/i);
    var valid = emailRegex.test($.trim(emailAddress));
    if (!valid) {
        return false;
    } else
        return true;
}

var MailingList = {
    Params: {
        ToList: '',
        CCList: '',
        BCCList: '',
        SiteID: '',
        SiteName: '',
        isUpdate: false // Update or insert new
    },
    orgLicenseeName: '', // for Popup question for update or no with licensee change data
    reloadId: 0,
    orgInputString: '', // for compare change to enabled submit
    currentInputString: '',
    Select: function () {
        var isSave = false;
        MailingList.currentInputString = '';
        MailingList.GetParam();
        if (MailingList.currentInputString.replace(";", "", "gi") != MailingList.orgInputString.replace(";", "", "gi")) {
            isSave = true;
            bCconfirm = '<tr><td> Do you want to save changes for licensee ' + MailingList.orgLicenseeName + ' ?</td></tr>';
            $('#tblconfirm').empty().append(bCconfirm);
            $("#tblconfirm").dialog({
                title: "",
                position: ['middle', 200],
                autoOpen: false,
                height: 100,
                width: 400,
                modal: true,
                buttons: {
                    "YES": function () {
                        $(this).dialog("close");
                        isSave = true;
                        MailingList.reloadId = $('#dllSite').val();
                        MailingList.Submit(true);
                    },
                    "NO": function () {
                        isSave = false;
                        $(this).dialog("close");
                        MailingList.BindData();
                    }
                },
                close: function (event, ui) {
                }
            });
            $('a.ui-dialog-titlebar-close').show();
            $("#tblconfirm").dialog("open");
        }
        //        $("#btnSubmit").attr('disabled', 'disabled');
        if (!isSave) {
            MailingList.BindData();
        }
    },
    BindData: function () {
        MailingList.ResetValue();
        MailingList.orgInputString = '';
        var siteId = $('#dllSite').val();
        if (siteId != -1) {
            var mailLicensee = null;
            MailingList.orgLicensee = $('#dllSite').text();
            $.each(_model.LicenseeList, function (i, v) {
                if (v.SiteID == siteId) {
                    mailLicensee = v;
                    MailingList.Params.SiteID = v.SiteID;
                    MailingList.orgLicenseeName = v.SiteName;
                    MailingList.Params.SiteName = v.SiteName;
                    MailingList.Params.isUpdate = v.IsMailExist ? true : false;
                }
            });

            MailingList.orgInputString += mailLicensee.ToList;
            MailingList.orgInputString += mailLicensee.CCList;
            MailingList.orgInputString += mailLicensee.BCCList;

            $("#tbxToList").tokenInput("clear").tokenInput("add", { id: mailLicensee.ToList, name: mailLicensee.ToList });
            $("#tbxCCList").tokenInput("clear").tokenInput("add", { id: mailLicensee.CCList, name: mailLicensee.CCList });
            $("#tbxBCCList").tokenInput("clear").tokenInput("add", { id: mailLicensee.BCCList, name: mailLicensee.BCCList });

            $("#content").show();
        }
    },
    GetParam: function () {
        MailingList.ResetValue();

        var lengthToList = $("#trToList").find("input").length;
        $("#trToList").find("input").each(function (i) {
            var value = $(this).val();
            if (IsValidEmail(value)) {
                if (i == lengthToList - 2) {
                    MailingList.Params.ToList += value;
                }
                else {
                    MailingList.Params.ToList += value + ';';
                }
                MailingList.currentInputString += value;
            }
        });

        var lengthCCList = $("#trCCList").find("input").length;
        $("#trCCList").find("input").each(function (i) {
            var value = $(this).val();
            if (IsValidEmail(value)) {
                if (i == lengthCCList - 2) {
                    MailingList.Params.CCList += value;
                }
                else {
                    MailingList.Params.CCList += value + ';';
                }
                MailingList.currentInputString += value;
            }
        });

        var lengthBCCList = $("#trBCCList").find("input").length;
        $("#trBCCList").find("input").each(function (i) {
            var value = $(this).val();
            if (IsValidEmail(value)) {
                if (i == lengthBCCList - 2) {
                    MailingList.Params.BCCList += value;
                    MailingList.currentInputString += value;
                }
                else {
                    MailingList.Params.BCCList += value + ';';
                    MailingList.currentInputString += value ;
                }
                
            }
        });
    },
    Submit: function (isSave) {
        MailingList.GetParam();
        $.ajax({
            type: "POST",
            url: window.rootUrl + "SpecialDeleteMailing/UpdateInsertMailingList",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(MailingList.Params),
            success: function (result) {
                var title = MailingList.Params.isUpdate ? "Update" : "Insert";
                if (result.ErrorMessage == '') {
                    setTimeout('$("#tblconfirm").dialog("close")', 4000);
                    var confirm = MailingList.Params.isUpdate ? '<div style="color: black; font-weight: bold; font-size: 15px; ">Update sucessfully.</div>' : '<div style="color: black; font-weight: bold; font-size: 15px; ">Insert sucessfully.</div>';
                    $('#tblconfirm').empty().append(confirm);
                    $("#tblconfirm").dialog({
                        title: title,
                        position: ['middle', 200],
                        autoOpen: false,
                        height: 100,
                        width: 400,
                        modal: true,
                        buttons: {

                        },
                        close: function (event, ui) {
                        }
                    });
                    $('a.ui-dialog-titlebar-close').show();
                    $("#tblconfirm").dialog("open");
                    window.location.href = window.rootUrl + "SpecialDeleteMailing/Index" + "?selectID=" + parseInt(isSave ? MailingList.reloadId : MailingList.Params.SiteID);
                }
                else {
                    $('#tblconfirm').empty().append(result.ErrorMessage);
                    $("#tblconfirm").dialog({
                        title: title,
                        position: ['middle', 200],
                        autoOpen: false,
                        height: 100,
                        width: 400,
                        modal: true,
                        buttons: {
                            "OK": function () {
                                $(this).dialog("close");
                            }
                        },
                        close: function (event, ui) {
                        }
                    });
                    $('a.ui-dialog-titlebar-close').show();
                    $("#tblconfirm").dialog("open");
                }

            },
            error: function () {
            },
            complete: function () {

                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    EnabledSubmit: function () {
        var temp = '';
        var enabled = false;
        $("#content").find("input").each(function () {
            var value = $(this).val();
            if (value != '' && value != 'N/A' && value != 'Submit') {
                var isValid = IsValidEmail(value);
                if (isValid) { // if mail is valid, compare with init value
                    temp += value + ';';
                    if (temp == MailingList.orgInputString) {
                        $("#btnSubmit").attr('disabled', 'disabled');
                    }
                    else {
                        $("#btnSubmit").removeAttr('disabled');
                    }
                }
                else {
                    $("#btnSubmit").attr('disabled', 'disabled');
                }
            }
        });

        if ($("#content").find(".invalidEmail").length > 0) {
            $("#btnSubmit").attr('disabled', 'disabled');
        }

    },
    ResetValue: function () {
        MailingList.Params.ToList = '';
        MailingList.Params.CCList = '';
        MailingList.Params.BCCList = '';
    }
};



head.ready(function (event) {
    for (var i = 0; i < mailboxArr.length; i++) {
        $('#' + mailboxArr[i] + '').tokenInput("", {
            theme: "facebook",
            allowFreeTagging: true,
            hintText: "",
            noResultsText: "",
            searchingText: "",
            isCheckEmail: true
//            enableSumitFunctionName: "MailingList.EnabledSubmit()"
        });
    }

    if (_isReload == 'True') {
        try { parent.BlockUI(); } catch (e) { }
        MailingList.Select();
        try { parent.jQuery.unblockUI(); } catch (e) { }
    }

    if (_groupId == groupIDTemp) {
        $("#trBCCList").show();
    }
});

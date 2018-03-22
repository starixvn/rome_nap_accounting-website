var expr = new RegExp('>[ \t\r\n\v\f]*<', 'g');
var t;
var CustomerList = {
    ShowMenuEdit: function (id) {
        var obj = $("#" + id);
        var parenttop = obj.parent().offset().top;
        var mtop = 0;
        if (parenttop > 500) {
            mtop = parenttop - (obj.height());
        }
        else {
            mtop = parenttop;
        }
        if (jQuery.browser.msie && jQuery.browser.version.indexOf("7.") == 0) {
            mtop += 14;
        }
        $('#popupedit').css({ top: mtop + "px" }).show().find("ul").html($("#" + id).html());

    },
    HideEditBox: function () {
        t = setTimeout("$('#popupedit').hide()", 200);
    },
    EditBoxHover: function () {
        clearTimeout(t);
    },
    getSelectedID: function () {
        var id = "";
        $("input[type='checkbox'][id!='chkAll']").each(function () {
            if (this.checked == true && this.id != '') {
                var length = this.id.length;
                var substring = this.id.substr(4, length);
                id += substring + ";";
            }
        });
        return id;
    },
    selectID: function () {
        var flag = true;
        //explore all check box except chkAll
        $("input[type='checkbox'][id!='chkAll']").each(function () {
            if (this.checked == false && !this.disabled)
                flag = false;
        });

        $("#chkAll").attr('checked', flag);
    },
    itemAllClicked: function () {
        $("input[type='checkbox']:enabled").attr('checked', $('#chkAll').is(':checked'));
    }
};

var pagerOpts = {
    callback: null,
    start_per_page: 50,
    step_per_page: 50,
    max_items_per_page: 200,
    items_per_page: 50,
    current_page: 1
};

function On_KeyPress(e) {
    if (!e) e = window.event;
    var code = (e.keyCode) ? e.keyCode : e.which;
    if (code == 13) {
        On_Submit();
        return false;
    }
    return true;
};
function on_Blur() {
    if (obj('txtSearch').value.length == 0)
        obj('txtSearch').value = 'All';
};
function On_Focus() {
    if (obj('txtSearch').value == 'All')
        obj('txtSearch').value = '';
};

$('.maincontent').delegate('.freegrid td .display', 'hover', function (event) {
    var obj = $(this);
    if (event.type === 'mouseenter') {
        var length = this.id.length;
        var content = this.id.substr(8, length);
            if (content.length > 20) {
                obj.tipsy({ gravity: "nw", html: true, title: function () { return content; } });
            }
    }
    else{
        obj.tipsy("hide");
        }
});
$('.maincontent').delegate('.freegrid tr', 'hover', function (event) {
    var obj = $('*[name=' + $(this).attr('name') + ']');
    if (event.type === 'mouseenter')
        obj.each(function () {
            $(this).addClass("yellow");
        })
    else
        obj.each(function () {
            $(this).removeClass("yellow");
        })
});

    function EditSportsBookSetting(roleid, custid, uplineid, username, siteName) {
        var url = "";
        var title = "";
        switch (roleid) {
            case 4:
                url = "EditSuper/EditSuperPage?custId=" + custid + '&uname=' + username ;
                title = "Edit Super Setting";
                break;
            case 3:
                url = "EditMaster/EditMasterPage?custId=" + custid + "&superId=" + uplineid + '&uname=' + username;
                 title = "Edit Master Setting";
                break;
            case 2:
                url = "EditAgent/EditAgentPage?custId=" + custid + "&masterId=" + uplineid + '&uname=' + username;
                 title = "Edit Agent Setting";
                break;
            default:
                url = "EditMember/EditMemberPage?custId=" + custid + "&agentId=" + uplineid + '&uname=' + username;
                 title = "Edit Member Setting";
                break;
        }
        showPopup(title, url + "&siteName=" + siteName, "", "", 900, 700, false, 2000, null);
    }


    function EditMinMaxPT(roleid, custid, uplineid, username) {
        var url = "";
        switch (roleid) {
            case 4:
                url = "EditSuperMinPt/EditSuperMinPtPage?&custId=" + custid + "&uname=" + username;
                break;
            case 3:
                url = "EditMasterMinMaxPt/EditMasterMinMaxPtPage?&custId=" + custid + "&uplineId=" + uplineid + '&uname=' + username;
                break;
            default:
                url = "EditAgentMinMaxPt/EditAgentMinMaxPtPage?&custId=" + custid + "&uplineId=" + uplineid + '&uname=' + username;
                break;
        }
        showPopup("Edit Min/Max PT", url, "", "", 800, 700, false, 2000, null);
    }

    function EditDCSByProduct(custId, productId, productName) {
        showPopup(productName, "", 0, 0, 880, 550, false, 2000, null);
        document.getElementById('productId').value = productId;
        document.getElementById('customerId').value = custId;
        document.getElementById("target").submit();
    }
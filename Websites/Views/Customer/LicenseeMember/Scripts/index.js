var cachedMembers = [];
var selectedAgentId = 0;
function CheckExistAgent() {
    $("#errMsg").html("");
    $("#tblmember").hide();
    $("#memberList").html("");
    var username = $("#txtAgent").val();
    if (username.trim().length === 0) {
        showAlertMessage('errMsg', "The agent is required.", 1);
        return;
    }
    var members = $("#txtMemberUsernames").val();
    if (members.trim().length === 0) {
        showAlertMessage('errMsg', "Please enter Member!", 1);
        return;
    }
    try { parent.BlockUI(); } catch (e) { };
    $.ajax({
        type: "GET",
        url: "CheckExistAgent?agentName=" + username,
        success: function (agentId) {
            if (agentId > 0) {
                GenerateMemberList(members, agentId);
            } else {
                showAlertMessage('errMsg', "The selected account is not a valid licensee dummy agent.", 1);
            }
        },
        error: function (err) {
            showAlertMessage('errMsg', "Internal Server Error.", 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { };
        }
    });
}

function GenerateMemberList(members, agentId) {
    var memberList = members.split(",").filter(onlyUnique);
    cachedMembers = memberList;
    selectedAgentId = agentId;
    var bodyRows = [];
    for (var i = 0; i < memberList.length; i++) {
        var memberName = memberList[i].trim();
        var row = { Columns: [] };
        row.Columns.push({ Name: 'No', Value: i+1 });
        row.Columns.push({ Name: 'Member', Value: memberName });
        row.Columns.push({ Name: 'Selected', Value: "x" });
        row.Columns.push({ Name: 'Status', Value: "Not created", CustomAttr: "id=\"status" + memberName + "\"" });
        bodyRows.push(row);
    }

    var gridOptions =
    {
        columns: [
            { HeaderText: "#", Width: 25, Name: "No", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Member", Width: 200, Name: "Member", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Selected", Width: 50, Name: "Selected", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Status", Width: 400, Name: "Status", HeaderAlign: "center", CellAlign: "left" }
        ],
        bodyRows: bodyRows,
        customPager: true,
        showDistinctRow: false
    };

    $('#gridMember').fanexGrid(gridOptions);
    $('#btnCreate').show();
}

function CreateMember() {
    if (cachedMembers.length === 0) {
        return;
    }

    var member = cachedMembers.shift();
    var request = {
        Recommend: selectedAgentId,
        Username: member
    };
    var statusDiv = $("#status" + member);
    statusDiv.removeClass();
    statusDiv.addClass("status-in-progress");
    statusDiv.html("In progress...");
    $.ajax({
        type: "POST",
        url: "InsertMemberToAgent",
        contentType: "application/json",
        data: JSON.stringify(request),
        success: function (result) {
            if (result.IsSuccess) {
                statusDiv.removeClass();
                statusDiv.addClass("status-created");
                statusDiv.html("Created");
            } else {
                statusDiv.removeClass();
                statusDiv.addClass("status-failed");
                statusDiv.html(result.ErrorMessage);
            }
            
        },
        error: function (err) {
            statusDiv.removeClass();
            statusDiv.addClass("status-failed");
            statusDiv.html("Failed to create account");
        },
        complete: function () {
            CreateMember();
        }
    });
}

function onlyUnique(value, index, self) {
    return self.indexOf(value) === index;
}

$(document).ready(function () {
    $("#txtMemberUsernames").off("change").on("change", function () {
        var memberUsername = $("#txtMemberUsernames");
        var memberUsernameValue = $.trim(memberUsername.val());
        if (memberUsernameValue.length > 0) {
            if (memberUsernameValue.indexOf("\n") >= 0) {
                memberUsernameValue = memberUsernameValue.replace(/\n/g, " ");
            }

            if (memberUsernameValue.indexOf("\r") >= 0) {
                memberUsernameValue = memberUsernameValue.replace(/\r/g, " ");
            }

            if (memberUsernameValue.indexOf("\t") >= 0) {
                memberUsernameValue = memberUsernameValue.replace(/\t/g, " ");
            }

            memberUsernameValue = memberUsernameValue.replace(/\s/g, ",");
            var usernameArray = memberUsernameValue.split(",").filter(item => item.length > 0);
            memberUsername.val(usernameArray.join(","));
        }
    });
});
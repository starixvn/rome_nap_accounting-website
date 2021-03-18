$(document).ready(function () {
    $('#txtDestinationMembers').off("change").on("change", function () {
        var destMembers = $.trim($(this).val());

        if (destMembers.length > 0) {
            if (destMembers.indexOf("\n") >= 0) {
                destMembers = destMembers.replace(/\n/g, " ");
            }

            if (destMembers.indexOf("\r") >= 0) {
                destMembers = destMembers.replace(/\r/g, " ");
            }

            if (destMembers.indexOf("\t") >= 0) {
                destMembers = destMembers.replace(/\t/g, " ");
            }

            destMembers = destMembers.replace(/\s/g, ",");
            var destMembersArray = destMembers.split(",").filter(item => item.length > 0);

            $(this).val(destMembersArray.join(","));
        }
    });

    $('#btnSubmit').click(function () {
        $('#errMsg').html('');
        var params = {
            SourceMember: $('#txtSourceMember').val(),
            DestinationMembers: $('#txtDestinationMembers').val()
        };

        if (params.SourceMember == undefined || params.SourceMember == '') {
            addAlertMessage('errMsg', 'Source Member is required.', 1);
            return;
        }

        if (params.DestinationMembers == undefined || params.DestinationMembers == '') {
            addAlertMessage('errMsg', 'Copy VR to members is required.', 1);
            return;
        }

        if (params.DestinationMembers.split(',').length > 500) {
            addAlertMessage('errMsg', 'The function can only copy up to 500 accounts per request.', 1);
            return;
        }

        parent.BlockUI();

        $.ajax({
            type: "POST",
            url: "Copy",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(params),
            success: function (result) {
                if (result.ErrorCode > 0) {
                    addAlertMessage('errMsg', result.ErrorMessage, 1);
                }

                for (var i = 0; i < result.length; i++)
                    addAlertMessage('errMsg', result[i].ErrorMessage, result[i].ErrorCode == 0 ? 0 : 1);
            },
            error: function () {
            },
            complete: function () {
                parent.jQuery.unblockUI();
            }
        });
    });
});

function addAlertMessage(containerid, msg, type) {

    switch (type) {
        case 0:
            document.getElementById(containerid).innerHTML += "<div class='alertboxscc'><div>" + msg + "</div></div>";
            break;
        case 1:
            document.getElementById(containerid).innerHTML += "<div class='alertboxerr'><div>" + msg + "</div></div>";
            break;
        case 2:
            document.getElementById(containerid).innerHTML += "<div class='alertboxwrn'><div>" + msg + "</div></div>";;
            break;
        default:
            break
    }

    window.scroll(0, 0);
    parent.scroll(0, 0);
}
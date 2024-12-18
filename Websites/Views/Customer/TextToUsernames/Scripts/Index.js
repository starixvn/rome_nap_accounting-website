var textToUsernames = {
    init: function () {
        $('#copy-tipsy').tipsy({
            html: true,
            gravity: 'nw'
        });

        $('#tab-ttu-text').keyup(function (e) {
            var code = e.keyCode || e.which;
            if (code === 13) {
                textToUsernames.getUsernames();
            }
        });

        $("#tab-ttu-btnSubmit").click(function () { textToUsernames.getUsernames(); });
        $("#copy-valids").click(function () {
            var copyText = $(".row-usernames");
            var text = copyText.html();

            if (navigator.clipboard) {
                navigator.clipboard.writeText(text);
            } else {
                const input = document.createElement('textarea');
                input.value = text;
                document.body.appendChild(input);
                input.select();
                document.execCommand('copy');
                document.body.removeChild(input);
            }

            $('#copy-tipsy').tipsy('show');
            setTimeout(function () { $('#copy-tipsy').tipsy('hide'); }, 3000);
        });

        $(".tab-ttu-icon-collapse-expand").click(function () {
            var $this = $(this);
            var $filter = $this.parent().find('>div');

            if ($this.hasClass("icon-down")) {
                $this.removeClass("icon-down");
                $this.addClass("icon-up");
                $filter.addClass('hidden');
            } else {
                $this.removeClass("icon-up");
                $this.addClass("icon-down");
                $filter.removeClass('hidden');
            }
        });
    },
    getUsernames: function () {
        parent.BlockUI();
        $('#tab-ttu-errMsg').html('');
        var text = $("#tab-ttu-text").val();
        if (text === "") {
            showAlertMessage('tab-ttu-errMsg', "Please input text", 1);
            parent.jQuery.unblockUI();
            return;
        }

        var request = {
            Text: text
        };

        $("#tab-ttu-usernames").addClass("hidden");
        $.ajax({
            type: "POST",
            url: "../TextToUsernames/GetUsernames",
            contentType: "application/json",
            data: JSON.stringify(request),
            success: function (data) {
                if (data.ValidUsernames) {
                    $(".tbl-valid-usernames .row-usernames").html(data.ValidUsernames);
                    $(".tbl-valid-usernames").removeClass("hidden");
                } else {
                    $(".tbl-valid-usernames").addClass("hidden");
                }

                if (data.InvalidUsernames.length === 0) {
                    $(".tbl-invalid-usernames").addClass("hidden");
                }
                else {
                    var row = '';
                    for (var i = 0; i < data.InvalidUsernames.length; i++) {
                        var u = data.InvalidUsernames[i];
                        row += '<tr><td>' + u.Username + '</td><td>' + u.Reason + '</td></tr>';
                    }

                    $(".tbl-invalid-usernames tbody").empty().append(row);
                    $(".tbl-invalid-usernames").removeClass("hidden");
                }

                $("#tab-ttu-usernames").removeClass("hidden");
            },
            complete: function () {
                parent.jQuery.unblockUI();
            },
            error: function (xhr) {
                if (xhr.status === 403) {
                    showAlertMessage("tab-ttu-errMsg", $.parseJSON(xhr.responseText).ErrorMessage, 1);
                }
            }
        });
    }
};

$(document).ready(function () {
    textToUsernames.init();
});

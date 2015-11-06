var virtualRateController;
$(document).ready(function () {
    virtualRateController.initPage();
});

var VirtualRateController = function () {
    var self = this;
    self.initPage = function () {
        self.renderReferrences();
    };

    self.renderReferrences = function () {
        var contentHtml = '';
        var referrenceCount = sportBetTypesReferences.Count;
        for (var i = 0; i < sportBetTypesReferences.length; i++) {
            var sportType = sportBetTypesReferences[i];
            contentHtml += '<div class="sport">\
                    <div class="header-sport' + (i == 0 ? ' first-child' : '') + '">\
                        <span class="iconBox expand-icon"></span>' + sportType.Name +
                    '</div>\
                <div>\
                    <table>\
                        <thead>\
                            <tr>\
                                <th class="header bettypeCol">' +
                                    resources.Bet_Type +
                                '</th>\
                                <th class="header" colspan="2">' +
                                    resources.Followed_By +
                                '</th>\
                            </tr>\
                        </thead>\
                        <tbody>';
            var betTypeLength = sportType.SubTypes.length;
            for (var j = 0; j < betTypeLength; j++) {
                var betType = sportType.SubTypes[j];
                var betTypeReferrenceCount = betType.SourceReferences.length;
                for (var k = 0; k < betTypeReferrenceCount; k++) {
                    var referrence = betType.SourceReferences[k];
                    contentHtml += '<tr>' +
                                        (k == 0 ? '<td class="bettype" rowspan="' + betTypeReferrenceCount + '">' + betType.Name + '</td>' : '') +
                                        '<td class="right followedByCol">' + referrence.SportTypeRef.Name + '</td>\
                                        <td class="left followedByCol">' + referrence.BetTypeRef.Name + '</td>\
                                    </tr>';
                }
            }

            contentHtml += '</tbody>\
                        </table>\
                    </div>\
                </div>';
        }

        $("#content").empty().append(contentHtml);
        $(".header-sport").click(function () {
            self.toggleSport(this);
        });
    };
    self.toggleSport = function (e) {
        var t = $(e).next();
        if (t.hasClass("hide")) {
            t.removeClass("hide");
            e.children[0].className = "iconBox expand-icon";
        } else {
            t.addClass("hide");
            e.children[0].className = "iconBox collapse-icon";
        }
    }
};

var virtualRateController = new VirtualRateController();
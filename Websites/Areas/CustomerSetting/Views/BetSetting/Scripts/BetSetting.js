function LoadSB() {
    Fanex.CustomerSetting.Core.BlockUI();
    document.location.href = toUrl('Sportsbook/Index/' + custId + '/' + uplineId + '/' + userId + '/' + siteName);
};

function LoadRacing() {
    Fanex.CustomerSetting.Core.BlockUI();
    document.location.href = toUrl('Racing/Index/' + custId + '/' + uplineId + '/' + userId + '/' + siteName);
};
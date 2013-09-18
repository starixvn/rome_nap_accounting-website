function LoadSB() {
    Fanex.CustomerSetting.Core.BlockUI();
    document.location.href = toUrl('Sportsbook/Index/' + custId + '/' + uplineId + '/' + userId + '/' + siteName);
};

function LoadRacing() {
    Fanex.CustomerSetting.Core.BlockUI();
    document.location.href = toUrl('RacingBetting/Index/' + custId + '/' + uplineId + '/' + userId + '/' + siteName);
};

function LoadVirtualSports() {
    Fanex.CustomerSetting.Core.BlockUI();
    document.location.href = toUrl('VirtualSportsBetting/Index/' + custId + '/' + uplineId + '/' + userId + '/' + siteName);
};

function LoadLiveCasino() {
    Fanex.CustomerSetting.Core.BlockUI();
    document.location.href = toUrl('LiveCasinoBetting/Index/' + custId + '/' + uplineId + '/' + userId + '/' + siteName);
};
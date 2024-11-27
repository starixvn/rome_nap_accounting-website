
function SwitchTab(tabName) {
    $('#tabs').find('li').removeClass('current');
    $('div.tab_content').hide();
    $('#' + tabName).addClass('current');
    $('#' + tabName + '_content').show();
}

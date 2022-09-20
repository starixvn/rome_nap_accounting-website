function SwitchTab(type) {
    $('#tabs').find('li').removeClass('current');
    $('div[id^="tab_content_"]').hide();
    $('#tab_' + type).addClass('current');
    $('#tab_content_' + type).show();
}
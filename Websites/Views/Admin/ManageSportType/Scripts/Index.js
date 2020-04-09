function SwitchTab(id, contentId) {
    $('#tabs').find('li').removeClass('current');
    $('div[id^="tab_content_"]').hide();
    $('#' + id).addClass('current');
    $('#' + contentId).show();
}
var requiredText = "此為必填欄位";
$('#memNickComfirm').submit(function () {        //送出表單前觸發的事件
    //表單為空時無法post

    checkform("#memNick", true, requiredText);
    if (!check)
        return false

    if (!flag)
        return false
    return true;
});
$('#i_newNick').keyup(function () {
    if ($('#i_newNick').val().length > 15) {
        checkform("#i_newNick", false, "暱稱不可超過15字");
    } else {
        clearErrText("#i_newNick");
    }
    checkform("#i_newNick", true, requiredText);
});

function checkform(id, required, msg) {
    if (required) {
        if ($(id).val() == "") {
            $(id).addClass('is-invalid');
            $(id + '~div').addClass('invalid-feedback').text(msg);
            check = false;
        }
    } else {
        $(id).addClass('is-invalid');
        $(id + '~div').addClass('invalid-feedback').text(msg);
        flag = false;
    }
}
function clearErrText(id) {
    $(id).removeClass('is-invalid').addClass('is-valid');
    $(id + '~div').removeClass('invalid-feedback').addClass('valid-feedback').text('');
    flag = true;
}
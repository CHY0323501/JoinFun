//更換鄉鎮市區選單
$('#memCounty').change(function () {
    $('#memDistrict').empty();
    $.ajax({
        type: "Get",
        url: "http://10.10.3.105/api/getDistrict?countyNo=" + $('#memCounty').val(),
        success: function (d) {
            console.log(d);
            d.forEach(function (currentValue, index, array) {
                if (index % 2 != 0) {
                    $('#memDistrict').append("<option value='" + array[index - 1] + "'>" + currentValue + "</option>");
                }
            });
        }
    });
});

////暱稱及電子信箱驗證
//var emailRegex = /^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z]+$/;         //寫正規表達式須/^開頭，$/結尾；\w表示至少一個大小寫英文、數字、底線
//var flag = false, check=false ;
//var requiredText = "此為必填欄位";
//$('#confirm').submit(function () {        //送出表單前觸發的事件
//    //表單為空時無法post

//    checkform("#Email", true, requiredText);
//    checkform("#memNick", true, requiredText);
//    if (!check) 
//        return false

//    if (!flag) 
//        return false
//     return true
//});
////即時驗證
//$('#Email').keyup(function () {
//    if (!emailRegex.test($('#Email').val())) {                //使用預設regular expression的test函數來驗證，通過回傳true
//        checkform("#Email", false, "Email格式錯誤");
//    } else {
//        clearErrText("#Email");
//    }
    
//    checkform("#Email", true, requiredText);

//});
//$('#memNick').keyup(function () {
//    if ($('#memNick').val().length > 15) {
//        checkform("#memNick", false, "暱稱不可超過15字");
//    } else {
//        clearErrText("#memNick");
//    }
//    checkform("#memNick", true, requiredText);
//});

//function checkform(id, required, msg) {
//    if (required) {
//        if ($(id).val() == "") {
//            $(id).addClass('is-invalid');
//            $(id + '~div').addClass('invalid-feedback').text(msg);
//            check = false;
//        }
//    } else {
//        $(id).addClass('is-invalid');
//        $(id + '~div').addClass('invalid-feedback').text(msg);
//        flag = false;
//    }
//}
//function clearErrText(id) {
//    $(id).removeClass('is-invalid').addClass('is-valid');
//    $(id + '~div').removeClass('invalid-feedback').addClass('valid-feedback').text('');
//    flag = false;
//}
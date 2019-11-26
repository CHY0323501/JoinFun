//更換鄉鎮市區選單
$('#county').change(function () {
    $('#district').empty();
    $.ajax({
        type: "Get",
        url: "http://localhost:54129/api/getDistrict?countyNo=" + $('#county').val(),
        success: function (d) {
            d.forEach(function (currentValue, index, array) {
                if (index % 2 != 0) {
                    $('#district').append("<option value='" + array[index - 1] + "'>" + currentValue + "</option>");
                }
            });
        },
    });
});

//更換活動類別說明
$('#classUl>li').click(function (e) {
    $('#classDescrip').empty();
    var value;
    switch ($(this).text()) {
        case "活動":
            value = "cls001";
            break;
        case "休閒":
            value = "cls002";
            break;
        case "商務":
            value = "cls003";
            break;
    }
    $.ajax({
        type: "Get",
        url: "http://localhost:54129/api/getClassDescription?id=" + value,
        success: function (d) {
            console.log(d);
            d.forEach(function (currentValue) {
                $('#classDescrip').val(currentValue);
            });
        }
    });
    $('#classId').val(value);
    if (e.target.id == "cls001") {
        $('#classUl>li').removeClass('bg-warning').addClass('bg-light');
        $("#cls001").removeClass('bg-light').addClass("bg-warning");
    }
    else if (e.target.id == "cls002") {
        $('#classUl>li').removeClass('bg-warning').addClass('bg-light');
        $("#cls002").removeClass('bg-light').addClass("bg-warning");
    } else {
        $('#classUl>li').removeClass('bg-warning').addClass('bg-light');
        $("#cls003").removeClass('bg-light').addClass("bg-warning");
    }

});

//點圖上傳圖片
$('#uploadMain').click(function () {
    $('#picture').click();
});
$('#addPic').click(function () {
    $('#contentPic').click();
});

//預覽主題圖片
$('#picture').on("change", function () {
    var file = this.files[0];
    if (this.files && file) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#uploadMain').attr("src", e.target.result);
        }
        reader.readAsDataURL(file);
    }
});
//預覽其他圖片(預設第一張)
$('#uploadPics>input').on("change", function (e) {
    var file = this.files[0];
    if (this.files && file) {
        var reader = new FileReader();
        reader.onload = function (evnt) {
            if (e.target.id == "contentPic")
                $('#addPic').attr("src", evnt.target.result).addClass('w-50');
        }
        reader.readAsDataURL(file);
    }
});

//增加上傳的照片
$('#addFile').click(function () {
    var n = $('#uploadPics').children().length;
    if (n < 6) {
        $('#uploadPics').append('<img src="/Assets/pages/images/add.png" id="addPic' + n / 2 + '" ' + 'class="img-fluid bg-transparent mr-1 mt-1" style="width:150px;height:auto" />')
            .append("<input id='contentPic" + n / 2 + "' " + "type='file' name='picture' class='form-control' style='display:none' />");
    }
    if (n < 4)
        $('#addPic1').click(function () {
            $('#contentPic1').click();
        });
    else if (n >= 4)
        $('#addPic2').click(function () {
            $('#contentPic2').click();
        });
    //預覽增加的圖片
    $('#uploadPics>input').on("change", function (e) {
        var file = this.files[0];
        if (this.files && file) {
            var reader = new FileReader();
            reader.onload = function (evnt) {
                if (e.target.id == "contentPic1")
                    $('#addPic1').attr("src", evnt.target.result).addClass('w-50');
                if (e.target.id == "contentPic2")
                    $('#addPic2').attr("src", evnt.target.result).addClass('w-50');
            }
            reader.readAsDataURL(file);
        }
    });
});

$('#createAct').click(function () {
    var ValidStatus = $('#newApply').valid();
    console.log(ValidStatus);
    if (ValidStatus == false) {
        return false;
    }
});


$('#buildUp').click(function () {
    //alert("請確認填寫的資料是否正確，揪團建立後僅有【揪團主題】、【揪團內容】、【付款方式】、【預算上限】可供編輯，若需刪除揪團請聯繫管理員！");

    if ($('#datepicker').val() == "") {
        $('#datepicker').addClass("is-invalid");
        $('#datepicker').prev().addClass('createTime').text('請選擇活動日期');
    }
    else {
        $('#datepicker').removeClass("is-invalid").addClass("is-valid");
        $('#datepicker').prev().text("");
    }
    if ($('#timepicker').val() == "") {
        $('#timepicker').addClass("is-invalid");
        $('#timepicker').prev().addClass('createTime').text('請選擇活動時間');
    } else {
        $('#timepicker').removeClass("is-invalid").addClass("is-valid");
        $('#timepicker').prev().text("");
    }
    if ($('#datepicker2').val() == "") {
        $('#datepicker2').addClass("is-invalid");
        $('#datepicker2').prev().addClass('createTime').text('請選擇截止日期');
    }
    else {
        $('#datepicker2').removeClass("is-invalid").addClass("is-valid");
        $('#datepicker2').prev().text("");
    }
    if ($('#timepicker2').val() == "") {
        $('#timepicker2').addClass("is-invalid");
        $('#timepicker2').prev().addClass('createTime').text('請選擇截止時間');
    } else {
        $('#timepicker2').removeClass("is-invalid").addClass("is-valid");
        $('#timepicker2').prev().text("");
    }
    if ($('#actRoad').val() == "") {
        $('#actRoad').addClass("is-invalid");
    }
    if ($('#actDescription').val() == "") {
        $('#actDescription').addClass("is-invalid");
    }
});

$('#actRoad').keyup(function () {
    if ($('#actRoad').val() != "") {
        $('#actRoad').removeClass("is-invalid");
    }
});

$('#actDescription').keyup(function () {
    if ($('#actDescription').val() != "") {
        $('#actDescription').removeClass("is-invalid");
    }
});
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//DateTimePicker for create.cshtml
var today = new Date();
today.setHours(23);
today.setMinutes(59);
today.setSeconds(59);
var upper = new Date();
upper.setFullYear(today.getFullYear() + 1);
upper.setDate(today.getDate());

// Date picker only
$('#datepicker').datetimepicker({
    format: 'YYYY-MM-DD',
    minDate: today,
    maxDate: upper
});
$('#datepicker2').datetimepicker({
    format: 'YYYY-MM-DD',
    minDate: today
});
// Time picker only
$('#timepicker').datetimepicker({
    format: 'LT'
});
$('#timepicker2').datetimepicker({
    format: 'LT'
});
var current, picked, chosen;
//判斷是否輸入活動日期與時間  
$('#startDate input').blur(function () {
    current = new Date();
    current = current.getHours() * 3600 + current.getMinutes() * 60 + current.getSeconds();
    picked = new Date($('#timepicker').data('DateTimePicker').date());
    picked = picked.getHours() * 3600 + picked.getMinutes() * 60 + picked.getSeconds();
    chosen = new Date($('#datepicker').data('DateTimePicker').date());
    chosen.setHours(23);
    chosen.setMinutes(59);
    chosen.setSeconds(59);
    switch ($(this).attr("id")) {
        case "datepicker":
            if ($('#datepicker').val() == "") {
                $('#datepicker').addClass("is-invalid");
                //$('#datepicker').addClass("invalid-feedback").text('請選擇活動日期');
                $('#datepicker').prev().addClass('createTime').text('請選擇活動日期');
            } else {
                $('#datepicker').removeClass("is-invalid").addClass("is-valid");
                $('#datepicker').prev().text("");
            }
            break;
        case "timepicker":
            if ($('#timepicker').val() == "") {
                $('#timepicker').addClass("is-invalid");
                $('#timepicker').prev().addClass('createTime').text('請選擇活動時間');
            } else if ($('#datepicker').val() == "") {
                $('#timepicker').addClass("is-invalid");
                $('#timepicker').prev().addClass('createTime').text('請先選擇活動日期');
            } else if (Date.parse(chosen) == Date.parse(today) && (picked - current) < 60 * 60 * 6 && (picked - current) >= 0) {
                $('#timepicker').addClass("is-invalid");
                $('#timepicker').prev().text("開團時間需與現在間隔6小時以上");
            } else if (Date.parse(chosen) == Date.parse(today) && (picked - current) <= 0) {
                $('#timepicker').addClass("is-invalid");
                $('#timepicker').prev().addClass("ml-1").text("開團時間超出範圍");
            }
            else {
                $('#timepicker').removeClass("is-invalid").addClass("is-valid");
                $('#timepicker').prev().text("");
            }
            break;
    }
    $('#datepicker2').data('DateTimePicker').maxDate(chosen);
});

//判斷是否輸入報名截止日期與時間
var endTime, endDate;
$('#deadLine input').blur(function () {
    endTime = new Date($('#timepicker2').data('DateTimePicker').date());
    endTime = endTime.getHours() * 3600 + endTime.getMinutes() * 60 + endTime.getSeconds();
    endDate = new Date($('#datepicker2').data('DateTimePicker').date());
    endDate.setHours(23);
    endDate.setMinutes(59);
    endDate.setSeconds(59);
    switch ($(this).attr("id")) {
        case "datepicker2":
            if ($('#datepicker2').val() == "") {
                $('#datepicker2').addClass("is-invalid");
                $('#datepicker2').prev().text('請選擇截止日期');
                //}else if (Date.parse(deadLine) > Date.parse(actDate)) {
                //    $('#datepicker2').addClass("is-invalid");
                //    $('#datepicker2').prev().text("截止日不得超過活動日"); 
                //}else if (Date.parse(deadLine) < Date.parse(today)) {
                //    $('#datepicker2').addClass("is-invalid");
                //    $('#datepicker2').prev().text("截止日不可早於今天"); 
            } else {
                $('#datepicker2').removeClass("is-invalid").addClass("is-valid");
                $('#datepicker2').prev().text("");
            }
            break;
        case "timepicker2":
            $('#timepicker2').blur(function () {
                if ($('#timepicker2').val() == "") {
                    $('#timepicker2').addClass("is-invalid");
                    $('#timepicker2').prev().text('請選擇截止時間');
                } else if ($('#datepicker2').val() == "") {
                    $('#timepicker2').addClass("is-invalid");
                    $('#timepicker2').prev().addClass('createTime').text('請先選擇截止日期');
                } else if (Date.parse(endDate) == Date.parse(chosen) && (picked - endTime) < 60 * 60 * 3 && (picked - endTime) >= 0) {
                    $('#timepicker2').addClass("is-invalid");
                    $('#timepicker2').prev().text("與開團時間需間隔3小時以上");
                } else if (Date.parse(endDate) == Date.parse(chosen) && (picked - endTime) <= 0) {
                    $('#timepicker2').addClass("is-invalid");
                    $('#timepicker2').prev().text("截止時間超出範圍");
                } else {
                    $('#timepicker2').removeClass("is-invalid").addClass("is-valid");
                    $('#timepicker2').prev().text("");
                }
            });
            break;
    }
});

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Javascript for 訊息中心
//全選或取消全選狀態
$("#clickAll").click(function () {
    if ($("#clickAll").prop("checked")) {
        $("#activeTbody input[name='clickOne']").each(function () {
            $(this).prop("checked", true);
        });
    } else {
        $("#activeTbody input[name='clickOne']").each(function () {
            $(this).prop("checked", false);
        });
    }
});
$("#delClkAll").click(function () {
    if ($("#delClkAll").prop("checked")) {
        $("#delTbody input[name='clickOne']").each(function () {
            $(this).prop("checked", true);
        });
    } else {
        $("#delTbody input[name='clickOne']").each(function () {
            $(this).prop("checked", false);
        });
    }
});

//切換資源回收桶與一般訊息，click後去掉checkbox選取狀態
$('#mList>li').click(function () {
    $("input[name='clickOne']").prop("checked", false)
    $("input[name='clickAll']").prop("checked", false)
    $("input[name='delClkAll']").prop("checked", false)
    switch ($(this).attr("id")) {
        case "messages":
            //$('#recycleOne').hide();
            //$('#activeOne').show();
            $('#recycle').removeClass("font-weight-bold");
            $(this).addClass("font-weight-bold");
            break;
        case "recycle":
            //$('#activeOne').hide();
            //$('#recycleOne').show();
            $('#messages').removeClass("font-weight-bold");
            $(this).addClass("font-weight-bold");
            break;
    }
});
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
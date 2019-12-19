//更換鄉鎮市區選單
$('#county').change(function () {
    $('#district').empty();
    $.ajax({
        type: "Get",
        url: "http://twhome217.asuscomm.com/api/getDistrict?countyNo=" + $('#county').val(),
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
        url: "http://twhome217.asuscomm.com/api/getClassDescription?id=" + value,
        success: function (d) {
            console.log(d);
            d.forEach(function (currentValue) {
                $('#classDescrip').val(currentValue);
            });
        }
    });
    $('#actClassId').val(value);
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
    $('#topPic').click();
});
$('#addPic').click(function () {
    $('#contentPic').click();
});

//預覽主題圖片
$('#topPic').on("change", function () {
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


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
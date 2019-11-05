
//$(document).ready(function () {
//    $('#County').change(function () { ChangeCounty(); });
//});

//function ChangeCounty() {
//    var selectedValue = $('#County option:selected').val();
//    //if ($.trim(selectedValue).length > 0) {
//        GetDistrict(selectedValue);
//    //}
//} 

//function GetDistrict(CountyNo) {
//    $.ajax({
//        url: '@Url.Action("District", "Display")',/*"http://localhost:54129/api/getDistrict" + $('#County').val(),*/
//        data: { CountyNo: CountyNo },
//        type: 'get',
//        cache: false,
//        async: false,
//        dataType: 'json',
//        success: function (data) {
//            if (data.length > 0) {
//                $('#District').empty();
//                $('#District').append($('<option></option>').val('').text('請選擇區域'));
//                $.each(data, function (i, item) {
//                    $('#District').append($('<option></option>').val(item.key).text(item.Value));
//                });
//            }
//        }
//    });
//}

//http://kevintsengtw.blogspot.com/2012/07/aspnet-mvc-3.html
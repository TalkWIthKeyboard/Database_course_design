$.ajax({
    url: "/Test/GetPersonInfo",
    type: "POST",
    dataType: "json",
    data: {},
    success: function (data) {

        $('#friendContent').html(data[0].Count);

    }
})
$(
    function () {
        var $body = $('body'),
            $window = $(window),
            $title = $('title');

        if ($title.html() !== 'login') {
            $body.height($window.height() - 50);
        } else {
            $body.height($window.height());
        }
        //ËÑË÷
        $('#search').keyup(function (event) {
            if (event.keyCode === 13) {
                var list = [];
                list.push($('#search').val());
                StandardPost('/Home/Search', {'search': $('#search').val()});
            }
        });

        $('#showFriends').click(function () {
            var $friends = $('#friends');
            if ($friends.css('width') === '0px') {
                $friends.animate({width: '82%'}, 800);
            } else {
                $friends.animate({width: '0' }, 800);
            }
        });
    }

)

function StandardPost(url, args) {
    var form = $("<form method='post'></form>");
    form.attr({ "action": url });
    for (arg in args) {
        var input = $("<input type='hidden'>");
        input.attr({ "name": arg });
        input.val(args[arg]);
        form.append(input);
    }
    form.submit();
}

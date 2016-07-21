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

        $('#showFriends').click(function () {
            var $friends = $('#friends');
            if ($friends.css('height') === '-17px') {
                $friends.css('height', 'calc(100% - 50px)');
            } else {
                $friends.css('height', '0');
            }
        });
        //搜索
        $('#search').keyup(function (event) {
            if (event.keyCode === 13) {
                StandardPost('/Home/Search', {'search': $('#search').val()});
            }
        });

        /*消息提示*/
        $('#messageList li div a').each(function (index, el) {
            $(el).click(function (event) {
                var agree = true;
                if ($(el).attr('class') === 'no') {
                    agree = false;
                }

                var type = $(el).parent().parent().attr('type');
                var $p = $(el).parent().prevAll('p');
                var userId = $p.attr('userId');
                var repId = $p.attr('repId');
                var fileId = $p.attr('fileId');
                var messageId = $(el).parent().parent().attr('messageId');
                alert(messageId);

                $.post('/Home/AcceptMessage', {'messageId': messageId, 'messageType': type, 'userId': userId, 'repId': repId, 'fileId': fileId, 'agree': agree });

                $(el).parent().parent()
                    .animate({ 'margin-left': '-100%' }, 400, function () {
                        $(this).remove();
                    });
            });
        });

        $('.repository').each(function (index, el) {
            $(el).click(function (event) {
                var rID = $(el).attr('repositoryId');
                StandardPost('/Home/Repository', { 'rID': rID });
            })
        });


        $('.friend').each(function (index, el) {
            $(el).click(function (event) {
                var rID = $(el).attr('friend_id');
                alert(rID);
                StandardPost('/Home/OverView', { 'friend_id': rID });
            })
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

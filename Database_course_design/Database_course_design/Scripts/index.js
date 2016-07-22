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

        // 起始动画效果
        $('.card').each(function (index, el) {
           $(el).delay(index * 200).animate({'margin-top': '16'}, 800);
        });
        
        // star,fork按钮点击变色
        $('.card .list-head a[type="btn"]').each(function (index, el) {

                $(el).click(function (event) {
                    var $i = $(el).find('i');
                    if ($i.css('color') !== 'rgb(133, 133, 133)') {
                        $i.css('color', '#858585');
                    } else {
                        $i.css('color', '#F4C600');
                    }
                    
                    var rID = $(el).parent().attr('repositoryId');

                    if ($(el).attr('class') === 'star') {
                        StandardPost('/Home/Star', { 'rID': rID });
                    } else {
                        StandardPost('/Home/Fork', { 'rID': rID });
                    }
                });
        });

        // 跳转
        $('#menu a').each(function (index, el) {
            $(el).click(function (event) {
                var office = 1;
                if ($(el).attr('office') === '0') {
                    office = 0;
                }
                StandardPost('/Home/Selected', { 'office': office });
            })
        });

        // 弹窗
        $('.createRepository').click(function () {
            $('#fill').show(0);
            $('#popup').addClass('show');
        });

        // 复选框
        $btns = $('#popup form .btn');
        $('#popup form .btn').each(function (index, el) {
            $(el).click(function (event) {
                $(el).css({ 'background-color': '#3EF1C4', 'color': 'white' });
                $($btns[1 - index]).css({ 'background-color': '#F3F3F3', 'color': 'black' });
            })
        });

        // 关弹窗
        $('#popup form div button').click(function (event) {
            $('#popup').removeClass('show');
            $('#fill').delay(200).hide(0);

            $btns.each(function (index, el) {
                $(el).css({ 'background-color': '#F3F3F3', 'color': 'black' });
            })
        });

        // 创建仓库传值
        $('#popup form .submitDiv input').click(function () {
            var repositoryName = $('#popup form input[name=repositoryName]').val();
            var repositoryDescription = $('#popup form textarea').val();
            var isPublic = true;
            if ($('#popup form :eq(4)') !== 'rgb(255, 255, 255)') {
                isPublic = false;
            }
            var tag = $('#popup form input[name=tag]').val();
            $('#popup form')[0].reset();

            StandardPost('/Home/CreateFork', {
                'repositoryName': repositoryName, 'repositoryDescription': repositoryDescription,
                'isPublic': isPublic, 'tag': tag});
        })


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

        //历史浏览
        $('.history').each(function (index, el) {
            $(el).click(function (event) {
                var rID = $(el).attr('history');
                StandardPost('/Home/History', { 'rID': rID });
            })
        });

        //个人收藏
        $('.starPage').each(function (index, el) {
            $(el).click(function (event) {
                var rID = $(el).attr('star');
                StandardPost('/Home/StarFork', { 'rID': rID });
            })
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

                $.post('/Home/AcceptMessage', {'messageId': messageId, 'messageType': type, 'userId': userId, 'repId': repId, 'fileId': fileId, 'agree': agree });

                $(el).parent().parent()
                    .animate({ 'margin-left': '-100%' }, 400, function () {
                        $(this).remove();
                    });
            });
        });

        $('.repository').each(function (index, el) {
            $(el).click(function (event) {
                var rID = $(el).parent().attr('repositoryId');
                
                StandardPost('/Home/Repository', { 'rID': rID });
            })
        });



        $('.friend').each(function (index, el) {
            $(el).click(function (event) {
                var rID = $(el).attr('friend_id');
               
                StandardPost('/Home/OverView', { 'friend_id': rID });
            })
        });
 
    }
);

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
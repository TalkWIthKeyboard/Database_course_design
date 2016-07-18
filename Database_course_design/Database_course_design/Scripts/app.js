$(
    function () {
        var $body = $('body'),
            $window = $(window),
            $title = $('title');

        if ($title.html() !== 'login') {
            $body.height($window.height() - 50);
            alert(1);
        } else {
            $body.height($window.height());
        }
    }
)

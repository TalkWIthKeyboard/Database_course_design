$(
    function () {
        var $body = $('body'),
            $window = $(window),
            $title = $('title');

        if ($title.html() !== '��¼') {
            $body.height($window.height() - 50);
        } else {
            $body.height($window.height());
        }
    }
)

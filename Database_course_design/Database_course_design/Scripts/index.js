$(
    function () {
        $('.card').each(function (index, el) {
           $(el).delay(index * 200).animate({'margin-top': '16'}, 800);
        });
        
        
        $('.card .list-head a').each(function (index, el) {
            if ($(this).attr('class') !== '') {
                $(el).click(function (event) {
                    var $i = $(el).find('i');
                    if ($i.css('color') !== 'rgb(133, 133, 133)') {
                        $i.css('color', '#858585');
                    } else {
                        $i.css('color', '#F4C600');
                    }
                });
            }
        });

        $('#menu a').each(function (index, el) {
            $(el).click(function (event) {
                var office = 1;
                if ($(el).attr('office') === '0') {
                    office = 0;
                }
                StandardPost('/Home/Selected', { 'office': office });
            })
        })
    }
);
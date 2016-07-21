$(
    function () {
        // ��ʼ����Ч��
        $('.card').each(function (index, el) {
           $(el).delay(index * 200).animate({'margin-top': '16'}, 800);
        });
        
        // star,fork��ť�����ɫ
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

        // ��ת
        $('#menu a').each(function (index, el) {
            $(el).click(function (event) {
                var office = 1;
                if ($(el).attr('office') === '0') {
                    office = 0;
                }
                StandardPost('/Home/Selected', { 'office': office });
            })
        });

        // ����
        $('.createRepository').click(function () {
            $('#fill').show(0);
            $('#popup').addClass('show');
        });

        // ��ѡ��
        $btns = $('#popup form .btn');
        $('#popup form .btn').each(function (index, el) {
            $(el).click(function (event) {
                $(el).css({ 'background-color': '#3EF1C4', 'color': 'white' });
                $($btns[1 - index]).css({ 'background-color': '#F3F3F3', 'color': 'black' });
            })
        });

        // �ص���
        $('#popup form div button').click(function (event) {
            $('#popup').removeClass('show');
            $('#fill').delay(200).hide(0);

            $btns.each(function (index, el) {
                $(el).css({ 'background-color': '#F3F3F3', 'color': 'black' });
            })
        });

        // �����ֿ⴫ֵ
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

    }
);